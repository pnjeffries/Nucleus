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
        /// TODO: Provide the combined offset to the centreline of the element's
        /// section or build-up from this vertex.
        /// NOT YET IMPLEMENTED PROPERLY!
        /// </summary>
        public Vector CombinedOffset
        {
            get
            {
                Vector result = Offset;
                if (Element is LinearElement)
                {
                    var lEl = (LinearElement)Element;
                    if (lEl.Family?.Profile != null)
                    {
                        Vector profileOffset = lEl.Family.GetTotalOffset();
                        var cSystem = GetLocalCoordinateSystem(false);
                        result -= cSystem.LocalToGlobal(profileOffset, true);
                    }
                }
                return result;
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

        /// <summary>
        /// The translational release in the X-axis direction
        /// </summary>
        public bool Release_X
        {
            get { return Releases.X; }
            set { Releases = Releases.WithX(value); }
        }

        /// <summary>
        /// The translational release in the Y-axis direction
        /// </summary>
        public bool Release_Y
        {
            get { return Releases.Y; }
            set { Releases = Releases.WithY(value); }
        }

        /// <summary>
        /// The translational release in the Z-axis direction
        /// </summary>
        public bool Release_Z
        {
            get { return Releases.Z; }
            set { Releases = Releases.WithZ(value); }
        }

        /// <summary>
        /// The rotational release about the X-axis
        /// </summary>
        public bool Release_XX
        {
            get { return Releases.XX; }
            set { Releases = Releases.WithXX(value); }
        }

        /// <summary>
        /// The rotational release about the Y-axis
        /// </summary>
        public bool Release_YY
        {
            get { return Releases.YY; }
            set { Releases = Releases.WithYY(value); }
        }

        /// <summary>
        /// The rotational release about the Z-axis
        /// </summary>
        public bool Release_ZZ
        {
            get { return Releases.ZZ; }
            set { Releases = Releases.WithZZ(value); }
        }

        /// <summary>
        /// Get the local coordinate system of the element at this vertex
        /// </summary>
        public CartesianCoordinateSystem LocalCoordinateSystem
        {
            get
            {
                return GetLocalCoordinateSystem();
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

        /// <summary>
        /// Get the local coordinate system of the element at this vertex
        /// </summary>
        /// <param name="allowFlip">If true, the coordinate system may be aligned
        /// to point towards the centre of the element.  Otherwise, it will be the
        /// 'real' coordinate system</param>
        /// <returns></returns>
        public CartesianCoordinateSystem GetLocalCoordinateSystem(bool allowFlip = true)
        {
            if (Element is LinearElement)
            {
                var lEl = (LinearElement)Element;
                if (Description == "End")
                {
                    var result = lEl.LocalCoordinateSystem(1);
                    if (allowFlip) result = result.ReverseXY();
                    return result;
                }
                else if (Description == "Start")
                {
                    return lEl.LocalCoordinateSystem();
                }
            }
            else if (Element is PanelElement)
            {
                var pEl = (PanelElement)Element;
                var result = pEl.LocalCoordinateSystem();
                result = new CartesianCoordinateSystem(result, Position);
                //TODO: Align panel element vertices?
                return result;
            }
            return null;
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
