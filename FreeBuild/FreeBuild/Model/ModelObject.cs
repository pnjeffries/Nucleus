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

using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for Model objects - objects which can be directly added to a model
    /// and form the top-level of data within that model.
    /// </summary>
    [Serializable]
    public abstract class ModelObject : DataOwner, IDeletable, IOwned<Model>
    {

        #region Properties

        /// <summary>
        /// Private backing field for IsDeleted property
        /// </summary>
        private bool _IsDeleted = false;

        /// <summary>
        /// Get a boolean value indicating whether this object has been
        /// marked for deletion.  This flag indicates that the object should be
        /// ignored in any operation that acts only on the current state of the
        /// model and that it should be removed during the next cleanup sweep.
        /// </summary>
        public bool IsDeleted
        {
            get
            {
                return _IsDeleted;
            }
        }

        /// <summary>
        /// Private backing field for Model property
        /// </summary>
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
            _IsDeleted = other.IsDeleted;
            // Other properties not necessary to copy
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete this object.
        /// The object itself will not be immediately removed from the model
        /// but will instead be flagged for future removal and ignored in most
        /// operations.  Check the IsDeleted property to see whether this
        /// object is marked for deletion.
        /// </summary>
        public void Delete()
        {
            if (!_IsDeleted)
            {
                _IsDeleted = true;
                NotifyPropertyChanged("IsDeleted");
            }
        }

        /// <summary>
        /// Undelete this object.
        /// If the deletion flag on this object is set it will be unset and
        /// the object restored.
        /// </summary>
        public void Undelete()
        {
            if (_IsDeleted)
            {
                _IsDeleted = false;
                NotifyPropertyChanged("IsDeleted");
            }
        }

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

        #endregion

    }
}
