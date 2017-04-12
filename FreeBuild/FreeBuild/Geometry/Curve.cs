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
using FreeBuild.Maths;
using FreeBuild.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Base class for curves.
    /// Curves are 1-Dimensional geometries defined by a set of ordered vertices. 
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <remarks>This class implements a default set of interrogation methods, which will treat
    /// this curve as a polyline of straight segments between vertices.</remarks>
    [Serializable]
    public abstract class Curve: VertexGeometry
    {
        #region Properties

        /// <summary>
        /// Whether this curve is closed.
        /// If true, the end of the curve is treated as being the same as the start point.
        /// Default (for most curve types) is false.
        /// </summary>
        public abstract bool Closed { get; protected set; }

        /// <summary>
        /// Get the number of segments that this curve posesses.
        /// Segments are stretches of the curve that can be evaluated independantly 
        /// of the rest of the curve.
        /// </summary
        public virtual int SegmentCount
        {
            get
            {
                if (Vertices.Count > 0)
                {
                    if (Closed) return Vertices.Count;
                    return Vertices.Count - 1;
                }
                else return 0;
            }
        }

        /// <summary>
        /// Get the parameter-space domain of this curve
        /// </summary>
        public virtual Interval Domain
        {
            get
            {
                return new Interval(0, 1.0);
            }
        }

        /// <summary>
        /// Get the vertex at the start of the curve (if there is one)
        /// </summary>
        public virtual Vertex Start
        {
            get
            {
                if (Vertices.Count > 0)
                    return Vertices.First();
                else return null;
            }
        }

        /// <summary>
        /// Get the vertex at the end of the curve (if there is one)
        /// </summary>
        public virtual Vertex End
        {
            get
            {
                if (Closed) return Start;
                else if (Vertices.Count > 0) return Vertices.Last();
                else return null;
            }
        }

        /// <summary>
        /// Get the start point of this curve
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public Vector StartPoint
        {
            get
            {
                Vertex start = Start;
                if (start != null) return start.Position;
                else return Vector.Unset;
            }
        }

        /// <summary>
        /// Get the end point of this curve
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public Vector EndPoint
        {
            get
            {
                Vertex end = End;
                if (end != null) return end.Position;
                else return Vector.Unset;
            }
        }

        /// <summary>
        /// Get the length of this curve
        /// </summary>
        [Dimension(DimensionType.Distance)]
        public double Length
        {
            get
            {
                return CalculateLength();
            }
        }

        #endregion

        #region Methods

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
        public virtual Vector PointAt(double t)
        {
            if (Closed) t = t % 1.0;

            //Calculate span
            double tSpan = t * SegmentCount;
            int span = (int)Math.Floor(tSpan);
            tSpan = tSpan % 1.0; //Position in span
            return PointAt(span, tSpan);
        }

        /// <summary>
        /// Evaluate a point on this curve a specified distance from the start or end.
        /// </summary>
        /// <param name="length">The length along the line </param>
        /// <param name="fromEnd"></param>
        /// <returns></returns>
        public Vector PointAtLength(double length, bool fromEnd = false)
        {
            if (!fromEnd) return StartPoint.Interpolate(EndPoint, length / Length);
            else return EndPoint.Interpolate(StartPoint, length / Length);
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
        public virtual Vector PointAt(int span, double tSpan)
        {
            if (!IsValid) return Vector.Unset; //No spans!
            Vertex start = SegmentStart(span); //Find the span start vertex
            if (start == null) return Vector.Unset; //If the start vertex doesn't exist, abort
            if (tSpan == 0) return start.Position;  //If tSpan is at the start of the span, just return the start position
            else
            {
                Vertex end = SegmentEnd(span); //Find the span end vertex
                return start.Position.Interpolate(end.Position, tSpan); //Interpolate position
            }
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
        public virtual Vector TangentAt(double t)
        {
            if (Closed) t = t % 1.0;

            //Calculate span
            if (t == 1.0)
            {
                return TangentAt(SegmentCount - 1, 1.0);
            }
            else
            {
                int segCount = SegmentCount;
                double tSpan = t * segCount;
                int span = (int)Math.Floor(tSpan);
                tSpan = tSpan % 1.0; //Position in span
                return TangentAt(span, tSpan);
            }
        }

        /// <summary>
        /// Evaluate the tangent unit vector of a point defined by a parameter within a specified span.
        /// </summary>
        /// <param name="span">The index of the span.  Valid range 0 to SegmentCount - 1</param>
        /// <param name="tSpan">A normalised parameter defining a point along this span of this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.
        /// </param>
        /// <returns>The unit vector describing the tangent of a point on the curve span at the specified parameter,
        /// if the curve definition and parameter are valid.  Else, null.</returns>
        /// <remarks>The base implementation treats the curve as being defined as a polyline, with straight lines
        /// between vertices.</remarks>
        public virtual Vector TangentAt(int span, double tSpan)
        {
            if (!IsValid) return Vector.Unset; //No spans!
            Vertex start = SegmentStart(span); //Find the span start vertex
            if (start == null) return Vector.Unset; //If the start vertex doesn't exist, abort
            
            Vertex end = SegmentEnd(span); //Find the span end vertex
            return (end.Position - start.Position).Unitize(); //Interpolate position
        }

        /// <summary>
        /// Evaluate the local coordinate system at a position along this curve.
        /// By convention, the x-axis of the local coordinate system will point along the
        /// curve and the z-axis will be orientated as closely as possible to global Z, unless
        /// the x-axis lies within a certain angular limit of z, in which case the global X axis
        /// will be used instead.
        /// </summary>
        /// <param name="tSpan">A normalised parameter defining a point along this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.</param>
        /// <param name="orientation">The orientation angle.  The rotation of the Y and Z axes of the coordinate 
        /// system around the X axis, relative to default reference orientation.</param>
        /// <returns></returns>
        public virtual CartesianCoordinateSystem LocalCoordinateSystem(double t, Angle orientation)
        {
            return LocalCoordinateSystem(t, orientation, Angle.FromDegrees(1));
        }

        /// <summary>
        /// Evaluate the local coordinate system at a position along this curve.
        /// By convention, the x-axis of the local coordinate system will point along the
        /// curve and the z-axis will be orientated as closely as possible to global Z, unless
        /// the x-axis lies within a certain angular limit of z, in which case the global X axis
        /// will be used instead.
        /// </summary>
        /// <param name="tSpan">A normalised parameter defining a point along this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.</param>
        /// <param name="orientation">The orientation angle.  The rotation of the Y and Z axes of the coordinate 
        /// system around the X axis, relative to default reference orientation.</param>
        /// <param name="zLimit">The angular limit within which if the local X and global Z approach each other,
        /// local Z will be aligned towards global X rather than global Z.  By default, this is 1 degree.</param>
        /// <returns></returns>
        public virtual CartesianCoordinateSystem LocalCoordinateSystem(double t, Angle orientation, Angle zLimit)
        {
            if (Closed) t = t % 1.0;

            // Calculate span
            double tSpan = t * SegmentCount;
            int span = (int)Math.Floor(tSpan);
            tSpan = tSpan % 1.0; //Position in span
            return LocalCoordinateSystem(span, tSpan, orientation, zLimit);
        }

        /// <summary>
        /// Evaluate the local coordinate system at a position along this curve.
        /// By convention, the x-axis of the local coordinate system will point along the
        /// curve and the z-axis will be orientated as closely as possible to global Z, unless
        /// the x-axis lies within one degree of z, in which case the global X axis
        /// will be used instead.
        /// </summary>
        /// <param name="tSpan">A normalised parameter defining a point along this span of this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.</param>
        /// <param name="orientation">The orientation angle.  The rotation of the Y and Z axes of the coordinate 
        /// system around the X axis, relative to default reference orientation.</param>
        /// <returns></returns>
        public CartesianCoordinateSystem LocalCoordinateSystem(int span, double tSpan, Angle orientation)
        {
            return LocalCoordinateSystem(span, tSpan, orientation, Angle.FromDegrees(1));
        }

        /// <summary>
        /// Evaluate the local coordinate system at a position along this curve.
        /// By convention, the x-axis of the local coordinate system will point along the
        /// curve and the z-axis will be orientated as closely as possible to global Z, unless
        /// the x-axis lies within a certain angular limit of z, in which case the global X axis
        /// will be used instead.
        /// </summary>
        /// <param name="span">The index of the span.  Valid range 0 to SegmentCount - 1</param>
        /// <param name="tSpan">A normalised parameter defining a point along this span of this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = span start, 1 = span end.</param>
        /// <param name="orientation">The orientation angle.  The rotation of the Y and Z axes of the coordinate 
        /// system around the X axis, relative to default reference orientation.</param>
        /// <param name="zLimit">The angular limit within which if the local X and global Z approach each other,
        /// local Z will be aligned towards global X rather than global Z.  By default, this is 1 degree.</param>
        /// <returns></returns>
        public virtual CartesianCoordinateSystem LocalCoordinateSystem(int span, double tSpan, Angle orientation, Angle zLimit)
        {
            Vector O = PointAt(span, tSpan);
            Vector T = TangentAt(span, tSpan);
            Vector alignZ = Vector.UnitZ;
            Angle angleBetween = T.AngleBetween(alignZ);
            if (angleBetween <= zLimit || angleBetween >= Angle.Straight - zLimit) alignZ = Vector.UnitX;
            Vector lY = alignZ.Cross(T);
            if (orientation != 0) lY = lY.Rotate(T, orientation);
            return new Plane(O, T, lY);
        }

        /// <summary>
        /// Get the vertex (if any) which defines the start of the specified segment.
        /// </summary>
        /// <param name="index">The segment index.  Valid range 0 to SegmentCount - 1</param>
        /// <returns>The start vertex of the given segment, if it exists.  Else null.</returns>
        public virtual Vertex SegmentStart(int index)
        {
            if (index < Vertices.Count) return Vertices[index];
            else return null;
        }

        /// <summary>
        /// Get the vertex (if any) which defines the end of the specified segment.
        /// </summary>
        /// <param name="index">The segment index.  Valid range 0 to SegmentCount - 1</param>
        /// <returns>The end vertex of the given segment, if it exists.  Else null.</returns>
        public virtual Vertex SegmentEnd(int index)
        {
            if (Closed && index == Vertices.Count - 1) return Start;
            else if (index < Vertices.Count - 1) return Vertices[index + 1];
            else return null;
        }

        /// <summary>
        /// Produce a set of points which represents a facetted version of this curve
        /// </summary>
        /// <param name="tolerance">The maximum angular deviation between the curve and the 
        /// facetted geometry.  If zero, the tolerance is taken as infinite and curves will
        /// not be facetted between kinks.</param>
        /// <returns></returns>
        public virtual Vector[] Facet(Angle tolerance)
        {
            Vector[] result = new Vector[Vertices.Count];
            for (int i = 0; i < Vertices.Count; i++)
            {
                result[i] = Vertices[i].Position;
            }
            return result;
        }

        /// <summary>
        /// Produce a set of coordinate systems along the curve which can be used to generate a solid representation
        /// of an element around this curve.
        /// </summary>
        /// <param name="tolerance">The maximum angular deviation between the curve and the 
        /// facetted geometry.  If zero, the tolerance is taken as infinite and curves will
        /// not be facetted between kinks.</param>
        /// <param name="orientation">The orientation angle.  The rotation of the Y and Z axes of the coordinate 
        /// system around the X axis, relative to default reference orientation.</param>
        /// <returns></returns>
        public IList<CartesianCoordinateSystem> FacetCSystems(Angle tolerance, Angle orientation)
        {
            return FacetCSystems(tolerance, orientation, Angle.FromDegrees(1));
        }

        /// <summary>
        /// Calculate the plane this curve lies on, if it is planar.
        /// Returns null if the curve is non-planar within the specified tolerance 
        /// or if its geometry is not sufficient to describe a plane (i.e. it is a line).
        /// </summary>
        /// <returns></returns>
        public Plane Plane(double tolerance = 0.0000001)
        {
            VertexCollection vertices = Vertices;
            return vertices.Plane(tolerance);
        }

        /// <summary>
        /// Produce a set of coordinate systems along the curve which can be used to generate a solid representation
        /// of an element around this curve.
        /// </summary>
        /// <param name="tolerance">The maximum angular deviation between the curve and the 
        /// facetted geometry.  If zero, the tolerance is taken as infinite and curves will
        /// not be facetted between kinks.</param>
        /// <param name="orientation">The orientation angle.  The rotation of the Y and Z axes of the coordinate 
        /// system around the X axis, relative to default reference orientation.</param>
        /// <param name="zLimit">The angular limit within which if the local X and global Z approach each other,
        /// local Z will be aligned towards global X rather than global Z.  By default, this is 1 degree.</param>
        /// <returns></returns>
        public virtual IList<CartesianCoordinateSystem> FacetCSystems(Angle tolerance, Angle orientation, Angle zLimit)
        {
            var result = new List<CartesianCoordinateSystem>();
            for (int i = 0; i < SegmentCount; i++)
            {
                result.Add(LocalCoordinateSystem(i, 0, orientation, zLimit));
                result.Add(LocalCoordinateSystem(i, 1, orientation, zLimit));
            }
            return result;
        }

        /// <summary>
        /// Calculate the length of the segment specified by the index
        /// </summary>
        /// <param name="index">The segment index.  Valid range 0 to SegmentCount - 1</param>
        /// <returns>The length of the segment, or Double.NaN if the segment isnot valid.</returns>
        public virtual double CalculateSegmentLength(int index)
        {
            Vertex end1 = SegmentStart(index);
            Vertex end2 = SegmentEnd(index);
            if (end1 != null && end2 != null)
            {
                return end1.Position.DistanceTo(end2.Position);
            }
            return double.NaN;
        }

        /// <summary>
        /// Calculate the length of this curve
        /// </summary>
        /// <returns>The calculated length of </returns>
        public virtual double CalculateLength()
        {
            double result = 0;
            for (int i = 0; i < SegmentCount; i++)
            {
                result += CalculateSegmentLength(i);
            }
            return result;
        }

        /// <summary>
        /// Calculate the area enclosed by this curve, were the start and end points to be 
        /// joined by a straight line segment.
        /// A plane may optionally be specified, otherwise by default the projected area on 
        /// the XY plane will be used.
        /// </summary>
        /// <param name="centroid">Output.  The centroid of the enclosed area.</param>
        /// <param name="onPlane">The plane to use to calculate the area.
        /// If not specified, the XY plane will be used.</param>
        /// <returns>The signed area enclosed by this curve on the specified plane,
        /// as a double.</returns>
        public virtual double CalculateEnclosedArea(out Vector centroid, Plane onPlane = null)
        {
            double result = 0;
            centroid = new Vector();
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector start = Vertices[i].Position;
                Vector end = i == Vertices.Count - 1 ? Vertices[0].Position : Vertices[i + 1].Position;
                if (onPlane != null)
                {
                    start = onPlane.GlobalToLocal(start);
                    end = onPlane.GlobalToLocal(end);
                }
                result += XYAreaUnder(start.X, start.Y, end.X, end.Y, ref centroid);
            }
            centroid /= result;
            return result;
        }

        /// <summary>
        /// Calculate the combined area enclosed by this curve, were the start and end points to be
        /// joined by a straight line segment, but excluding areas bounded by a specified set of void curves.
        /// A plane may optionally be specified, otherwise by default the projected area on the XY plane will
        /// be used.
        /// </summary>
        /// <param name="centroid">Output.  The centroid of the enclosed area.</param>
        /// <param name="voids">A collection of curves which represent the boundaries of void spaces
        /// within the perimeter of this curve.  Voids must be co-planar with and wholly within the
        /// bounds of this curve for the calculation to be accurate.  May be null.</param>
        /// <param name="onPlane">The plane to use to calculate the area.
        /// If not specified, the XY plane will be used.</param>
        /// <returns>The signed area enclosed by this curve on the specified plane, as a double.</returns>
        public double CalculateEnclosedArea(out Vector centroid, CurveCollection voids, Plane onPlane = null)
        {
            double result = CalculateEnclosedArea(out centroid, onPlane);
            if (voids != null)
            {
                centroid *= result;
                foreach (Curve voidCrv in voids)
                {
                    if (voidCrv != null && voidCrv.IsValid)
                    {
                        Vector voidCentroid;
                        double voidArea = result.Sign() * Math.Abs(voidCrv.CalculateEnclosedArea(out voidCentroid, onPlane));
                        centroid -= voidCentroid * voidArea;
                        result -= voidArea;
                    }
                }
                centroid /= result;
            }
            return result;
        }

        /// <summary>
        /// Calculate the signed area 'under' the line segment between the two specified points
        /// on the XY plane - i.e. the area between the line and the X-axis.
        /// </summary>
        /// <param name="x0">The x-coordinate of the start of the line</param>
        /// <param name="y0">The y-coordinate of the start of the line</param>
        /// <param name="x1">The x-coordinate of the end of the line</param>
        /// <param name="y1">The y-coordinate of the end of the line</param>
        /// <param name="centroid">The cumulative centroid.  The centroid of the block under
        /// the line will be added to the value passed in here</param>
        /// <returns>The signed area as a double.</returns>
        protected static double XYAreaUnder(double x0, double y0, double x1, double y1, ref Vector centroid)
        {
            //Area is calculated as a rectangle and a triangle combined:
            double areaRectangle = y0 * (x1 - x0);
            double areaTri = (y1 - y0) * (x1 - x0) * 0.5;
            centroid = centroid.Add(
                (x1 + x0) * 0.5 * areaRectangle + (x0 + (x1 - x0) * 2 / 3) * areaTri,
                y0 * 0.5 * areaRectangle + (y0 + (y1 - y0) / 3) * areaTri);
            return areaRectangle + areaTri;
        }

        /// <summary>
        /// Calculate the second moment of area enclosed by this curve on a the XY plane about the
        /// X-axis, were the start and end points joined by a straight line segment.
        /// A coordinate system may be specified, otherwise by default the global XY plane and
        /// X-axis will be used.
        /// </summary>
        /// <param name="onPlane">The coordinate system in which the second moment of area is
        /// to be calculated.  The second moment of area will be calculated on the XY plane and
        /// about the X-axis of this system.</param>
        /// <returns>The signed second moment of area enclosed by this curve, as a
        /// double.</returns>
        public virtual double CalculateEnclosedIxx(Plane onPlane = null)
        {
            double result = 0;
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector start = Vertices[i].Position;
                Vector end = i == Vertices.Count - 1 ? Vertices[0].Position : Vertices[i + 1].Position;
                if (onPlane != null)
                {
                    start = onPlane.GlobalToLocal(start);
                    end = onPlane.GlobalToLocal(end);
                }
                result += IxxUnder(start.X, start.Y, end.X, end.Y);
            }
            return result;
        }

        /// <summary>
        /// Calculate the second moment of area enclosed by this curve on a the XY plane about the
        /// X-axis, were the start and end points joined by a straight line segment, excluding
        /// the void regions specififed as boundaries.
        /// A coordinate system may be specified, otherwise by default the global XY plane and
        /// X-axis will be used.
        /// </summary>
        /// <param name="voids">A collection of curves which represent the boundaries of void spaces
        /// within the perimeter of this curve.  Voids must be co-planar with and wholly within the
        /// bounds of this curve for the calculation to be accurate.  May be null.</param>
        /// <param name="onPlane">The coordinate system in which the second moment of area is
        /// to be calculated.  The second moment of area will be calculated on the XY plane and
        /// about the X-axis of this system.</param>
        /// <returns>The signed second moment of area enclosed by this curve, as a
        /// double.</returns>
        public double CalculateEnclosedIxx(CurveCollection voids, Plane onPlane = null)
        {
            double result = CalculateEnclosedIxx(onPlane);
            if (voids != null)
            {
                foreach (Curve voidCrv in voids)
                {
                    double voidArea = Math.Abs(voidCrv.CalculateEnclosedIxx(onPlane)) * result.Sign();
                    result -= voidArea;
                }
            }
            return result;
        }

        /// <summary>
        /// Calculate the signed second moment of area 'under' the line segment between the
        /// two specified coordinates on the XY plane about the X-axis
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns>The signed Ixx as a double</returns>
        protected static double IxxUnder(double x0, double y0, double x1, double y1)
        {
            double aR = y0 * (x1 - x0); //Area of rectangle
            double IxxR = ((x1 - x0) * y0*y0*y0 ) / 12; //(bh^3)/12
            double yCR = y0 / 2; //y of centroid of rectangle
            double aT = (y1 - y0) * (x1 - x0) * 0.5; //Area of triangle
            double IxxT = ((x1 - x0) * Math.Pow((y1 - y0),3)) / 36; //(bh^3)/36
            double yCT = (y0 + (y1 - y0) / 3); //y of centroid triangle
            return IxxR + aR * yCR * yCR + IxxT + aT * yCT * yCT;
        }

        public override string ToString()
        {
            return "Curve";
        }

        #endregion
    }
}
