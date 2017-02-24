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

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of linear elements
    /// </summary>
    [Serializable]
    public class LinearElementCollection : ElementCollection<LinearElement, LinearElementCollection>
    {
        #region Constructors

        /// <summary>
        /// Initialise a new empty LinearElementCollection
        /// </summary>
        public LinearElementCollection() : base() { }

        /// <summary>
        /// Initialise a new LinearElementCollection containing the specified single item
        /// </summary>
        /// <param name="element"></param>
        public LinearElementCollection(LinearElement element) : base()
        {
            Add(element);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the subset of items in this collection which has a recorded modification 
        /// after the specified date and time
        /// </summary>
        /// <param name="since">The date/time to filter by</param>
        /// <returns></returns>
        public LinearElementCollection Modified(DateTime since)
        {
            return this.Modified<LinearElementCollection, LinearElement>(since);
        }

        #endregion
    }
}
