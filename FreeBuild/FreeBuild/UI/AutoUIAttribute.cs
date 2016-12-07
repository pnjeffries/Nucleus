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

namespace FreeBuild.UI
{
    /// <summary>
    /// Attribute for automatic UI generation.  Add this attribute to properties
    /// for which you wish to have UI controls generated automatically.
    /// </summary>
    public class AutoUIAttribute : Attribute
    {
        #region properties

        /// <summary>
        /// The order weighting for this property.  Those with a lower order
        /// weighting will be displayed first.
        /// </summary>
        public double Order { get; set; } = 0;

        /// <summary>
        /// The text that the field should be labelled with.  If not set,
        /// the property name itself will be used.
        /// </summary>
        public string Label { get; set; } = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoUIAttribute() { }

        /// <summary>
        /// Order constructor
        /// </summary>
        /// <param name="order"></param>
        public AutoUIAttribute(double order)
        {
            Order = order;
        }

        #endregion
    }
}
