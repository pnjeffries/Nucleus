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

using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// An Element Orientation expressed by a rotation angle
    /// </summary>
    [Serializable]
    public class ElementOrientationAngle : ElementOrientation
    {
        #region Properties

        /// <summary>
        /// The angle value, in radians
        /// </summary>
        public Angle Value { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialisa an ElementOrientationAngle with the specified angle value
        /// </summary>
        /// <param name="value"></param>
        public ElementOrientationAngle(Angle value)
        {
            Value = value;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implicit conversion from an ElementOrientationAngle to a double
        /// </summary>
        /// <param name="angle"></param>
        public static implicit operator double(ElementOrientationAngle angle)
        {
            return angle.Value;
        }

        /// <summary>
        /// Implicit conversion from an ElementOrientationAngle to an angle
        /// </summary>
        /// <param name="angle"></param>
        public static implicit operator Angle(ElementOrientationAngle angle)
        {
            return angle.Value;
        }

        /// <summary>
        /// Implicit conversion from a double to an ElementOrientationAngle
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ElementOrientationAngle(double value)
        {
            return new ElementOrientationAngle(value);
        }

        /// <summary>
        /// Implicit conversion from an angle to an ElementOrientationAngle
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ElementOrientationAngle(Angle value)
        {
            return new ElementOrientationAngle(value);
        }

        #endregion
    }
}
