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
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    //SPECULATIVE - Could we replace putting nodes on shape vertices with something like this?
    //HMMMMM... Maybe stick with current scheme until we come across a compelling reason to change?

    /// <summary>
    /// A wrapper for a vertex which forms part of an element definition, with
    /// properties set up to make the local properties easier to interrogate and set.
    /// This is a temporary object that is created when necessary and is not persisted
    /// beyond that.
    /// </summary>
    public struct ElementVertex : IOwned<Element>
    {
        #region Properties

        /// <summary>
        /// Private backing field for Description property
        /// </summary>
        private string _Description;

        /// <summary>
        /// The description of the position of this vertex relative to its parent element.
        /// Typically a number, "Start" or "End"
        /// </summary>
        public string Description
        {
            get { return _Description; }
        }

        /// <summary>
        /// Private backing field for Element property
        /// </summary>
        private Element _Element;

        /// <summary>
        /// The element that this vertex belongs to
        /// </summary>
        public Element Element { get { return _Element; } }

        Element IOwned<Element>.Owner { get { return _Element; } }

        /// <summary>
        /// Private backing field for Vertex property
        /// </summary>
        private Vertex _Vertex;

        /// <summary>
        /// The vertex on the element
        /// </summary>
        public Vertex Vertex
        {
            get { return _Vertex; }
        }

        /// <summary>
        /// Get or set the node that this vertex is connected to
        /// </summary>
        public Node Node
        {
            get { return _Vertex.Node; }
            set { _Vertex.Node = value; }
        }

        /// <summary>
        /// The position of the vertex
        /// </summary>
        public Vector Position
        {
            get { return Vertex.Position; }
            set { Vertex.Position = value; }
        }

        /// <summary>
        /// The offset of this vertex from its node.
        /// Setting this property will change the position of this vertex
        /// with reference to the node position, provided that a node exists for this
        /// vertex.
        /// </summary>
        public Vector Offset
        {
            get { return Vertex.NodalOffset(); }
            set
            {
                if (Vertex.Node != null)
                {
                    Vertex.Position = Vertex.Node.Position + value;
                }
            }
        }

        /// <summary>
        /// The translational and rotational releases of the element vertex
        /// </summary>
        public Bool6D Releases
        {
            get
            {
                if (Vertex.HasData<VertexReleases>())
                    return Vertex.GetData<VertexReleases>().Releases;
                else return new Bool6D(false);
            }
            set
            {
                if (Vertex.HasData<VertexReleases>())
                    Vertex.GetData<VertexReleases>().Releases = value;
                else
                    Vertex.Data.Add(new VertexReleases(value));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.  Initialises a new ElementVertex belonging to the specified element
        /// </summary>
        /// <param name="element"></param>
        public ElementVertex(Element element, Vertex vertex, string description)
        {
            _Element = element;
            _Vertex = vertex;
            _Description = description;
        }

        #endregion

        #region Methods

        public MultiElementVertex ToMultiElementVertex()
        {
            return new MultiElementVertex(this);
        }

        /// <summary>
        /// Set the releases data component on this vertex
        /// </summary>
        /// <param name="releases"></param>
        public void SetReleases(VertexReleases releases)
        {
            Vertex.SetData(releases);
        }

        /// <summary>
        /// Remove any released degrees of freedom from this element vertex
        /// </summary>
        public void ClearReleases()
        {
            Vertex.CleanData(typeof(VertexReleases));
        }

        #endregion
    }

    public static class ElementVertexExtensions
    {
        /// <summary>
        /// Convert this collection of ElementVertices into a collection of MultiElementVertices
        /// </summary>
        /// <param name="verts"></param>
        /// <returns></returns>
        public static IList<MultiElementVertex> ToMultiElementVertices(this IList<ElementVertex> verts)
        {
            var result = new List<MultiElementVertex>(verts.Count);
            foreach (var eV in verts)
            {
                result.Add(eV.ToMultiElementVertex());
            }
            return result;
        }
    }
}
