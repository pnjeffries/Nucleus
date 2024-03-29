﻿// Copyright (c) 2016 Paul Jeffries
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
using Nucleus.Geometry;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Meshing
{
    /// <summary>
    /// A special type of mesh face created via a Delaunay Triangulation meshing
    /// </summary>
    [Serializable]
    public class DelaunayTriangle : MeshFace
    {
        #region Fields

        /// <summary>
        /// If true, this triangle can be excluded from future containment checks because
        /// subsequent vertices will be too far to the right of this triangle to 
        /// </summary>
        private bool _ExcludeFromContainmentCheck = false;

        /// <summary>
        /// The bounding interval of this triangle in the X-direction
        /// </summary>
        //private Interval _IntervalX = Interval.Unset;

        /// <summary>
        /// The bounding interval of this triangle in the Y-direction
        /// </summary>
        //private Interval _IntervalY = Interval.Unset;

        private Vector _Circumcentre = Vector.Unset;

        private double _CircumRadiusSquared = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Get the circumcentre of the first three vertices of this face on the XY plane
        /// </summary>
        /// <returns></returns>
        internal override Vector XYCircumcentre
        {
            get
            {
                if (!_Circumcentre.IsValid()) _Circumcentre = CalculateXYCircumcentre();
                return _Circumcentre;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a triangular mesh face between the three specified vertices
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        public DelaunayTriangle(Vertex v1, Vertex v2, Vertex v3) : base(v1, v2, v3) { }

        /// <summary>
        /// Initialise a triangular mesh face between an edge and a vertex
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="v"></param>
        public DelaunayTriangle(MeshEdge edge, Vertex v) : base(edge, v) { }

        #endregion

        #region Methods

        public override void Set(Vertex v1, Vertex v2, Vertex v3)
        {
            this[0] = v1;
            this[1] = v2;
            this[2] = v3;
            _Circumcentre = Vector.Unset;
            _CircumRadiusSquared = -1;
        }

        /// <summary>
        /// Tests whether a point lies in the circumcircle of this face.  Used during delaunay
        /// triangulation on the XY plane.
        /// Currently works only for triangle faces which are counterclockwise on XY plane.
        /// Hence declared internal until it can be made more robust.
        /// </summary>
        /// <param name="point">The position vector to test</param>
        /// <returns></returns>
        internal override bool XYCircumcircleContainmentQuickCheck(Vertex point)
        {
            
            if (_ExcludeFromContainmentCheck) return false;

            if (!_Circumcentre.IsValid())
                _Circumcentre = CalculateXYCircumcentre();
            if (_CircumRadiusSquared < 0) _CircumRadiusSquared = this[0].Position.XYDistanceToSquared(_Circumcentre);

            double xDistSqd = (_Circumcentre.X - point.X).Squared();
            double distSqd = xDistSqd + (_Circumcentre.Y - point.Y).Squared();

            if (distSqd < _CircumRadiusSquared) return true;
            else
            {
                if (_Circumcentre.X < point.X && xDistSqd > _CircumRadiusSquared) _ExcludeFromContainmentCheck = true;
                return false;
            }

            // See https://en.wikipedia.org/wiki/Delaunay_triangulation#Algorithms for methodology -
            // Calculates the determinant of a matrix containing the vertex and point coordinates
            // If vertices are counterclockwise and point lies inside, determinant will be > 0

            /*Vertex v0 = this[0];
            Vertex v1 = this[1];
            Vertex v2 = this[2];
            double x0 = v0.X - point.X;
            double y0 = v0.Y - point.Y;
            double x1 = v1.X - point.X;
            double y1 = v1.Y - point.Y;
            double x2 = v2.X - point.X;
            double y2 = v2.Y - point.Y;

            double det_01 = x0 * y1 - x1 * y0;
            double det_12 = x1 * y2 - x2 * y1;
            double det_20 = x2 * y0 - x0 * y2;

            double square0 = x0 * x0 + y0 * y0;
            double square1 = x1 * x1 + y1 * y1;
            double square2 = x2 * x2 + y2 * y2;

            double det = square0 * det_12 + square1 * det_20 + square2 * det_01;
            if (det > 0) return true;
            else
            {
                //TODO: Check if far from X
                if (!_IntervalX.IsValid()) _IntervalX = new Interval(v0.X, v1.X, v2.X);
                if (point.X > _IntervalX.Max + _IntervalX.Size /2)
                {
                    if (!_IntervalY.IsValid()) _IntervalY = new Interval(v0.Y, v1.Y, v2.Y);
                    if (point.X > _IntervalX.Max + _IntervalY.Size /2) _ExcludeFromContainmentCheck = true;
                }
                return false;
            }*/
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Generate a triangular mesh face that encompasses the specified bounding box
        /// Used as the initial stage of delaunary triangulation.
        /// The triangle will be created using temporary vertices that will not be added to
        /// the vertices of this mesh.
        /// </summary>
        /// <returns></returns>
        internal static DelaunayTriangle GenerateSuperTriangleXY(BoundingBox box)
        {
            Vertex v0 = new Vertex(box.MinX + box.SizeX * 2, box.MinY - 2*box.SizeY);
            Vertex v1 = new Vertex(box.MinX + box.SizeX/2, box.MinY + box.SizeY * 2);
            Vertex v2 = new Vertex(box.MinX - box.SizeX, box.MinY - box.SizeY);

            return new DelaunayTriangle(v0, v1, v2);
        }

        #endregion
    }
}
