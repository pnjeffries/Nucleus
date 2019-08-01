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
    /// A planar surface described by a perimeter boundary curve and (optionally)
    /// a set of 'cut out' perimeter voids.
    /// </summary>
    [Serializable]
    public class PlanarRegion : Surface
    {
        #region Properties

        /// <summary>
        /// Private backing field for Perimeter property
        /// </summary>
        [Copy(CopyBehaviour.DUPLICATE)]
        private Curve _Perimeter;

        /// <summary>
        /// The outer perimeter boundary curve of this surface
        /// </summary>
        public Curve Perimeter
        {
            get { return _Perimeter; }
            set
            {
                _Perimeter = value;
                NotifyGeometryUpdated();
                NotifyPropertyChanged("Perimeter");
            }
        }

        /// <summary>
        /// Private backing field for Voids property
        /// </summary>
        [CollectionCopy(CopyBehaviour.DUPLICATE, CopyBehaviour.DUPLICATE)]
        private CurveCollection _Voids;

        /// <summary>
        /// The collection of curves that describe the boundaries of void regions
        /// 'cut out' of this surface
        /// </summary>
        public CurveCollection Voids
        {
            get
            {
                if (_Voids == null) _Voids = new CurveCollection();
                return _Voids;
            }
        }

        /// <summary>
        /// Does this surface have any voids defined?
        /// </summary>
        public bool HasVoids
        {
            get
            {
                return !(_Voids == null || _Voids.Count == 0);
            }
        }

        /// <summary>
        /// Private backing field for Plane property
        /// </summary>
        private Plane _Plane;

        /// <summary>
        /// The plane of this surface.
        /// This may be null if the definition is not valid.
        /// </summary>
        public Plane Plane
        {
            get
            {
                if (_Plane == null && Perimeter != null)
                {
                    _Plane = Perimeter.Plane();
                }
                return _Plane;
            }
        }

        /// <summary>
        /// Is this region valid?
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return (Perimeter != null && Perimeter.IsValid && Perimeter.Closed && Plane != null);
            }
        }

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// Different shapes will provide different means of editing this collection.
        /// DO NOT directly modify the collection returned from this property unless you are
        /// sure you know what you are doing.
        /// For PlanarSurfaces, this will generate a combined collection of all definition curve
        /// vertices.
        /// </summary>
        public override VertexCollection Vertices
        {
            get
            {
                if (Voids != null && Voids.Count > 0)
                {
                    VertexCollection combined = new VertexCollection();
                    combined.AddRange(Perimeter.Vertices);
                    foreach (Curve voidCrv in Voids)
                    {
                        combined.AddRange(voidCrv.Vertices);
                    }
                    //TODO: Additional vertices?
                    return combined;
                }
                else return Perimeter.Vertices;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new PlanarSurface with no geometry.
        /// </summary>
        public PlanarRegion() { }

        /// <summary>
        /// Initialises a new quadrangular PlanarRegion with the specified corner points.
        /// The four points specified should all lie on the same plane for the resulting surface to be valid.
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pt3"></param>
        /// <param name="attributes"></param>
        public PlanarRegion(Vector pt0, Vector pt1, Vector pt2, Vector pt3, GeometryAttributes attributes = null)
            : this(new PolyLine(new Vector[] { pt0, pt1, pt2, pt3 }, true, attributes)) { }

        /// <summary>
        /// Initialises a new PlanarRegion with the specified edge vertices.
        /// There should be a minimum of three points given and all should lie on the same
        /// plane for the resulting surface to be valid.
        /// </summary>
        /// <param name="pts"></param>
        public PlanarRegion(params Vector[] pts)
            : this(new PolyLine(pts, true)) { }

        /// <summary>
        /// Initialises a new PlanarSurface with the specified perimeter curve.
        /// </summary>
        /// <param name="perimeter"></param>
        public PlanarRegion(Curve perimeter, GeometryAttributes attributes = null)
        {
            _Perimeter = perimeter;
            Attributes = attributes;
        }

        /// <summary>
        /// Initialises a new PlanarSurface with the specified perimeter and void curves.
        /// </summary>
        /// <param name="perimeter"></param>
        /// <param name="voids"></param>
        public PlanarRegion(Curve perimeter, CurveCollection voids, GeometryAttributes attributes = null) : this(perimeter, attributes)
        {
            _Voids = voids;
        }

        #endregion

        #region Methods

        protected override void InvalidateCachedGeometry()
        {
            base.InvalidateCachedGeometry();
            _Plane = null;
        }

        /// <summary>
        /// Calculate the area of this region
        /// </summary>
        /// <param name="centroid"></param>
        /// <returns></returns>
        public override double CalculateArea(out Vector centroid)
        {
            if (_Perimeter != null)
            {
                Plane plane = Plane;
                if (plane != null)
                {
                    double area = _Perimeter.CalculateEnclosedArea(out centroid, _Voids, plane);
                    centroid = plane.LocalToGlobal(centroid);
                    return area;
                }
            }

            centroid = Vector.Unset;
            return 0;
        }

        public override string ToString()
        {
            return "Planar Region";
        }

        public override CartesianCoordinateSystem LocalCoordinateSystem(int i, double u, double v, Angle orientation, Angle xLimit)
        {
            //TODO: Offset according to u, v coordinates?
            Plane result = Perimeter.Plane();
            Vector alignX = Vector.UnitX;
            Angle angleBetween = result.Z.AngleBetween(alignX);
            if (angleBetween <= xLimit || angleBetween >= Angle.Straight - xLimit) alignX = Vector.UnitY;
            Angle toOrient = result.X.AngleBetween(alignX);
            return result.Rotate(toOrient + orientation);
        }
        
        /// <summary>
        /// Does the specified point fall within this region on the XY plane?
        /// </summary>
        /// <param name="pt">The point to test</param>
        /// <returns></returns>
        public bool ContainsXY(Vector pt)
        {
            if (Perimeter.EnclosesXY(pt))
            {
                if (_Voids != null)
                    foreach (var voidCrv in _Voids)
                        if (voidCrv.EnclosesXY(pt)) return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Perform a boolean not (subtraction) operation on this region
        /// </summary>
        /// <param name="cutter"></param>
        /// <returns></returns>
        private IList<PlanarRegion> Not(PlanarRegion cutter)
        {
            var result = new List<PlanarRegion>();

            IList<CurveCurveIntersection> perimeterInts = Intersect.CurveCurveXYIntersections(Perimeter, cutter.Perimeter);
            IList<CurveCurveIntersection> cutterInts = new List<CurveCurveIntersection>();
            foreach (var cCI in perimeterInts)
            {
                cCI.ProcessCounter = 1;
                cutterInts.Add(cCI);
            }
            // TODO: cutter void intersection

            CurveCollection freeVoids = new CurveCollection(); //Un-cut voids
            // Find intersection points on voids:
            foreach (var voidCrv in Voids)
            {
                var voidInts = Intersect.CurveCurveXYIntersections(voidCrv, cutter.Perimeter);
                foreach (var cCI in voidInts)
                {
                    cCI.ProcessCounter = 1;
                    cutterInts.Add(cCI);
                }
                // TODO: cutter void intersection

                if (voidInts.Count < 2)
                {
                    freeVoids.Add(voidCrv);
                }
            }

            // Determine direction of travel around cutter:
            int cutterDir = 1; // TODO: Determine

            //'Fake' intersection to represent perimeter start:
            CurveCurveIntersection startInt = new CurveCurveIntersection(Perimeter, null, 0, double.NaN)
            { ProcessCounter = 1 };
            // ...and end:
            CurveCurveIntersection endInt = startInt;

            CurveCurveIntersection currentInt = startInt;
            IList<CurveCurveIntersection> nextStartInts = new List<CurveCurveIntersection>();
            PolyCurve newPerimeter = null;
            PlanarRegion newRegion = null;
            Curve travelOn = Perimeter;
            // Loop through intersections and build up new sub-regions by traversing connected
            // intersections along the perimeter, cutter and void curves
            int segmentCount = (int)Math.Floor(cutterInts.Count * 1.5);
            for (int i = 0; i < segmentCount; i++)
            {
                CurveCurveIntersection nextInt;

                // Travel to the next intersection, along he cutters or perimeter
                if (travelOn == Perimeter)
                {
                    // Travel along perimeter
                    nextInt = perimeterInts.ItemWithNext(cCI => cCI.ParameterA, currentInt.ParameterA);
                    // 'Fake' intersection to represent perimeter end:
                    if (nextInt == null) nextInt = endInt;
                    else nextStartInts.Add(nextInt);
                }
                else
                {
                    // Travel along cutter:
                    if (cutterDir == 1)
                    {
                        nextInt = cutterInts.ItemWithNext(cCI => cCI.ParameterB, currentInt.ParameterB);
                        if (nextInt == null) // Wrap
                            nextInt = cutterInts.ItemWithMin(cCI => cCI.ParameterB);
                    }
                    else
                    {
                        nextInt = cutterInts.ItemWithPrevious(cCI => cCI.ParameterB, currentInt.ParameterB);
                        if (nextInt == null) // Wrap
                            nextInt = cutterInts.ItemWithMax(cCI => cCI.ParameterB);
                    }
                }

                if (nextInt != null
                    && (currentInt.ProcessCounter > 0 || nextInt.ProcessCounter > 0))
                {
                    Curve curve;
                    if (travelOn == Perimeter)
                    {
                        // Perimeter segment
                        curve = Perimeter;
                    }
                    else if (currentInt.CurveA == nextInt.CurveA && currentInt.CurveA != Perimeter)
                    {
                        // Void segment
                        curve = currentInt.CurveA;
                    }
                    else
                    {
                        // Cutter segment
                        curve = currentInt.CurveB;
                    }
                    //TODO: Check if inside
                }
                // TODO
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Determine which side of a splitting line a given point lies.
        /// Used during region splitting.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="leftPt"></param>
        /// <param name="rightPt"></param>
        /// <param name="rightDir"></param>
        /// <returns></returns>
        private static HandSide SideOfSplit(Vector point,Vector leftPt, Vector rightPt, Vector rightDir)
        {
            double ptDot = point.Dot(rightDir);
            double lDot = leftPt.Dot(rightDir);
            double rDot = rightPt.Dot(rightDir);
            if (ptDot < lDot) return HandSide.Left;
            else if (ptDot > rDot) return HandSide.Right;
            else return HandSide.Undefined;
        }

        /// <summary>
        /// Work out the direction of travel that should be taken along the right splitting line
        /// in order to close split off subregions.
        /// The inverse will apply on the left splitting line.
        /// </summary>
        /// <returns></returns>
        private static int RightCutterDirectionOfTravel(IList<CurveLineIntersection> perimeterInts, Vector rightDir, bool hasWidth = true)
        {
            int result = -1;
            CurveLineIntersection firstInt = perimeterInts.ItemWithMin(pInt => pInt.CurveParameter);
            CurveLineIntersection lastInt = perimeterInts.ItemWithMax(pInt => pInt.CurveParameter);
            if (hasWidth)
            {
                if (firstInt.Side != lastInt.Side)
                {
                    // Special case: Start point is between cutting lines
                    CurveLineIntersection nextInt = perimeterInts.ItemWithNext(pInt => pInt.CurveParameter, firstInt.CurveParameter);
                    if (firstInt.Side == HandSide.Left) result *= -1;
                    if (nextInt.LineParameter < firstInt.LineParameter) result *= -1;
                }
                else
                {
                    //TODO: Other condition?
                    if (lastInt.LineParameter < firstInt.LineParameter) result *= -1;
                    if (firstInt.Side == HandSide.Right) result *= -1;
                }
            }
            else
            {
                Vector firstTan = firstInt.Curve.TangentAt(firstInt.CurveParameter);
                double dot = firstTan.Dot(rightDir);
                if (dot < 0) result *= -1;
                if (lastInt.LineParameter < firstInt.LineParameter) result *= -1;
            }
            return result;
        }

        /// <summary>
        /// Split this region into two (or more) sub-regions along a straight line
        /// </summary>
        /// <param name="splitPt">A point on the splitting line</param>
        /// <param name="splitDir">The direction of the line</param>
        /// <param name="splitWidth">Optional.  The width of the split.</param>
        /// <returns>The resultant list of regions.  If the line does not bisect
        /// this region and the region could not be split, this collection will contain
        /// only the original region.</returns>
        public IList<PlanarRegion> SplitByLineXY(Vector splitPt, Vector splitDir, double splitWidth = 0)
        {
            var result = new List<PlanarRegion>();

            // Offset splitting line by half splitWidth in each direction:
            Vector offsetDir = splitDir.PerpendicularXY().Unitize();
            Vector offset = offsetDir * splitWidth / 2;
            Vector leftPt = splitPt - offset;
            Vector rightPt = splitPt + offset;

            // Find intersection points on perimeter:
            var perimeterInts = new List<CurveLineIntersection>();

            // Left side:
            IList<CurveLineIntersection> leftInts = Intersect.CurveLineXYIntersections(Perimeter, leftPt, splitDir);
            perimeterInts.AddRange(leftInts);

            IList<CurveLineIntersection> rightInts;
            // Shortcut: If the split has no width, left and right intersections will be the same
            if (splitWidth == 0) rightInts = leftInts;
            else
            {
                rightInts = Intersect.CurveLineXYIntersections(Perimeter, rightPt, splitDir);
                perimeterInts.AddRange(rightInts);
            }

            int processesPerInt = 1;
            if (splitWidth == 0) processesPerInt = 2;
            foreach (var pI in perimeterInts) pI.ProcessCounter = processesPerInt;

            // If no intersections on the perimeter, the region does not need to be split:
            if (leftInts.Count <= 1 && rightInts.Count <= 1)
            {
                result.Add(this);
                return result;
            }

            // Voids which do not intersect but will need to be placed:
            CurveCollection freeVoids = new CurveCollection();
            // Find intersection points on voids:
            foreach (var voidCrv in Voids)
            {
                // Left side:
                var leftVoidInts = Intersect.CurveLineXYIntersections(voidCrv, leftPt, splitDir);
                foreach (var vI in leftVoidInts)
                {
                    vI.ProcessCounter = processesPerInt;
                    leftInts.Add(vI);
                }

                IList<CurveLineIntersection> rightVoidInts;
                // Shortcut: If the split has no width, left and right intersections will be the same
                if (splitWidth == 0) rightVoidInts = leftVoidInts;
                else rightVoidInts = Intersect.CurveLineXYIntersections(voidCrv, rightPt, splitDir);
                foreach (var vI in rightVoidInts)
                {
                    vI.ProcessCounter = processesPerInt;
                    rightInts.Add(vI);
                }

                if (leftVoidInts.Count <= 1 && rightVoidInts.Count <= 1)
                {
                    // Void is wholly to one side or the other...
                    freeVoids.Add(voidCrv);
                }
            }

            // Tag left and right intersections
            foreach (var lI in leftInts) lI.Side = HandSide.Left;
            foreach (var rI in rightInts) rI.Side = HandSide.Right;

            // Determine direction of travel along cutting lines:
            int rightDir = RightCutterDirectionOfTravel(perimeterInts, offsetDir, splitWidth != 0);

            // 'Fake' intersection to represent perimeter start:
            CurveLineIntersection startInt = new CurveLineIntersection(Perimeter, 0, double.NaN)
            { ProcessCounter = 1 };
            // ...and end:
            CurveLineIntersection endInt = startInt;
                //new CurveLineIntersection(Perimeter, 1, double.NaN)
            //{ ProcessCounter = 1 };

            // Loop through intersections and build up new sub-regions by traversing connected
            // intersections along the perimeter, cutting lines and void curves
            CurveLineIntersection currentInt = startInt;
            IList<CurveLineIntersection> nextStartInts = new List<CurveLineIntersection>();
            PolyCurve newPerimeter = null;
            PlanarRegion newRegion = null;
            // The cutter to travel along (Undefined is perimeter or void cuve):
            HandSide travelSide = HandSide.Undefined;
            HandSide lastSide = HandSide.Undefined; // The last side a curve was extracted from
            int segmentCount = (int)Math.Floor((leftInts.Count + rightInts.Count) * 1.5); //TODO: Include void intersections
            for (int i = 0; i < segmentCount; i++) // Loop through intersections
            {
                CurveLineIntersection nextInt;
                HandSide side = HandSide.Undefined;

                if (currentInt == null) break;

                // Travel to next intersection, along the cutters or perimeter
                if (travelSide == HandSide.Left)
                {
                    // Travel along left cutter
                    if (rightDir == 1)
                        nextInt = leftInts.ItemWithPrevious(cLI => cLI.LineParameter, currentInt.LineParameter);
                    else
                        nextInt = leftInts.ItemWithNext(cLI => cLI.LineParameter, currentInt.LineParameter);

                }
                else if (travelSide == HandSide.Right)
                {
                    // Travel along right cutter
                    if (rightDir == 1)
                        nextInt = rightInts.ItemWithNext(cLI => cLI.LineParameter, currentInt.LineParameter);
                    else
                        nextInt = rightInts.ItemWithPrevious(cLI => cLI.LineParameter, currentInt.LineParameter);
                }
                else
                {
                    // Travel to next intersection on perimeter
                    nextInt = perimeterInts.ItemWithNext(cLI => cLI.CurveParameter, currentInt.CurveParameter);
                    // 'Fake' intersection to represent perimeter end:
                    if (nextInt == null) nextInt = endInt;
                    else nextStartInts.Add(nextInt);
                }

                if (nextInt != null && currentInt.Curve == nextInt.Curve &&  // Check both ints on same curve
                    (travelSide == HandSide.Undefined || currentInt.Curve != Perimeter)
                    && (currentInt.ProcessCounter > 0 || nextInt.ProcessCounter > 0))
                {
                    Curve curve = nextInt.Curve;
                    Interval subDomain = new Interval(currentInt.CurveParameter, nextInt.CurveParameter);
                    Vector crvPt = curve.PointAt(curve.ParameterAtMid(subDomain));
                    side = SideOfSplit(crvPt, leftPt, rightPt, offsetDir);
                    if (side != HandSide.Undefined)
                    {
                        bool reverse = false;
                        // TODO: Adjust void curve direction
                        if (curve != Perimeter)
                        {
                            if (side != lastSide)
                            {
                                subDomain = subDomain.Invert();
                                reverse = true;
                            }
                            side = lastSide;
                        }
                        else
                        {
                            lastSide = side;
                        }

                        // Add perimeter to outputs
                        Curve subCrv = curve.Extract(subDomain);
                        if (reverse) subCrv.Reverse();
                        if (newPerimeter == null)
                        {
                            newPerimeter = new PolyCurve(subCrv.Explode());
                            newRegion = new PlanarRegion(newPerimeter);
                            result.Add(newRegion);
                        }
                        else newPerimeter.Add(subCrv, true, true);
                    }
                    else
                    {
                        //Traversing the gap, need a new start Point
                        startInt = nextInt;
                    }
                }



                if (nextInt == null || nextInt == startInt || //nextInt.CurveParameter >= 1 ||
                    nextInt.ProcessCounter <= 0)
                {
                    if (nextInt != null) nextInt.ProcessCounter -= 1;
                    // Reset to start next segment
                    nextInt = nextStartInts.FirstOrDefault();
                    nextStartInts.RemoveFirst();
                    startInt = nextInt;

                    if (newPerimeter != null) newPerimeter.Close();
                    newPerimeter = null;
                    newRegion = null;

                    travelSide = HandSide.Undefined;
                }
                else
                { 
                    if (nextInt != null) nextInt.ProcessCounter -= 1;

                    if (travelSide != HandSide.Undefined &&
                    (nextInt == null || nextInt.Curve == Perimeter))
                    {
                        travelSide = HandSide.Undefined;
                    }
                    else if (side != HandSide.Undefined)
                    {
                        travelSide = side;//nextInt.Side;
                    }
                    else if (splitWidth == 0)
                    {
                        travelSide = lastSide;
                    }
                    else if (nextInt != null)
                    {
                        travelSide = nextInt.Side;
                    }
                }

                //if (currentInt != null) currentInt.ProcessCounter -= 1;
                currentInt = nextInt;
            }
            if (newPerimeter != null) newPerimeter.Close();

            //TODO: Assign un-intersected voids
            foreach (var voidCrv in freeVoids)
            {
                foreach (var region in result)
                {
                    if (region.ContainsXY(voidCrv.StartPoint))
                    {
                        region.Voids.Add(voidCrv);
                        break;
                    }
                }
            }


            return result;

        }

        /// <summary>
        /// Split this region into two (or more) sub-regions along a straight line
        /// </summary>
        /// <param name="splitPt">A point on the splitting line</param>
        /// <param name="splitDir">The direction of the line</param>
        /// <param name="splitWidth">Optional.  The width of the split.</param>
        /// <returns>The resultant list of regions.  If the line does not bisect
        /// this region and the region could not be split, this collection will contain
        /// only the original region.</returns>
        private IList<PlanarRegion> SplitByLineXY_OLD(Vector splitPt, Vector splitDir, double splitWidth = 0)
        {
            var result = new List<PlanarRegion>();
            var lineInts = new List<double>();
            var outerInts = Intersect.CurveLineXY(Perimeter, splitPt, splitDir, null, 0, 1, false, lineInts);

            if (outerInts.Count > 1)
            {
                // Sort intersections by position along curve:
                var sortedInts = new SortedList<double, double>(outerInts.Count);
                for (int i = 0; i < outerInts.Count; i++)
                {
                    sortedInts.Add(outerInts[i], lineInts[i]);
                }

                outerInts = sortedInts.Keys.ToList();
                lineInts = sortedInts.Values.ToList();
                int offset = lineInts.IndexOfMin();
                outerInts.Shift(offset);
                lineInts.Shift(offset);

                // Create segments data structure
                var segments = new List<PerimeterSegment>(outerInts.Count - 1);
                for (int i = 0; i < outerInts.Count; i++)
                {
                    double t0 = outerInts[i];
                    double t1 = outerInts.GetWrapped(i + 1);
                    double tC0 = lineInts[i];
                    double tC1 = lineInts.GetWrapped(i + 1);
                    var segment = new PerimeterSegment(Perimeter, t0, t1, tC0, tC1);
                    segments.Add(segment);
                }

                //TODO: void intersections

                bool backwards = true;
                while (segments.Count > 0)
                {
                    var offsets = new List<double>();
                    PerimeterSegment segment = segments.First();
                    
                    PolyCurve newPerimeter = segment.Extract().ToPolyCurve();
                    for (int i = 0; i < newPerimeter.SegmentCount; i++) offsets.Add(0);
                    PerimeterSegment nextSegment = FindNextPerimeterSegment(segments, segment.CutterDomain.End, backwards);
                    while (nextSegment != null && nextSegment != segment)
                    {
                        Curve nextCurve = nextSegment.Extract();
                        newPerimeter.AddLine(nextCurve.StartPoint);
                        offsets.Add(splitWidth / 2);
                        newPerimeter.Add(nextCurve);
                        for (int i = 0; i < nextCurve.SegmentCount; i++) offsets.Add(0);
                        segments.Remove(nextSegment);
                        nextSegment = FindNextPerimeterSegment(segments, nextSegment.CutterDomain.End, backwards);
                    }
                    segments.RemoveAt(0);
                    if (!newPerimeter.Closed)
                    {
                        /*if (splitWidth > 0)
                        {
                            // Temporary bodge to get rid of 'blades' at ends of split
                            Vector endToEnd = (newPerimeter.StartPoint - newPerimeter.EndPoint).Unitize();
                            var line = new Line(
                                newPerimeter.EndPoint - endToEnd * splitWidth / 4,
                                newPerimeter.StartPoint + endToEnd * splitWidth / 4);
                            newPerimeter.AddLine(line.StartPoint);
                            offsets.Add(0);
                            newPerimeter.Add(line);
                            offsets.Add(splitWidth / 2);
                            newPerimeter.Close();
                            offsets.Add(0);
                        }
                        else
                        {*/
                            newPerimeter.Close();
                            offsets.Add(splitWidth / 2);
                        //}
                    }
                    backwards = !backwards;

                    if (splitWidth > 0)
                    {
                        var newNewPerimeter = newPerimeter.OffsetInwards(offsets);
                        // Check offset has not inverted perimeter:
                        // TODO: Do this automatically when offsetting?
                        if (newNewPerimeter != null && newNewPerimeter.IsClockwiseXY() == newPerimeter.IsClockwiseXY())
                        {
                            newPerimeter = newNewPerimeter.ToPolyCurve();
                        }
                        else newPerimeter = null;
                    }

                    if (newPerimeter != null) result.Add(new PlanarRegion(newPerimeter, Attributes?.Duplicate()));
                }

                // OLD VERSION:
                /*for (int i = 0; i < outerInts.Count; i++)
                {
                    double t0 = outerInts[i];
                    double t1 = outerInts.GetWrapped(i + 1);
                    Curve newPerimeter = Perimeter.Extract(new Interval(t0, t1))?.ToPolyCurve();
                    //TODO: Cut through and include voids
                    if (!newPerimeter.Closed)
                    {
                        ((PolyCurve)newPerimeter).Close();
                        if (splitWidth > 0)
                        {
                            var offsets = new double[newPerimeter.SegmentCount];
                            offsets[offsets.Length - 1] = splitWidth / 2;
                            var newNewPerimeter = newPerimeter.OffsetInwards(offsets);
                            // Check offset has not inverted perimeter:
                            // TODO: Do this automatically when offsetting?
                            if (newNewPerimeter != null && newNewPerimeter.IsClockwiseXY() == newPerimeter.IsClockwiseXY())
                            {
                                newPerimeter = newNewPerimeter;
                            }
                            else newPerimeter = null;
                        }
                    }
                    if (newPerimeter != null) result.Add(new PlanarRegion(newPerimeter, Attributes?.Duplicate()));
                }*/
            }
            else
            {
                result.Add(this); //Return the original
            }
            return result;
        }

        /// <summary>
        /// Find the next connecting perimeter segment along the cutting curve
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="tFrom"></param>
        /// <param name="backwards"></param>
        /// <returns></returns>
        private static PerimeterSegment FindNextPerimeterSegment(IList<PerimeterSegment> segments, double tFrom, bool backwards)
        {
            PerimeterSegment next = null;
            if (backwards)
            {
                foreach (var pSeg in segments)
                {
                    if (pSeg.CutterDomain.Start < tFrom)
                    {
                        if (next == null || next.CutterDomain.Start < pSeg.CutterDomain.Start)
                            next = pSeg;
                    }
                }
            }
            else
            {
                foreach (var pSeg in segments)
                {
                    if (pSeg.CutterDomain.Start > tFrom)
                    {
                        if (next == null || next.CutterDomain.Start > pSeg.CutterDomain.Start)
                            next = pSeg;
                    }
                }
            }
            return next;
        }

        #endregion

        #region Classes

        /// <summary>
        /// Temporary data structure to hold segments of perimeter curve during a slicing operation
        /// </summary>
        [Serializable]
        private class PerimeterSegment
        {
            public Curve SourceCurve = null;
            public Interval Domain = Interval.Unset;
            public Interval CutterDomain = Interval.Unset;

            public PerimeterSegment(Curve sourceCurve, Interval domain, Interval cutterDomain)
            {
                SourceCurve = sourceCurve;
                Domain = domain;
                CutterDomain = cutterDomain;
            }

            public PerimeterSegment(Curve sourceCurve, double t0, double t1, double tC0, double tC1)
                : this(sourceCurve, new Interval(t0, t1), new Interval(tC0, tC1))
            { }

            public Curve Extract()
            {
                return SourceCurve?.Extract(Domain);
            }
        }

        #endregion

    }

    /// <summary>
    /// Extension methods for the PlanarRegion class and collections thereof
    /// </summary>
    public static class PlanarRegionExtensions
    {
        /// <summary>
        /// Calculate the total (unsigned) area of all regions in this collection
        /// </summary>
        /// <param name="regions"></param>
        /// <returns></returns>
        public static double CalculateTotalArea(this IList<PlanarRegion> regions)
        {
            return regions.Sum(region => region.CalculateArea().Abs());
        }
    }

}
