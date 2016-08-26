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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Maths
{
    /// <summary>
    /// Base class for matrices of doubles.
    /// </summary>
    [Serializable]
    public abstract class Matrix : IDuplicatable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the value at a specific position in the matrix
        /// </summary>
        /// <param name="i">The column index</param>
        /// <param name="j">The row index</param>
        /// <returns>The value at [i,j] in this matrix</returns>
        public abstract double this[int i, int j] { get; set; }

        /// <summary>
        /// The number of rows in this matrix
        /// </summary>
        public int Rows { get; protected set; }

        /// <summary>
        /// The number of columns in this matrix
        /// </summary>
        public int Columns { get; protected set; }

        /// <summary>
        /// Returns true if this is a square matrix -
        /// i.e. the number of rows equals the number of columns
        /// </summary>
        public bool IsSquare { get { return Rows == Columns; } }

        #endregion

        #region Construtors

        /// <summary>
        /// Base constructor.  The number of rows and columns must be specified
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        protected Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Internal function to create a new blank matrix of the same type 
        /// as this one. 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        protected abstract Matrix CreateNewMatrix(int rows, int columns);

        /// <summary>
        /// Set the values of the fields in the 'main diagonal',
        /// i.e. the fields running from 'top left' to 'bottom right',
        /// or where i = j.
        /// </summary>
        /// <param name="value"></param>
        public void SetMainDiagonal(double value)
        {
            int max = Math.Min(Rows, Columns);
            for (int i = 0; i < max; i++)
            {
                this[i, i] = value;
            }
        }

        /// <summary>
        /// Create the transpose of this matrix.
        /// The transpose of a matrix is obtained by flipping rows and columns.
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            Matrix result = CreateNewMatrix(Columns, Rows);
            for (int i = 0; i < Rows; i++)
            {
                for (int j =0; j < Columns; j++)
                {
                    result[j, i] = this[j, i];
                }
            }
            return result;
        }

        /// <summary>
        /// Matrix addition in-place.  Adds another matrix to this one.
        /// To obtain a new sum matrix without modifying this one, use the + operator instead.
        /// </summary>
        /// <param name="other">Another matrix with the same dimensions as this one.</param>
        /// <returns></returns>
        public void Add(Matrix other)
        {
            if (Rows != other.Rows || Columns != other.Columns) throw new Exception("Matrix dimension mis-match.  Only matrices with the same dimensions can be added together.");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] += other[i, j];
                }
            }
        }

        /// <summary>
        /// Matrix subtraction in-place.  Subtracts another matrix from this one.
        /// To obtain a new sum matrix without modifying this one, use the - operator instead.
        /// </summary>
        /// <param name="other">Another matrix with the same dimensions as this one.</param>
        public void Subtract(Matrix other)
        {
            if (Rows != other.Rows || Columns != other.Columns) throw new Exception("Matrix dimension mis-match.  Only matrices with the same dimensions can be added together.");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] -= other[i, j];
                }
            }
        }

        /// <summary>
        /// Multiply this matrix by a scalar in-place.
        /// All fields of this matrix will be multiplied by the specified factor.
        /// </summary>
        /// <param name="scalar">The factor to multiply by</param>
        public void Multiply(double scalar)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] *= scalar;
                }
            }
        }

        /// <summary>
        /// Divide this matrix by a scalar in-place.
        /// All fields of this matrix will be divided by the specified factor.
        /// </summary>
        /// <param name="scalar">The factor to devide by</param>
        public void Divide(double scalar)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] /= scalar;
                }
            }
            this.Duplicate();
        }

        ///// <summary>
        ///// Create a duplicate of this matrix
        ///// </summary>
        ///// <returns></returns>
        //public abstract Matrix Duplicate();

        #endregion

        #region Operators

        /// <summary>
        /// Addition operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            Matrix result = a.Duplicate();
            result.Add(b);
            return result;
        }

        /// <summary>
        /// Subtraction operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix a, Matrix b)
        {
            Matrix result = a.Duplicate();
            result.Subtract(b);
            return result;
        }

        /// <summary>
        /// Scalar multiplication operator
        /// </summary>
        /// <param name="s"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix operator *(double s, Matrix m)
        {
            Matrix result = m.Duplicate();
            result.Multiply(s);
            return result;
        }

        /// <summary>
        /// Scalar multiplication operator
        /// </summary>
        /// <param name="s"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix m, double s)
        {
            Matrix result = m.Duplicate();
            result.Multiply(s);
            return result;
        }

        /// <summary>
        /// Matrix multiplication operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks>Uses standard matrix multiplication.
        /// Possibly switch out for Strassen algorithm?</remarks>
        public static Matrix operator *(Matrix a, Matrix b)
        {
            int aColumns = a.Columns;
            if (aColumns != b.Rows)
                throw new Exception(
                    "Matrix dimensions miss-match.  The number of columns of the first matrix do not match the number of rows of the second.  Therefore the product of the two cannot be caulculated.");
            int rows = a.Rows;
            int columns = b.Columns;
            
            Matrix result = a.CreateNewMatrix(rows, columns);
            
            //for (int i = 0; i < result.Rows; i++) //Old, un-parallel version
            Parallel.For(0, rows, i => //Calculation will be run in parallel for each row
                {
                    for (int j = 0; j < columns; j++)
                    {
                        // The value in each field will be the cumulative multiplication of the equivalent
                        // row from A and column from B:
                        double value = 0;
                        for (int k = 0; k < aColumns; k++)
                        {
                            value += a[i, k] * b[k, j];
                        }
                        result[i, j] = value;
                    }
                }
            );

            return result;
        }

        /// <summary>
        /// Scalar division operator
        /// </summary>
        /// <param name="s"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix operator /(Matrix m, double s)
        {
            Matrix result = m.Duplicate();
            result.Divide(s);
            return result;
        }

        #endregion

    }
}
