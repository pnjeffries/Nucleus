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

using FreeBuild.Extensions;
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

        /// <summary>
        /// Initialise an arc that forms a complete circle
        /// </summary>
        /// <param name="circle"></param>
        public Arc(Circle circle):this()
        {
            Vertices.Add(new Vertex(circle.PointAt(0.0)));
            Vertices.Add(new Vertex(circle.PointAt(Math.PI)));
            Vertices.Add(new Vertex(circle.PointAt(2*Math.PI)));
            Closed = true;
            _Circle = circle;
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

        /// <summary>
        /// Evaluate a point defined by a parameter within a specified span.
        /// </summary>
        /// <param name="span">The index of the span.  Valid range 0 to SegmentCount - 1</param>
        /// <param name="tSpan">A normalised parameter defining a point along this span of this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.
        /// </param>
        /// <returns>The vector coordinates describing a point on the curve span at the specified parameter,
        /// if the curve definition and parameter are valid.  Else, null.</returns>
        /// <remarks>The base implementation treats the curve as being defined as a polyline, with straight lines
        /// between vertices.</remarks>
        public override Vector PointAt(int span, double tSpan)
        {
            return PointAt(tSpan);
        }

        /// <summary>
        /// Evaluate the tangent unit vector of a point on this curve defined by a parameter t
        /// </summary>
        /// <param name="t">A normalised parameter defining a point along this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = curve start, 1 = curve end.
        /// For open curves, parameters outside the range 0-1 will be invalid.
        /// For closed curves, parameters outside this range will 'wrap'.</param>
        /// <returns>The tangent unit vector of the curve at the specified parameter</returns>
        /// <remarks>The base implementation treats the curve as being defined as a polyline, with straight lines
        /// between vertices.</remarks>
        public override Vector TangentAt(double t)
        {
            if (Circle != null)
            {
                return Circle.TangentAt(t * RadianMeasure);
            }
            return Vector.Unset;
        }

        public override Vector TangentAt(int span, double tSpan)
        {
            return TangentAt(tSpan);
        }

        protected override void InvalidateCachedGeometry()
        {
            if (!Closed) _Circle = null;
            else
            {
                //TODO; Update circle!
            }
            base.InvalidateCachedGeometry();
        }

        public override void Move(Vector translation)
        {
            base.Move(translation);
            if (_Circle != null) _Circle = _Circle.Move(translation);
        }

        public override Vector[] Facet(Angle tolerance)
        {
            int divisions = 1;
            if (tolerance > 0 && tolerance < RadianMeasure)
                divisions = (int)Math.Ceiling(RadianMeasure / tolerance);
            return Circle.Divide(divisions, 0.0, RadianMeasure);
        }

        public override double CalculateEnclosedArea(out Vector centroid, Plane onPlane = null)
        {
            if (Circle == null)
            {
                centroid = Vector.Unset;
                return 0;
            }
            else
            {
                //Area:
                double angle = RadianMeasure;
                double radius = Circle.Radius;
                double area = (angle - Math.Sin(angle)) * (radius.Squared() / 2);
                if (!IsClockwise) area *= -1;
                //TODO: Adjust by area ratio

                //Centroid:
                double rBar = (4 * (Math.Sin(angle / 2).Power(3))) / (3 * (angle - Math.Sin(angle)));
                Vector origin = Circle.Origin;
                Vector mid = PointAt(0.5);
                centroid = origin.Interpolate(mid, rBar);

                return area;
            }
        }

        public override double CalculateEnclosedIxx(Plane onPlane = null)
        {
            if (Circle != null)
            {
                Vector o = Circle.Origin;
                double r = Circle.Radius;
                Vector c;
                double a = CalculateEnclosedArea(out c, onPlane).Abs();
                //TODO
            }
            return 0;
        }

        /// <summary>
        /// The area moment of inertia of a filled circular sector or angle theta in radians and radius r with respect to an axis through the centroid of the circle
        /// Used in the calculation of enclosed area second moments of area
        /// </summary>
        /// <param name="theta">The sector angle in radians in the range 0 to 2*PI</param>
        /// <param name="radius"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static double SectorIxx(double theta, double radius)
        {
            if (theta > Math.PI * 1.5)
            {
                theta = Math.PI * 2 - theta;
                //Full circle, minus reflex angle
                return (Math.PI / 4) * radius.Power(4) - ((theta * 2) - Math.Sin(theta * 2)) * (radius.Power(4)) / 16;
            }
            else if (theta > Math.PI)
            {
                theta = theta - Math.PI; //Find for angle over PI, then add half circle
                return ((theta * 2) - Math.Sin(theta * 2)) * (radius.Power(4)) / 16 + ((Math.PI / 8) * radius.Power(4));
            }
            else if (theta > Math.PI / 2) //PI/2 to PI
            {
                //Calculate for half circle, then subtract reflex sector
                theta = Math.PI - theta;
                return ((Math.PI / 4) * radius.Power(4) - (theta * 2 - Math.Sin(theta * 2)) * (radius.Power(4)) / 8) / 2;
            }
            else //0 to PI/2
                return ((theta * 2) - Math.Sin(theta * 2)) * (radius.Power(4)) / 16;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Attempt to create a new Arc defined by a start point, start tangent and end point.
        /// This function may return an Arc, a Line (in the case that the tangent vector directly
        /// leads to the end point) or null if the start and end point are coincident.
        /// </summary>
        /// <param name="startPt"></param>
        /// <param name="startTangent"></param>
        /// <param name="endPt"></param>
        /// <returns></returns>
        public static Curve StartTangentEnd(Vector startPt, Vector startTangent, Vector endPt)
        {
            Vector SE = endPt - startPt;
            double dist = SE.Magnitude();
            if (dist > 0)
            {
                SE /= dist;
                startTangent = startTangent.Unitize();
                if (startTangent.Equals(SE,0.001))
                {
                    return new Line(startPt, endPt);
                }
                else
                {
                    Vector BS = (SE + startTangent).Unitize();
                    BS *= (0.5 * dist) / (BS.Dot(startTangent));
                    return new Arc(startPt, startPt + BS, endPt);
                }
            }
            else return null;
        }



        #endregion
    }
}
