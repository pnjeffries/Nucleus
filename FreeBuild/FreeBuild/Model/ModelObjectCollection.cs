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
    /// A collection of model objects.
    /// Generic version to allow further specificity of object type.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [Serializable]
    public class ModelObjectCollection<TItem> : OwnedCollection<TItem, Model> where TItem : ModelObject
    {

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new model object collection
        /// </summary>
        public ModelObjectCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises a model object collection with an owning model.
        /// </summary>
        /// <param name="model"></param>
        protected ModelObjectCollection(Model model) : base(model) { }

        /// <summary>
        /// Collection combination constructor
        /// </summary>
        /// <param name="toBeCombined"></param>
        public ModelObjectCollection(IEnumerable<IEnumerable<TItem>> toBeCombined) : base(toBeCombined) { }

        #endregion

        #region Methods

        protected override void SetItemOwner(TItem item)
        {
            if (Owner != null) item.Model = Owner;
        }

        protected override void ClearItemOwner(TItem item)
        {
            if (Owner != null) item.Model = null;
        }

        /// <summary>
        /// Find the first item in this collection which has the specified name (if any)
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <param name="ignore">Optional.  If specified this object will be ignore during the search.</param>
        /// <returns></returns>
        public virtual TItem FindByName(string name, TItem ignore = null)
        {
            foreach(TItem mO in this)
            {
                if (mO.Name == name && mO != ignore) return mO;
            }
            return null;
        }

        /// <summary>
        /// Return the next version of this name with an attached numerical postfix that
        /// will be a unique name in this collection.
        /// </summary>
        /// <param name="baseName">The base name</param>
        /// <param name="ignore">Optional.  If specified, this object will be ignored during the search.</param>
        /// <param name="enforcePostFix">Optional.  If set true, a postfix numeral will always be applied, even if it is 1.</param>
        /// <returns></returns>
        public string NextAvailableName(string baseName, TItem ignore = null, bool enforcePostFix = false)
        {
            if (!enforcePostFix && FindByName(baseName) == null) return baseName;
            else
            {
                int postFix = 2;
                if (enforcePostFix) postFix = 1;
                while (postFix < 100000)
                {
                    string nextName = baseName + " " + postFix;
                    if (FindByName(nextName, ignore) == null) return nextName;
                    postFix++;
                }
            }
            return baseName;
        }

        #endregion
    }

    /// <summary>
    /// A collection of model objects
    /// </summary>
    [Serializable]
    public class ModelObjectCollection : ModelObjectCollection<ModelObject>
    {
        /// <summary>
        /// Default constructor.  Initialises a new model object collection
        /// </summary>
        public ModelObjectCollection() : base() { }

        /// <summary>
        /// Collection combination constructor
        /// </summary>
        /// <param name="toBeCombined"></param>
        public ModelObjectCollection(IEnumerable<IEnumerable<ModelObject>> toBeCombined) : base(toBeCombined) { }
    }
}
