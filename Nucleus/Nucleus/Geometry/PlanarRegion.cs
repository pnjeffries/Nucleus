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
    public class PlanarRegion : Surface, IFastDuplicatable
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
                if (HasVoids)
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
        /// Initialises a new PlanarRegion with the specified perimeter and void curves.
        /// </summary>
        /// <param name="perimeter"></param>
        /// <param name="voids"></param>
        public PlanarRegion(Curve perimeter, CurveCollection voids, GeometryAttributes attributes = null) 
            : this(perimeter, attributes)
        {
            _Voids = voids;
        }

        /// <summary>
        /// Initialise a new PlanarRegion with the specified perimeter and void curves
        /// </summary>
        /// <param name="perimeter"></param>
        /// <param name="voidCrv"></param>
        /// <param name="attributes"></param>
        public PlanarRegion(Curve perimeter, Curve voidCrv, GeometryAttributes attributes = null)
            : this(perimeter, new CurveCollection(voidCrv), attributes)
        { }

        /// <summary>
        /// Initialises a new PlanarRegion, copying properties from another.
        /// Internally this uses FastDuplicate methods, making it faster than
        /// calling the ordinary Duplicate() function.
        /// </summary>
        /// <param name="other"></param>
        public PlanarRegion(PlanarRegion other) : this()
        {
            _Perimeter = other.Perimeter.FastDuplicate();
            if (other.HasVoids)
            {
                _Voids = new CurveCollection();
                foreach (var voidCrv in other.Voids)
                    _Voids.Add(voidCrv.FastDuplicate());
            }
            Attributes = other.Attributes;
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
                if (HasVoids)
                    foreach (var voidCrv in _Voids)
                        if (voidCrv.EnclosesXY(pt)) return false;
                return true;
            }
            return false;
        }

        public bool Overlaps(PlanarRegion other)
        {
            // Shortcut; test start points
            return (ContainsXY(other.Perimeter.StartPoint) || ContainsXY(other.Perimeter.EndPoint));
            // At least one part of the perimeter is outside: 

            // TODO
        }

        /// <summary>
        /// Determine whether the first segment of the specified curve falls inside the specified other curve.
        /// Used during boolean operations to determine the alternating inclusion pattern between intersections.
        /// </summary>
        /// <param name="forCrv"></param>
        /// <param name="inCrv"></param>
        /// <param name="intersections"></param>
        /// <returns></returns>
        private bool FirstSegmentInside(Curve forCrv, Curve inCrv, IList<CurveCurveIntersection> intersections)
        {
            // TODO: deal with start point being coincident with first intersections
            return inCrv.EnclosesXY(forCrv.StartPoint);
        }

        /// <summary>
        /// Determine whether the first segment of the specified curve falls inside the specified other curve.
        /// Used during boolean operations to determine the alternating inclusion pattern between intersections.
        /// </summary>
        /// <param name="forCrv"></param>
        /// <param name="inCrv"></param>
        /// <param name="intersections"></param>
        /// <returns></returns>
        private bool FirstSegmentInside(Curve forCrv, PlanarRegion inRegion, IList<CurveCurveIntersection> intersections)
        {
            return inRegion.ContainsXY(forCrv.StartPoint);
        }

        /// <summary>
        /// Perform a boolean not (subtraction) operation on this region
        /// </summary>
        /// <param name="cutter"></param>
        /// <returns></returns>
        /// <remarks>Vaguely similar to the Greiner–Hormann algorithm.</remarks>
        public IList<PlanarRegion> Not(PlanarRegion cutter)
        {
            var result = new List<PlanarRegion>();

            if (Perimeter == null) return result;

            var curveData = new Dictionary<Curve, CurveBooleanData>();

            // Find intersection points on perimeter:
            IList<CurveCurveIntersection> perimeterInts = Intersect.CurveCurveXYIntersections(Perimeter, cutter.Perimeter);
            IList<CurveCurveIntersection> cutterInts = new List<CurveCurveIntersection>();
            foreach (var cCI in perimeterInts)
            {
                cCI.ProcessCounter = 1;
                cutterInts.Add(cCI);
            }

            perimeterInts.Sort((i1, i2) => i1.ParameterA.CompareTo(i2.ParameterA));
            var perimeterData = new CurveBooleanData(Perimeter, perimeterInts);
            perimeterData.IncludeFirst = !FirstSegmentInside(Perimeter, cutter.Perimeter, perimeterInts);
            curveData.Add(Perimeter, perimeterData);
            // TODO: cutter void intersection

            CurveCollection freeVoids = new CurveCollection(); //Un-cut voids
            var voidDatas = new List<CurveBooleanData>();
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
                    if (!FirstSegmentInside(voidCrv, cutter.Perimeter, voidInts))
                    {
                        freeVoids.Add(voidCrv);
                    }
                }
                else
                {
                    voidInts.Sort((i1, i2) => i1.ParameterA.CompareTo(i2.ParameterA));
                    var voidData = new CurveBooleanData(voidCrv, voidInts);
                    voidData.IncludeFirst = !FirstSegmentInside(voidCrv, cutter.Perimeter, voidInts);
                    curveData.Add(voidCrv, voidData);
                    voidDatas.Add(voidData);
                }
            }

            // Determine direction of travel around cutter:
            cutterInts.Sort((i1, i2) => i1.ParameterB.CompareTo(i2.ParameterB));
            var cutterData = new CurveBooleanData(cutter.Perimeter, cutterInts, true);
            cutterData.IncludeFirst = FirstSegmentInside(cutter.Perimeter, this, cutterInts);
            curveData.Add(cutter.Perimeter, cutterData);

            //'Fake' intersection to represent perimeter start:
            if (perimeterData.IncludeFirst && perimeterData.Intersections.Count > 0)
            {
                var startInt = new CurveCurveIntersection(Perimeter, null, 0, double.NaN, 1);
                perimeterData.Intersections.Insert(0, startInt);
                perimeterData.IncludeFirst = !perimeterData.IncludeFirst;
                // ...and end:
                var endInt = new CurveCurveIntersection(Perimeter, null, 1, double.NaN, 1);
                perimeterData.Intersections.Add(endInt);
            }

            IList<CurveCurveIntersection> nextStartInts = new List<CurveCurveIntersection>();
            PolyCurve newPerimeter = null;
            PlanarRegion newRegion = null;
            Curve travelOn = Perimeter;
            CurveCurveIntersection currentInt;

            if (perimeterData.Intersections.Count == 0)
            {
                // No perimeter intersections
                if (!perimeterData.IncludeFirst) return result; //Region fully enclosed

                if (voidDatas.Count == 0)
                {
                    // No void intersections

                    // Copy region, add new void:
                    newRegion = new PlanarRegion(Perimeter);

                    foreach (var voidCrv in freeVoids)
                        newRegion.Voids.Add(voidCrv.Duplicate());

                    if (cutterData.IncludeFirst)
                        newRegion.Voids.Add(cutter.Perimeter.Duplicate());

                    result.Add(newRegion);
                    return result;
                }

                newRegion = new PlanarRegion(Perimeter.Duplicate());
                newPerimeter = new PolyCurve();
                newRegion.Voids.Add(newPerimeter);
                currentInt = voidDatas[0].Intersections[0];
                travelOn = voidDatas[0].Curve;
                result.Add(newRegion);
            }
            else
            {
                currentInt = perimeterData.Intersections[0];
            }

            //TODO: Deal with inner void intersections only


            // Loop through intersections and build up new sub-regions by traversing connected
            // intersections along the perimeter, cutter and void curves
            int segmentCount = (int)Math.Floor(cutterInts.Count * 1.5);
            for (int i = 0; i < segmentCount; i++)
            {
                var data = curveData[travelOn];
                double tStart = currentInt.ParameterOn(travelOn);
                int iCurrent = data.Intersections.IndexOf(currentInt);
                int iNext;
                bool reverse = false;
               
                // Determine direction of travel:
                if (data.IncludeFirst ^ iCurrent.IsEven())
                {
                    iNext = iCurrent + 1;
                    if (iNext >= data.Intersections.Count) iNext = 0;
                }
                else
                {
                    iNext = iCurrent - 1;
                    if (iNext < 0) iNext = data.Intersections.Count - 1;
                    reverse = true;
                }

                CurveCurveIntersection nextInt = data.Intersections[iNext];

                if (currentInt.ProcessCounter > 0)
                {
                    // Extract segment:
                    double tEnd = nextInt.ParameterOn(travelOn);
                    double t0, t1;
                    if (reverse)
                    {
                        t0 = tEnd;
                        t1 = tStart;
                    }
                    else
                    {
                        t0 = tStart;
                        t1 = tEnd;
                    }
                    Curve segment = travelOn.Extract(t0, t1);
                    if (reverse) segment.Reverse();
                    if (newPerimeter == null)
                    {
                        newPerimeter = new PolyCurve(segment);
                        newRegion = new PlanarRegion(newPerimeter);
                        result.Add(newRegion);
                    }
                    else
                    {
                        newPerimeter.Add(segment);
                    }

                    currentInt.ProcessCounter--;
                    currentInt = nextInt;
                }

                var travelOnNext = nextInt.OtherCurve(travelOn);
                if (data.IsCutter == false && reverse == false && travelOnNext != null)
                {
                    var nextStartInt = data.Intersections.GetWrapped(iNext + 1);
                    if (nextStartInt.ProcessCounter > 0)
                        nextStartInts.Add(nextStartInt);
                }

                if (travelOnNext == null || nextInt.ProcessCounter <= 0)
                {
                    if (newPerimeter != null)
                    {
                        // Close loop
                        newPerimeter.Close();

                        newPerimeter = null;
                        newRegion = null;
                    }
                    if (nextStartInts.Count > 0)
                    {
                        currentInt = nextStartInts.GetAndRemove(0);
                    }
                    else break;
                }
                else travelOn = travelOnNext;
            }

            //Re-add voids:
            foreach (var voidCrv in freeVoids)
            {
                foreach (var subRegion in result)
                {
                    if (subRegion.ContainsXY(voidCrv.StartPoint))
                    {
                        subRegion.Voids.Add(voidCrv);
                        break;
                    }
                }
            }

            return result;
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
        private static int RightCutterDirectionOfTravel(IList<CurveLineIntersection> rightInts, IList<CurveLineIntersection> leftInts)
        {
            int result = 1;
            IList<CurveLineIntersection> testInts = rightInts;
            if (rightInts == null || rightInts.Count == 0)
            {
                testInts = leftInts;
                result *= -1;
            }
            CurveLineIntersection firstInt = testInts.ItemWithMin(pInt => pInt.LineParameter);
            CurveLineIntersection lastInt = testInts.ItemWithMax(pInt => pInt.LineParameter);
            //CurveLineIntersection nextInt = testInts.ItemWithNext(pInt => pInt.LineParameter, firstInt.LineParameter);

            if (firstInt.CurveParameter < lastInt.CurveParameter) result *= -1;

            return result;
        }

        /// <summary>
        /// Work out the direction of travel that should be taken along the right splitting line
        /// in order to close split off subregions.
        /// The inverse will apply on the left splitting line.
        /// </summary>
        /// <returns></returns>
        private static int RightCutterDirectionOfTravel(IList<CurveLineIntersection> perimeterInts, IList<CurveLineIntersection> rightInts, 
            IList<CurveLineIntersection> leftInts, Vector rightDir, HandSide startSide, bool hasWidth = true)
        {
            int result = -1;
            CurveLineIntersection firstInt = perimeterInts.ItemWithMin(pInt => pInt.CurveParameter);
            CurveLineIntersection lastInt = perimeterInts.ItemWithMax(pInt => pInt.CurveParameter);
            if (hasWidth)
            {
                /*if (firstInt.Side != lastInt.Side)
                {
                    return RightCutterDirectionOfTravel(rightInts, leftInts);
                    // Special case: Start point is on segment bridging between cutting lines
                    //CurveLineIntersection nextInt = perimeterInts.ItemWithNext(pInt => pInt.CurveParameter, firstInt.CurveParameter);
                    //if (firstInt.Side == HandSide.Left) result *= -1;
                    //if (nextInt.LineParameter < firstInt.LineParameter) result *= -1;
                }
                else */
                if (startSide == HandSide.Undefined)
                {
                    result *= -1;

                    IList<CurveLineIntersection> lineInts;
                    if (firstInt.Side == HandSide.Right)
                    {
                        result *= -1;
                        lineInts = rightInts;
                    }
                    else lineInts = leftInts;

                    bool indeterminate = false;
                    var sList = new SortedList<double, CurveLineIntersection>();
                    foreach (var cLI in lineInts)
                    {
                        if (sList.ContainsKey(cLI.LineParameter) &&
                            firstInt.LineParameter == cLI.LineParameter) indeterminate = true;
                        sList.AddSafe(cLI.LineParameter, cLI);
                    }
                    int firstIndex = sList.IndexOfValue(firstInt);
                    if (firstIndex.IsOdd()) result *= -1;

                    if (indeterminate) return 0; //TODO: Use fallback method?

                    // Start point is on segment that is dipping into the cut region
                    //if (firstInt.Side == HandSide.Left) result *= -1;
                    // if (lastInt.LineParameter < firstInt.LineParameter) result *= -1;
                }
                else
                {
                    IList<CurveLineIntersection> lineInts;
                    if (firstInt.Side == HandSide.Right)
                    {
                        result *= -1;
                        lineInts = rightInts;
                    }
                    else lineInts = leftInts;

                    bool indeterminate = false;
                    var sList = new SortedList<double, CurveLineIntersection>();
                    foreach (var cLI in lineInts)
                    {
                        if (sList.ContainsKey(cLI.LineParameter) && 
                            firstInt.LineParameter == cLI.LineParameter) indeterminate = true;
                        sList.AddSafe(cLI.LineParameter, cLI);
                    }
                    int firstIndex = sList.IndexOfValue(firstInt);
                    if (firstIndex.IsOdd()) result *= -1;

                    if (indeterminate) return 0; //TODO: Use fallback method?


                    //CurveLineIntersection firstLineInt = lineInts.ItemWithMin(pInt => pInt.LineParameter);
                    //CurveLineIntersection nextLineInt = 
                    //CurveLineIntersection nextLineInt = lineInts.ItemWithNext(pInt => pInt.LineParameter, firstInt.LineParameter);
                    //if (nextLineInt == null || firstInt.CurveParameter < nextLineInt.CurveParameter) result *= -1;
                    //if (nextLineInt != lastInt) result *= -1;

                    //TODO: Other condition?
                    //if (lastInt.LineParameter < firstInt.LineParameter) result *= -1;
                    
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
        /// <param name="perimeterMappers">Option.  A collection to contain a set of CurveParameterMapper
        /// objects which can be used to map between the parameter space of the original region's perimeter
        /// and </param>
        /// <returns>The resultant list of regions.  If the line does not bisect
        /// this region and the region could not be split, this collection will contain
        /// only the original region.</returns>
        public IList<PlanarRegion> SplitByLineXY(Vector splitPt, Vector splitDir, 
            double splitWidth = 0, IList<CurveParameterMapper> perimeterMappers = null)
        {
            var result = new List<PlanarRegion>();

            if (Perimeter == null) return result;

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

            HandSide startSide = SideOfSplit(Perimeter.StartPoint, leftPt, rightPt, offsetDir);
            // Determine direction of travel along cutting lines:
            //int rightDir = RightCutterDirectionOfTravel(rightInts, leftInts);
            int rightDir = RightCutterDirectionOfTravel(perimeterInts, rightInts, leftInts, offsetDir, startSide, splitWidth != 0);

            if (rightDir == 0) return result; // Indeterminate direction of travel - abort!

            // 'Fake' intersection to represent perimeter start:
            CurveLineIntersection startInt = new CurveLineIntersection(Perimeter, 0, double.NaN)
            { ProcessCounter = 1 };
            // ...and end:
            CurveLineIntersection endInt = startInt;
            //new CurveLineIntersection(Perimeter, 1, double.NaN)
            //{ ProcessCounter = 1 };
            //rightDir *= -1; //TEMP!
            // Loop through intersections and build up new sub-regions by traversing connected
            // intersections along the perimeter, cutting lines and void curves
            CurveLineIntersection currentInt = startInt;
            IList<CurveLineIntersection> nextStartInts = new List<CurveLineIntersection>();
            PolyCurve newPerimeter = null;
            PlanarRegion newRegion = null;
            CurveParameterMapper newMapper = null;
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
                        if (subCrv != null)
                        {
                            if (reverse) subCrv.Reverse();
                            if (newPerimeter == null)
                            {
                                newPerimeter = new PolyCurve(subCrv.Explode());
                                if (newPerimeter != null)
                                {
                                    newRegion = new PlanarRegion(newPerimeter);
                                    result.Add(newRegion);
                                    if (perimeterMappers != null)
                                    {
                                        newMapper = new CurveParameterMapper(Perimeter, newPerimeter);
                                        perimeterMappers.Add(newMapper);
                                        newMapper.AddSpanDomains(curve.SpanDomains(subDomain));
                                    }
                                }
                            }
                            else
                            {
                                //Connect up:
                                Vector endPt = newPerimeter.EndPoint;
                                if (endPt.DistanceToSquared(subCrv.StartPoint) > (Tolerance.Distance * Tolerance.Distance))
                                {
                                    //Out of tolerance; create a line:
                                    newPerimeter.Add(new Line(endPt, subCrv.StartPoint));
                                    if (newMapper != null) newMapper.AddSpanDomain(Interval.Unset);
                                }
                                newPerimeter.Add(subCrv, false, true);
                                if (newMapper != null)
                                {
                                    if (curve == Perimeter) newMapper.AddSpanDomains(curve.SpanDomains(subDomain));
                                    else newMapper.AddNullSpanDomains(curve.SpanDomains(subDomain));
                                }
                            }
                        }
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

                    if (newPerimeter != null && !newPerimeter.Closed)
                    {
                        newPerimeter.Close();
                        if (newMapper != null) newMapper.AddNullSpanDomain();
                    }
                    newPerimeter = null;
                    newRegion = null;
                    newMapper = null;

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
            // Check last perimeter is closed:
            if (newPerimeter != null && !newPerimeter.Closed)
            {
                newPerimeter.Close();
                if (newMapper != null) newMapper.AddNullSpanDomain();
            }

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

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new PlanarRegion(this);
        }

        #endregion

        #region Classes

        /// <summary>
        /// Temporary data structure to hold data about curves during booleaning operations
        /// </summary>
        [Serializable]
        private class CurveBooleanData
        {
            public Curve Curve = null;
            public bool IncludeFirst = false;
            public bool IsCutter = false;
            public IList<CurveCurveIntersection> Intersections = new List<CurveCurveIntersection>();

            public CurveBooleanData(Curve curve, IList<CurveCurveIntersection> intersections, bool isCutter = false)
            {
                Curve = curve;
                Intersections = intersections;
            }
        }

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
