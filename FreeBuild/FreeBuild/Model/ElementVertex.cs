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
    //SPECULATIVE - Could we replace putting nodes on shape vertices with something like this?
    //HMMMMM... Maybe stick with current scheme until we come across a compelling reason to change?

    /// <summary>
    /// A position along an element
    /// </summary>
    public class ElementVertex : Unique, IOwned<Element>
    {
        #region Properties

        /// <summary>
        /// Private backing field for Element property
        /// </summary>
        private Element _Element;

        /// <summary>
        /// The element that this vertex belongs to
        /// </summary>
        public Element Element { get { return _Element; } }

        Element IOwned<Element>.Owner { get { return _Element; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.  Initialises a new ElementVertex belonging to the specified element
        /// </summary>
        /// <param name="element"></param>
        public ElementVertex(Element element)
        {
            _Element = element;
        }

        #endregion
    }
}
