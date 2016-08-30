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

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A planar arc described by three points.
    /// </summary>
    public class Arc : Curve
    {
        #region Properties

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// The first vertex defines the start point of the arc.  The second describes a point on
        /// the arc.  The third describes the end of the arc in the case of an open arc, or a distinct
        /// third point somewhere on the circle if the arc is closed.
        /// </summary>
        public override VertexCollection Vertices { get; }

        /// <summary>
        /// Private backing member for Circle property
        /// </summary>
        [NonSerialized]
        private Circle _Circle = null;

        /// <summary>
        /// The full circle that this arc forms part of
        /// </summary>
        public Circle Circle
        {
            get
            {
                if (_Circle == null && Vertices.Count >= 3)
                {
                    _Circle = new Circle(Vertices[0].Position, Vertices[1].Position, Vertices.Last().Position);
                }
                return _Circle;
            }
        }

        /// <summary>
        /// Does this arc run clockwise or anticlockwise, with reference to the Circle
        /// derived from its vertices
        /// </summary>
        public bool IsClockwise
        {
            get
            {
                if (Vertices.Count >= 3 && Circle != null)
                {
                    return Circle.Azimuth(Vertices.Last().Position) >
                        Circle.Azimuth(Vertices[1].Position);
                }
                return false;
            }
        }

        /// <summary>
        /// The length of the arc expressed as an angle
        /// </summary>
        public Angle ArcLength
        {
            get
            {
                if (Circle != null)
                {
                    return (Circle.)
                }
            }
        } 

        /// <summary>
        /// Is this Arc closed?  (i.e. does it represent a circle?)
        /// </summary>
        public override bool Closed { get; protected set; }

        /// <summary>
        /// Is this Arc valid?
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return Vertices.Count == 3;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises an arc with no geometry.
        /// Will not be valid.
        /// </summary>
        protected Arc()
        {
            Vertices = new VertexCollection(this);
        }

        /// <summary>
        /// Initialise an arc from three points
        /// </summary>
        /// <param name="startPt">The start point</param>
        /// <param name="ptOnArc">A point that lies somewhere on the arc</param>
        /// <param name="endPt">The end point</param>
        public Arc(Vector startPt, Vector ptOnArc, Vector endPt) : this()
        {
            Vertices.Add(new Vertex(startPt));
            Vertices.Add(new Vertex(ptOnArc));
            Vertices.Add(new Vertex(endPt));
        }

        #endregion
    }
}
