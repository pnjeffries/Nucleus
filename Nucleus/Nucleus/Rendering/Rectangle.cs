using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A class representing a 2D axis-aligned rectangle
    /// </summary>
    public class Rectangle
    {
        #region Properties

        /// <summary>
        /// The starting value of the rectangle in the X-direction
        /// </summary>
        public double Left { get; }

        /// <summary>
        /// The ending value of the rectangle in the X-direction
        /// </summary>
        public double Right { get; }

        /// <summary>
        /// The starting value of the rectangle in the Y-direction
        /// </summary>
        public double Top { get; }

        /// <summary>
        /// The ending value of the rectangle in the Y-direction
        /// </summary>
        public double Bottom { get; }

        /// <summary>
        /// Get the width of this rectangle
        /// </summary>
        public double Width { get { return Right - Left; } }

        /// <summary>
        /// Get the height of this rectangle
        /// </summary>
        public double Height { get { return Top - Bottom; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Rectangle with the specified positions
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public Rectangle(double left, double right, double top, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        #endregion
    }
}
