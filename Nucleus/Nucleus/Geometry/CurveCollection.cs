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

namespace Nucleus.Geometry
{
    /// <summary>
    /// A collection of curve objects
    /// </summary>
    [Serializable]
    public class CurveCollection : VertexGeometryCollection<Curve>
    {
        #region Constructors

        /// <summary>
        /// Initialise a new empty CurveCollection
        /// </summary>
        public CurveCollection() : base() { }
        
        /// <summary>
        /// Initialise a new CurveCollection containing the specified curve
        /// </summary>
        /// <param name="curve"></param>
        public CurveCollection(Curve curve) : this()
        {
            Add(curve);
        }

        /// <summary>
        /// Initialise a new CurveCollection containing the specified curves
        /// </summary>
        /// <param name="curves"></param>
        public CurveCollection(IList<Curve> curves) : this()
        {
            foreach (var crv in curves) Add(crv);
        }

        /// <summary>
        /// Initialise a new CurveCollection containing the specifed curves 
        /// </summary>
        /// <param name="curves"></param>
        public CurveCollection(params Curve[] curves) : this((IList<Curve>)curves)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Calculate the combined enclosed area of all curves within this collection
        /// </summary>
        /// <returns></returns>
        public double TotalEnclosedArea(Plane onPlane = null)
        {
            double result = 0;
            foreach (Curve crv in this)
            {
                Vector centroid;
                result += crv.CalculateEnclosedArea(out centroid, onPlane).Abs();
            }
            return result;
        }

        /// <summary>
        /// The total length of all curves within this collection
        /// </summary>
        /// <returns></returns>
        public double TotalLength()
        {
            double result = 0;
            foreach (Curve crv in this)
            {
                result += crv.Length;
            }
            return result;
        }

        /// <summary>
        /// Get the curve in this collection with the greatest length
        /// </summary>
        /// <returns></returns>
        public Curve GetLongest()
        {
            double maxLength = 0;
            Curve longest = null;
            foreach (Curve curve in this)
            {
                double length = curve.Length;
                if (length > maxLength)
                {
                    maxLength = length;
                    longest = curve;
                }
            }
            return longest;
        }

        /// <summary>
        /// Find the closest curve to a given point within this collection
        /// </summary>
        /// <param name="toPoint">The point to test from</param>
        /// <param name="minDist">Output.  The minimum curve-point distance found.</param>
        /// <param name="tClosest">Output.  The parameter of the closest point on the closest curve.</param>
        /// <param name="closestPt">Output.  The closest point on the closest curve.</param>
        /// <returns></returns>
        public Curve ClosestCurve(Vector toPoint, out double minDist, out double tClosest, out Vector closestPt)
        {
            Curve result = null;
            minDist = double.NaN;
            tClosest = double.NaN;
            closestPt = Vector.Unset;
            foreach (Curve crv in this)
            {
                double t = crv.ClosestParameter(toPoint);
                Vector pt = crv.PointAt(t);
                if (pt.IsValid())
                {
                    double dist = pt.DistanceToSquared(toPoint);
                    if (result == null || dist < minDist)
                    {
                        minDist = dist;
                        result = crv;
                        tClosest = t;
                        closestPt = pt;
                    }
                }
            }
            if (!minDist.IsNaN()) minDist = minDist.Root();
            return result;
        }

        #endregion

    }
}
