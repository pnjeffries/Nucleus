using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A wrapper for combined vertices which form part of multiple elements' definitions, with
    /// properties set up to make the local properties easier to interrogate and set for all of those
    /// elements at once.
    /// This is a temporary object that is created when necessary and is not persisted
    /// beyond that.
    /// </summary>
    public struct MultiElementVertex
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
        /// Private backing field for Elements properties
        /// </summary>
        private ElementCollection _Elements;

        /// <summary>
        /// The elements that this vertex is linked to
        /// </summary>
        public ElementCollection Elements
        {
            get { return _Elements; }
        }

        /// <summary>
        /// Private backing field for Vertices property
        /// </summary>
        private VertexCollection _Vertices;

        /// <summary>
        /// The collection of vertices represented by this MultiElementVertex
        /// </summary>
        public VertexCollection Vertices
        {
            get { return _Vertices; }
        }

        /// <summary>
        /// Get or set the node shared by all the element vertices this
        /// object represents.  If all vertices do not share the same node,
        /// null will be returned.
        /// </summary>
        public Node Node
        {
            get { return Vertices.CombinedValue(i=>i.Node); }
            set { foreach (Vertex v in Vertices) v.Node = value; }
        }

        /// <summary>
        /// Get or set the position shared by all the element vertices this
        /// object represents.  If all vertices do not share the same position,
        /// Vector.Unset will be returned.
        /// </summary>
        public Vector Position
        {
            get { return Vertices.CombinedValue(i => i.Position, Vector.Unset, Vector.Unset); }
            set { foreach (Vertex v in Vertices) v.Position = value; }
        }

        /// <summary>
        /// Get or set the position X-coordinate shared by all the element vertices
        /// this object represents.  If all vertices do not lie on the same X-coordinate,
        /// double.NaN will be returned.
        /// </summary>
        public double X
        {
            get { return Vertices.CombinedValue(i => i.X, double.NaN, double.NaN); }
            set { foreach (Vertex v in Vertices) v.X = value; }
        }

        /// <summary>
        /// Get or set the position Y-coordinate shared by all the element vertices
        /// this object represents.  If all vertices do not lie on the same Y-coordinate,
        /// double.NaN will be returned.
        /// </summary>
        public double Y
        {
            get { return Vertices.CombinedValue(i => i.Y, double.NaN, double.NaN); }
            set { foreach (Vertex v in Vertices) v.Y = value; }
        }

        /// <summary>
        /// Get or set the position Z-coordinate shared by all the element vertices
        /// this object represents.  If all vertices do not lie on the same Z-coordinate,
        /// double.NaN will be returned.
        /// </summary>
        public double Z
        {
            get { return Vertices.CombinedValue(i => i.Z, double.NaN, double.NaN); }
            set { foreach (Vertex v in Vertices) v.Z = value; }
        }

        /// <summary>
        /// The offset vector of this element vertex from the node which defines it
        /// </summary>
        public Vector Offset
        {
            get { return Vertices.CombinedValue(i => i.NodalOffset(), Vector.Unset, Vector.Unset); }
            set { foreach (Vertex v in Vertices) v.SetPositionByNodalOffset(value); }
        }

        /// <summary>
        /// The X-coordinate of the offset vector
        /// </summary>
        public double Offset_X
        {
            get { return Vertices.CombinedValue(i => i.NodalOffset().X, double.NaN, double.NaN); }
            set { foreach (Vertex v in Vertices) v.SetPositionByNodalOffset(v.NodalOffset().WithX(value)); }
        }

        /// <summary>
        /// The Y-coordinate of the offset vector
        /// </summary>
        public double Offset_Y
        {
            get { return Vertices.CombinedValue(i => i.NodalOffset().Y, double.NaN, double.NaN); }
            set { foreach (Vertex v in Vertices) v.SetPositionByNodalOffset(v.NodalOffset().WithY(value)); }
        }

        /// <summary>
        /// The Z-coordinate of the offset vector
        /// </summary>
        public double Offset_Z
        {
            get { return Vertices.CombinedValue(i => i.NodalOffset().Z, double.NaN, double.NaN); }
            set { foreach (Vertex v in Vertices) v.SetPositionByNodalOffset(v.NodalOffset().WithZ(value)); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new MultiElementVertex for the specified elements and vertices
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="vertices"></param>
        /// <param name="description"></param>
        public MultiElementVertex(ElementCollection elements, VertexCollection vertices, string description)
        {
            _Description = description;
            _Elements = elements;
            _Vertices = vertices;
        }

        /// <summary>
        /// Initialise a MultiElementVertex helper object combining the
        /// specified set of ElementVertices.
        /// </summary>
        /// <param name="elementVertices"></param>
        public MultiElementVertex(IList<ElementVertex> elementVertices)
        {
            _Description = elementVertices.CombinedValue(i => i.Description, "[Multi]");
            _Elements = new ElementCollection();
            _Vertices = new VertexCollection();
            foreach (var elVert in elementVertices)
            {
                _Elements.Add(elVert.Element);
                _Vertices.Add(elVert.Vertex);
            }
        }

        #endregion
    }
}
