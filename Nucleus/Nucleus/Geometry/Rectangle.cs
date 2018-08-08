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
        /// Get the maximum value in the first dimension
        /// </summary>
        public TCoord YMax
        {
            get
            {
                if (YStart.CompareTo(YEnd) < 0) return YEnd;
                else return YStart;
            }
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
    public class Rectangle : Rectangle<double>
    {
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
        /// Initialise a rectangle with both start and end values
        /// set to the specified values in each dimension
        /// </summary>
        /// <param name="x">The value to initialise the first dimension bounds to</param>
        /// <param name="y">The value to initialise the second dimension bounds to</param>
        public Rectangle(double x, double y) : base(x, y)
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

        #endregion
    }

    /// <summary>
    /// A rectangular two-dimensional interval defined by integer value bounds
    /// </summary>
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

        protected override int Add(int v1, int v2)
        {
            return v1 + v2;
        }

        protected override int Subtract(int v1, int v2)
        {
            return v1 - v2;
        }

        #endregion
    }

}
