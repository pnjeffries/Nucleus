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

using Nucleus.Base;
using Nucleus.Exceptions;
using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nucleus.Maths
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
        /// <param name="i">The row index</param>
        /// <param name="j">The column index</param>
        /// <returns>The value at [i,j] in this matrix</returns>
        public abstract double this[int i, int j] { get; set; }

        /// <summary>
        /// Private backing field for Rows property
        /// </summary>
        private int _Rows;

        /// <summary>
        /// The number of rows in this matrix
        /// </summary>
        public int Rows
        {
            get { return _Rows; }
            protected set { _Rows = value; }
        }

        /// <summary>
        /// Private backing field for Columns property
        /// </summary>
        private int _Columns;

        /// <summary>
        /// The number of columns in this matrix
        /// </summary>
        public int Columns
        {
            get { return _Columns; }
            protected set { _Columns = value; }
        }

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
        /// Set the values of the fields in the specified row.
        /// </summary>
        /// <param name="row">The row index</param>
        /// <param name="values">The new set of values to be placed within the row</param>
        public void SetRow(int row, double[] values)
        {
            for(int j = 0; j < Math.Min(Columns,values.Length); j++)
            {
                this[row, j] = values[j];
            }
        }

        /// <summary>
        /// Set the values of the fields in the specified column.
        /// </summary>
        /// <param name="column">The column index</param>
        /// <param name="values">The new set of values to be placed within the column</param>
        public void SetColumn(int column, double[] values)
        {
            for (int i = 0; i < Math.Min(Rows, values.Length); i++)
            {
                this[i,column] = values[i];
            }
        }

        /// <summary>
        /// Clear a row (i.e. set all values in that row to 0)
        /// </summary>
        /// <param name="row">The index of the row to clear</param>
        public virtual void ClearRow(int row)
        {
            for (int j = 0; j < Columns; j++) this[row, j] = 0;
        }

        /// <summary>
        /// Clear a row (i.e. set all values in that row to 0),
        /// optionally maintaining non-singularity by setting the
        /// diagonal to 1.
        /// </summary>
        /// <param name="row">The index of the row to clear</param>
        /// <param name="maintainNonSingularity">If true, non-singularity will be 
        /// maintained by setting the diagonal in the row to be 1.</param>
        public void ClearRow(int row, bool maintainNonSingularity)
        {
            ClearRow(row);
            if (maintainNonSingularity && Columns > row) this[row, row] = 1;
        }

        /// <summary>
        /// Clear a column (i.e. set all values in that column to 0)
        /// </summary>
        /// <param name="column">The index of the column to clear</param>
        public virtual void ClearColumn(int column)
        {
            for (int i = 0; i < Columns; i++) this[i, column] = 0;
        }

        /// <summary>
        /// Clear a column (i.e. set all values in that column to 0),
        /// optionally maintaining non-singularity by setting the
        /// diagonal to 1.
        /// </summary>
        /// <param name="column">The index of the column to clear</param>
        /// /// <param name="maintainNonSingularity">If true, non-singularity will be 
        /// maintained by setting the diagonal in the column to be 1.</param>
        public void ClearColumn(int column, bool maintainNonSingularity)
        {
            ClearColumn(column);
            if (maintainNonSingularity && Rows > column) this[column, column] = 1;
        }

        /// <summary>
        /// Overwrites a section of this matrix with the values defined within another,
        /// starting at the specified indices
        /// </summary>
        /// <param name="startRow">The starting row index for insertion</param>
        /// <param name="startColumn">The starting column index for insertion</param>
        /// <param name="values">The matrix of values to be inserted</param>
        public virtual void SetBlock(int startRow, int startColumn, Matrix values)
        {
            //Possibly faster method using Array.Copy?
            Parallel.For(0, Math.Min(Rows - startRow,values.Rows), i =>
            {
                for (int j = 0; j < Math.Min(Columns - startColumn, values.Columns); j++)
                {
                    this[i + startRow, j + startColumn] = values[i, j];
                }
            });
        }

        /// <summary>
        /// Exchange the values in two rows in this matrix.
        /// An elementary row operation.
        /// </summary>
        /// <param name="iA">The first row index</param>
        /// <param name="iB">The second row index</param>
        public virtual void SwapRows(int iA, int iB)
        {
            for (int j = 0; j < Columns; j++)
            {
                double valA = this[iA, j];
                this[iA, j] = this[iB, j];
                this[iB, j] = valA;
            }
        }

        /// <summary>
        /// Multiply a row by a factor.
        /// An elementary row operation.
        /// </summary>
        /// <param name="i">The index of the row to be modified.</param>
        /// <param name="scalar">The factor to multiply by.</param>
        public virtual void MultiplyRow(int i, double scalar)
        {
            for (int j = 0; j < Columns; j++)
            {
                this[i, j] *= scalar;
            }
        }


        /// <summary>
        /// Divide a row by a non-zero constant.
        /// An elementary row operation.
        /// </summary>
        /// <param name="i">The row index</param>
        /// <param name="divisor">The value to divide by.
        /// Should be non-zero.</param>
        public virtual void DivideRow(int i, double divisor)
        {
            for (int j = 0; j < Columns; j++)
            {
                this[i, j] /= divisor;
            }
        }

        /// <summary>
        /// Subtract row B times a factor from row A.
        /// An elementary row operation.
        /// </summary>
        /// <param name="iA">The index of the row to subtract from</param>
        /// <param name="iB">The index of the row to subtract</param>
        /// <param name="factor">The factor by which the second row is to be multiplied
        /// before subtraction.</param>
        public void SubtractRow(int iA, int iB, double factor)
        {
            for (int j = 0; j < Columns; j++)
            {
                this[iA, j] -= this[iB, j] * factor;
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
            //for (int i = 0; i < Rows; i++)
            Parallel.For(0, Rows, i =>
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[j, i] = this[i, j];
                }
            });
            return result;
        }

        /// <summary>
        /// Uses Gaussian Elimination to put the matrix into row-echelon form.
        /// </summary>
        public void GaussianElimination()
        {
            int i = 0;
            int j = 0;
            while (i < Rows && j < Columns)
            {
                // Find 'pivot' point in column j, starting from row i:
                int iMax = i;
                double max = Math.Abs(this[i, j]);
                for (int k = i + 1; k < Rows; i++)
                {
                    double test = Math.Abs(this[k, j]);
                    if (test > max)
                    {
                        iMax = k;
                        max = test;
                    }
                }
                if (max != 0)
                {
                    // Move entry with the largest absolute value to the pivot position
                    SwapRows(i, iMax);
                    DivideRow(i, this[i, j]); //[i,j] will now be 1
                    for (int k = i + 1; k < Rows; k++)
                    {
                        SubtractRow(k, i, this[k, j]);
                    }
                    i++;
                }
                j++;
            }
        }

        /// <summary>
        /// Express this matrix as the product of:
        /// L, a singular lower triangular matrix and
        /// U, an upper triangular matrix.
        /// This matrix must be square.
        /// </summary>
        /// <param name="L">Output.  A singular lower triangular matrix.</param>
        /// <param name="U">Output.  An upper triangular matrix.</param>
        public void LUDecomposition(out Matrix L, out Matrix U)
        {
            L = CreateNewMatrix(Rows, Columns);
            U = Duplicate();
            for (int j = 0; j < Columns; j++)
            {
                double div = U[j, j];
                L[j, j] = 1;
                for (int i = j+1; i < Rows; i++)
                {
                    double factor = U[i, j] / div;
                    L[i, j] = factor;
                    U.SubtractRow(i, j, factor);
                }
            }
        }

        /// <summary>
        /// Solve a matrix representing a system of simultaneous equations.
        /// The matrix must be augmented and in row-echelon form, with each column
        /// before the last indicating a variable coefficient 
        /// </summary>
        /// <returns>A single-row matrix going the solution for each column.</returns>
        public Matrix Solve()
        {
            Matrix result = CreateNewMatrix(1, Columns - 1);
            bool[] solved = new bool[Columns - 1]; // Stores whether a given solution has been found

            // Work backwards through the rows, solving each one
            for (int i = Rows - 1; i >= 0; i--)
            {
                int uC = 0; // Counts the number of unsolved variables
                int jU = -1; // The position of the last unsolved variable

                // Get the right hand side of the equation:
                double right = this[i, Columns - 1];

                for (int j = 0; j < Columns - 1; j++)
                {
                    double value = this[i, j];
                    if (value != 0)
                    {
                        if (!solved[j])
                        {
                            // Variable isn't solved yet - keep going
                            uC += 1;
                            jU = j;
                        }
                        else
                        {
                            // Substitute for the solved variable:
                            right -= value * result[0, j];
                        }
                    }
                }
                if (uC == 1) // There is only one unsolved variable in this row - we can solve it!
                {
                    result[0, jU] = right / this[i, jU];
                    solved[jU] = true;
                }
            }
            foreach (bool vSol in solved)
                if (!vSol)
                    throw new MatrixException("Solutions could not be found for all variables!");

            return result;
            // TODO: Could this be simplified?
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
            //for (int i = 0; i < Rows; i++)
            Parallel.For(0, Rows, i =>
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] += other[i, j];
                }
            });
        }

        /// <summary>
        /// Matrix subtraction in-place.  Subtracts another matrix from this one.
        /// To obtain a new sum matrix without modifying this one, use the - operator instead.
        /// </summary>
        /// <param name="other">Another matrix with the same dimensions as this one.</param>
        public void Subtract(Matrix other)
        {
            if (Rows != other.Rows || Columns != other.Columns) throw new Exception("Matrix dimension mis-match.  Only matrices with the same dimensions can be added together.");
            //for (int i = 0; i < Rows; i++)
            Parallel.For(0, Rows, i =>
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] -= other[i, j];
                }
            });
        }

        /// <summary>
        /// Multiply this matrix by a scalar in-place.
        /// All fields of this matrix will be multiplied by the specified factor.
        /// </summary>
        /// <param name="scalar">The factor to multiply by</param>
        public void Multiply(double scalar)
        {
            //for (int i = 0; i < Rows; i++)
            Parallel.For(0, Rows, i =>
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] *= scalar;
                }
            });
        }

        /// <summary>
        /// Divide this matrix by a scalar in-place.
        /// All fields of this matrix will be divided by the specified factor.
        /// </summary>
        /// <param name="scalar">The factor to devide by</param>
        public void Divide(double scalar)
        {
            Parallel.For(0, Rows, i => //for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] /= scalar;
                }
            });
        }

        /// <summary>
        /// Return a sub-matrix of this matrix by eliminating one row and one column
        /// </summary>
        /// <param name="row">The row number to remove</param>
        /// <param name="column">The column number to remove</param>
        /// <returns></returns>
        public virtual Matrix SubMatrix(int row, int column)
        {
            Matrix result = CreateNewMatrix(Rows - 1, Columns - 1);
            Parallel.For(0, Rows, i =>
            {
                if (i != row)
                {
                    int iT = i;
                    if (i > row) iT -= 1;
                    for (int j = 0; j < Columns; j++)
                    {
                        if (j < column) result[iT, j] = this[i, j];
                        else if (j > column) result[iT, j - 1] = this[i, j];
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// Calculate the adjugate, classical adjoint or adjunct of this matrix
        /// (i.e. the transpose of the matrix of cofactors).
        /// This is only valid for square matrices.
        /// </summary>
        /// <returns></returns>
        public Matrix Adjugate()
        {
            if (Rows != Columns) throw new MatrixException("Matrix is not square and therefore has no adjugate.");
            Matrix AT = this;//Transpose();
            Matrix result = CreateNewMatrix(AT.Rows, AT.Columns);
            Parallel.For(0, Rows, i =>
            {
                for (int j = 0; j < Columns; j++)
                {
                    Matrix sMatrix = AT.SubMatrix(i, j);
                    result[i, j] = sMatrix.Determinant() * Math.Pow(-1, i + j + 2);
                }
            });
            return result.Transpose();
        }

        /// <summary>
        /// Calculate the inverse of this matrix, if it exists.
        /// If there is no inverse (i.e. the determinant = 0) then null will
        /// be returned.
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            double det = Determinant();
            if (det == 0) return null;
            else return Adjugate() / det;
        }

        /// <summary>
        /// Calculate the determinant of this matrix by expanding along the first row.
        /// This is only valid for square matrices.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Recursive and of factorial order - not recommended for large matrices!</remarks>
        public virtual double Determinant()
        {
            if (Rows != Columns) throw new MatrixException("Matrix is not square and therefore has no determinant.");
            else if (Rows == 2) //2x2 Matrix - we can calculate that!
            {
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            }
            else if (Rows == 1) //1x1 Matrix - the determinant is the single value stored
            {
                return this[0, 0];
            }
            else //Larger matrix
            {
                double result = 0;
                Parallel.For(0, Columns, j =>
                {
                    double a = this[0, j];
                    if (a != 0)
                    {
                        Matrix subMatrix = SubMatrix(0, j);
                        DoubleExtensions.InterlockedAdd(ref result, a * subMatrix.Determinant());
                    }
                });
                return result;
            }
        }

        /// <summary>
        /// Generate an augmented form of this matrix by adding the columns from the specified other matrix
        /// </summary>
        /// <param name="columns">The values to be placed in the new columns.  Should have the same number of
        /// rows as this matrix.</param>
        /// <returns></returns>
        public Matrix Augmented(Matrix columns)
        {
            Matrix result = CreateNewMatrix(Rows, Columns + columns.Columns);
            result.SetBlock(0, 0, this);
            result.SetBlock(0, Columns, columns);
            return result;
        }

        /// <summary>
        /// Create a duplicate of this matrix
        /// </summary>
        /// <returns></returns>
        public abstract Matrix Duplicate();

        /// <summary>
        /// Get the contents of this matrix expressed as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.Append(this[i, j].ToString());
                    if (j < Columns - 1) sb.Append('\t');
                }
                if (i < Rows - 1) sb.AppendLine();
            }
            return sb.ToString();
        }

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
