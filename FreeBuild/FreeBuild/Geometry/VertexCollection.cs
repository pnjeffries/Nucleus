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
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An obervable, keyed collection of vertices
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [Serializable]
    public class VertexCollection : OwnedCollection<Vertex, Shape>
    {
        
        #region Constructor

        /// <summary>
        /// Owner constructor
        /// </summary>
        /// <param name="owner"></param>
        public VertexCollection(Shape owner): base(owner)
        {
        }

        /// <summary>
        /// Default, parameterless constructor.
        /// The owner of this vertex collection will be null.
        /// </summary>
        public VertexCollection() : this(null) { }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the owning geometry of the specified vertex
        /// </summary>
        /// <param name="item"></param>
        protected override void SetItemOwner(Vertex item)
        {
            if (Owner != null)
            {
                if (item.Owner != null && item.Owner != Owner)
                {
                    throw new Exception("Vertex already has an owner.  The same vertex cannot belong to more than one piece of geometry.");
                }
                else
                    item.Owner = Owner;
            }
        }

        /// <summary>
        /// Clears the owning geometry of the specified
        /// </summary>
        /// <param name="item"></param>
        protected override void ClearItemOwner(Vertex item)
        {
            if (item.Owner == Owner) item.Owner = null;
        }

        /// <summary>
        /// Calculates and returns the bounding box of all of the vertices
        /// within this collection.
        /// </summary>
        /// <returns>A new bounding box containing all vertices in this collection.</returns>
        public BoundingBox BoundingBox()
        {
            return new BoundingBox(this);
        }

        /// <summary>
        /// Do any of the vertices in this collection contain a reference to
        /// the specified node?
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool ContainsNode(Node node)
        {
            foreach (Vertex v in this)
            {
                if (v.Node == node) return true;
            }
            return false;
        }

        #endregion
    }
}
