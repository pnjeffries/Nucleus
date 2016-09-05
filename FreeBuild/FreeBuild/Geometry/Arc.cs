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
    /// A planar arc of constant radius described by three points.
    /// </summary>
    [Serializable]
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
        /// The angle subtended by this arc
        /// </summary>
        public Angle RadianMeasure
        {
            get
            {
                if (Circle != null)
                {
                    if (Closed) return Angle.Complete;

                    Angle toEnd = Circle.Azimuth(Vertices.Last().Position);
                    Angle toMid = Circle.Azimuth(Vertices[1].Position);
                    if (toMid > toEnd) return toEnd.Explement();
                    else return toEnd;
                }
                return Angle.Zero;
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

        /// <summary>
        /// The number of segments in this curve.
        /// For valid arcs, this is always 1.
        /// </summary>
        public override int SegmentCount
        {
            get
            {
                if (IsValid) return 1;
                else return 0;
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

        #region Methods

        /// <summary>
        /// Calculate and return the length of this arc
        /// </summary>
        /// <returns></returns>
        public override double CalculateLength()
        {
            if (Circle != null)
            {
                return Circle.Circumference * RadianMeasure / 2 * Math.PI;
            }
            else return 0;
        }

        public override double CalculateSegmentLength(int index)
        {
            return CalculateLength();
        }

        /// <summary>
        /// Evaluate a point on this curve defined by a parameter t
        /// </summary>
        /// <param name="t">A normalised parameter defining a point along this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = curve start, 1 = curve end.
        /// For open curves, parameters outside the range 0-1 will be invalid.
        /// For closed curves, parameters outside this range will 'wrap'.</param>
        /// <returns>The vector coordinates describing a point on the curve at the specified parameter,
        /// if the curve definition and parameter are valid.  Else, an unset vector.</returns>
        /// <remarks>The base implementation treats the curve as being defined as a polyline, with straight lines
        /// between vertices.</remarks>
        public override Vector PointAt(double t)
        {
            if (Circle != null)
            {
                return Circle.PointAt(t * RadianMeasure);
            }
            return Vector.Unset;
        }

        public override Vector PointAt(int span, double tSpan)
        {
            return PointAt(tSpan);
        }

        protected override void InvalidateCachedGeometry()
        {
            _Circle = null;
            base.InvalidateCachedGeometry();
        }

        #endregion
    }
}
