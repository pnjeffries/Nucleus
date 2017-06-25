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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Conversion
{
    /// <summary>
    /// Enum representing the possible directions of a data conversion
    /// </summary>
    public enum ConversionDirection
    {
        /// <summary>
        /// Conversions in both directions are possible
        /// </summary>
        Both = 0,
        /// <summary>
        /// This conversion mapping applies from type A to type B, but not the other way
        /// </summary>
        AtoB = 1,
        /// <summary>
        /// This conversion mapping applies from type B to type A, but not the other way
        /// </summary>
        BtoA = 2
    }

    /// <summary>
    /// ConversionDirections extension helper methods
    /// </summary>
    public static class ConversionDirectionsExtensions
    {
        /// <summary>
        /// Invert this conversion direction
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConversionDirection Invert(this ConversionDirection value)
        {
            if (value == ConversionDirection.AtoB) return ConversionDirection.BtoA;
            else if (value == ConversionDirection.BtoA) return ConversionDirection.AtoB;
            else return ConversionDirection.Both;
        }
    }
}
