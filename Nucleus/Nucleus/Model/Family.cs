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
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A shared property that describes how to resolve
    /// an element's editable set-out geometry into a full
    /// 3D solid object.
    /// </summary>
    [Serializable]
    public abstract class Family : DataOwner<FamilyDataStore, IFamilyDataComponent, Family>
    {
        #region Methods

        /// <summary>
        /// Get a collection of all elements in the same model which have this property assigned
        /// </summary>
        /// <returns></returns>
        public ElementCollection Elements()
        {
            if (Model != null) return Model.Elements.AllWith(this);
            else return new ElementCollection();
        }

        protected override FamilyDataStore NewDataStore()
        {
            return new FamilyDataStore(this);
        }

        /// <summary>
        /// Get the primary material of which this family is composed.
        /// This will be the material of the outer profile for Sections,
        /// the thickest layer for Build-Up families and so on.
        /// </summary>
        /// <returns></returns>
        public abstract Material GetPrimaryMaterial();

        #endregion
    }
}
