using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A generic base class for two-dimensional intervals
    /// </summary>
    /// <typeparam name="TCoord"></typeparam>
    [Serializable]
    public abstract class Rectangle<TCoord>
        where TCoord : IComparable
    {
        #region Properties

        /// <summary>
        /// The first value in the first dimension
        /// </summary>
        public TCoord XStart { get; set; }

        /// <summary>
        /// The first value in the second dimension
        /// </summary>
        public TCoord YStart { get; set; }

        /// <summary>
        /// The second value in the first dimension
        /// </summary>
        public TCoord XEnd { get; set; }

        /// <summary>
        /// The second value in the second dimension
        /// </summary>
        public TCoord YEnd { get; set; }

        /// <summary>
        /// Get the minimum value in the first dimension
        /// </summary>
        public TCoord XMin
        {
            get
            {
                if (XStart.CompareTo(XEnd) > 0) return XEnd;
                else return XStart;
            }
        }

        /// <summary>
        /// Get the maximum value in the first dimension
        /// </summary>
        public TCoord XMax
        {
            get
            {
                if (XStart.CompareTo(XEnd) < 0) return XEnd;
                else return XStart;
            }
        }

        /// <summary>
        /// Get the minimum value in the second dimension
        /// </summary>
        public TCoord YMin
        {
            get
            {
                if (YStart.CompareTo(YEnd) > 0) return YEnd;
                else return YStart;
            }
        }

        /// <summary>
        /// Get the maximum value in the second dimension
        /// </summary>
        public TCoord YMax
        {
            get
            {
                if (YStart.CompareTo(YEnd) < 0) return YEnd;
                else return YStart;
            }
        }

        /// <summary>
        /// Get the size of the domain in the first dimension
        /// </summary>
        public TCoord XSize
        {
            get { return Subtract(XEnd, XStart); }
        }

        /// <summary>
        /// Get the size of the domain in the second dimension
        /// </summary>
        public TCoord YSize
        {
            get { return Subtract(YEnd, YStart); }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Rectangle() { }

        /// <summary>
        /// Initialise the bounds of this rectangle to the specified values
        /// </summary>
        /// <param name="xStart">The start value of the x-domain</param>
        /// <param name="xEnd">The end value of the x-domain</param>
        /// <param name="yStart">The start value of the y-domain</param>
        /// <param name="yEnd">The end value of the y-domain</param>
        public Rectangle(TCoord xStart, TCoord xEnd, TCoord yStart, TCoord yEnd)
        {
            XStart = xStart;
            YStart = yStart;
            XEnd = xEnd;
            YEnd = yEnd;
        }

        /// <summary>
        /// Initialise all bounds to a given value
        /// </summary>
        /// <param name="value"></param>
        public Rectangle(TCoord value)
        {
            XStart = value;
            YStart = value;
            XEnd = value;
            YEnd = value;
        }

        /// <summary>
        /// Initialise a rectangle with both start and end values
        /// set to the specified values in each dimension
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Rectangle(TCoord x, TCoord y)
        {
            XStart = x;
            XEnd = x;
            YStart = y;
            YEnd = y;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Do the specified values in each dimension fall within this rectangular interval
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsInside(TCoord x, TCoord y)
        {
            if (x.CompareTo(XMin) >= 0 && x.CompareTo(XMax) <= 0 &&
                y.CompareTo(YMin) >= 0 && y.CompareTo(YMax) <= 0) return true;
            else return false;
        }

        /// <summary>
        /// Expand this rectangular interval equally in all directions by a specified amount
        /// </summary>
        /// <param name="amount"></param>
        public void Expand(TCoord amount)
        {
            XStart = Subtract(XStart, amount);
            XEnd = Add(XEnd, amount);
            YStart = Subtract(YStart, amount);
            YEnd = Add(YEnd, amount);
        }

        /// <summary>
        /// Grow this rectangle in the specified direction
        /// by the specified amount
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="amount"></param>
        public void Grow(CompassDirection direction, TCoord amount)
        {
            if (direction == CompassDirection.North)
                YEnd = Add(YEnd, amount);
            else if (direction == CompassDirection.East)
                XEnd = Add(XEnd, amount);
            else if (direction == CompassDirection.South)
                YStart = Subtract(YStart, amount);
            else if (direction == CompassDirection.West)
                XStart = Subtract(XStart, amount);
        }

        /// <summary>
        /// Move this rectangle in the specified direction by the
        /// specified distance
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        public void Move(CompassDirection direction, TCoord distance)
        {
            if (direction == CompassDirection.North)
            {
                YStart = Add(YStart, distance);
                YEnd = Add(YEnd, distance);
            }
            else if (direction == CompassDirection.East)
            {
                XStart = Add(XStart, distance);
                XEnd = Add(XEnd, distance);
            }
            else if (direction == CompassDirection.South)
            {
                YStart = Subtract(YStart, distance);
                YEnd = Subtract(YEnd, distance);
            }
            else if (direction == CompassDirection.West)
            {
                XStart = Subtract(XStart, distance);
                XEnd = Subtract(XEnd, distance);
            }
        }

        /// <summary>
        /// Add two values
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        protected abstract TCoord Add(TCoord v1, TCoord v2);

        /// <summary>
        /// Subtract two values
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        protected abstract TCoord Subtract(TCoord v1, TCoord v2);

        #endregion
    }

    /// <summary>
    /// A rectangular two-dimensional interval defined by double value bounds
    /// </summary>
    [Serializable]
    public class Rectangle : Rectangle<double>
    {
        #region Properties

        /// <summary>
        /// Get the width of the rectangle
        /// </summary>
        public double Width
        {
            get { return XEnd - XStart; }
        }

        /// <summary>
        /// Get the height of the rectangle
        /// </summary>
        public double Height
        {
            get { return YEnd - YStart; }
        }

        /// <summary>
        /// The mid-point X coordinate
        /// </summary>
        public double XMid
        {
            get { return (XStart + XEnd) * 0.5; }
        }

        /// <summary>
        /// The mid-point Y coordinate
        /// </summary>
        public double YMid
        {
            get { return (YStart + YEnd) * 0.5; }
        }

        /// <summary>
        /// The area of the rectangle
        /// </summary>
        public double Area
        {
            get { return Width * Height; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialise all bounds to 0.
        /// </summary>
        public Rectangle()
        {
        }

        /// <summary>
        /// Initialise all bounds to a given value
        /// </summary>
        /// <param name="value"></param>
        public Rectangle(double value) : base(value)
        {
        }

        /// <summary>
        /// Initialise a rectangle with the specified width and height, centred on the origin
        /// </summary>
        /// <param name="width">The width (dimension in X) of the rectangle</param>
        /// <param name="height">The height (dimension in Y) of the rectangle</param>
        public Rectangle(double width, double height) : base(-width/2, width/2, -height/2, height/2)
        {
        }

        /// <summary>
        /// Initialise the bounds of this rectangle to the specified values
        /// </summary>
        /// <param name="xStart">The start value of the x-domain</param>
        /// <param name="xEnd">The end value of the x-domain</param>
        /// <param name="yStart">The start value of the y-domain</param>
        /// <param name="yEnd">The end value of the y-domain</param>
        public Rectangle(double xStart, double xEnd, double yStart, double yEnd) : base(xStart, xEnd, yStart, yEnd)
        {
        }

        #endregion

        #region Methods

        protected override double Add(double v1, double v2)
        {
            return v1 + v2;
        }

        protected override double Subtract(double v1, double v2)
        {
            return v1 - v2;
        }

        /// <summary>
        /// Scale this rectangle evenly in all directions about its own centroid
        /// </summary>
        /// <param name="scalar"></param>
        public void Scale(double scalar)
        {
            double xBar = (XStart + XEnd) / 2.0;
            double yBar = (YStart + YEnd) / 2.0;
            XStart = xBar + (XStart - xBar) * scalar;
            XEnd = xBar + (XEnd - xBar) * scalar;
            YStart = yBar + (YStart - yBar) * scalar;
            YEnd = yBar + (YEnd - yBar) * scalar;
        }

        #endregion
    }

    /// <summary>
    /// A rectangular two-dimensional interval defined by integer value bounds
    /// </summary>
    [Serializable]
    public class IntRectangle : Rectangle<int>
    {

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialise all bounds to 0.
        /// </summary>
        public IntRectangle()
        {
        }

        /// <summary>
        /// Initialise all bounds to a given value
        /// </summary>
        /// <param name="value"></param>
        public IntRectangle(int value) : base(value)
        {
        }

        /// <summary>
        /// Initialise a rectangle with both start and end values
        /// set to the specified values in each dimension
        /// </summary>
        /// <param name="x">The value to initialise the first dimension bounds to</param>
        /// <param name="y">The value to initialise the second dimension bounds to</param>
        public IntRectangle(int x, int y) : base(x, y)
        {
        }

        /// <summary>
        /// Initialise the bounds of this rectangle to the specified values
        /// </summary>
        /// <param name="xStart">The start value of the x-domain</param>
        /// <param name="xEnd">The end value of the x-domain</param>
        /// <param name="yStart">The start value of the y-domain</param>
        /// <param name="yEnd">The end value of the y-domain</param>
        public IntRectangle(int xStart, int xEnd, int yStart, int yEnd) : base(xStart, xEnd, yStart, yEnd)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new IntRectangle representing the area to be added if this
        /// rectangle were to grow by the specified distance in the specified
        /// direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public IntRectangle GrowZone(CompassDirection direction, int distance)
        {
            if (direction == CompassDirection.North)
                return new IntRectangle(XStart, XEnd, YEnd + 1, YEnd + distance);
            else if (direction == CompassDirection.East)
                return new IntRectangle(XEnd + 1, XEnd + distance, YStart, YEnd);
            else if (direction == CompassDirection.South)
                return new IntRectangle(XStart, XEnd, YStart - distance, YStart - 1);
            else
                return new IntRectangle(XStart - distance, XStart - 1, YStart, YEnd);
        }

        protected override int Add(int v1, int v2)
        {
            return v1 + v2;
        }

        protected override int Subtract(int v1, int v2)
        {
            return v1 - v2;
        }

        /// <summary>
        /// Choose a random point on the outside edge of this rectangle
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="rng"></param>
        /// <param name="endOffset"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void RandomPointOnEdge(CompassDirection edge, Random rng, int endOffset, ref int i, ref int j)
        {
            if (edge == CompassDirection.North) j = YMax + 1;
            else if (edge == CompassDirection.East) i = XMax + 1;
            else if (edge == CompassDirection.South) j = YMin - 1;
            else if (edge == CompassDirection.West) i = XMin - 1;

            if (edge.IsHorizontal()) j = rng.Next(YMin, YMax - endOffset + 1);
            else i = rng.Next(XMin, XMax - endOffset + 1);
        }

        #endregion
    }

}
