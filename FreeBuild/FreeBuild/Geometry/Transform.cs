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

using FreeBuild.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A transform is an operation which can be applied to vertex geometry
    /// in order to change its position or orientation.
    /// Represented as a 4 x 4 transformation matrix
    /// </summary>
    public class Transform : ArrayMatrix
    {
        #region Constructors

        /// <summary>
        /// Default constructor - creates a new identity transform.
        /// Equates to a 4x4 matrix with the main diagonal set to 1.0.
        /// </summary>
        public Transform() : base(4,4)
        {
            SetMainDiagonal(1.0);
        }

        /// <summary>
        /// Translation constructor.  Creates a translation transform along the specified vector
        /// </summary>
        /// <param name="translationVector">The vector of the translation</param>
        public Transform(Vector translationVector) : this()
        {
            this[0, 3] = translationVector.X;
            this[1, 3] = translationVector.Y;
            this[2, 3] = translationVector.Z;
        }

        #endregion
    }
}
