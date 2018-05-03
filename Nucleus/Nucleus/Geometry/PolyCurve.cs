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
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (SubCurves.Count > 0)
                {
                    foreach (Curve subCrv in SubCurves)
                    {
                        if (!subCrv.IsValid) return false;
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
        public PolyCurve() { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PolyCurve(GeometryAttributes attributes = null)
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

        #endregion

        #region Methods

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
        /// Offset this curve on the XY plane by varying distances for
        /// each span.
        /// </summary>
        /// <param name="distances">The offset distance.
        /// Positive numbers will result in the offset curve being to the right-hand 
        /// side, looking along the curve.  Negative numbers to the left.</param>
        /// <param name="tidy">If true (default) collapsed segments will be removed.</param>
        /// <returns></returns>
        public override Curve Offset(IList<double> distances, bool tidy = true)
        {
            //TODO: Implement collapsed segments tidying

            var result = new PolyCurve();
            int distIndex = 0;

            // Offset sub curves:
            foreach (Curve crv in SubCurves)
            {
                Curve offsetCrv = crv.Offset(distances.SubListFrom(distIndex));
                distIndex += crv.SegmentCount;
                if (distIndex > distances.Count - 1) distIndex = distances.Count - 1;

                if (offsetCrv != null)
                {
                    if (result.SubCurves.Count > 0)
                    {
                        // Adjust offset curve ends to node out
                        Curve prevCrv = result.SubCurves.Last();
                        MatchEnds(prevCrv.End, offsetCrv.Start);
                    }
                    result.Add(offsetCrv);
                }
            }

            // Match end to start
            if (Closed && result.SubCurves.Count > 1)
                MatchEnds(result.SubCurves.Last().End, result.SubCurves.First().Start);

            if (tidy)
            {
                List<Curve> originals = new List<Curve>();
                originals.AddRange(SubCurves);

                // Removed flipped segments
                int j = 0;
                while (j < result.SubCurves.Count)
                {
                    Curve offset = result.SubCurves[j];
                    Curve original = originals[j];

                    if (IsOffsetCurveFlipped(original, offset))
                    {
                        if (result.SubCurves.Count > 0)
                        {
                            Curve previous = result.SubCurves.GetWrapped(j - 1);
                            Curve subsequent = result.SubCurves.GetWrapped(j + 1);
                            MatchEnds(previous.End, subsequent.Start);
                        }
                        result.SubCurves.RemoveAt(j);
                        originals.RemoveAt(j);
                        //j--;
                    }
                    else
                        j++;
                }
            }

            return result;
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
                if (subDomain.Contains(crvDom)) result.Add(subCrv.Duplicate());
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
                    if (chuck != null && chuck.Length > 0) return true;
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


