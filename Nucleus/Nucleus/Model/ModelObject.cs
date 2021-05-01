// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Abstract base class for Model objects - objects which can be directly added to a model
    /// and form the top-level of data within that model.
    /// </summary>
    [Serializable]
    public abstract class ModelObject : Deletable, IOwned<Model>
    {

        #region Properties

        /// <summary>
        /// Private backing field for Model property
        /// </summary>
        [Copy(CopyBehaviour.MAP)]
        private Model _Model;

        /// <summary>
        /// The Model, if any, that this object currently belongs to.
        /// May be null.
        /// </summary>
        public Model Model
        {
            get { return _Model; }
            internal set { _Model = value; }
        }

        /// <summary>
        /// Get the owning Model of this object
        /// </summary>
        Model IOwned<Model>.Owner{ get { return _Model; }}


        /// <summary>
        /// Private backing field for Modified property
        /// </summary>
        [Copy(CopyBehaviour.DO_NOT_COPY)]
        private DateTime _Modified = DateTime.Now;

        /// <summary>
        /// The date and time of the last significant modification
        /// performed on this object.
        /// </summary>
        public DateTime Modified { get { return _Modified; } }


        /// <summary>
        /// Private backing field for NumericID property
        /// </summary>
        private long _NumericID = 0;

        /// <summary>
        /// The table number of this object.
        /// This is used as a display ID and also when syncing with other
        /// software that uses numeric IDs.  However it is not generally used
        /// as an identifier internally as it is not guaranteed to be unique.
        /// </summary>
        public long NumericID
        {
            get
            {
                return _NumericID;
            }
            set
            {
                _NumericID = value;
                NotifyPropertyChanged("NumericID");
            }
        }


        /// <summary>
        /// Get a description of this object.
        /// By default, this is the name of the object, but when the name is
        /// not set some types will generate a replacement description.
        /// </summary>
        public virtual string Description
        {
            get
            {
                return Name;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Protected base default constructor
        /// </summary>
        protected ModelObject() : base() {}

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other"></param>
        protected ModelObject(ModelObject other) : base(other)
        {
            // Other properties not necessary to copy
        }

        /// <summary>
        /// Name constructor
        /// </summary>
        /// <param name="name"></param>
        protected ModelObject(string name) : base(name) { }

        #endregion

        #region Methods

        /// <summary>
        /// Raise a PropertyChanged event for the specified property name.
        /// Will also update the stored last modification time.
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void NotifyPropertyChanged(string propertyName)
        {
            _Modified = DateTime.UtcNow;
            base.NotifyPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raise a PropertyChanged event for the specified property name
        /// with extended arguments.
        /// Will also update the stored last modification time.
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void NotifyPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            _Modified = DateTime.UtcNow;
            base.NotifyPropertyChanged(propertyName, oldValue, newValue);
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = Description;
            if (result != null) return result;
            else return base.ToString();
        }

        /// <summary>
        /// Delete from this object any references in its properties
        /// pointing to objects from a different model
        /// </summary>
        public void ClearDataFromMismatchedModel()
        {
            Model model = Model;
            if (model != null)
            {
                foreach (PropertyInfo pInfo in GetType().GetProperties())
                {
                    if (pInfo.CanRead && pInfo.CanWrite && 
                        pInfo.PropertyType.IsAssignableFrom(typeof(ModelObject))
                        || typeof(ModelObject).IsAssignableFrom(pInfo.PropertyType))
                    {
                        ModelObject mObj = pInfo.GetValue(this) as ModelObject;
                        if (mObj?.Model != null && mObj.Model != model)
                        {
                            // Null the property if it belongs to
                            // a different model:
                            pInfo.SetValue(this, null); 
                        }
                    }

                }
            }
        }

        #endregion

    }

    /// <summary>
    /// Static extension methods for collections of ModelObjects
    /// </summary>
    public static class ModelObjectExtensions
    {
        /// <summary>
        /// Find the index of the item in this list with the lowest NumericID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int IndexOfLowestNumericID<T>(this IList<T> list)
            where T : ModelObject
        {
            if (list == null || list.Count == 0) return -1;

            int result = 0;
            long lowestID = list[0].NumericID;
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].NumericID < lowestID)
                {
                    lowestID = list[i].NumericID;
                    result = i;
                }
            }

            return result;
        }

        /// <summary>
        /// Find and return from this set of model objects the one with the specified numeric ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="numericID"></param>
        /// <returns></returns>
        public static T GetByNumericID<T>(this IEnumerable<T> enumerable, long numericID)
            where T : ModelObject
        {
            foreach (T mObj in enumerable)
            {
                if (mObj.NumericID == numericID) return mObj;
            }
            return null;
        }
    }
}
