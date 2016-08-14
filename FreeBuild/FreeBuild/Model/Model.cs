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
    /// A self-contained data structure that represents an entire
    /// BIM or analysis model.
    /// </summary>
    [Serializable]
    public class Model : Unique
    {
        #region Properties

        /// <summary>
        /// Get the collection of elements that form the geometric representation 
        /// of this model.
        /// </summary>
        public ElementCollection Elements { get; }

        /// <summary>
        /// Get the collection of nodes that belong to this model.
        /// </summary>
        public NodeCollection Nodes { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Model()
        {
            Elements = new ElementCollection();
            Nodes = new NodeCollection();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new element to this model
        /// </summary>
        /// <param name="element">The element to be added</param>
        public void Add(IElement element)
        {
            Elements.Add(element);
        }

        /// <summary>
        /// Add a new node to this model
        /// </summary>
        /// <param name="node"></param>
        public void Add(Node node)
        {
            Nodes.Add(node);
        }

        #endregion

    }
}
