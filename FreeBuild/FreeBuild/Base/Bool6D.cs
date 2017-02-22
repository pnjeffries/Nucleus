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

using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// 6D Boolean Structure.
    /// Represents true or false values related to the three dimensions X,Y,Z and to
    /// rotations about these axes (termed XX, YY and ZZ respectively).
    /// Immutable.
    /// </summary>
    /// <remarks>
    /// This type is currently implemented as a struct rather than a class,
    /// though this will be subject to review and may be changed.</remarks>
    [Serializable]
    public struct Bool6D
    {
        #region fields

        /// <summary>
        /// The value in the X-direction
        /// </summary>
        public readonly bool X;

        /// <summary>
        /// The value in the Y-direction
        /// </summary>
        public readonly bool Y;

        /// <summary>
        /// The value in the Z-direction
        /// </summary>
        public readonly bool Z;

        /// <summary>
        /// The value about the XX axis
        /// </summary>
        public readonly bool XX;

        /// <summary>
        /// The value about the YY axis
        /// </summary>
        public readonly bool YY;

        /// <summary>
        /// The value about the ZZ axis
        /// </summary>
        public readonly bool ZZ;

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if all six values in this Bool6D are false
        /// </summary>
        public bool AllFalse
        {
            get { return !(X || Y || Z || XX || YY || ZZ); }
        }

        /// <summary>
        /// Returns true if all six values in this Bool6D are true
        /// </summary>
        public bool AllTrue
        {
            get { return X && Y && Z && XX && YY && ZZ; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a Bool6D with the specified values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <param name="zz"></param>
        public Bool6D(bool x, bool y, bool z, bool xx, bool yy, bool zz)
        {
            X = x;
            Y = y;
            Z = z;
            XX = xx;
            YY = yy;
            ZZ = zz;
        }

        /// <summary>
        /// Initialise a Bool6D with all values set to either true or false
        /// </summary>
        /// <param name="all"></param>
        public Bool6D(bool all) : this(all, all, all, all, all, all) { }

        /// <summary>
        /// Initialise a Bool6D with the specified X,Y,Z values.
        /// XX, YY and ZZ will be set to false.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Bool6D(bool x, bool y, bool z) : this(x, y, z, false, false, false) { }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new Bool6D as a negated copy of this one.
        /// All components will have the opposite values.
        /// </summary>
        /// <returns></returns>
        public Bool6D Invert()
        {
            return new Bool6D(!X, !Y, !Z, !XX, !YY, !ZZ);
        }

        /// <summary>
        /// Create a new Bool6D copying all values from this one
        /// bar the new specified value of X.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bool6D WithX(bool value)
        {
            return new Bool6D(value, Y, Z, XX, YY, ZZ);
        }

        /// <summary>
        /// Create a new Bool6D copying all values from this one
        /// bar the new specified value of Y.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bool6D WithY(bool value)
        {
            return new Bool6D(X, value, Z, XX, YY, ZZ);
        }

        /// <summary>
        /// Create a new Bool6D copying all values from this one
        /// bar the new specified value of Z.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bool6D WithZ(bool value)
        {
            return new Bool6D(X, Y, value, XX, YY, ZZ);
        }

        /// <summary>
        /// Create a new Bool6D copying all values from this one
        /// bar the new specified value of XX.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bool6D WithXX(bool value)
        {
            return new Bool6D(X, Y, Z, value, YY, ZZ);
        }

        /// <summary>
        /// Create a new Bool6D copying all values from this one
        /// bar the new specified value of YY.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bool6D WithYY(bool value)
        {
            return new Bool6D(X, Y, Z, XX, value, ZZ);
        }

        /// <summary>
        /// Create a new Bool6D copying all values from this one
        /// bar the new specified value of ZZ.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bool6D WithZZ(bool value)
        {
            return new Bool6D(X, Y, Z, XX, YY, value);
        }

        /// <summary>
        /// Turn this Bool6D into a string representing the current values
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (X) sb.Append("X");
            if (Y) sb.Append("Y", "-");
            if (Z) sb.Append("Z", "-");
            if (XX) sb.Append("XX", "-");
            if (YY) sb.Append("YY", "-");
            if (ZZ) sb.Append("ZZ", "-");
            if (sb.Length == 0) sb.Append("-");
            return sb.ToString();
        }

        /// <summary>
        /// Turn this Bool6D to a string describing it as a restraint condition.
        /// Gives the same result as ToString(), but with the additional special cases
        /// of 'Pin' and 'Fixed' for translational and full restraint respectively.
        /// </summary>
        /// <returns></returns>
        public string ToRestraintDescription()
        {
            var sb = new StringBuilder();
            if (X && Y && Z)
            {
                if (XX && YY && ZZ) return "Fixed";
                else sb.Append("Pin");
            }
            else
            {
                if (X) sb.Append("X");
                if (Y) sb.Append("Y", "-");
                if (Z) sb.Append("Z", "-");
            }
            if (XX) sb.Append("XX", "-");
            if (YY) sb.Append("YY", "-");
            if (ZZ) sb.Append("ZZ", "-");
            if (sb.Length == 0) sb.Append("-");
            return sb.ToString();
        }

        #endregion
    }
}
