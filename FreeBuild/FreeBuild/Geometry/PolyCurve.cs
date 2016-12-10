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
    /// A curve formed of several continuous curves joined together.
    /// </summary>
    [Serializable]
    public class PolyCurve : Curve
    {
        #region Properties

        /// <summary>
        /// The sub-curves of this PolyCurve
        /// </summary>
        public CurveCollection SubCurves { get; } = new CurveCollection();

        /// <summary>
        /// Whether this curve is closed.
        /// If true, the end of the curve is treated as being the same as the start point.
        /// Default (for most curve types) is false.
        /// </summary>
        public override bool Closed
        {
            get
            {
                return StartPoint.Equals(EndPoint, Tolerance.Geometric);
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
        public PolyCurve()
        {

        }

        /// <summary>
        /// Curve constructor.  Initialises a polycurve starting with the specifed curve
        /// </summary>
        /// <param name="curve"></param>
        public PolyCurve(Curve curve)
        {
            SubCurves.Add(curve);
        }

        /// <summary>
        /// Initialises a new polycurve 
        /// </summary>
        /// <param name="curves"></param>
        public PolyCurve(IEnumerable<Curve> curves)
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
                result += XYAreaUnder(start.X, start.Y, end.X, end.Y, ref centroid);
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

        #endregion

        #region Static Methods

        /// <summary>
        /// Generates a rectangular polycurve on the XY plane centred on the origin
        /// </summary>
        /// <param name="depth">The depth of the rectangle</param>
        /// <param name="width">The width of the rectangle</param>
        /// <returns></returns>
        public static PolyCurve Rectangle(double depth, double width)
        {
            double x = width / 2;
            double y = depth / 2;

            PolyCurve result = new PolyCurve(new Line(x,y,-x, y));
            result.AddLine(-x, -y);
            result.AddLine(x, -y);
            result.AddLine(x, y);

            return result;
        }

        #endregion
    }
}
