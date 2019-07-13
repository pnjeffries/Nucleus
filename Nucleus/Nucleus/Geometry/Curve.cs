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
using Nucleus.Extensions;
using Nucleus.Maths;
using Nucleus.Model;
using Nucleus.Rendering;
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
    /// this curve as a polyline of straight segments between vertices.
    /// While most curve functionality is implemented on the relevent subclass, any new curve types
    /// should also take care to also add support to the relevent functions within the Intersect
    /// helper class and any relevant conversion classes in dependent libraries.</remarks>
    [Serializable]
    public abstract class Curve: VertexGeometry, IFastDuplicatable
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
            double tSpan;
            int span = SpanAt(t, out tSpan);
            return PointAt(span, tSpan);
        }

        /// <summary>
        /// Returns the span index at the specified parameter along this curve
        /// </summary>
        /// <param name="t">A normalised parameter defining a point along this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = curve start, 1 = curve end.
        /// For open curves, parameters outside the range 0-1 will be invalid.
        /// For closed curves, parameters outside this range will 'wrap'.</param>
        /// <param name="tSpan">Output.  The normalised parameter along the span.</param>
        /// <returns></returns>
        public int SpanAt(double t, out double tSpan)
        {
            if (Closed) t = t % 1.0;

            //Calculate span
            tSpan = t * SegmentCount;
            int span = (int)Math.Floor(tSpan);
            tSpan = tSpan % 1.0; //Position in span
            // Adjust end-points to still lie in their span
            if (tSpan == 0 && span > 0)
            {
                span--;
                tSpan = 1;
            }

            return span;
        }

        /// <summary>
        /// Evaluate a set of points along this curve defined by a set of parameters
        /// </summary>
        /// <param name="t">A set of normalised parameters defining points along this curve.
        /// Note that parameter-space is not necessarily uniform and does not equate to a normalised length.
        /// 0 = curve start, 1 = curve end.
        /// For open curves, parameters outside the range 0-1 will be invalid.
        /// For closed curves, parameters outside this range will 'wrap'.</param>
        /// <returns>The vector coordinates describing a point on the curve at the specified parameter,
        /// if the curve definition and parameter are valid.  Else, an unset vector.</returns>
        /// <returns></returns>
        public Vector[] PointsAt(IList<double> t)
        {
            var result = new Vector[t.Count];
            for (int i = 0; i < t.Count; i++)
            {
                result[i] = PointAt(t[i]);
            }
            return result;
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
        /// Evaluate a point on this curve a specified distance from the start or end.
        /// </summary>
        /// <param name="length">The length along the curve</param>
        /// <param name="fromEnd">If true, the length will be measured from the end
        /// of the curve.  If false (default) it will be measured from the start.</param>
        /// <returns></returns>
        public virtual Vector PointAtLength(double length, bool fromEnd = false)
        {
            if (fromEnd) length = Length - length;
            double t = ParameterAt(length);
            return PointAt(t);
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
                return verts.IndexOf(vertex) / (double)(SegmentCount);
            else return 0;
        }

        /// <summary>
        /// Returns the parameter at the specified position on the specified span
        /// </summary>
        /// <param name="span">The span index</param>
        /// <param name="tSpan">The normalised parameter along the specified span, where
        /// 0 = span start and 1 = span end.</param>
        /// <returns></returns>
        public double ParameterAt(int span, double tSpan)
        {
            int segCount = SegmentCount;
            double spanDom = 1.0 / segCount;
            if (Closed) span = span % segCount;
            return (span + tSpan) * spanDom;
        }

        /// <summary>
        /// Get the curve parameter at the vertex at the specified index
        /// </summary>
        /// <param name="vertexIndex"></param>
        /// <returns></returns>
        public virtual double ParameterAtVertexIndex(int vertexIndex)
        {
            return vertexIndex / (double)(SegmentCount);
        }

        /// <summary>
        /// Get the curve parameter at the specified length along this curve.
        /// If the returned parameter falls outside the range 0-1, the specified
        /// length does not fall within the domain of the curve.
        /// </summary>
        /// <param name="length">The distance along the curve from the start of the curve 
        /// to the point in question</param>
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
        /// Returns a set of parameters evenly spaced along the curve with a specified spacing
        /// </summary>
        /// <param name="spacing">The distance between points</param>
        /// <param name="firstOffset">The distance of the first point from the start of the curve.</param>
        /// <param name="countLimit">Optional.  The maximum number of parameters to be produced.
        /// If set to 0 or below, the number of possible returned parameters is unlimited.</param>
        /// <returns></returns>
        public virtual IList<double> ParametersAtSpacing(double spacing, double firstOffset = 0, int countLimit = 0)
        {
            var result = new List<double>();
            if (spacing > 0.00001)
            {
                double l0 = 0;
                double next = firstOffset;
                for (int i = 0; i < SegmentCount; i++)
                {
                    double lS = CalculateSegmentLength(i);
                    double l1 = l0 + lS;
                    while (l1 >= next)
                    {
                        double t = ((double)i) / SegmentCount + ((next - l0) / lS) / SegmentCount;
                        result.Add(t);
                        next += spacing;
                        // Cap at maxCount:
                        if (countLimit > 0 && result.Count >= countLimit) return result;
                    }
                    l0 = l1;
                }
            }
            else
            {
                // Spacing too small - just give the first point
                result.Add(ParameterAt(firstOffset));
            }
            return result;
        }

        /// <summary>
        /// Returns a set of parameters at equidistant points along this curve produced by dividing the
        /// curve into equal length segments.
        /// </summary>
        /// <param name="divisions">The number of equal segments to divide the curve into.</param>
        /// <param name="proportionOfDivision">The position of the point within each segment, as a
        /// proportion of the segment length.
        /// Between 0-1, where for e.g. 0 is at the start of each segment, 0.5 is in the middle
        /// and so on.</param>
        /// <returns></returns>
        public virtual IList<double> ParametersAtDivisions(int divisions, double proportionOfDivision = 0)
        {
            double length = Length;
            double segLength = length / divisions;
            var result = ParametersAtSpacing(segLength, segLength * proportionOfDivision);
            if (proportionOfDivision % 1 == 0 && proportionOfDivision <= divisions
                && !Closed && (1.0 - result.Last()).Abs() > 0.1 / divisions)
                result.Add(1.0); // Add a last point to the end, if floating point errors mean it is missing
            return result;
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
            if (t.IsDecreasing && Closed)
                return (Length - LengthAt(t.End) + LengthAt(t.Start));
            else return LengthAt(t.End) - LengthAt(t.Start);
        }

        /// <summary>
        /// Filter out from the specified list of domain intervals on this curve any which have a length
        /// lower than the specified limit.
        /// </summary>
        /// <param name="intervals"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IList<Interval> FilterBelowLength(IList<Interval> intervals, double limit)
        {
            var result = new List<Interval>();
            foreach (var i in intervals)
            {
                if (LengthOf(i) >= limit) result.Add(i);
            }
            return result;
        }

        /// <summary>
        /// Break down the supplied interval into subdomains for each span contained within
        /// that parameter space on this curve
        /// </summary>
        /// <param name="within"></param>
        /// <returns></returns>
        public IList<Interval> SpanDomains(Interval within)
        {
            int spanCount = SegmentCount;
            var result = new List<Interval>(spanCount);

            double tSpanStart;
            int startSpan = SpanAt(within.Start, out tSpanStart);
            double tSpanEnd;
            int endSpan = SpanAt(within.End, out tSpanEnd);
            if (tSpanEnd == 0)
            {
                tSpanEnd = 1;
                endSpan -= 1;
                if (endSpan < 0) endSpan += spanCount;
            }

            if (within.IsDecreasing && Closed)
            {
                endSpan += spanCount;
            }

            for (int span = startSpan; span <= endSpan; span++)
            {
                double tSpan0 = 0;
                if (span == startSpan) tSpan0 = tSpanStart;
                double tSpan1 = 1;
                if (span == endSpan) tSpan1 = tSpanEnd;

                int iSpan = span % spanCount;
                double t0 = ParameterAt(iSpan, tSpan0);
                double t1 = ParameterAt(iSpan, tSpan1);

                result.Add(new Interval(t0, t1));
            }

            return result;
        }

        /// <summary>
        /// Get a curve parameter subdomain specified by the mid-point parameter
        /// of the subdomain and the length of the subdomain along the curve.
        /// </summary>
        /// <param name="tMid">The curve parameter at the mid-point of the specified subdomain</param>
        /// <param name="length">The length of the subdomain (i.e. the domain will start and end
        /// half of this distance from tMid) </param>
        /// <param name="shuntToFit">If true, this curve is open and tMid lies within length/2 of one
        /// end, the subdomain will be 'shunted' along so that as much as possible of the specified
        /// length can still be obtained, albeit not centred on tMid anymore.</param>
        /// <returns></returns>
        public virtual Interval SubdomainByCentre(double tMid, double length, bool shuntToFit = false)
        {
            double midLength = LengthAt(tMid);
            double startLength = midLength - length / 2.0;
            double endLength = midLength + length / 2.0;

            if (Closed)
            {
                double crvLength = Length;
                if (length > crvLength) return new Interval(0, 1);
                startLength = startLength.WrapTo(new Interval(0,crvLength));
                endLength = endLength.WrapTo(new Interval(0, crvLength));
            }
            else if (shuntToFit)
            {
                // Check if the domain falls off one end or the other and if so
                // 'shunt' it's full length back into the curve domain
                if (startLength < 0)
                {
                    startLength = 0;
                    endLength = length;
                }
                else
                {
                    double crvLength = Length;
                    if (endLength > crvLength)
                    {
                        startLength = crvLength - length;
                        endLength = length;
                    }
                }
            }
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
        /// Evaluate the tangent unit vector at the specified node, which is
        /// attached to a vertex of this curve.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Vector TangentAt(Node node)
        {
            var verts = Vertices;
            foreach (Vertex v in node.Vertices)
            {
                if (verts.Contains(v.GUID))
                {
                    return TangentAt(v);
                }
            }
            return Vector.Unset;
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
        /// Compare this curve to another by span tangencies.
        /// Returns true only if the two curves have the same number of spans
        /// and the curve tangencies at the mid-point of each span are facing
        /// within 90 degrees of one another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CheckSegmentTangents(Curve other)
        {
            if (SegmentCount != other.SegmentCount) return false;
            for (int i = 0; i < SegmentCount; i++)
            {
                if (TangentAt(i, 0.5) * other.TangentAt(i, 0.5) < 0) return false;
            }
            return true;
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
        /// <param name="t">A normalised parameter defining a point along this curve.
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
        /// <param name="span">The index of the span to evaluate.</param>
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
        /// Find the region of this curve (expressed as a parameter interval) which
        /// overlaps with another curve, determined by projecting the start and end 
        /// points of said curve onto this one.
        /// </summary>
        /// <param name="other">Another curve.  For the most meaningful results,
        /// should run roughly parallel to this one.</param>
        /// <returns></returns>
        public Interval OverlapWith(Curve other)
        {
            //TODO: More sophisticated method for closed curves?
            return new Interval(
                ClosestParameter(other.StartPoint),
                ClosestParameter(other.EndPoint));
        }

        /// <summary>
        /// Find the region of this curve (expressed as a parameter interval) which
        /// overlaps with a region of another curve, determined by projecting the
        /// start and end of the specified domain on the other curve onto this one.
        /// </summary>
        /// <param name="other">Another curve.  For the most meaningful results,
        /// should run roughly parallel to this one.</param>
        /// <param name="domainOnOther">The region of the other curve to overlap
        /// with this curve.</param>
        /// <returns></returns>
        public Interval OverlapWith(Curve other, Interval domainOnOther)
        {
            return new Interval(
                ClosestParameter(other.PointAt(domainOnOther.Start)),
                ClosestParameter(other.PointAt(domainOnOther.End)));
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
            if (index < Vertices.Count && index >= 0)  return Vertices[index];
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
        /// Calculate the (signed) area enclosed by this curve, were the start and end points to be 
        /// joined by a straight line segment.
        /// A plane may optionally be specified, otherwise by default the projected area on 
        /// the XY plane will be used.
        /// </summary>
        /// <param name="onPlane">The plane to use to calculate the area.
        /// If not specified, the XY plane will be used.</param>
        /// <returns>The signed area enclosed by this curve on the specified plane,
        /// as a double.</returns>
        public double CalculateEnclosedArea(Plane onPlane = null)
        {
            Vector c;
            return CalculateEnclosedArea(out c, onPlane);
        }

        /// <summary>
        /// Calculate the (signed) area enclosed by this curve, were the start and end points to be 
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
        /// Calculate the combined (signed) area enclosed by this curve, were the start and end points to be
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
            return Closed && Vertices.PolygonContainmentXY(point);
        }
        
        /// <summary>
        /// Offset this curve on the XY plane.
        /// </summary>
        /// <param name="distance">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <param name="tidy">If true, automatic post-processing operations to 'tidy'
        /// the offset curve by removing collapsed regions will be performed.</param>
        /// <returns></returns>
        public virtual Curve Offset(double distance, bool tidy = true, bool copyAttributes = true)
        {
            return Offset(new double[] { distance }, tidy, copyAttributes);
        }

        /// <summary>
        /// Offset this curve on the XY plane by varying distances for
        /// each span.
        /// </summary>
        /// <param name="distances">The offset distances for each span, in order.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <param name="tidy">If true (default) collapsed segments will be removed.</param>
        /// <param name="copyAttributes">If true (default) the attributes of this curve will be copied
        /// to the offset one.</param>
        /// <returns></returns>
        public abstract Curve Offset(IList<double> distances, bool tidy = true, bool copyAttributes = true);

        /// <summary>
        /// Offset this curve on the XY plane, automatically determining (where possible)
        /// the direction of offset which will result in the curve being offset within itself.
        /// Note that it will not be possible to accurately predict this for all curves.
        /// </summary>
        /// <param name="distance">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.  This will be
        /// automatically inverted (in-place) if the curve is anticlockwise so that
        /// positive numbers entered will result in an offset inwards and negative numbers outwards.</param>
        /// <returns></returns>
        public virtual Curve OffsetInwards(ref double distance, bool tidy = true, bool copyAttributes = true)
        {
            double cTS = Vertices.ClockwiseTestSum();
            if (cTS < 0) distance *= -1;
            return Offset(distance, tidy, copyAttributes);
        }

        /// <summary>
        /// Offset this curve on the XY plane by varying distances for each span, 
        /// automatically determining (where possible)
        /// the direction of offset which will result in the curve being offset within itself.
        /// Note that it will not be possible to accurately predict this for all curves.
        /// </summary>
        /// <param name="distances">The offset distances for each span, in order.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.  This will be
        /// automatically inverted (in-place) if the curve is anticlockwise so that
        /// positive numbers entered will result in an offset inwards and negative numbers outwards.</param>
        /// <returns></returns>
        public virtual Curve OffsetInwards(IList<double> distances, bool tidy = true, bool copyAttributes = true)
        {
            double cTS = Vertices.ClockwiseTestSum();
            if (cTS < 0)
            {
                for (int i = 0; i < distances.Count; i++)
                    distances[i] *= -1;
            }
            return Offset(distances, tidy, copyAttributes);
        }

        /// <summary>
        /// Is this curve clockwise in the XY plane?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsClockwiseXY()
        {
            return Vertices.ClockwiseTestSum() > 0;
        }

        /// <summary>
        /// Decompose this curve down to simple primitive curve types such
        /// as line and arc segments.  This may return a list containing only
        /// itself if the curve is already a simple type
        /// </summary>
        /// <returns></returns>
        public abstract IList<ISimpleCurve> ToSimpleCurves();

        /// <summary>
        /// Get a polycurve version of this curve.  If the curve is already a
        /// polycurve then this will return a reference to the initial object,
        /// otherwise a new polycurve will be created that contains the original
        /// curve.
        /// </summary>
        /// <returns></returns>
        public virtual PolyCurve ToPolyCurve()
        {
            return new PolyCurve(this, Attributes?.Duplicate());
        }

        /// <summary>
        /// Does this curve self-intersect on the XY plane?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSelfIntersectingXY()
        {
            return false;
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
        /// Extract a subset of this curve between the specified start and end parameters
        /// as a new curve.
        /// </summary>
        /// <param name="tStart">The start parameter of the subdomain of this curve to be extracted</param>
        /// <param name="tEnd">The end parameter of the subdomain of this curve to be extracted</param>
        /// <returns></returns>
        public Curve Extract(double tStart, double tEnd)
        {
            return Extract(new Interval(tStart, tEnd));
        }

        /// <summary>
        /// Extract a subset of this curve within the specified domain
        /// as a new curve.
        /// </summary>
        /// <param name="subDomain">The subdomain of this curve to be extracted</param>
        /// <returns></returns>
        public abstract Curve Extract(Interval subDomain);

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

        /// <summary>
        /// Reduce the length of this curve, trimming from the specifed end vertex
        /// by the specified value
        /// </summary>
        /// <param name="lineEnd">The vertex at the end of this curve
        /// to trim back.  The entered value should be either the
        /// start or end vertex of this line.</param>
        /// <param name="length">The length to cut back from the curve end</param>
        /// <returns>True if successful, false if not.</returns>
        public virtual bool Trim(Vertex lineEnd, double length)
        {
            if (lineEnd == Start) return TrimStart(length);
            else if (lineEnd == End) return TrimEnd(length);
            else return false;
        }

        /// <summary>
        /// Reduce the length of this curve from the start
        /// by the specified value
        /// </summary>
        /// <param name="length">The length to cut back from the curve end</param>
        /// <returns>True if successful, false if not.</returns>
        public virtual bool TrimStart(double length)
        {
            Vector pt = PointAtLength(length);
            if (pt.IsValid())
            {
                Start.Position = pt;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Reduce the length of this curve from the end
        /// by the specified value
        /// </summary>
        /// <param name="length">The length to cut back from the curve end</param>
        /// <returns>True if successful, false if not.</returns>
        public virtual bool TrimEnd(double length)
        {
            Vector pt = PointAtLength(length, true);
            if (pt.IsValid())
            {
                End.Position = pt;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Collapse any segments of this curve which have a length shorter than the
        /// value specified.  The end-points of the curve will be kept the same, but
        /// short polyline segments and polycurve subcurves will be removed and the
        /// adjacent curves adjusted accordingly.
        /// </summary>
        /// <param name="minLength">The length below which segments will be removed.</param>
        /// <returns>True if any segments were removed.</returns>
        public virtual bool CollapseShortSegments(double minLength)
        {
            return false;
        }

        /// <summary>
        /// Find the self-intersection parameters of this curve
        /// on the XY plane.
        /// Returns a sorted list where the keys are the intersection
        /// parameters along this curve and the values are their matching
        /// parameters.
        /// </summary>
        /// <returns></returns>
        public virtual SortedList<double, double> SelfIntersectionsXY()
        {
            // Intersection parameters
            var result = new SortedList<double, double>();

            IList<ISimpleCurve> simples = ToSimpleCurves();
            int max = simples.Count;
            //if (Closed) max++;
            // Walk along the curve forwards to find the next intersection
            for (int i = 0; i < max - 1; i++)
            {
                ISimpleCurve crvA = simples[i];
                for (int j = i + 1; j < max; j++)
                {
                    ISimpleCurve crvB = simples.GetWrapped(j);
                    Vector[] chuck = Intersect.CurveCurveXY(crvA, crvB, 0.0001);
                    if (chuck != null && chuck.Length > 0)
                    {
                        foreach (Vector pt in chuck)
                        {
                            // Work out intersection parameters and add to result
                            double st0 = crvA.ClosestParameter(pt);
                            double st1 = crvB.ClosestParameter(pt);
                            double t0 = ParameterAt(i, st0);
                            double t1 = ParameterAt(j, st1);
                            result.Add(t0, t1);
                            result.Add(t1, t0);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Break down this curve into loops, delineated by the positions
        /// where the curve intersects itself and in so doing produces
        /// one or more closed regions.  The returned loops may be thought
        /// of as the perimeter of each region.
        /// This may be used (and is) as part of a post-processing step once
        /// a curve has been offset in order to eliminate regions where the
        /// offset curve has collapsed.  Any produced loops which have the
        /// inverse winding direction to the original curve will denote regions
        /// where the offset curve has collapsed on itself and may be removed
        /// to create a 'tidied' set of offset curves.
        /// </summary>
        /// <returns></returns>
        public IList<Curve> SelfIntersectionXYLoops()
        {
            var result = new List<Curve>();

            SortedList<double, double> chucks = SelfIntersectionsXY();

            if (chucks.Count > 0)
            {
                double tLoopStart = 0;
                double tStart = 0;
                double tNextLoopStart = 0;
                var tGoBackTo = new List<double>();
                PolyCurve current = null;
                for (int i = 0; i < chucks.Count + 1; i++)
                {
                    double tEnd;
                    bool found;
                    double tNext = chucks.NextAfter(tStart, out found, out tEnd, false);
                    if (!found) // Last segment
                    {
                        tEnd = 1;
                        if (Closed) tNext = 0;
                    }
                    Curve crv = Extract(tStart, tEnd);
                    if (current == null) //TODO: Deal with open curves - don't join ends
                    {
                        // Start a new loop
                        current = new PolyCurve(Attributes);
                        result.Add(current);
                    }
                    
                    current.Add(crv, false, true);
                    if (tNext == tLoopStart || !found)
                    {
                        current = null;
                        // End the loop
                        // Jump back to the start of the next one:
                        if (tGoBackTo.Count > 0)
                        {
                            tNextLoopStart = tGoBackTo.Last();
                            tGoBackTo.RemoveLast();
                        }
                        tNext = tNextLoopStart;
                        tLoopStart = tNextLoopStart;
                    }
                    else
                    {
                        tNextLoopStart = tEnd; //Where we should hop back to
                        tGoBackTo.Add(tNextLoopStart);
                    }
                    tStart = tNext; //Hop over to the other side of the intersection to continue the loop
                    //chucks.Remove(tNext);
                }
            }
            else result.Add(this);
            return result;
        }

        /// <summary>
        /// Convert this curve to a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Curve";
        }

        /// <summary>
        /// Convert this curve to an SVG HTML polygon definition.
        /// </summary>
        /// <param name="stroke">The colour of the line</param>
        /// <param name="fill">The colour of the filled region within the curve</param>
        /// <param name="strokeThickness">The width of the drawn line</param>
        /// <returns></returns>
        public string ToSVG(Colour stroke, Colour fill, double strokeThickness = 1.0)
        {
            var sb = new StringBuilder();
            sb.Append("<polygon ");
            sb.Append("points=\"");
            Vertex lastVert = null;
            foreach (Vertex vert in Vertices)
            {
                if (lastVert != null) sb.Append(" ");
                sb.Append(vert.X);
                sb.Append(",");
                sb.Append(vert.Y);
                lastVert = vert;
            }
            sb.Append("\" style:\"");
            sb.Append("fill:#");
            sb.Append(fill.ToHex());
            sb.Append(";stroke:#");
            sb.Append(stroke.ToHex());
            sb.Append(";stroke-width:");
            sb.Append(strokeThickness);
            sb.Append("\" />");

            return sb.ToString();
        }

        /// <summary>
        /// Reverse the direction of this curve
        /// </summary>
        public virtual void Reverse()
        {
            Vertices.Reverse();
        }

        protected virtual IFastDuplicatable CurveFastDuplicate()
        {
            return this.Duplicate();
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return CurveFastDuplicate();
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
        /*public static bool MatchEnds(Vertex crvEnd0, Vertex crvEnd1, bool detectMismatches = false, bool canTrim0 = true, bool canTrim1 = true)
        {
            double crv0Int, crv1Int;
            return MatchEnds(crvEnd0, crvEnd1, out crv0Int, out crv1Int, detectMismatches, canTrim0, canTrim1);
        }*/

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
        public static bool MatchEnds(Vertex crvEnd0, Vertex crvEnd1, bool detectMismatches = false, bool canTrim0 = true, bool canTrim1 = true)
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
                        throw new NotImplementedException("Arc-Arc end matching not yet implemented.");
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
                        double crv0Int, crv1Int;
                        return MatchEnds(crvEnd0, crv0.TangentAt(crvEnd0), crvEnd1, crv1.TangentAt(crvEnd1), out crv0Int, out crv1Int, detectMismatches, canTrim0, canTrim1);
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
        /// <param name="detectMismatches">Optional.  If true, additional checking will be
        /// performed and only matches where both ends are extended or both ends are trimmed
        /// will be permitted.</param>
        /// <param name="canTrim0"></param>
        /// <param name="canTrim1"></param>
        /// <returns></returns>
        private static bool MatchEnds(Vertex crvEnd0, Vector tan0, Vertex crvEnd1, Vector tan1, 
            out double t0, out double t1,
            bool detectMismatches = false, bool canTrim0 = true, bool canTrim1 = true)
        {
            t0 = double.NaN;
            t1 = double.NaN;
            if (tan0.Z.IsTiny() && tan1.Z.IsTiny()) // 2D intersection:
            {
               
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
                            if ((!canTrim0 && !ext0) || (!canTrim1 && !ext1)) return false;

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
        /// <param name="deltaT"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private static bool IsExtension(double deltaT, Vertex v)
        {
            return (v.IsEnd && deltaT > 0) || (v.IsStart && deltaT < 0);
        }

        /// <summary>
        /// Test all offset vectors in the specified collection to see whether they lie within tolerance
        /// for curve reduction.
        /// </summary>
        /// <param name="offsets"></param>
        /// <param name="perp"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        protected bool AllInToleranceForReduction(List<Vector> offsets, Vector perp, Interval tolerance)
        {
            foreach (Vector offset in offsets)
            {
                double dot = offset.Dot(perp);
                if (!tolerance.Contains(dot)) return false;
            }
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Static class containing helper extension methods for collections of curves
    /// </summary>
    public static class CurveExtensions
    {

        /// <summary>
        /// Offset all curves in this collection on the XY plane by the specified distance
        /// </summary>
        /// <param name="curves"></param>
        /// <param name="distance">The offset distance</param>
        /// <returns></returns>
        public static CurveCollection OffsetAll(this IList<Curve> curves, double distance)
        {
            var result = new CurveCollection();
            foreach (var crv in curves)
            {
                var offsetCrv = crv.Offset(distance);
                if (offsetCrv != null) result.Add(offsetCrv);
            }
            return result;
        }

        /// <summary>
        /// Offset all curves in this collection on the XY plane, automatically determining
        /// for closed curves (where possible) the direction of offset that will result in the offset
        /// curve being inside the starting curve.  Note that determining this may not be possible for
        /// all curves.
        /// </summary>
        /// <param name="curves"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static CurveCollection OffsetAllInwards(this IList<Curve> curves, double distance)
        {
            var result = new CurveCollection();
            foreach (var crv in curves)
            {
                double dist2 = distance;
                var offsetCrv = crv.OffsetInwards(ref dist2);
                if (offsetCrv != null) result.Add(offsetCrv);
            }
            return result;
        }

        /// <summary>
        /// 'Explode' these curves into individual curve segments
        /// </summary>
        /// <param name="curves"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static CurveCollection ExplodeAll(this IList<Curve> curves, bool recursive = true)
        {
            var result = new CurveCollection();
            foreach (var crv in curves)
            {
               result.AddRange(crv.Explode());
            }
            return result;
        }

        /// <summary>
        /// Join as many curves as possible in this collection which have matching ends
        /// into PolyCurves.  Returns the collection of curves post joining.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveCollection JoinCurves(this IList<Curve> curves)
        {
            var matched = new List<List<Curve>>();
            foreach (var crv in curves)
            {
                bool found = false;
                foreach (var matchedSet in matched)
                {
                    if (crv.StartPoint.DistanceToSquared(matchedSet.Last().EndPoint).IsTiny())
                    {
                        matchedSet.Add(crv);
                        found = true;
                        break;
                    }
                    else if (crv.EndPoint.DistanceToSquared(matchedSet.First().StartPoint).IsTiny())
                    {
                        matchedSet.Insert(0, crv);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    matched.Add(new List<Curve>() { crv });
                }
            }
            var result = new CurveCollection();
            foreach (var matchedSet in matched)
            {
                var pCrv = new PolyCurve(matchedSet);
                result.Add(pCrv);
            }
            //TODO: Recursive joining for unordered curve sets
            return result;
        }

        /// <summary>
        /// Join together a sequential set of curves where their end points
        /// lie within tolerance of one another.  Where multiple curves can be
        /// joined they will be brought together as PolyCurves, otherwise they
        /// will be added directly to the result collection.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static CurveCollection JoinOrderedCurves(this IList<Curve> curves, CurveCollection result = null)
        {
            if (result == null) result = new CurveCollection();
            IList<Curve> current = new List<Curve>();
            double tolerance = Tolerance.Distance * Tolerance.Distance;
            for (int i = 0; i < curves.Count; i++)
            {
                Curve crv = curves[i];
                if (current.Count == 0 ||
                    current.Last().EndPoint.DistanceToSquared(crv.StartPoint) < tolerance)
                {
                    // Carry on building the chain... 
                }
                else
                {
                    // Break the chain... (Sorry, Fleetwood Mack)
                    if (current.Count == 1) result.Add(current[0]);
                    else
                    {
                        var pCrv = new PolyCurve(current);
                        result.Add(pCrv);
                        current.Clear();
                    }
                }
                current.Add(crv);
            }

            if (current.Count == 1) result.Add(current[0]);
            else if (current.Count > 0)
            {
                var pCrv = new PolyCurve(current);
                result.Add(pCrv);
            }

            return result;
        }

        /// <summary>
        /// Extract an array of all the tangencies of the curves in
        /// this collection at the specified parameter location.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="curves"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector[] TangentsAt<T>(IList<T> curves, double t)
            where T:Curve
        {
            var result = new Vector[curves.Count];
            for (int i = 0; i < curves.Count; i++)
            {
                result[i] = curves[i]?.TangentAt(t) ?? Vector.Unset;
            }
            return result;
        }

        /// <summary>
        /// Returns a reversed duplicate of this curve.
        /// </summary>
        /// <typeparam name="TCurve"></typeparam>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static TCurve Reversed<TCurve>(this TCurve curve)
            where TCurve : Curve
        {
            var crv = curve.FastDuplicate();
            crv.Reverse();
            return crv;
        }

        /// <summary>
        /// Remove from this collection any curves which do not have the same winding
        /// direction on the XY plane as the specified winding direction
        /// </summary>
        /// <typeparam name="TCurve"></typeparam>
        /// <param name="curves"></param>
        /// <param name="isClockwise">If true, anti-clockwise curves will be removed.  If false,
        /// clockwise curves will be removed.</param>
        public static void RemoveInvertedCurvesXY<TCurve>(this IList<TCurve> curves, bool isClockwise)
            where TCurve : Curve
        {
            for (int i = curves.Count - 1; i >= 0; i--)
            {
                if (curves[i].IsClockwiseXY() != isClockwise)
                    curves.RemoveAt(i);
            }
        }
        
        /// <summary>
        /// Remove from this collection any curves which do not have the same winding direction
        /// on the XY plane as the specified base curve
        /// </summary>
        /// <typeparam name="TCurve"></typeparam>
        /// <param name="curves"></param>
        /// <param name="original"></param>
        public static void RemoveInvertedCurvesXY<TCurve>(this IList<TCurve> curves, Curve original)
            where TCurve : Curve
        {
            curves.RemoveInvertedCurvesXY(original.IsClockwiseXY());
        }
    }
}
