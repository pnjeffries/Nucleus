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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A curve formed of several continuous curves joined together.
    /// </summary>
    [Serializable]
    public class PolyCurve : Curve
    {
        #region Properties

        private CurveCollection _SubCurves = new CurveCollection();

        /// <summary>
        /// The sub-curves of this PolyCurve
        /// </summary>
        public CurveCollection SubCurves
        {
            get { return _SubCurves; }
        }

        /// <summary>
        /// Whether this curve is closed.
        /// If true, the end of the curve is treated as being the same as the start point.
        /// Default (for most curve types) is false.
        /// </summary>
        public override bool Closed
        {
            get
            {
                return StartPoint.Equals(EndPoint, Tolerance.Distance);
            }
            protected set { }
        }

        /// <summary>
        /// Is the definition of this shape valid?
        /// i.e. does it have the correct number of vertices, are all parameters within
        /// acceptable limits, etc.
        /// Checks for validity of all subcurves and that the end of each curve is within tolerance
        /// of the start of the next.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (SubCurves.Count > 0)
                {
                    Vertex lastEnd = null;
                    foreach (Curve subCrv in SubCurves)
                    {
                        if (!subCrv.IsValid) return false;
                        Vertex sVert = subCrv.Start;
                        if (lastEnd != null && lastEnd.Position.ManhattenDistanceTo(sVert.Position) > Tolerance.Distance * 2)
                            return false;
                        lastEnd = subCrv.End;
                    }
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// Different shapes will provide different means of editing this collection.
        /// DO NOT directly modify the collection returned from this property unless you are
        /// sure you know what you are doing.
        /// For PolyCurves, this will generate a combined collection of all sub-curve vertices.
        /// </summary>
        public override VertexCollection Vertices
        {
            get
            {
                VertexCollection allVertices = new VertexCollection();
                foreach (Curve subCrv in SubCurves)
                {
                    allVertices.AddRange(subCrv.Vertices);
                }
                return allVertices;
            }
        }

        /// <summary>
        /// Get the number of segments that this curve posesses.
        /// Segments are stretches of the curve that can be evaluated independantly 
        /// of the rest of the curve.
        /// In the case of polycurves, this will be the combined segment count of all
        /// constituent curves.
        /// </summary
        public override int SegmentCount
        {
            get
            {
                int count = 0;
                foreach (Curve subCrv in SubCurves)
                {
                    count += subCrv.SegmentCount;
                }
                return count;
            }
        }

        /// <summary>
        /// Get the vertex at the start of the curve (if there is one)
        /// </summary>
        public override Vertex Start
        {
            get
            {
                if (SubCurves.Count > 0) return SubCurves.First().Start;
                else return null;
            }
        }

        /// <summary>
        /// Get the vertex at the end of the curve (if there is one)
        /// </summary>
        public override Vertex End
        {
            get
            {
                if (SubCurves.Count > 0) return SubCurves.Last().End;
                else return null;
            }      
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PolyCurve()
        {
            ListenToSubCurves();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PolyCurve(GeometryAttributes attributes = null) : this()
        {
            Attributes = attributes;
        }

        /// <summary>
        /// Curve constructor.  Initialises a polycurve starting with the specifed curve
        /// </summary>
        /// <param name="curve"></param>
        public PolyCurve(Curve curve, GeometryAttributes attributes = null) : this(attributes)
        {
            SubCurves.Add(curve);
        }

        /// <summary>
        /// Curve constructor.  Initialises a polycurve starting with the specifed curve,
        /// optionally exploding it into its constituant parts
        /// </summary>
        /// <param name="curve"></param>
        public PolyCurve(Curve curve, bool autoExplode, GeometryAttributes attributes = null)
            :this(attributes)
        {
            Add(curve, autoExplode);
        }

        /// <summary>
        /// Initialises a new polycurve 
        /// </summary>
        /// <param name="curves"></param>
        public PolyCurve(IEnumerable<Curve> curves, GeometryAttributes attributes = null) : this(attributes)
        {
            foreach (Curve crv in curves)
            {
                SubCurves.Add(crv);
            }
        }

        /// <summary>
        /// Creates a new polycurve as a copy of another one
        /// </summary>
        /// <param name="other"></param>
        public PolyCurve(PolyCurve other)
            : this(other.FastDuplicateSubCurves(), other.Attributes)
        { }

        /// <summary>
        /// Creates a new PolyCurve consisting of the specified sub-curves
        /// </summary>
        /// <param name="curves"></param>
        public PolyCurve(params Curve[] curves) : this(curves, null) { }

        #endregion

        #region Methods

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            ListenToSubCurves();
        }

        private void ListenToSubCurves()
        {
            _SubCurves.CollectionChanged += _SubCurves_CollectionChanged;
        }

        private void _SubCurves_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InvalidateCachedGeometry();
        }


        /// <summary>
        /// Calculate the total length of this polycurve
        /// </summary>
        /// <returns></returns>
        public override double CalculateLength()
        {
            double totalLength = 0;
            foreach (Curve subCrv in SubCurves)
            {
                totalLength += subCrv.Length;
            }
            return totalLength;
        }

        /// <summary>
        /// Calculate the length of the specified segment in this polycurve
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override double CalculateSegmentLength(int index)
        {
            foreach (Curve subCrv in SubCurves)
            {
                int segCount = subCrv.SegmentCount;
                if (segCount > index)
                {
                    return subCrv.CalculateSegmentLength(index);
                }
                index -= segCount;
            }
            return 0;
        }

        /// <summary>
        /// Find the closest position along this polycurve to a test point,
        /// expressed as a parameter value from 0-1 (start-end)
        /// </summary>
        /// <param name="toPoint">The test point</param>
        /// <returns></returns>
        public override double ClosestParameter(Vector toPoint)
        {
            double result = double.NaN;
            double minDistSqd = 0;
            int segs = 0;
            int segCount = SegmentCount;
            foreach (Curve subCrv in SubCurves)
            {
                double t = subCrv.ClosestParameter(toPoint);
                Vector v = subCrv.PointAt(t);
                double distSqd = v.DistanceToSquared(toPoint);
                if (double.IsNaN(result) || distSqd < minDistSqd)
                {
                    result = (segs + (t * subCrv.SegmentCount)) / segCount;
                    minDistSqd = distSqd;
                }
                segs += subCrv.SegmentCount;
            }
            return result;
        }

        /// <summary>
        /// Get the curve parameter at the specified vertex
        /// </summary>
        /// <param name="vertex">The vertex.  Must be a defining vertex of this curve.</param>
        /// <returns>A curve parameter</returns>
        public override double ParameterAt(Vertex vertex)
        {
            int segs = 0;
            int segCount = SegmentCount;
            foreach (Curve subCrv in SubCurves)
            {
                if (subCrv.Vertices.Contains(vertex.GUID))
                {
                    double t = subCrv.ParameterAt(vertex);
                    return (segs + (t * subCrv.SegmentCount)) / segCount;
                }
                segs += subCrv.SegmentCount;
            }
            return double.NaN;
        }

        /// <summary>
        /// Get the curve parameter at the vertex at the specified index
        /// </summary>
        /// <param name="vertexIndex"></param>
        /// <returns></returns>
        public override double ParameterAtVertexIndex(int vertexIndex)
        {
            return ParameterAt(Vertices[vertexIndex]);
        }

        /*
        /// <summary>
        /// Get the curve parameter at the specified length along this curve.
        /// If the returned parameter falls outside the range 0-1, the specified
        /// length does not fall within the domain of the curve.
        /// </summary>
        /// <param name="length">The distance along the curve from the start of the curve to the point in question</param>
        /// <returns>A curve parameter</returns>
        public override double ParameterAt(double length)
        {
            int segs = 0;
            int segCount = SegmentCount;
            double l0 = 0;
            foreach (Curve subCrv in SubCurves)
            {
                double crvLength = subCrv.Length;
                double l1 = l0 + crvLength;
                if (l1 > length)
                {
                    double t = subCrv.ParameterAt(length - l0);
                    return (segs + (t * subCrv.SegmentCount)) / segCount;
                }
                segs += subCrv.SegmentCount;
            }
            return double.NaN;
        }
        */

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
            if (tSpan == 0 && span > 0)
            {
                // Fix end-point in non-existent segement problem
                span--;
                tSpan = 1;
            }
            foreach (Curve subCrv in SubCurves)
            {
                int segCount = subCrv.SegmentCount;
                if (segCount > span)
                {
                    return subCrv.PointAt(span, tSpan);
                }
                span -= segCount;
            }
            return Vector.Unset;
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
        public override Vector TangentAt(int span, double tSpan)
        {
            foreach (Curve subCrv in SubCurves)
            {
                int segCount = subCrv.SegmentCount;
                if (segCount > span)
                {
                    return subCrv.TangentAt(span, tSpan);
                }
                span -= segCount;
            }
            return Vector.Unset;
        }

        /// <summary>
        /// Calculate the area enclosed by this curve, were the start and end points to be 
        /// joined by a straight line segment.
        /// A plane may optionally be specified, otherwise by default the projected area on 
        /// the XY plane will be used.
        /// </summary>
        /// <param name="centroid">Output.  The centroid of the enclosed area, in local coordinates
        /// on the specified plane.</param>
        /// <param name="onPlane">The plane to use to calculate the area.
        /// If not specified, the XY plane will be used.</param>
        /// <returns>The signed area enclosed by this curve on the specified plane,
        /// as a double.</returns>
        public override double CalculateEnclosedArea(out Vector centroid, Plane onPlane = null)
        {
            double result = 0;
            centroid = Vector.Zero;
            for (int i = 0; i < SubCurves.Count; i++)
            {
                Curve subCrv = SubCurves[i];
                Vector start = subCrv.StartPoint;
                Vector end = subCrv.EndPoint;
                if (onPlane != null)
                {
                    start = onPlane.GlobalToLocal(start);
                    end = onPlane.GlobalToLocal(end);
                }
                double areaUnder = MathsHelper.AreaUnder(start.X, start.Y, end.X, end.Y, ref centroid);
                result += areaUnder;
                Vector subCentroid;
                double subArea = subCrv.CalculateEnclosedArea(out subCentroid, onPlane);
                result += subArea;
                centroid += subCentroid * subArea;
            }
            centroid /= result;
            return result;
        }

        /// <summary>
        /// Produce a set of points which represents a facetted version of this curve
        /// </summary>
        /// <param name="tolerance">The maximum angular deviation between the curve and the 
        /// facetted geometry.  If zero, the tolerance is taken as infinite and curves will
        /// not be facetted between kinks.</param>
        /// <returns></returns>
        public override Vector[] Facet(Angle tolerance)
        {
            var result = new List<Vector>();
            foreach (Curve subCrv in SubCurves)
            {
                result.AddRange(subCrv.Facet(tolerance));
            }
            return result.ToArray();
        }

        /// <summary>
        /// Add a new sub-curve to this PolyCurve.
        /// Note that to form a valid polycurve the new sub-curve *must*
        /// start at a point within tolerance of the end point of the
        /// last sub-curve.
        /// </summary>
        /// <param name="subCurve">The curve to add.</param>
        public void Add(Curve subCurve)
        {
            SubCurves.Add(subCurve);
        }

        /// <summary>
        /// Add a new sub-curve to this PolyCurve.
        /// Note that to form a valid polycurve the new sub-curve *must*
        /// start at a point within tolerance of the end point of the
        /// last sub-curve.  If autoConnect is set to true, this condition
        /// will be checked for and a line segment automatically added to
        /// connect the curve ends if necessary.
        /// </summary>
        /// <param name="subCurve">The curve to add.</param>
        /// <param name="autoConnect">If true, a line segment will automatically be added between
        /// the end of the last curve and the start of the newly-added one to close
        /// any gap between them, if necessary.</param>
        /// <param name="autoExplode">If true and the added curve is itself a PolyCurve, the subcurves
        /// of that polycurve will be added instead of the parent curve.</param>
        public void Add(Curve subCurve, bool autoConnect, bool autoExplode = false)
        {
            if (autoConnect == true && SubCurves.Count > 0)
            {
                Vector endPt = SubCurves.Last().EndPoint;
                if (endPt.DistanceToSquared(subCurve.StartPoint) > (Tolerance.Distance*Tolerance.Distance))
                {
                    //Out of tolerance; create a line:
                    Add(new Line(endPt, subCurve.StartPoint));
                }
            }
            if (autoExplode && !(subCurve is ISimpleCurve))
            {
                var exploded = subCurve.Explode();
                foreach (Curve subSubCurve in exploded)
                    Add(subSubCurve);
            }
            else Add(subCurve);
        }

        /// <summary>
        /// Add a new line segment to the end of this polycurve.
        /// The line will run from the end of the last subcurve in this polycurve
        /// to the specified point.
        /// This polycurve must contain at least one subcurve already in order for the start
        /// point to be determined.
        /// </summary>
        /// <param name="lineEnd">The end point of the new line segment.</param>
        /// <returns></returns>
        public Line AddLine(Vector lineEnd)
        {
            if (SubCurves.Count > 0)
            {
                Vector startPoint = SubCurves.Last().EndPoint;
                Line result = new Line(startPoint, lineEnd);
                Add(result);
                return result;
            }
            else return null;
        }

        /// <summary>
        /// Add a new line segment to the end of this polycurve.
        /// The line will run from the end of the last subcurve in this polycurve
        /// to the specified point.
        /// This polycurve must contain at least one subcurve already in order for the start
        /// point to be determined.
        /// </summary>
        /// <param name="endX">The x-coordinate of the end point of the new line segement</param>
        /// <param name="endY">The y-coordinate of the end point of the new line segement</param>
        /// <param name="endZ">The z-coordinate of the end point of the new line segement</param>
        /// <returns></returns>
        public Line AddLine(double endX, double endY, double endZ = 0)
        {
            return AddLine(new Vector(endX, endY, endZ));
        }

        /// <summary>
        /// Add a new arc segment to the end of this polycurve.
        /// The arc will run from the end of the last subcurve in this polycurve
        /// to the specified, maintaining tangency with the last segment.
        /// This polycurve must contain at least one subcurve already in order for the
        /// start point to be determined.
        /// </summary>
        /// <param name="arcEnd"></param>
        /// <returns></returns>
        public Curve AddArc(Vector arcEnd)
        {
            if (SubCurves.Count > 0)
            {
                Curve last = SubCurves.Last();
                Vector tangent = last.TangentAt(1);
                Vector startPt = last.EndPoint;
                Curve result = Arc.StartTangentEnd(startPt, tangent, arcEnd);
                if (result != null) Add(result);
                return result;
            }
            else return null;
        }
        
        /// <summary>
        /// Add a new arc segment to the end of this polycurve.
        /// The arc will run from the end of the last subcurve in this polycurve
        /// to the specified, maintaining tangency with the last segment.
        /// This polycurve must contain at least one subcurve already in order for the
        /// start point to be determined.
        /// </summary>
        /// <param name="endX">The x-coordinate of the end point of the new arc segement</param>
        /// <param name="endY">The y-coordinate of the end point of the new arc segement</param>
        /// <param name="endZ">The z-coordinate of the end point of the new arc segement</param>
        /// <returns></returns>
        public Curve AddArc(double endX, double endY, double endZ = 0)
        {
            return AddArc(new Vector(endX, endY, endZ));
        }

        /// <summary>
        /// Add a new arc segment to the end of this polycurve.
        /// The arc will run from the end of the last subcurve in this polycurve
        /// to the specified point, following the specified tangent at its start.
        /// This polycurve must contain at least one subcurve already in order for the
        /// start point to be determined.
        /// </summary>
        /// <param name="endX">The x-coordinate of the end point of the new arc segement</param>
        /// <param name="endY">The y-coordinate of the end point of the new arc segement</param>
        /// <param name="endZ">The z-coordinate of the end point of the new arc segement</param>
        /// <returns></returns>
        public Curve AddArcTangent(Vector startTangent, Vector arcEnd)
        {
            if (SubCurves.Count > 0)
            {
                Curve last = SubCurves.Last();
                Vector startPt = last.EndPoint;
                Curve result = Arc.StartTangentEnd(startPt, startTangent, arcEnd);
                if (result != null) Add(result);
                return result;
            }
            else return null;
        }

        /// <summary>
        /// Add a new arc segment to the end of this polycurve.
        /// The arc will run from the end of the last subcurve in this polycurve
        /// to the specified point, following the specified tangent at its start.
        /// This polycurve must contain at least one subcurve already in order for the
        /// start point to be determined.
        /// </summary>
        /// <param name="endX">The x-coordinate of the end point of the new arc segement</param>
        /// <param name="endY">The y-coordinate of the end point of the new arc segement</param>
        /// <param name="endZ">The z-coordinate of the end point of the new arc segement</param>
        /// <returns></returns>
        public Curve AddArcTangent(Vector startTangent, double endX, double endY, double endZ = 0)
        {
            return AddArcTangent(startTangent, new Vector(endX, endY, endZ));
        }

        /// <summary>
        /// Close this polycurve by adding an extra line segment between end and start,
        /// if it is not already closed
        /// </summary>
        public void Close()
        {
            if (!Closed)
            {
                AddLine(StartPoint);
            }
        }

        /// <summary>
        /// Resolve a discontinuity between a curve and the curve preceeding it in an offset operation
        /// </summary>
        /// <param name="result"></param>
        /// <param name="offsetCrv"></param>
        /// <param name="options"></param>
        private bool ResolveOffsetDiscontinuity(PolyCurve result, Curve offsetCrv, CurveOffsetParameters options,
            GeometryAttributes chamferAttributes)
        {
            if (result.SubCurves.Count == 0) return false;

            // Adjust offset curve ends to node out
            Curve prevCrv = result.SubCurves.Last();
            // Sharp corner:
            if (MatchEnds(prevCrv.End, offsetCrv.Start, true, true, true, prevCrv.Length, offsetCrv.Length))
                return true;

            if (options.CollapseInvertedSegments)
            {
                result.SubCurves.RemoveLast(); //Remove last curve
                return ResolveOffsetDiscontinuity(result, offsetCrv, options, chamferAttributes); //Recuse to next
            }

            if (prevCrv.EndPoint != offsetCrv.StartPoint)
            {
                // If there is a extension/trim mismatch between the curves
                // we instead just join them with a chamfer temporarily
                result.Add(new Line(prevCrv.EndPoint, offsetCrv.StartPoint, chamferAttributes));
            }

            return true;
        }

        /// <summary>
        /// Offset this curve on the XY plane by varying distances for
        /// each span.
        /// </summary>
        /// <param name="distances">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <param name="tidy">If true (default) collapsed segments will be removed.</param>
        /// <param name="copyAttributes">If true, the attributes of the original subcurves will be
        /// copied to their new offset equivalents.</param>
        /// <returns></returns>
        public override Curve Offset(IList<double> distances, bool tidy = true, bool copyAttributes = true)
        {
            return Offset(distances, new CurveOffsetParameters(tidy, copyAttributes));
        }

        /// <summary>
        /// Offset this curve on the XY plane by a constant distance.
        /// </summary>
        /// <param name="distances">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <param name="options">The parameters used to define optional aspects
        /// of the way in which curves should be offset.</param>
        /// <returns></returns>
        public override Curve Offset(double distance, CurveOffsetParameters options)
        {
            return Offset(new double[]{ distance }, options);
        }

        /// <summary>
        /// Offset this curve on the XY plane by varying distances for
        /// each span.
        /// </summary>
        /// <param name="distances">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <param name="options">The parameters used to define optional aspects
        /// of the way in which curves should be offset.</param>
        /// <returns></returns>
        public Curve Offset(IList<double> distances, CurveOffsetParameters options)
        {
            //TODO: Implement collapsed segments tidying
            var dummyAttributes = new GeometryAttributes("DUMMY");

            var result = new PolyCurve();
            int distIndex = 0;

            // Offset sub curves:
            foreach (Curve crv in SubCurves)
            {
                if (crv.Length > 0)
                {
                    Curve offsetCrv = crv.Offset(distances.SubListFrom(distIndex), false);
                    distIndex += crv.SegmentCount;
                    if (distIndex > distances.Count - 1) distIndex = distances.Count - 1;

                    if (offsetCrv != null)
                    {
                        ResolveOffsetDiscontinuity(result, offsetCrv, options, dummyAttributes);
                        result.Add(offsetCrv);
                        if (options.CopyAttributes && offsetCrv.Attributes == null) offsetCrv.Attributes = Attributes;
                    }
                }
            }

            // Match end to start
            if (Closed && result.SubCurves.Count > 1)
            {
                Curve firstCrv = result.SubCurves.First();
                if (!ResolveOffsetDiscontinuity(result, firstCrv, options, dummyAttributes)
                    && options.CollapseInvertedSegments)
                {
                    return null;
                }
            }

            if (options.Tidy)
            {
                // Return the largest non-inverted loop
                var loops = result.SelfIntersectionXYLoopsAlignedWith(this);
                var biggest = loops.ItemWithMax(i => i.Length);
                if (biggest != null && biggest is PolyCurve &&
                    ((PolyCurve)biggest).SubCurves.AllHaveAttributes(dummyAttributes))
                    return null; // Eliminate if entirely composed of temporary chamfers
                else
                    return biggest;
            }

            return result;
        }

        /// <summary>
        /// Store the original domain(s) of each subcurve on that subcurve
        /// as an OriginalDomainGeometryAttrubutes.  A new attributes object will
        /// be generated for each subcurve, copying over data from the original.
        /// Note that only base type properties will be copied and this may result
        /// in a loss of data if other specialised GeometryAttributes subtypes
        /// are already attached to each curve.
        /// </summary>
        public void AttachOriginalDomainsToSubCurves()
        {
            double segCount = SegmentCount;
            double domStart = 0;
            foreach (var subCrv in SubCurves)
            {
                double domLength = subCrv.SegmentCount / segCount;
                double domEnd = domStart + domLength;
                subCrv.Attributes = new OriginalDomainGeometryAttributes(subCrv.Attributes,
                    new Interval(domStart, domEnd));
            }
        }

        /// <summary>
        /// Interpolate between this polycurve and another.  If this curve's subcurves
        /// have attached OriginalDomainGeometryAttributes then the stored domains may
        /// optionally be used to guide the interpolation.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public PolyCurve Interpolate(PolyCurve other, double proportion = 0.5, bool useStoredDomains = false)
        {
            throw new NotImplementedException();
        }

        private bool IsOffsetCurveFlipped(Curve original, Curve offset)
        {
            if (original.Closed)
            {
                return (original.IsClockwiseXY() != offset.IsClockwiseXY());
            }
            else
            {
                Vector v1 = original.EndPoint - original.StartPoint;
                Vector v2 = offset.EndPoint - offset.StartPoint;
                return v1.Dot(v2) < 0;
            }
        }

        /// <summary>
        /// Extract a portion of this curve as a new curve
        /// </summary>
        /// <param name="subDomain">The subdomain of this curve to
        /// be extracted as a new curve</param>
        /// <returns></returns>
        public override Curve Extract(Interval subDomain)
        {
            var result = new PolyCurve();
            
            Interval subDomainAdj = subDomain;
            if (subDomain.IsDecreasing) subDomainAdj = new Interval(subDomain.Start, 1.0);

            PopulateWithSubCurves(subDomainAdj, result);

            // If looping, do a second pass:
            if (Closed && subDomain.IsDecreasing)
            {
                PopulateWithSubCurves(new Maths.Interval(0.0, subDomain.End), result);
            }

            return result;
        }
        
        /// <summary>
        /// Populate another polycurve with the portions of this one which lie within
        /// the specified subDomain
        /// </summary>
        /// <param name="subDomain"></param>
        /// <param name="result"></param>
        private void PopulateWithSubCurves(Interval subDomain, PolyCurve result)
        {
            double segCount = SegmentCount;
            double segStart = 0;
            foreach (Curve subCrv in SubCurves)
            {
                double subSC = subCrv.SegmentCount;
                double segEnd = segStart + subSC;
                Interval crvDom = new Interval(segStart / segCount, segEnd / segCount);
                if (subDomain.Contains(crvDom)) result.Add(subCrv.FastDuplicate());
                else if (subDomain.Overlaps(crvDom))
                {
                    Curve subSubCrv = subCrv.Extract(crvDom.ParameterOf(subDomain.Overlap(crvDom)));
                    if (subSubCrv != null) result.Add(subSubCrv);
                }
                segStart = segEnd;
            }
        }

        /// <summary>
        /// Could this PolyCurve be accurately represented using a polyline?
        /// </summary>
        /// <returns></returns>
        public bool IsPolyline()
        {
            foreach (Curve crv in SubCurves)
            {
                if (!(crv is Line || crv is PolyLine))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Convert this polycurve to a (typically lighter)
        /// polyline utilising the same vertex positions.
        /// </summary>
        /// <returns></returns>
        public PolyLine ToPolyLine()
        {
            var pts = new List<Vector>();
            bool closed = Closed;
            pts.Add(StartPoint);
            for (int i = 0; i < SubCurves.Count - 1; i++)
            {
                Curve crv = SubCurves[i];
                var vertices = crv.Vertices;
                for (int j = 1; j < vertices.Count; j++)
                {
                    pts.Add(vertices[j].Position);
                }
            }

            return new PolyLine(pts, closed, Attributes);
        }

        /// <summary>
        /// 'Explode' this curve into a collection of its constituent
        /// segments as individual curves.
        /// </summary>
        /// <param name="recursive">If true (default), any sub-curves of
        /// this curve which themselves have sub-curves will also be exploded
        /// and added to the collection.</param>
        /// <returns></returns>
        public override CurveCollection Explode(bool recursive = true)
        {
            var result = new CurveCollection();
            foreach (var subCrv in SubCurves)
            {
                if (recursive)
                {
                    result.AddRange(subCrv.Explode());
                }
                else result.Add(subCrv);
            }
            return result;
        }

        /// <summary>
        /// Decompose this polycurve down to simple primitive curve types such
        /// as line and arc segments
        /// </summary>
        /// <returns></returns>
        public override IList<ISimpleCurve> ToSimpleCurves()
        {
            var result = new List<ISimpleCurve>();
            foreach (Curve subCrv in SubCurves)
            {
                if (subCrv is ISimpleCurve) result.Add((ISimpleCurve)subCrv);
                else result.AddRange(subCrv.ToSimpleCurves());
            }
            return result;
        }

        /// <summary>
        /// Get a polycurve version of this curve.  As this curve is
        /// already a polycurve, this simply returns a self-reference.
        /// </summary>
        /// <returns></returns>
        public override PolyCurve ToPolyCurve(bool autoExplode = false)
        {
            return this;
        }

        /// <summary>
        /// Does this curve self-intersect on the XY plane?
        /// </summary>
        /// <returns></returns>
        public override bool IsSelfIntersectingXY()
        {
            var simples = ToSimpleCurves();
            int max = simples.Count;
            if (Closed) max++;
            for (int i = 0; i < max - 1; i++)
            {
                ISimpleCurve crvA = simples[i];
                for (int j = i + 1; j < max; j++)
                {
                    ISimpleCurve crvB = simples.GetWrapped(j);
                    var chuck = Intersect.CurveCurveXY(crvA, crvB, 0.0001);
                    if (chuck != null && chuck.Length > 0)
                        return true;
                }
            }
            return false;
        }

        

        public override string ToString()
        {
            return "PolyCurve";
        }

        /// <summary>
        /// Reverse the direction of this curve
        /// </summary>
        public override void Reverse()
        {
            foreach (var subCrv in SubCurves)
            {
                subCrv.Reverse();
            }
            SubCurves.Reverse();
        }

        /// <summary>
        /// Reduce the length of this curve from the start
        /// by the specified value
        /// </summary>
        /// <param name="length">The length to cut back from the curve end</param>
        /// <returns>True if successful, false if not.</returns>
        public override bool TrimStart(double length)
        {
            bool result = false;
            double l0 = 0;
            for (int i = 0; i < SubCurves.Count; i++)
            {
                Curve subCrv = SubCurves[i];
                double crvLength = subCrv.Length;
                double l1 = l0 + crvLength;
                if (l1 > length)
                {
                    result = subCrv.TrimStart(length - l0);
                    for (int j = 0; j < i; j++)
                    {
                        SubCurves.RemoveAt(0);
                    }
                    break;
                }
            }

            return result;

        }

        /// <summary>
        /// Reduce the length of this curve from the end
        /// by the specified value
        /// </summary>
        /// <param name="length">The length to cut back from the curve end</param>
        /// <returns>True if successful, false if not.</returns>
        public override bool TrimEnd(double length)
        {
            bool result = false;
            double l0 = 0;
            for (int i = 0; i < SubCurves.Count; i++)
            {
                Curve subCrv = SubCurves[SubCurves.Count - 1 - i];
                double crvLength = subCrv.Length;
                double l1 = l0 + crvLength;
                if (l1 > length)
                {
                    result = subCrv.TrimStart(length - l0);
                    for (int j = 0; j < i; j++)
                    {
                        SubCurves.RemoveAt(0);
                    }
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculate the angle between the tangents at the end of one curve and the start of another
        /// </summary>
        /// <param name="endOf"></param>
        /// <param name="startOf"></param>
        /// <returns></returns>
        private static Angle AngleBetweenEnds(Curve endOf, Curve startOf)
        {
            if (endOf != null && startOf != null)
            {
                Vector eV = endOf.TangentAt(1);
                Vector sV = startOf.TangentAt(0);
                return eV.AngleBetween(sV);
            }
            else return Angle.Undefined;
        }

        /// <summary>
        /// Remove from this polycurve any subcurves at the start or end of the curve
        /// whose length is below the specified limit.  Returns true if any segments have been removed.
        /// Note that this will only remove short segments at the very ends of the curve - to ensure
        /// that no end curve exists below the specified limit this should be called repeatedly.
        /// If the end curve is itself a PolyCurve then this function will be called recursively to remove
        /// any below-tolerance curve at the relevant end of that curve.  An angle tolerance may be specified
        /// so that end curves which are short cut have continuity with the adjacent curve may be excluded from
        /// being trimmed.
        /// </summary>
        /// <param name="minLength">The minimum value of length.  And subcurves below this length at the ends
        /// of the curve will be removed.</param>
        /// <param name="angleTolerance">If the angle between the end curve and the adjacent curve is
        /// less than this tolerance value, it will not be removed.</param>
        /// <param name="atStart">If true, the curve at the start will be tested for removal.</param>
        /// <param name="atEnd">If true, the curve at the end will be tested for removal.</param>
        /// <returns></returns>
        /// <remarks>A curve pair which is below angle tolerance will not be removed even if that pairing itself
        /// has a combined length less than the minimum.</remarks>
        public bool TrimShortEndCurves(double minLength, Angle angleTolerance = new Angle(), bool atStart = true, bool atEnd = true)
        {
            bool result = false;
            if (!Closed)
            {
                if (atStart)
                {
                    var startCrv = SubCurves.FirstOrDefault();
                    if (startCrv != null && startCrv.Length < minLength)
                    {
                        if (angleTolerance <= 0 || AngleBetweenEnds(startCrv, SubCurves.GetOrDefault(1)) >= angleTolerance)
                        {
                            SubCurves.RemoveFirst();
                            result = true;
                        }
                    }
                    else if (startCrv is PolyCurve && ((PolyCurve)startCrv).TrimShortEndCurves(minLength, angleTolerance, atStart, false))
                    {
                        result = true;
                        if (!startCrv.IsValid) SubCurves.RemoveFirst();
                    }
                }
                if (atEnd)
                {
                    var endCrv = SubCurves.LastOrDefault();
                    if (endCrv != null && endCrv.Length < minLength)
                    {
                        if (angleTolerance <= 0 || AngleBetweenEnds(SubCurves.GetOrDefault(SubCurves.Count - 2), endCrv) >= angleTolerance)
                        {
                            SubCurves.RemoveLast();
                            result = true;
                        }
                    }
                    else if (endCrv is PolyCurve && ((PolyCurve)endCrv).TrimShortEndCurves(minLength, angleTolerance, false, atEnd))
                    {
                        result = true;
                        if (!endCrv.IsValid) SubCurves.RemoveLast();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Automatically fillet any sharp corners between segments in the PolyCurve
        /// </summary>
        /// <param name="angleLimit">The smallest permissible angle</param>
        /// <param name="filletLength">The smallest permissible length of the added fillet curve</param>
        public void FilletSharpCorners(Angle angleLimit, double filletLength)
        {
            int iMax = SubCurves.Count - 1;
            if (Closed) iMax += 1;
            for (int i = 0; i < iMax; i++)
            {
                Curve crvA = SubCurves.GetWrapped(i);
                //TODO: Recursively fillet polys
                Curve crvB = SubCurves.GetWrapped(i + 1);

                Vector tA = crvA.TangentAt(1);
                Vector tB = crvB.TangentAt(0);

                Angle angle = (Math.PI - tA.AngleBetween(tB).Abs());

                if (angle < angleLimit)
                {
                    double cutBack = filletLength / (2 * Math.Sin(angle / 2));
                    if (cutBack > 0)
                    {
                        if (crvA.Length > cutBack && crvB.Length > cutBack &&
                            crvA.TrimEnd(cutBack) && crvB.TrimStart(cutBack))
                        {
                            Line fillet = new Line(crvA.EndPoint, crvB.StartPoint);
                            SubCurves.Insert(i + 1, fillet);
                            i++;
                            iMax++;
                        }
                    }
                    //TODO: Deal with failed trims better
                }

            }
        }

        /// <summary>
        /// Extract from this PolyCurve a chain of subcurves, starting with the
        /// longest and continuing to select the longest edge until no more can
        /// be added without adding an edge which is within an angle tolerance of
        /// an existing curve within the chain.
        /// </summary>
        /// <returns></returns>
        public PolyCurve ExtractLongCurveChain(Angle maxAngle)
        {
            Curve longest = SubCurves.GetLongest();
            if (longest != null)
            {
                var tangents = new List<Vector>();
                tangents.Add(longest.TangentAt(0.5));
                var result = new PolyCurve(longest);
                int iL = SubCurves.IndexOf(longest);
                int i0 = iL - 1;
                int i1 = iL + 1;
                bool closed = Closed;
                bool tryPre = closed || i0 >= 0;
                bool tryPost = closed || i1 < SubCurves.Count;
                while (tryPre || tryPost)
                {
                    Curve pre = tryPre ? SubCurves.GetWrapped(i0, closed) : null;
                    Curve post = tryPost ? SubCurves.GetWrapped(i1, closed) : null;
                    if (post != null && (pre == null || post.Length > pre.Length))
                    {
                        Vector tang = post.TangentAt(0.5);
                        if (result.SubCurves.Contains(post.GUID) || tangents.MaximumAngleBetween(tang).Abs() > maxAngle)
                        {
                            tryPost = false;
                        }
                        else
                        {
                            result.SubCurves.Add(post);
                            tangents.Add(tang);
                            i1++;
                            if (!closed && i1 >= SubCurves.Count) tryPost = false;
                        }
                    }
                    else if (pre != null)
                    {
                        Vector tang = pre.TangentAt(0.5);
                        if (result.SubCurves.Contains(pre.GUID) || tangents.MaximumAngleBetween(tang).Abs() > maxAngle)
                        {
                            tryPre = false;
                        }
                        else
                        { 
                            result.SubCurves.Insert(0, pre);
                            tangents.Add(tang);
                            i0--;
                            if (!closed && i0 < 0) tryPre = false;
                        }
                    }
                    else
                    {
                        tryPre = false;
                        tryPost = false;
                    }
                }
                return result;

            }
            return null;
        }

        /// <summary>
        /// Create a collection of fast duplicates of the sub-curves in
        /// this polycurve
        /// </summary>
        /// <returns></returns>
        public IList<Curve> FastDuplicateSubCurves()
        {
            Curve[] result = new Curve[SubCurves.Count];
            for (int i = 0; i < SubCurves.Count; i++)
            {
                result[i] = SubCurves[i].FastDuplicate();
            }
            return result;
        }


        /// <summary>
        /// Walk the sub-curves of this polycurve, returning the next sub-curve along from
        /// the specified start parameter before the specified end parameter.  This enables
        /// sub-curves to be extracted and processed one-by-one.
        /// </summary>
        /// <param name="tStart">The start parameter.</param>
        /// <param name="tEnd">The end parameter</param>
        /// <param name="tSubEnd">Output.  The end of the returned sub-curve expressed as a parameter
        /// on this polycurve.  To walk the curve, this should be used as the input value of tStart in the
        /// next iteration.</param>
        /// <param name="subCrvDomain"> Output.  The equivalent domain on the sub-curve between tStart and tEnd
        /// on the parent polycurve.</param>
        /// <remarks>Will not loop in closed curves.</remarks>
        /// <returns></returns>
        public Curve WalkSubCurves(double tStart, double tEnd, out double tSubEnd, out Interval subCrvDomain)
        {
            double tSpan;
            int span = SpanAt(tStart, out tSpan);

            double tStartNext;
            int subSpan;
            var nextCrv = SubCurveAt(tStart, out tStartNext, out subSpan);

            if (nextCrv != null)
            {
                double tEndNext = 1.0;
                double tSubStart = (span - subSpan) / (double)SegmentCount;
                tSubEnd = (span - subSpan + nextCrv.SegmentCount) / (double)SegmentCount;
                if (tSubEnd > tEnd)
                {
                    tEndNext = (tEnd = tSubStart) / (tSubEnd - tSubStart);
                }
                subCrvDomain = new Interval(tStartNext, tEndNext);
            }
            else
            {
                tSubEnd = double.NaN;
                subCrvDomain = Interval.Unset;
            }

            return nextCrv;
        }

        /// <summary>
        /// Get the sub-curve at the specified parameter along this PolyCurve
        /// </summary>
        /// <param name="t">A normalised parameter along this PolyCurve, where
        /// 0 = curve start and 1 = curve end.  Note that the domain in between
        /// does not necessarily scale in proportion to length.</param>
        /// <param name="tSub">Output.  The normalised parameter along the sub-curve.</param>
        /// <param name="subSpan">Output.  The span index of the point on the sub-curve.</param>
        /// <param name="tSubSpan">Output.  The normalised parameter along the sub-curve span.</param>
        /// <returns></returns>
        public Curve SubCurveAt(double t, out double tSub, out int subSpan, out double tSubSpan)
        {
            subSpan = SpanAt(t, out tSubSpan);
            foreach (Curve subCrv in SubCurves)
            {
                int segCount = subCrv.SegmentCount;
                if (segCount > subSpan)
                {
                    tSub = subCrv.ParameterAt(subSpan, tSubSpan);

                    return subCrv;
                }
                subSpan -= segCount;
            }
            tSub = double.NaN;
            return null;
        }

        /// <summary>
        /// Get the sub-curve at the specified parameter along this PolyCurve
        /// </summary>
        /// <param name="t">A normalised parameter along this PolyCurve, where
        /// 0 = curve start and 1 = curve end.  Note that the domain in between
        /// does not necessarily scale in proportion to length.</param>
        /// <param name="tSub">Output.  The normalised parameter along the sub-curve.</param>
        /// <param name="subSpan">Output.  The span index of the point on the sub-curve.</param>
        /// <returns></returns>
        public Curve SubCurveAt(double t, out double tSub, out int subSpan)
        {
            double tSubSpan;
            return SubCurveAt(t, out tSub, out subSpan, out tSubSpan);
        }

        /// <summary>
        /// Get the parameter domain of the specified sub-curve along this PolyCurve.
        /// The curve in question must be a sub-curve of this PolyCurve already.
        /// </summary>
        /// <param name="subCurve">The subcurve to extract the subdomain for</param>
        /// <returns></returns>
        public Interval SubCurveDomain(Curve subCurve)
        {
            int span = 0;
            foreach (Curve subCrv in SubCurves)
            {
                if (subCrv == subCurve)
                {
                    return new Interval(
                        ParameterAt(span, 0),
                        ParameterAt(span + subCrv.SegmentCount, 1));
                }
                span += subCrv.SegmentCount;
            }
            return Interval.Unset;
        }

        /// <summary>
        /// Collapse any segments of this curve which have a length shorter than the
        /// value specified.  The end-points of the curve will be kept the same, but
        /// short polyline segments and polycurve subcurves will be removed and the
        /// adjacent curves adjusted accordingly.
        /// </summary>
        /// <param name="minLength">The length below which segments will be removed.</param>
        /// <returns>True if any segments were removed.</returns>
        public override bool CollapseShortSegments(double minLength)
        {
            bool removedAny = false;

            for (int i = 0; i < SubCurves.Count; i++)
            {
                if (SubCurves.Count > 1)
                {
                    var subCrv = SubCurves[i];
                    double sCLength = subCrv.Length;
                    if (sCLength < minLength)
                    {
                        if (!Closed && i == 0)
                        {
                            // Snap next to start
                            var nextCrv = SubCurves[1];
                            nextCrv.Start.Position = subCrv.StartPoint;
                        }
                        else if (!Closed && i == SubCurves.Count - 1)
                        {
                            // Snap prev to end
                            var prevCrv = SubCurves[i - 1];
                            prevCrv.End.Position = subCrv.EndPoint;
                        }
                        else
                        {
                            // Snap adjacent to mid-point
                            var nextCrv = SubCurves.GetWrapped(i + 1);
                            var prevCrv = SubCurves.GetWrapped(i - 1);
                            Vector midPt = subCrv.PointAtLength(sCLength / 2);
                        }
                        SubCurves.RemoveAt(i);
                        i--;
                        removedAny = true;
                    }
                }
            }
            if (removedAny) NotifyGeometryUpdated();
            return removedAny;
        }

        /// <summary>
        /// Reduce this polycurve by removing line subcurves where
        /// they can be adequately represented within tolerance by
        /// adjusting an adjoining line curve to replace them.
        /// </summary>
        /// <param name="tolerance">The tolerance range.
        /// Line ends which fall within this range of signed distance
        /// of the replacement straight line will be removed.  Positive
        /// values are to the left of the curve and negative values are to
        /// the right, meaning that this range allows you to specify different
        /// tolerances to each side of the curve.</param>
        /// <returns>The number of sub-curves removed by this operation.</returns>
        public override int Reduce(Interval tolerance)
        {
            int result = 0;
            int modifier = 0;
            if (!Closed) modifier = -1;
            var previouslyRemoved = new List<Vector>();
            for (int i = 0; i < SubCurves.Count + modifier; i++)
            {
                Curve crvA = SubCurves[i];
                Curve crvB = SubCurves.GetWrapped(i + 1);
                if (crvA is Line && crvB is Line)
                {
                    // Check perp distance of removal candidate to
                    // potential new line segment
                    Vector toEnd = crvB.EndPoint - crvA.StartPoint;
                    Vector perp = toEnd.PerpendicularXY().Unitize();
                    Vector toMidPt = crvB.StartPoint - crvA.StartPoint;
                    double dot = toMidPt.Dot(perp);
                    // Is the mid-point (and any previous) within tolerance to
                    // allow removal?
                    if (tolerance.Contains(dot) &&
                        AllInToleranceForReduction(previouslyRemoved, perp, tolerance))
                    {
                        crvA.End.Position = crvB.EndPoint;
                        SubCurves.Remove(crvB);
                        i--;
                        result++;
                        // Store to check any subsequent reductions involving
                        // crvA do not take the removed point out of tolerance
                        previouslyRemoved.Add(toMidPt);
                    }
                    else
                    {
                        // Staring afresh, no longer need to check against olds
                        previouslyRemoved.Clear();
                    }
                }
            }
            return result;
        }


        #endregion

        #region Static Methods

        /// <summary>
        /// Generates a rectangular polycurve on the XY plane centred on the origin
        /// </summary>
        /// <param name="depth">The depth of the rectangle</param>
        /// <param name="width">The width of the rectangle</param>
        /// <returns></returns>
        public static PolyCurve Rectangle(double depth, double width, double cornerRadius = 0.0)
        {
            cornerRadius = Math.Max(cornerRadius, 0.0);
            double x = width / 2;
            double y = depth / 2;
            double x2 = Math.Max(x - cornerRadius, 0.0);
            double y2 = Math.Max(y - cornerRadius, 0.0);

            PolyCurve result = new PolyCurve(new Line(x2,y,-x2, y));
            if (cornerRadius > 0) result.AddArcTangent(new Vector(-1, 0), new Vector(-x, y2));
            result.AddLine(-x, -y2);
            if (cornerRadius > 0) result.AddArcTangent(new Vector(0,-1), new Vector(-x2, -y));
            result.AddLine(x2, -y);
            if (cornerRadius > 0) result.AddArcTangent(new Vector(1,0), new Vector(x, -y2));
            result.AddLine(x, y2);
            if (cornerRadius > 0) result.AddArcTangent(new Vector(0,1), new Vector(x2, y));

            return result;
        }

        /// <summary>
        /// Generates a trapezoid polygon on the XY plane with the midpoint of each edge 
        /// centred on the origin and with a different top and bottom width.
        /// </summary>
        /// <param name="depth">The depth of the trapezoid</param>
        /// <param name="topWidth">The width of the top of the trapezoid</param>
        /// <param name="baseWidth">The width of the base of the trapezoid</param>
        /// <returns></returns>
        public static PolyCurve Trapezoid(double depth, double topWidth, double baseWidth)
        {
            double xT = topWidth / 2;
            double xB = baseWidth / 2;
            double y = depth / 2;

            PolyCurve result = new PolyCurve(new Line(xT, y, -xT, y));
            result.AddLine(-xB, -y);
            result.AddLine(xB, -y);
            result.AddLine(xT, y);

            return result;
        }

        #endregion
    }
}


