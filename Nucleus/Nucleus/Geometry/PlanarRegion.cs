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
        /// Does the specified point fall within this region?
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
            var outerInts = Intersect.CurveLineXY(Perimeter, splitPt, splitDir).ToList();
            outerInts.Sort();
            //TODO: void intersections
            if (outerInts.Count > 1)
            {
                for (int i = 0; i < outerInts.Count; i++)
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
                            newPerimeter = newPerimeter.OffsetInwards(offsets);
                        }
                    }
                    if (newPerimeter != null) result.Add(new PlanarRegion(newPerimeter, Attributes?.Duplicate()));
                }
            }
            else
            {
                result.Add(this); //Return the original
            }
            return result;
        }
        

        #endregion
    }
}
