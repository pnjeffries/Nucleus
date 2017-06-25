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

namespace Nucleus.Geometry
{
    /// <summary>
    /// Structure that represents an edge between two vertices in a mesh.
    /// A temporary construct used during certain mesh operations - does not
    /// form part of the core definition of a mesh geometry
    /// </summary>
    public struct MeshEdge
    {
        #region Properties

        /// <summary>
        /// The vertex at the start of this edge
        /// </summary>
        public Vertex Start { get; }

        /// <summary>
        /// The vertex at the end of this edge
        /// </summary>
        public Vertex End { get; }

        /// <summary>
        /// Get the start point of this edge
        /// </summary>
        public Vector StartPoint { get { return Start.Position; } }

        /// <summary>
        /// Get the end point of this edge
        /// </summary>
        public Vector EndPoint { get { return End.Position; } }

        /// <summary>
        /// Get the length of this edge
        /// </summary>
        public double Length
        {
            get { return StartPoint.DistanceTo(EndPoint); }
        }

        /// <summary>
        /// Get the squared length of this edge
        /// </summary>
        public double LengthSquared
        {
            get { return StartPoint.DistanceToSquared(EndPoint); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor, initialising an edge between the two specified vertices
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public MeshEdge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            return Start.GetHashCode() * End.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this == (MeshEdge)obj;
        }

        public bool Equals(ref MeshEdge other)
        {
            return ((Start == other.Start && End == other.End) || (Start == other.End && End == other.Start));
        }

        #endregion

        #region Operators

        /// <summary>
        /// Tests whether the two edges are equal.
        /// Direction does not matter for two edges to be equal.
        /// </summary>
        /// <param name="edge1"></param>
        /// <param name="edge2"></param>
        /// <returns></returns>
        public static bool operator == (MeshEdge edge1, MeshEdge edge2)
        {
            //if (ReferenceEquals(edge1, edge2)) return true;
            //else if ((object)edge1 == null || (object)edge2 == null) return false;
            //else 
            return ((edge1.Start == edge2.Start && edge1.End == edge2.End) || (edge1.Start == edge2.End && edge1.End == edge2.Start));
        }

        /// <summary>
        /// Tests whether the two edges are not equal.
        /// Direction does not matter for two edges to be equal.
        /// </summary>
        /// <param name="edge1"></param>
        /// <param name="edge2"></param>
        /// <returns></returns>
        public static bool operator != (MeshEdge edge1, MeshEdge edge2)
        {
            return !(edge1 == edge2);
        }

        #endregion
    }
}
