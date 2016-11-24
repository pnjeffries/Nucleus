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

using FreeBuild.Base;
using FreeBuild.Maths;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An axis-aligned bounding box representing a region in space
    /// </summary>
    [Serializable]
    [Copy(CopyBehaviour.DUPLICATE)]
    public class BoundingBox : IDuplicatable
    {
        #region Properties

        /// <summary>
        /// The minimum x-coordinate
        /// </summary>
        public double MinX { get; set; }

        /// <summary>
        /// The maximum x-coordinate
        /// </summary>
        public double MaxX { get; set; }

        /// <summary>
        /// The minimum y-coordinate
        /// </summary>
        public double MinY { get; set; }

        /// <summary>
        /// The maximum y-coordinate
        /// </summary>
        public double MaxY { get; set; }

        /// <summary>
        /// The minimum z-coordinate
        /// </summary>
        public double MinZ { get; set; }

        /// <summary>
        /// The maximum z-coordinate
        /// </summary>
        public double MaxZ { get; set; }

        /// <summary>
        /// Get the mid-point of the box in the X-axis
        /// </summary>
        public double MidX { get { return (MinX + MaxX) / 2; } }

        /// <summary>
        /// Get the mid-point of the box in the Y-axis
        /// </summary>
        public double MidY { get { return (MinY + MaxY) / 2; } }

        /// <summary>
        /// Get the mid-point of the box in the Z-axis
        /// </summary>
        public double MidZ { get { return (MinZ + MaxZ) / 2; } }

        /// <summary>
        /// The size of this box in the x-axis
        /// </summary>
        public double SizeX { get { return MaxX - MinX; } }

        /// <summary>
        /// The size of this box in the y-axis
        /// </summary>
        public double SizeY { get { return MaxY - MinY; } }

        /// <summary>
        /// The size of this box in the z-axis
        /// </summary>
        public double SizeZ { get { return MaxZ - MinZ; } }

        /// <summary>
        /// The interval of the values this box occupies in the X-Axis
        /// </summary>
        public Interval X
        {
            get { return new Interval(MinX, MaxX); }
            set { MinX = value.Min; MaxX = value.Max; }
        }

        /// <summary>
        /// The interval of the values this box occupies in the Y-Axis
        /// </summary>
        public Interval Y
        {
            get { return new Interval(MinY, MaxY); }
            set { MinY = value.Min; MaxY = value.Max; }
        }

        /// <summary>
        /// The interval of values this box occupies in the Z-Axis
        /// </summary>
        public Interval Z
        {
            get { return new Interval(MinZ, MaxZ); }
            set { MinZ = value.Min; MaxZ = value.Max; }
        }

        /// <summary>
        /// Get or set the vector representing the minimum corner of
        /// this bounding box.
        /// </summary>
        public Vector Min
        {
            get { return new Vector(MinX, MinY, MinZ); }
            set { MinX = value.X; MinY = value.Y; MinZ = value.Z; }
        }

        /// <summary>
        /// Get or set the vector representing the maximum corner of
        /// this bounding box.
        /// </summary>
        public Vector Max
        {
            get { return new Vector(MaxX, MaxY, MaxZ); }
            set { MaxX = value.X; MaxY = value.Y; MaxZ = value.X; }
        }

        /// <summary>
        /// Get the vector representing the mid-point, or volume centre of
        /// this bounding box.
        /// </summary>
        public Vector Mid { get { return new Vector(MidX, MidY, MidZ); } }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.  Initialises a bounding box of zero dimension at the origin.
        /// </summary>
        public BoundingBox() : this(0, 0, 0) { }

        /// <summary>
        /// Initialise a bounding box to the specified set of minimum and maximum values
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="minZ"></param>
        /// <param name="maxZ"></param>
        public BoundingBox(double minX, double maxX, double minY, double maxY, double minZ, double maxZ)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            MinZ = minZ;
            MaxZ = maxZ;
        }

        /// <summary>
        /// Initialise a bounding box as a singularity at the specified coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public BoundingBox(double x, double y, double z) : this(x, x, y, y, z, z) { }

        /// <summary>
        /// Initialise a bounding box as a singularity at the specified points
        /// </summary>
        /// <param name="point"></param>
        public BoundingBox(Vector point) : this(point.X, point.Y, point.Z) { }

        /// <summary>
        /// Constructor to fit a bounding box around a set of points.
        /// </summary>
        /// <param name="points">The points to fit the bounding box around</param>
        public BoundingBox(IEnumerable<IPosition> points)
        {
            Fit(points);
        }

        /// <summary>
        /// Constructor to fit a bounding box around an element
        /// </summary>
        /// <param name="elements"></param>
        public BoundingBox(IEnumerable<IElement> elements)
        {
            Fit(elements);
        }

        /// <summary>
        /// Initialise a bounding box as a copy of another
        /// </summary>
        /// <param name="other"></param>
        public BoundingBox(BoundingBox other)
            :this(other.MinX, other.MaxZ, other.MinY, other.MaxY, other.MinZ, other.MaxZ){}

        #endregion

        #region Methods

        /// <summary>
        /// Fit this bounding box around a collection of positional
        /// objects
        /// </summary>
        /// <param name="points"></param>
        protected void Fit(IEnumerable<IPosition> points)
        {
            bool first = true;
            foreach (IPosition point in points)
            {
                if (first)
                {
                    //Initialise to first node:
                    Vector pt0 = points.First().Position;
                    MinX = pt0.X;
                    MaxX = pt0.X;
                    MinY = pt0.Y;
                    MaxY = pt0.Y;
                    MinZ = pt0.Z;
                    MaxZ = pt0.Z;
                    first = false;
                }
                else
                {
                    //Scale to subsequent nodes:
                    Include(point.Position);
                }
            }
        }

        /// <summary>
        /// Fit this bounding box around a collection of vertices
        /// </summary>
        /// <param name="vertices"></param>
        public void Fit(IList<Vertex> vertices)
        {
            if (vertices.Count > 0)
            {
                Vector pt = vertices[0].Position;
                MinX = pt.X;
                MaxX = pt.X;
                MinY = pt.Y;
                MaxY = pt.Y;
                MinZ = pt.Z;
                MaxZ = pt.Z;

                for (int i = 1; i < vertices.Count; i++)
                {
                    Include(vertices[i].Position);
                }
            }
        }

        /// <summary>
        /// Fit this bounding box around a collection of elements
        /// </summary>
        /// <param name="elements"></param>
        public void Fit(IEnumerable<IElement> elements)
        {
            bool first = true;
            foreach (IElement element in elements)
            {
                if (first)
                {
                    //Initialise to first element:
                    Fit(element);
                    first = false;
                }
                else
                {
                    //Scale to subsequent elements:
                    Include(element);
                }
            }
        }

        /// <summary>
        /// Fit this bounding box around an element
        /// </summary>
        /// <param name="element"></param>
        public void Fit(IElement element)
        {
            Fit(element.Geometry);
        }

        /// <summary>
        /// Fit this bounding box around a shape
        /// </summary>
        /// <param name="shape"></param>
        public void Fit(Shape shape)
        {
            if (shape != null) Fit(shape.Vertices);
        }

        /// <summary>
        /// Expand this bounding box to include the positions of the
        /// specified set of positional objects
        /// </summary>
        /// <param name="points"></param>
        public void Include(IEnumerable<IPosition> points)
        {
            foreach (IPosition point in points)
            {
                Include(point.Position);
            }
        }

        /// <summary>
        /// Expand this bounding box to include the positions of the 
        /// specified set of elements
        /// </summary>
        /// <param name="elements"></param>
        public void Include(IEnumerable<IElement> elements)
        {
            foreach (IElement element in elements) Include(element);
        }

        /// <summary>
        /// Expand the bounding box to include the specified point, if necessary
        /// </summary>
        /// <param name="pt"></param>
        public void Include(Vector pt)
        {
            if (pt.X < MinX) MinX = pt.X;
            else if (pt.X > MaxX) MaxX = pt.X;
            if (pt.Y < MinY) MinY = pt.Y;
            else if (pt.Y > MaxY) MaxY = pt.Y;
            if (pt.Z < MinZ) MinZ = pt.Z;
            else if (pt.Z > MaxZ) MaxZ = pt.Z;
        }

        /// <summary>
        /// Expand the bounding box to include the specified point, if necessary
        /// </summary>
        /// <param name="pt"></param>
        public void Include(ref Vector pt)
        {
            if (pt.X < MinX) MinX = pt.X;
            else if (pt.X > MaxX) MaxX = pt.X;
            if (pt.Y < MinY) MinY = pt.Y;
            else if (pt.Y > MaxY) MaxY = pt.Y;
            if (pt.Z < MinZ) MinZ = pt.Z;
            else if (pt.Z > MaxZ) MaxZ = pt.Z;
        }

        /// <summary>
        /// Expand this bounding box to contain another, if necessary.
        /// </summary>
        /// <param name="other">The box to be included.  If any part
        /// of the box falls outside of this one, this box will be expanded
        /// to the relevant limits of the other.  May be null.</param>
        public void Include(BoundingBox other)
        {
            if (other != null)
            {
                if (other.MinX < MinX) MinX = other.MinX;
                if (other.MaxX > MaxX) MaxX = other.MaxX;
                if (other.MinY < MinY) MinY = other.MinY;
                if (other.MaxY > MaxY) MaxY = other.MaxY;
                if (other.MinZ < MinZ) MinZ = other.MinZ;
                if (other.MaxZ > MaxZ) MaxZ = other.MaxZ;
            }
        }

        /// <summary>
        /// Expand this bounding box to include the specified element
        /// </summary>
        /// <param name="element"></param>
        public void Include(IElement element)
        {
            Include(element.Geometry);
        }

        /// <summary>
        /// Expand this bounding box to contain the specified geometry, if
        /// necessary.
        /// </summary>
        /// <param name="geometry"></param>
        public void Include(Shape geometry)
        {
            if (geometry != null) Include(geometry.Vertices);
        }

        /// <summary>
        /// Does this bounding box contain the specified point?
        /// </summary>
        /// <param name="point">The point to test for containment</param>
        /// <returns>True if the point falls inside, or is on the surface of, 
        /// this box</returns>
        public bool Contains(Vector point)
        {
            return (point.IsValid() &&
                point.X >= MinX && point.X <= MaxX &&
                point.Y >= MinY && point.Y <= MaxY &&
                point.Z >= MinZ && point.Z <= MaxZ);
        }

        /// <summary>
        /// Generate a random point inside this bounding box
        /// </summary>
        /// <param name="rng">The random number generator used to generate the point</param>
        /// <returns>A new vector at a random position somewhere inside this box</returns>
        public Vector RandomPointInside(Random rng)
        {
            return new Vector(rng.NextDouble() * SizeX + MinX,
                rng.NextDouble() * SizeY + MinY,
                rng.NextDouble() * SizeZ + MinZ);
        }

        /// <summary>
        /// Generate a set of random points inside this bounding box
        /// </summary>
        /// <param name="rng">The random number generator used to generate the point coordinates</param>
        /// <param name="number">The number of points to generate</param>
        /// <returns></returns>
        public Vector[] RandomPointsInside(Random rng, int number)
        {
            var result = new Vector[number];

            for (int i = 0; i < number; i++)
            {
                result[i] = RandomPointInside(rng);
            }

            return result;
        }

        /// <summary>
        /// Get the minimum value in the specified dimension
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public double MinInDimension(Dimension dimension)
        {
            if (dimension == Dimension.X) return MinX;
            else if (dimension == Dimension.Y) return MinY;
            else return MinZ;
        }

        /// <summary>
        /// Get the maximum value in the specified dimension
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public double MaxInDimension(Dimension dimension)
        {
            if (dimension == Dimension.X) return MaxX;
            else if (dimension == Dimension.Y) return MaxY;
            else return MaxZ;
        }

        /// <summary>
        /// Get the interval of values in the specified dimension
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public Interval IntervalInDimension(Dimension dimension)
        {
            if (dimension == Dimension.X) return X;
            else if (dimension == Dimension.Y) return Y;
            else return Z;
        }

        /// <summary>
        /// Check whether the specified box region overlaps this box.
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="minZ"></param>
        /// <param name="maxZ"></param>
        /// <returns></returns>
        public bool Overlaps(double minX, double maxX, double minY, double maxY, double minZ, double maxZ)
        {
            return (maxX >= MinX && minX <= MaxX &&
                maxY >= MinY && minY <= MaxY &&
                maxZ >= MinZ && minZ <= MaxZ);
        }

        /// <summary>
        /// Check whether the specified other bounding box overlaps this one
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(BoundingBox other)
        {
            return Overlaps(other.MinX, other.MaxX, other.MinY, other.MaxY, other.MinZ, other.MaxZ);
        }

        /// <summary>
        /// Expand this bounding box in all directions by the specified amount
        /// </summary>
        /// <param name="distance"></param>
        public void Expand(double distance)
        {
            MinX -= distance;
            MaxX += distance;
            MinY -= distance;
            MaxY += distance;
            MinZ -= distance;
            MaxZ += distance;
        }

        /// <summary>
        /// Scale this bounding box by a factor in all directions about its own
        /// centroid
        /// </summary>
        /// <param name="factor"></param>
        public void Scale(double factor)
        {
            Vector mid = Mid;
            MinX = Mid.X + (MinX - Mid.X) * factor;
            MaxX = Mid.X + (MaxX - Mid.X) * factor;
            MinY = Mid.Y + (MinY - Mid.Y) * factor;
            MaxY = Mid.Y + (MaxY - Mid.Y) * factor;
            MinZ = Mid.Z + (MinZ - Mid.Z) * factor;
            MaxZ = Mid.Z + (MaxX - Mid.Z) * factor;
        }

        #endregion
    }
}
