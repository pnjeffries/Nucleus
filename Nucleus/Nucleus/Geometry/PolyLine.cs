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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Maths;
using Nucleus.Base;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A curve consisting of straight lines between vertices.
    /// A polyline may be either open or closed - if closed the
    /// last vertex is assumed to have a line segment connecting
    /// it to the first vertex.
    /// </summary>
    [Serializable]
    public class PolyLine : Curve
    {
        #region Properties

        /// <summary>
        /// Whether this curve is closed.
        /// If true, the end of the curve is treated as being the same as the start point.
        /// Default is false.
        /// </summary>
        public override bool Closed { get; protected set; }

        /// <summary>
        /// Is this polyline valid?
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (Vertices.Count > 1) return true;
                else return false;
            }
        }

        /// <summary>
        /// Private backing field for Vertices collection
        /// </summary>
        private VertexCollection _Vertices;

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this polyline.
        /// The polyline will be defined as straight lines in between the vertices
        /// in this collection.
        /// </summary>
        public override VertexCollection Vertices
        {
            get { return _Vertices; }
        }

        /// <summary>
        /// Get an array of the points that define the geometry of this polyline
        /// </summary>
        public Vector[] Points
        {
            get
            {
                var result = new Vector[VertexCount];
                for (int i = 0; i < VertexCount; i++)
                {
                    result[i] = Vertices[i].Position;
                }
                return result;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PolyLine()
        {
            _Vertices = new VertexCollection(this);
        }

        /// <summary>
        /// Initialise a polyline curve between the given set of points
        /// </summary>
        /// <param name="points"></param>
        public PolyLine(params Vector[] points) : this(points, false, null) { }

        /// <summary>
        /// Initialise a polyline curve between the given set of points, optionally closing the loop
        /// </summary>
        /// <param name="close"></param>
        /// <param name="points"></param>
        public PolyLine(bool close, params Vector[] points) : this(points, close, null) { }

        /// <summary>
        /// Points constructor.
        /// Creates a polyline between the specified set of points
        /// </summary>
        /// <param name="points"></param>
        /// <param name="close"></param>
        public PolyLine(IEnumerable<Vector> points, bool close, GeometryAttributes attributes = null) : this()
        {
            foreach (Vector pt in points)
            {
                Vertices.Add(new Vertex(pt));
            }
            Closed = close;
            Attributes = attributes;
        }

        /// <summary>
        /// Create a PolyLine that is a copy of another PolyLine
        /// </summary>
        /// <param name="other"></param>
        public PolyLine(PolyLine other)
            : this(other.Points, other.Closed, other.Attributes)
        { }

        /// <summary>
        /// Points constructor.
        /// Creates a polyline between the specified set of points.
        /// Automatically closes the polyline (and removes the final point) if
        /// the last point at the first point are coincident.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="close"></param>
        public PolyLine(IEnumerable<Vector> points, GeometryAttributes attributes = null) : this()
        {
            foreach (Vector pt in points)
            {
                Vertices.Add(new Vertex(pt));
            }
            if (Vertices.Count > 1 && Vertices.First().DistanceToSquared(Vertices.Last()) <= Tolerance.Distance * Tolerance.Distance)
            {
                Vertices.RemoveAt(Vertices.Count - 1);
                Closed = true;
            }
            Attributes = attributes;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Close this polyline, so that a line segment joins the last vertex and the first one.
        /// </summary>
        /// <param name="close">If true, polyline will be made closed.  If false, will be made unclosed.</param>
        public void Close(bool close = true)
        {
            Closed = close;
        }

        /// <summary>
        /// Add a new point to the end of the polyline 
        /// (creating a new polyline segment if this is not the first point added)
        /// </summary>
        /// <param name="pt"></param>
        public Vertex Add(Vector pt)
        {
            Vertex result = new Vertex(pt);
            Vertices.Add(result);
            return result;
        }

        /// <summary>
        /// Set the positions and number of the vertices in this polyline to the
        /// specified set of positions
        /// </summary>
        /// <param name="pts"></param>
        public void SetPoints(IList<Vector> pts)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                if (Vertices.Count > i)
                {
                    // Update
                    Vertices[i].Position = pts[i];
                }
                else
                {
                    // Create
                    Add(pts[i]);
                }
            }
            // Remove extraneous vertices:
            for (int i = Vertices.Count - 1; i >= pts.Count; i--)
            {
                Vertices.RemoveAt(i);
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
        public override Curve Offset(IList<double> distances, bool tidy = true, bool copyAttributes = true)
        {
            IList<Vector> pts = new Vector[Vertices.Count]; //The offset points
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector p0 = Vector.Unset; // Last point
                Vector p1 = Vertices[i].Position; // This point
                Vector p2 = Vector.Unset; // Next point

                if (i > 0) p0 = Vertices[i - 1].Position;
                else if (Closed) p0 = Vertices.Last().Position;

                if (i < Vertices.Count - 1) p2 = Vertices[i + 1].Position;
                else if (Closed) p2 = Vertices.First().Position;

                Vector v1 = Vector.Unset;
                Vector v2 = Vector.Unset;

                if (p0.IsValid()) v1 = (p1 - p0).Unitize();
                if (p2.IsValid()) v2 = (p2 - p1).Unitize();

                double d1 = distances.GetWrapped(i - 1);
                double d2 = distances.GetBounded(i);

                if (v1.IsValid() && !v1.IsZero() && v2.IsValid() && !v2.IsZero() && !v1.IsParallelTo(v2))
                {
                    // Calculate intersection of offset lines:
                    Vector o1 = v1.PerpendicularXY() * d1;
                    Vector o2 = v2.PerpendicularXY() * d2;
                    Vector pOff = Intersect.LineLineXY(p0 + o1, v1, p2 + o2, v2);
                    pts[i] = pOff;
                    /*bool add = true;

                    if (tidy && pts.Count > 0)
                    {
                        // Check for segment inversion:
                        // TODO!
                        Vector v1B = (pOff - pts.Last());
                        if (v1B.Dot(v1) < 0)
                        {
                            // Segment flipped!
                            add = false;

                            //Adjust last point to lie on the intersection with the prior segment:

                        }
                    }
                    if (add) pts.Add(pOff);*/
                }
                else
                {
                    // Can't intersect, so translate instead
                    // Figure out which span to use:
                    Vector sV;
                    double d;
                    if (v1.IsValidNonZero())
                    {
                        sV = v1;
                        d = d1;
                    }
                    else
                    {
                        sV = v2;
                        d = d2;
                    }

                    if (sV.IsValid() && !sV.IsZero())
                    {
                        Vector pOff = p1 + sV.PerpendicularXY() * d;
                        pts[i] = pOff;
                    }
                    else pts[i] = p1; // Can't offset!
                }
            }

            if (tidy)
            {
                IList<Vector> tidyPts = new List<Vector>();

                int max = pts.Count;
                if (!Closed) max -= 1;
                // Post-processing step: remove collapsed segments
                for (int i = 0; i < max; i++)
                {
                    Vector vOff = pts.VectorToNext(i);
                    Vector vOri = Vertices.VectorToNext(i);
                    if (vOff.Dot(vOri) < 0) // Flipped!
                    {

                    }
                    //TODO
                }
            }

            return new PolyLine(pts, Closed, copyAttributes ? Attributes : null);
        }

        /// <summary>
        /// Convert this polycurve into an equivalent list
        /// of Line objects
        /// </summary>
        /// <returns></returns>
        public IList<Line> ToLines()
        {
            var result = new List<Line>(SegmentCount);
            for (int i = 0; i < SegmentCount; i++)
            {
                result.Add(new Geometry.Line(SegmentStart(i).Position, SegmentEnd(i).Position, Attributes));
            }
            return result;
        }


        /// <summary>
        /// Does this polyline self-intersect on the XY plane?
        /// </summary>
        /// <returns></returns>
        public override bool IsSelfIntersectingXY()
        {
            if (Vertices.Count < 4) return false;
            for (int i = 0; i < SegmentCount - 1; i++)
            {
                Vertex vA0 = Vertices[i];
                Vertex vA1 = Vertices[i + 1];
                for (int j = i + 2; j < SegmentCount; j++)
                {
                    if (!(Closed && i == 0 && j == SegmentCount - 1))
                    {
                        Vertex vB0 = Vertices[j];
                        Vertex vB1 = Vertices.GetWrapped(j + 1);
                        double t0 = 0;
                        double t1 = 0;
                        Vector intersection = Intersect.LineLineXY(vA0.Position, vA1.Position - vA0.Position,
                            vB0.Position, vB1.Position - vB0.Position, ref t0, ref t1);
                        if (intersection.IsValid() && t0 >= 0 && t0 <= 1 && t1 >= 0 && t1 <= 1)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Convert this polyline into a polycurve containing
        /// line objects representing the same geometry
        /// </summary>
        /// <returns></returns>
        public override PolyCurve ToPolyCurve(bool autoExplode = false)
        {
            return new PolyCurve(ToLines(), Attributes);
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
            result.AddRange(ToLines());
            return result;
        }

        
        /// <summary>
        /// Extract a portion of this curve as a new curve
        /// </summary>
        /// <param name="subDomain">The subdomain of this curve to
        /// be extracted as a new curve</param>
        /// <returns></returns>
        public override Curve Extract(Interval subDomain)
        {
            var pts = new List<Vector>();
            pts.Add(PointAt(subDomain.Start));
            double offset = 1;
            if (Closed) offset = 0;

            if (!subDomain.IsDecreasing)
            {
                for (int i = 0; i < VertexCount; i++)
                {
                    double t = ((double)i) / (VertexCount - offset);
                    if (t > subDomain.Start && t < subDomain.End)
                        pts.Add(Vertices[i].Position);
                }
            }
            else
            {
                //Loops!
                for (int i = 0; i < VertexCount; i++)
                {
                    if (((double)i) / (VertexCount - offset) > subDomain.Start)
                        pts.Add(Vertices[i].Position);
                }
                if (Closed) // Should this treat the polyline as closed even if it isn't?
                {
                    for (int i = 0; i < VertexCount; i++)
                    {
                        if (((double)i) / (VertexCount - offset) < subDomain.End)
                            pts.Add(Vertices[i].Position);
                    }
                }
            }
            pts.Add(PointAt(subDomain.End));
            return new PolyLine(pts, Attributes);
        }

        /// <summary>
        /// Decompose this curve down to simple primitive curve types such
        /// as line and arc segments.
        /// </summary>
        /// <returns></returns>
        public override IList<ISimpleCurve> ToSimpleCurves()
        {
            var result = new List<ISimpleCurve>();
            result.AddRange(ToLines());
            return result;
        }

        /// <summary>
        /// Automatically tidy up this polyline by removing any adjacent 
        /// duplicate vertices
        /// </summary>
        public bool Clean()
        {
            bool result = false;
            double limit = Tolerance.Distance * Tolerance.Distance;
            for (int i = SegmentCount; i > 0; i--)
            {
                // Check distance between adjacent vertices and remove
                // any that are closer than tolerance
                Vertex v0 = Vertices[i-1];
                Vertex v1 = Vertices.GetWrapped(i);
                if (v0.DistanceToSquared(v1) <= limit)
                {
                    Vertices.RemoveAt(i-1);
                    result = true;
                }  
            }
            return result;
        }

        /// <summary>
        /// Reduce this polyline by removing vertices where
        /// they can be adequately represented within tolerance by
        /// the line between the two adjoining vertices.
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
            for (int i = 0; i < Vertices.Count + modifier; i++)
            {
                Vertex vA = Vertices[i];
                Vertex vB = Vertices.GetWrapped(i + 1);
                Vertex vC = Vertices.GetWrapped(i + 2);

                // Check perp distance of removal candidate to
                // potential new line segment
                Vector toEnd = vC.Position - vA.Position;
                Vector perp = toEnd.PerpendicularXY().Unitize();
                Vector toMidPt = vB.Position - vA.Position;
                double dot = toMidPt.Dot(perp);
                // Is the mid-point (and any previous) within tolerance to
                // allow removal?
                if (tolerance.Contains(dot) &&
                    AllInToleranceForReduction(previouslyRemoved, perp, tolerance))
                {
                    int iRemove = (i + 1) % Vertices.Count;
                    Vertices.RemoveAt(iRemove);
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
            return result;
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
            int removeTo = 0;
            for (int i = 0; i < SegmentCount; i++)
            {
                double lS = CalculateSegmentLength(i);
                double l1 = l0 + lS;
                if (l1 > length)
                {
                    // Found trim point
                    removeTo = i;
                    Vertices[i].Position = PointAt(i, ((length - l0) / lS));
                    result = true;
                    break;
                }
                l0 = l1;
            }
            if (result)
            {
                //Remove preceding vertices
                for (int i = 0; i < removeTo; i++)
                {
                    if (Vertices.Count > 0) Vertices.RemoveAt(0);
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
            int removeTo = 0;
            for (int i = SegmentCount - 1; i >= 0; i--)
            {
                double lS = CalculateSegmentLength(i);
                double l1 = l0 + lS;
                if (l1 > length)
                {
                    // Found trim point
                    removeTo = i + 1;
                    Vertices[i + 1].Position = PointAt(i, 1 - ((length - l0) / lS));
                    result = true;
                    break;
                }
                l0 = l1;
            }
            if (result)
            {
                for (int i = Vertices.Count - 1; i > removeTo; i--)
                {
                    //Remove vertices after
                    if (Vertices.Count > 0) Vertices.RemoveAt(Vertices.Count - 1);
                }
            }
            return result;
        }

        /// <summary>
        /// Determine and return the spanIndex of the shortest segment in this polyline
        /// </summary>
        /// <returns></returns>
        public int ShortestSegment()
        {
            int shortest = 0;
            double minLength = CalculateSegmentLength(0);
            for (int i = 1; i < SegmentCount; i++)
            {
                double length = CalculateSegmentLength(i);
                if (length < minLength)
                {
                    minLength = length;
                    shortest = i;
                }
            }
            return shortest;
        }

        /// <summary>
        /// Trim the shortest segment which can be removed from this curve
        /// while still leaving a valid continuous curve.  This will be the shortest either
        /// the start or end segment in the case of open curves or the shortest segment in a closed
        /// curve.
        /// If the curve has only a single segment, nothing will be removed and the operation will fail.
        /// </summary>
        /// <returns>True if a segment was successfully removed.</returns>
        public override bool TrimShortestSegment()
        {
            if (Vertices.Count <= 2) return false;

            if (Closed)
            {
                int iShort = ShortestSegment();
                if (iShort == 0)
                {
                    Closed = false;
                    var startVert = Vertices[0];
                    Vertices.RemoveAt(0);
                    // Move to end to keep last span:
                    Vertices.Add(startVert);
                }
                else if (iShort == SegmentCount - 1)
                {
                    Closed = false;
                    // If closed, no need to remove a vertex, just toggle off the last span
                }
                else
                {
                    Closed = false;
                    // Re-order so that 'broken' end span is at location of shortest segment:
                    var endVerts = Vertices.SubListFrom(iShort + 1);
                    for (int i = Vertices.Count - 1; i > iShort ; i--)
                    {
                        Vertices.RemoveAt(i);
                    }
                    // Move previous end vertices to start:
                    for (int i = endVerts.Count - 1; i >= 0; i--)
                    {
                        Vertices.Insert(0, endVerts[i]);
                    }
                }
            }
            else // Open curves
            {
                if (CalculateSegmentLength(0) < CalculateSegmentLength(SegmentCount - 1))
                {
                    Vertices.RemoveFirst();
                }
                else
                {
                    Vertices.RemoveLast();
                }
            }

            return true;
        }

        public override string ToString()
        {
            return "Polyline";
        }

        protected override IFastDuplicatable CurveFastDuplicate()
        {
            return new PolyLine(this);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Static factory method to create a polyline representing a trapezoid centred on the
        /// origin of the XY plane.  If depth or both widths are 0 or lower, null will be returned instead.
        /// </summary>
        /// <param name="depth">The depth of the trapezoid</param>
        /// <param name="topWidth">The width of the top of the trapezoid</param>
        /// <param name="baseWidth">The width of the base of the trapezoid</param>
        /// <returns></returns>
        public static PolyLine Trapezoid(double depth, double topWidth, double baseWidth)
        {
            if (depth > 0 && (topWidth > 0 || baseWidth > 0))
            {
                topWidth = Math.Max(0, topWidth);
                baseWidth = Math.Max(0, baseWidth);
                return new PolyLine(new Vector[]
                {
                    new Vector(topWidth/2, depth/2),
                    new Vector(-topWidth/2, depth/2),
                    new Vector(-baseWidth/2, -depth/2),
                    new Vector(baseWidth/2, -depth/2)
                }, true);
            }
            else return null;
        }

        /// <summary>
        /// Static factory method to create a polyline representing a rectangle centred on the
        /// origin on the XY plane.  If the depth or width are 0 or lower null will be returned instead.
        /// </summary>
        /// <param name="depth">The depth of the rectangle</param>
        /// <param name="width">The width of the rectangle</param>
        /// <returns>A polyline representing the specified rectangle with vertices arranged in an
        /// anti-clockwise manner, or null is the input depth and width are invalid.</returns>
        public static PolyLine Rectangle(double depth, double width)
        {
            if (depth > 0 && width > 0)
            {
                return new PolyLine(new Vector[]
                {
                new Vector(width/2, depth/2),
                new Vector(-width/2, depth/2),
                new Vector(-width/2, -depth/2),
                new Vector(width/2, -depth/2)
                }, true);
            }
            else return null;
        }

        /// <summary>
        /// Static factory method to create a polyline representing a rectangle with the specified
        /// corner point coordinates.
        /// </summary>
        /// <returns>A polyline representing the specified rectangle with vertices arranged in an
        /// anti-clockwise manner, or null is the input depth and width are invalid.</returns>
        public static PolyLine Rectangle(double x0, double y0, double x1, double y1)
        {
            return new PolyLine(new Vector[]
            {
                new Vector(x0, y0),
                new Vector(x1, y0),
                new Vector(x1, y1),
                new Vector(x0, y1),
            }, true);
        }

        /// <summary>
        /// Static factory method to create a polyline representing a rectangle with the specified
        /// corner point coordinates.
        /// </summary>
        /// <returns>A polyline representing the specified rectangle with vertices arranged in an
        /// anti-clockwise manner, or null is the input depth and width are invalid.</returns>
        public static PolyLine Rectangle(double x0, double y0, double x1, double y1, ICoordinateSystem cSystem)
        {
            return new PolyLine(new Vector[]
            {
                cSystem.LocalToGlobal(new Vector(x0, y0)),
                cSystem.LocalToGlobal(new Vector(x1, y0)),
                cSystem.LocalToGlobal(new Vector(x1, y1)),
                cSystem.LocalToGlobal(new Vector(x0, y1)),
            }, true);
        }

        /// <summary>
        /// Static factory method to create a polyline representing a rectangle on the XY plane between the two specified
        /// corner points.
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static PolyLine Rectangle(Vector v0, Vector v1)
        {
            return Rectangle(Math.Min(v0.X, v1.X), Math.Min(v0.Y, v1.Y), Math.Max(v0.X, v1.X), Math.Max(v0.Y, v1.Y));
        }

        #endregion
    }
}
