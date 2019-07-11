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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A collection of Materials
    /// </summary>
    [Serializable]
    public class MaterialCollection : ModelObjectCollection<Material>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MaterialCollection(): base() { }

        /// <summary>
        /// Constructor initialising a new MaterialCollection containing 
        /// the specified item
        /// </summary>
        /// <param name="item"></param>
        public MaterialCollection(Material item) : base()
        {
            Add(item);
        }

        /// <summary>
        /// Initialise a new MaterialCollection containing the specified items
        /// </summary>
        /// <param name="materials"></param>
        public MaterialCollection(IEnumerable<Material> materials) : base()
        {
            AddRange(materials);
        }

        /// <summary>
        /// Owner constructor
        /// </summary>
        /// <param name="model"></param>
        protected MaterialCollection(Model model) : base(model) { }

        #endregion
    }
}
