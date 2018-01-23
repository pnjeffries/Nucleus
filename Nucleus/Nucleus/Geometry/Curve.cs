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

using Nucleus.Extensions;
using Nucleus.Maths;
using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
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
        public virtual Vector StartPoint
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
        public virtual Vector EndPoint
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
        /// Get the curve parameter at the specified vertex
        /// </summary>
        /// <param name="vertex">The vertex.  Must be a defining vertex of this curve.</param>
        /// <returns>A curve parameter</returns>
        public virtual double ParameterAt(Vertex vertex)
        {
            var verts = Vertices;
            if (verts.Count > 0 && verts.Contains(vertex.GUID))
                return verts.IndexOf(vertex) / (double)(verts.Count - 1);
            else return 0;
        }

        /// <summary>
        /// Get the curve parameter at the specified length along this curve.
        /// If the returned parameter falls outside the range 0-1, the specified
        /// length does not fall within the domain of the curve.
        /// </summary>
        /// <param name="length">The distance along the curve from the start of the curve to the point in question</param>
        /// <returns>A curve parameter</returns>
        public virtual double ParameterAt(double length)
        {
            double l0 = 0;
            for (int i = 0; i < SegmentCount; i++)
            {
                double lS = CalculateSegmentLength(i);
                double l1 = l0 + lS;
                if (l1 > length) return ((double)i) / SegmentCount + ((length - l0) / lS) / SegmentCount;
                l0 = l1;
            }
            return length/l0;
        }

        /// <summary>
        /// Calculate the length along the curve of the specified parameter
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual double LengthAt(double t)
        {
            double result = 0;
            if (Closed && (t < 0.0 || t > 1.0))
            {
                double nt = t % 1.0;
                double length = Length;
                result += length * (t - nt);
                t = nt;
            }
            double tS = 1.0 / SegmentCount; //Parameter-size per segment
            for (int i = 0; i < SegmentCount; i++)
            {
                double lS = CalculateSegmentLength(i);
                if ((i + 1.0) * tS > t)
                {
                    // Parameter is inside segment
                    result += ((t - (i * tS)) / tS) * lS;
                    return result;
                }
                else // Keep going...
                    result += lS;
            }
            return result;
        }

        /// <summary>
        /// Calculate the length of a section of this curve defined
        /// by a parameter interval
        /// </summary>
        /// <param name="t">The curve parameter interval the length of which is to be measured.</param>
        /// <returns></returns>
        public double LengthOf(Interval t)
        {
            return LengthAt(t.End) - LengthAt(t.Start);
        }

        /// <summary>
        /// Get a curve parameter subdomain specified by the mid-point parameter
        /// of the subdomain and the length of the subdomain along the curve.
        /// </summary>
        /// <param name="tMid">The curve parameter at the mid-point of the specified subdomain</param>
        /// <param name="length">The length of the subdomain (i.e. the domain will start and end
        /// half of this distance from tMid) </param>
        /// <returns></returns>
        public Interval SubdomainByCentre(double tMid, double length)
        {
            double midLength = LengthAt(tMid);
            double startLength = midLength - length / 2.0;
            double endLength = midLength + length / 2.0;
            var result = new Interval(ParameterAt(startLength), ParameterAt(endLength));
            if (!Closed) return result.Overlap(new Interval(0, 1));
            else return result;
        }

        /// <summary>
        /// Evaluate the tangent unit vector at the specified vertex of this
        /// curve.
        /// </summary>
        /// <param name="vertex">The vertex.  
        /// Must be a defining vertex of this curve.</param>
        /// <returns></returns>
        public Vector TangentAt(Vertex vertex)
        {
            double t = ParameterAt(vertex);
            return TangentAt(t);
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
            if (t >= 1.0)
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
        /// <param name="t">A normalised parameter defining a point along this curve.
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
            if (span >= SegmentCount)
            {
                span = SegmentCount - 1;
                tSpan = 1.0;
            }
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
        /// Find the closest point on this curve to a test point, expressed as a
        /// parameter value from 0-1.  This may be a position on the curve or it may
        /// be the start (0) or end (1) of the curve depending on the relative location
        /// of the test point.
        /// </summary>
        /// <param name="toPoint">The test point to find the closest point to</param>
        /// <returns></returns>
        public virtual double ClosestParameter(Vector toPoint)
        {
            double result = double.NaN;
            double minDistSqd = 0;
            for (int i = 0; i < SegmentCount; i++)
            {
                Vector start = SegmentStart(i).Position;
                Vector end = SegmentEnd(i).Position;
                double t = Line.ClosestParameter(start, end, toPoint);
                Vector v = start.Interpolate(end, t);
                double distSqd = v.DistanceToSquared(toPoint);
                if (double.IsNaN(result) || distSqd < minDistSqd)
                {
                    minDistSqd = distSqd;
                    result = ((double)i) / SegmentCount + t * 1.0 / SegmentCount;
                }
            }
            return result;
        }


        /// <summary>
        /// Find the closest point on this curve to a test point, expressed as a
        /// vector in 3d space.  This may be a position on the curve or it may
        /// be the start or end of the curve depending on the relative location
        /// of the test point.
        /// </summary>
        /// <param name="toPoint">The test point to find the closest point to</param>
        /// <returns></returns>
        public virtual Vector ClosestPoint(Vector toPoint)
        {
            return PointAt(ClosestParameter(toPoint));
        }

        /// <summary>
        /// Calculate the shortest distance squared from this curve to the specified point
        /// </summary>
        /// <param name="point">The test point to find the distance to</param>
        /// <returns></returns>
        public double DistanceToSquared(Vector point)
        {
            return ClosestPoint(point).DistanceToSquared(point);
        }

        /// <summary>
        /// Calculate the shortest distance from this curve to the specified point
        /// </summary>
        /// <param name="point">The test point to find the distance to</param>
        /// <returns></returns>
        public double DistanceTo(Vector point)
        {
            return ClosestPoint(point).DistanceTo(point);
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
        /// Divide up this curve into the specified number of equal-length
        /// segments and return the positions between those segments.
        /// </summary>
        /// <param name="divisions"></param>
        /// <returns></returns>
        public virtual Vector[] Divide(int divisions)
        {
            int vCount = divisions;
            if (!Closed) vCount += 1; //Open curve
            Vector[] result = new Vector[vCount];

            double[] lengths = new double[SegmentCount];
            double length = 0;
            for (int i = 0; i < SegmentCount; i++)
            {
                double segLength = CalculateSegmentLength(i);
                lengths[i] = segLength;
                length += segLength;
            }
            double divLength = length / divisions;

            double x = 0;
            double segStartX = 0;
            int j = 0;

            result[0] = StartPoint;
            for (int n = 1; n < vCount; n++)
            {
                x += divLength;
                while (segStartX + lengths[j] < x && j < SegmentCount - 1)
                {
                    //Move to next segment
                    j++;
                    segStartX += lengths[j];
                }
                double t = (x - segStartX) / lengths[j];
                result[n] = PointAt(j, t);
            }

            return result;
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
                result += MathsHelper.AreaUnder(start.X, start.Y, end.X, end.Y, ref centroid);
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
                result += MathsHelper.IxxUnder(start.X, start.Y, end.X, end.Y);
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
        /// Calculate the second moment of area enclosed by this curve on a the XY plane about the
        /// Y-axis, were the start and end points joined by a straight line segment, excluding
        /// the void regions specififed as boundaries.
        /// </summary>
        /// <param name="voids">Optional.  A collection of curves which represent the boundaries of void spaces
        /// within the perimeter of this curve.  Voids must be co-planar with and wholly within the
        /// bounds of this curve for the calculation to be accurate.  May be null.</param>
        /// <returns>The signed second moment of area enclosed by this curve, as a
        /// double.</returns>
        public double CalculateEnclosedIyy(CurveCollection voids = null)
        {
            Plane onPlane = Geometry.Plane.GlobalYX;
            return CalculateEnclosedIxx(voids, onPlane);
        }

        /// <summary>
        /// Check whether the specified point lies within the area enclosed by this curve
        /// on the XY plane
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        /// <remarks>TODO: Implement more refined checks on arcs</remarks>
        public virtual bool EnclosesXY(Vector point)
        {
            return Vertices.PolygonContainmentXY(point);
        }

        /// <summary>
        /// Offset this curve on the XY plane.
        /// </summary>
        /// <param name="distances">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <returns></returns>
        public virtual Curve Offset(double distance)
        {
            return Offset(new double[] { distance });
        }

        /// <summary>
        /// Offset this curve on the XY plane by varying distances for
        /// each span.
        /// </summary>
        /// <param name="distances">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <returns></returns>
        public abstract Curve Offset(IList<double> distances);

        /// <summary>
        /// Is this curve clockwise in the XY plane?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsClockwiseXY()
        {
            return Vertices.ClockwiseTestSum() > 0;
        }

        /// <summary>
        /// 'Explode' this curve into a collection of its constituent
        /// segments as individual curves.
        /// </summary>
        /// <param name="recursive">If true (default), any sub-curves of
        /// this curve which themselves have sub-curves will also be exploded
        /// and added to the collection.</param>
        /// <returns></returns>
        public virtual CurveCollection Explode(bool recursive = true)
        {
            var result = new CurveCollection();
            result.Add(this);
            return result;
        }

        /// <summary>
        /// On entering either the start or end vertex of this curve,
        /// the other will be returned.
        /// </summary>
        /// <param name="curveEnd"></param>
        /// <returns></returns>
        public Vertex GetOtherEnd(Vertex curveEnd)
        {
            if (curveEnd == Start) return End;
            return Start;
        }

        // TODO: Reinstate
        // Temporarily removed as the need to deal with looping &
        // reversing made extraction a lot more complicated than first thought!

        /*
        /// <summary>
        /// Extract a portion of this curve as a new curve
        /// </summary>
        /// <param name="subDomain">The subdomain of this curve to
        /// be extracted as a new curve</param>
        /// <returns></returns>
        public abstract Curve Extract(Interval subDomain);
        */

        public override string ToString()
        {
            return "Curve";
        }

        /// <summary>
        /// Reverse the direction of this curve
        /// </summary>
        public virtual void Reverse()
        {
            Vertices.Reverse();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Join two curves to create a polycurve.  The start of the endCurve should be coincident
        /// with the end of the startCurve.
        /// If one or other (or both) of the curves is already a polycurve, the other curve(s) will
        /// be added to that PolyCurve rather than creating a new one.
        /// </summary>
        /// <param name="startCurve"></param>
        /// <param name="endCurve"></param>
        /// <returns></returns>
        public static Curve Join(Curve startCurve, Curve endCurve)
        {
            if (startCurve == null) return endCurve;
            else if (endCurve == null) return startCurve;
            else if (startCurve is PolyCurve)
            {
                var pCrv = (PolyCurve)startCurve;
                if (endCurve is PolyCurve)
                {
                    foreach (var subCrv in ((PolyCurve)endCurve).SubCurves)
                        pCrv.Add(subCrv);
                }
                else pCrv.Add(endCurve);
                return pCrv;
            }
            else if (endCurve is PolyCurve)
            {
                var pCrv = (PolyCurve)endCurve;
                pCrv.SubCurves.Insert(0, startCurve);
                return pCrv;
            }
            else
            {
                return new PolyCurve(new Curve[] { startCurve, endCurve }, startCurve.Attributes);
            }
        }

        /// <summary>
        /// Connect the specified curve end vertex to the specified point with a straight line
        /// segment to be joined with the original curve.
        /// </summary>
        /// <param name="curveEnd"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Curve Connect(Vertex curveEnd, Vector point)
        {
            if (curveEnd.Owner != null && curveEnd.Owner is Curve)
            {
                if (curveEnd.IsStart)
                {
                    return Join(new Line(point, curveEnd.Position), (Curve)curveEnd.Owner);
                }
                else if (curveEnd.IsEnd)
                {
                    return Join((Curve)curveEnd.Owner, new Line(curveEnd.Position, point));
                }
            }
            return null;
        }

        /// <summary>
        /// Extend (or trim) the end of a curve to lie at the intersection between the current
        /// curve and a line on the XY plane.
        /// </summary>
        /// <param name="curveEnd">The curve end to modify</param>
        /// <param name="lineOrigin">A point which lies on the line</param>
        /// <param name="lineDir">The direction vector of the line</param>
        /// <returns></returns>
        public static bool ExtendToLineXY(Vertex curveEnd, Vector lineOrigin, Vector lineDir)
        {
            Curve curve = curveEnd.Owner as Curve;
            if (curve != null)
            {
                if (curve is Arc)
                {
                    //TODO
                }
                else
                {
                    Vector tan = curve.TangentAt(curveEnd);
                    Vector intPt = Intersect.LineLineXY(curveEnd.Position, tan, lineOrigin, lineDir);
                    if (intPt.IsValid())
                    {
                        curveEnd.Position = curveEnd.Position.WithXY(intPt.X, intPt.Y);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Match the end positions of two curves by repositioning vertices
        /// to either extend or trim the curve ends to a common intersection
        /// point, if possible.
        /// The input vertices must be the first or last vertices of curves, else
        /// the matching will fail.
        /// </summary>
        /// <param name="crvEnd0">The end vertex of the first curve to be modified.</param>
        /// <param name="crvEnd1">The end vertex of the second curve to be modified.</param>
        /// <param name="detectMismatches">Optional.  If true, additional checking will be
        /// performed and only matches where both ends are extended or both ends are trimmed
        /// will be permitted.</param>
        /// <returns>True if the curve ends could be successfully matched, else false.</returns>
        public static bool MatchEnds(Vertex crvEnd0, Vertex crvEnd1, bool detectMismatches = false)
        {
            Curve crv0 = crvEnd0.Owner as Curve;
            Curve crv1 = crvEnd1.Owner as Curve;
            if (crv0 != null && crv1 != null)
            {
                if (crv0 is Arc)
                {
                    if (crv1 is Arc)
                    {
                        //  TODO!
                    }
                    else
                    {
                        return MatchEnds(crvEnd1, crv1.TangentAt(crvEnd1), crvEnd0, (Arc)crv0);
                    }
                }
                else
                {
                    if (crv1 is Arc)
                    {
                        return MatchEnds(crvEnd0, crv0.TangentAt(crvEnd0), crvEnd1, (Arc)crv1);
                    }
                    else
                    {
                        return MatchEnds(crvEnd0, crv0.TangentAt(crvEnd0), crvEnd1, crv1.TangentAt(crvEnd1), detectMismatches);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Match curve ends by intersecting a straight line from the specified end vertices
        /// with the specified tangents
        /// </summary>
        /// <param name="crvEnd0">The curve end vertex of the first curve</param>
        /// <param name="tan0">The end tangency of the first curve</param>
        /// <param name="crvEnd1">The curve end vertex of the second curve</param>
        /// <param name="tan1">The end tangency of the second curve</param>
        /// <returns></returns>
        private static bool MatchEnds(Vertex crvEnd0, Vector tan0, Vertex crvEnd1, Vector tan1, bool detectMismatches = false)
        {
            if (tan0.Z.IsTiny() && tan1.Z.IsTiny()) // 2D intersection:
            {
                double t0 = 0;
                double t1 = 0;
                Vector intPt = Intersect.LineLineXY(crvEnd0.Position, tan0, crvEnd1.Position, tan1, ref t0, ref t1);
                if (intPt.IsValid())
                {
                    if (detectMismatches)
                    {
                        bool ext0 = IsExtension(t0, crvEnd0);
                        bool ext1 = IsExtension(t1, crvEnd1);
                        if (ext0 != ext1)
                        {
                            // May be a mis-match!
                            Curve crv0 = crvEnd0.Owner as Curve;
                            Curve crv1 = crvEnd1.Owner as Curve;
                            if (crv0 == null || crv1 == null ||
                                crv0.GetOtherEnd(crvEnd0).Position.IsCloser(crvEnd0.Position, intPt) ||
                                crv1.GetOtherEnd(crvEnd1).Position.IsCloser(crvEnd1.Position, intPt))
                                return false;
                        }
                    }
                    crvEnd0.Position = crvEnd0.Position.WithXY(intPt.X, intPt.Y);
                    crvEnd1.Position = crvEnd1.Position.WithXY(intPt.X, intPt.Y);
                    return true;
                }
            }
            else
            {
                // 3D intersection: 
                Vector intPt0 = Axis.ClosestPoint(crvEnd0.Position, tan0, crvEnd1.Position, tan1);
                if (intPt0.IsValid())
                {
                    Vector intPt1 = Axis.ClosestPoint(crvEnd1.Position, tan1, intPt0);
                    crvEnd0.Position = intPt0;
                    crvEnd1.Position = intPt1;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Match curve ends by intersecting a straight line and an arc as closely as possible
        /// </summary>
        /// <param name="crvEnd0"></param>
        /// <param name="tan0"></param>
        /// <param name="crvEnd1"></param>
        /// <param name="arc1"></param>
        /// <returns></returns>
        private static bool MatchEnds(Vertex crvEnd0, Vector tan0, Vertex crvEnd1, Arc arc1)
        {
            // Solve intersection on plane of arc:
            Plane plane = arc1.Plane();
            Vector linePt = plane.GlobalToLocal(crvEnd0.Position).WithZ(0);
            Vector lineDir = plane.GlobalToLocal(tan0, true).WithZ(0);
            // TODO: Deal with special case where line is perpendicular to plane? (i.e. lineDir is (0,0,0))
            double[] intersects = Intersect.LineCircleXY(linePt, lineDir, plane.GlobalToLocal(arc1.Circle.Origin), arc1.Circle.Radius);
            // Translate to intersection points in global space:
            Vector[] intPts = new Vector[intersects.Length];
            for (int i = 0; i < intersects.Length; i++)
                intPts[i] = plane.LocalToGlobal(linePt + lineDir * intersects[i]);
            // Select point closest to current arc end:
            Vector intPt = intPts.FindClosest(crvEnd1.Position);
            if (intPt.IsValid())
            {
                crvEnd1.Position = intPt;
                crvEnd0.Position = Axis.ClosestPoint(crvEnd0.Position, tan0, intPt);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Match curve ends by intersecting two arcs as closely as possible.
        /// The arcs must be coplanar for this to function correctly.
        /// </summary>
        /// <param name="crvEnd0"></param>
        /// <param name="arc0"></param>
        /// <param name="crvEnd1"></param>
        /// <param name="arc1"></param>
        /// <returns></returns>
        public static bool MatchEnds(Vertex crvEnd0, Arc arc0, Vertex crvEnd1, Arc arc1)
        {
            // Find intersections on the plane of arc0:
            Plane plane = arc0.Plane();
            Vector pt0 = plane.GlobalToLocal(arc0.Circle.Origin);
            Vector pt1 = plane.GlobalToLocal(arc1.Circle.Origin);
            Vector[] intPts = Intersect.CircleCircleXY(pt0, arc0.Circle.Radius, pt1, arc1.Circle.Radius);
            intPts = intPts.LocalToGlobal(plane);

            // Determine most appropriate intersection point based on proximity:
            // TODO: Instead check which requires minimum change in arc length?
            Vector intPt = intPts.FindClosest(new Vector[] { crvEnd0.Position, crvEnd1.Position });
            if (intPt.IsValid())
            {
                crvEnd0.Position = intPt;
                crvEnd1.Position = arc1.Plane().Project(intPt);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Is the specified parameter change at the specified end vertex an extension
        /// or a contraction of the curve?
        /// </summary>
        /// <param name="t"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private static bool IsExtension(double deltaT, Vertex v)
        {
            return (v.IsEnd && deltaT > 0) || (v.IsStart && deltaT < 0);
        }

        #endregion
    }

}
