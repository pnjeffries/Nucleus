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

namespace FreeBuild.Maths
{
    [Serializable]
    public class ArrayMatrix : Matrix
    {
        #region Properties

        /// <summary>
        /// Internal backing array of matrix values
        /// </summary>
        private double[,] _Values;

        /// <summary>
        /// Gets or sets the value at a specific position in the matrix
        /// </summary>
        /// <param name="i">The column index</param>
        /// <param name="j">The row index</param>
        /// <returns>The value at [i,j] in this matrix</returns>
        public override double this[int i, int j]
        {
            get { return _Values[i, j]; }
            set { _Values[i, j] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor creating an empty matrix with the specified number of rows and columns
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public ArrayMatrix(int rows, int columns) : base(rows,columns)
        {
            _Values = new double[rows, columns];
        }

        /// <summary>
        /// Constructor creating a matrix from a two-dimensional array of doubles
        /// </summary>
        /// <param name="values">The 2D array of values</param>
        public ArrayMatrix(double[,] values) : base(values.GetLength(0),values.GetLength(1))
        {
            _Values = values;
        }

        /// <summary>
        /// Constructor creating a matrix from a vector.
        /// The vector will be taken as a column vector.
        /// </summary>
        /// <param name="columnVector">The vector to convert to a matrix</param>
        public ArrayMatrix(Vector columnVector) : base(3,1)
        {
            _Values = new double[,] { { columnVector.X }, { columnVector.Y }, { columnVector.Z } };
        }

        /// <summary>
        /// Initialise a matrix filled with (pseudo)random numbers
        /// </summary>
        /// <param name="rows">The number of rows in the matrix</param>
        /// <param name="columns">The number of columns in the matrix</param>
        /// <param name="populateWith">The random number generator that will provide the (pseudo)random values</param>
        /// <param name="scale">The maximum value of the random values</param>
        public ArrayMatrix(int rows, int columns, Random populateWith, double scale = 1) : base(rows,columns)
        {
            _Values = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    this[i, j] = populateWith.NextDouble() * scale;
                }
            }
        }

        /// <summary>
        /// Constructor creating a 3 x 3 matrix with the specified values
        /// </summary>
        /// <param name="a11">Value in row 1, column 1</param>
        /// <param name="a12">Value in row 1, column 2</param>
        /// <param name="a13">Value in row 1, column 3</param>
        /// <param name="a21">Value in row 2, column 1</param>
        /// <param name="a22">Value in row 2, column 2</param>
        /// <param name="a23">Value in row 2, column 3</param>
        /// <param name="a31">Value in row 3, column 1</param>
        /// <param name="a32">Value in row 3, column 2</param>
        /// <param name="a33">Value in row 3, column 3</param>
        public ArrayMatrix(
            double a11, double a12, double a13,
            double a21, double a22, double a23,
            double a31, double a32, double a33) : base(3,3)
        {
            _Values = new double[,] { 
                { a11, a12, a13 }, 
                { a21, a22, a23 }, 
                { a31, a32, a33 } };
        }

        /// <summary>
        /// Constructor creating a 4 x 4 matrix with the specified values
        /// </summary>
        /// <param name="a11"></param>
        /// <param name="a12"></param>
        /// <param name="a13"></param>
        /// <param name="a14"></param>
        /// <param name="a21"></param>
        /// <param name="a22"></param>
        /// <param name="a23"></param>
        /// <param name="a24"></param>
        /// <param name="a31"></param>
        /// <param name="a32"></param>
        /// <param name="a33"></param>
        /// <param name="a34"></param>
        /// <param name="a41"></param>
        /// <param name="a42"></param>
        /// <param name="a43"></param>
        /// <param name="a44"></param>
        public ArrayMatrix(
            double a11, double a12, double a13, double a14,
            double a21, double a22, double a23, double a24,
            double a31, double a32, double a33, double a34,
            double a41, double a42, double a43, double a44) : base(4, 4)
        {
            _Values = new double[,] { 
                { a11, a12, a13, a14 }, 
                { a21, a22, a23, a24 }, 
                { a31, a32, a33, a34 },
                { a41, a42, a43, a44 } };
        }

        /// <summary>
        /// Constructor creating a clone of an existing matrix
        /// </summary>
        /// <param name="other"></param>
        public ArrayMatrix(Matrix other) : base(other.Rows, other.Columns)
        {
            SetBlock(0, 0, other);
        }

        #endregion

        #region Methods

        protected override Matrix CreateNewMatrix(int rows, int columns)
        {
            return new ArrayMatrix(rows, columns);
        }

        ///// <summary>
        ///// Create a duplicate of this matrix.
        ///// </summary>
        ///// <returns>A new ArrayMatrix with the same values as this one.</returns>
        public override Matrix Duplicate()
        {
            return new ArrayMatrix((double[,])_Values.Clone());
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create a square identity matrix of the given size
        /// </summary>
        /// <param name="size">The number of rows and columns the matrix will have</param>
        /// <returns></returns>
        public static ArrayMatrix Identity(int size)
        {
            ArrayMatrix result = new ArrayMatrix(size, size);
            result.SetMainDiagonal(1.0);
            return result;
        }

        #endregion

    }
}
