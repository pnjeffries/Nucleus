using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A map composed of interlocked hexagonal cells
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class HexCellMap<T> : RegularCellMap<T>
    {
        #region Constants

        /// <summary>
        /// Cached value of √3
        /// </summary>
        private static readonly double _ROOT3 = Math.Sqrt(3);

        #endregion

        #region Fields

        /// <summary>
        /// The backing array of cells
        /// </summary>
        private T[] _Cells;

        #endregion

        #region Properties


        private HexGridLayout _Layout = HexGridLayout.EvenColumns;

        /// <summary>
        /// Get the orientation of this hex-grid
        /// </summary>
        public HexGridLayout Layout
        {
            get { return _Layout; }
        }

        /// <summary>
        /// Get the number of cells in this map
        /// </summary>
        public override int CellCount
        {
            get { return _Cells.Length; }
        }

        /// <summary>
        /// Private backing field for Origin property
        /// </summary>
        private Vector _Origin = Vector.Zero;

        /// <summary>
        /// The origin of the map
        /// </summary>
        public Vector Origin { get { return _Origin; } }

        /// <summary>
        /// Private backing field for SizeX property
        /// </summary>
        private int _SizeX;

        /// <summary>
        /// The number of cells in the X-direction
        /// </summary>
        public int SizeX
        {
            get { return _SizeX; }
        }

        /// <summary>
        /// Private backing field for SizeY property
        /// </summary>
        private int _SizeY;

        /// <summary>
        /// The number of cells in the Y-direction
        /// </summary>
        public int SizeY
        {
            get { return _SizeY; }
        }

        /// <summary>
        /// Private backing field for CellRadius property
        /// </summary>
        private double _CellRadius = 0.5;

        /// <summary>
        /// The distance from the centre of each cell to each one of its vertices
        /// </summary>
        public double CellRadius
        {
            get { return _CellRadius;  }
        }

        /// <summary>
        /// Get the overall horizontal distance between cell centres
        /// </summary>
        public double CellSpacingX
        {
            get
            {
                if (Layout.HasAlignedColumns())
                {
                    return 2 * CellRadius;
                }
                else return _ROOT3 * CellRadius;
            }
        }

        /// <summary>
        /// Get the overall vertical distance between cell centres
        /// </summary>
        public double CellSpacingY
        {
            get
            {
                if (Layout.HasAlignedColumns())
                {
                    return _ROOT3 * CellRadius;
                }
                else return 2 * CellRadius;
            }
        }

        /// <summary>
        /// Get or set the contents of the cell at the specified cell index
        /// </summary>
        /// <param name="cellIndex">The 1-dimensional cell index</param>
        /// <returns></returns>
        public override T this[int cellIndex]
        {
            get { return _Cells[cellIndex]; }

            set { _Cells[cellIndex] = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Does a cell exist at the specified index?
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public override bool Exists(int cellIndex)
        {
            return (cellIndex >= 0 && cellIndex < _Cells.Length);
        }

        /// <summary>
        /// Get the index of the cell at the specified location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override int IndexAt(Vector location)
        {
            return IndexAt(location.X, location.Y);
        }

        /// <summary>
        /// Get the index of the cell at the specified x and y coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int IndexAt(double x, double y)
        {
            throw new NotImplementedException();
            // TODO!
            // return IndexAt((int)((x - _Origin.X) / CellSize), (int)((y - _Origin.Y) / CellSize));
        }

        /// <summary>
        /// Get the index of the cell at the specified i and j indices
        /// (aligned to the x and y axes respectively)
        /// </summary>
        /// <param name="columnIndex">The column index</param>
        /// <param name="rowIndex">The row index</param>
        /// <returns></returns>
        public int IndexAt(int columnIndex, int rowIndex)
        {
            if (columnIndex >= 0 && rowIndex >= 0 && columnIndex < SizeX && rowIndex < SizeY)
                return columnIndex + rowIndex * SizeX;
            else return -1;
        }



        /// <summary>
        /// Get the column index of the cell with the specified cell index
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public int ColumnIndex(int cellIndex)
        {
            return cellIndex % SizeX;
        }

        /// <summary>
        /// Get the row index of the cell with the specified cell index
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public int RowIndex(int cellIndex)
        {
            return cellIndex / SizeX;
        }

        /// <summary>
        /// Get the maximum number of possible adjacent cells for the specified
        /// cellIndex.  Note that this is only the maximum possible number and
        /// does not guarantee that all adjacencyIndices up to this number will
        /// return a cell that exists - you should check for this.
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public override int AdjacencyCount(int cellIndex)
        {
            if (Exists(cellIndex)) return 6;
            else return 0;
        }

        public override int AdjacentCellIndex(int cellIndex, Vector direction)
        {
            throw new NotImplementedException();
        }

        public override int AdjacentCellIndex(int cellIndex, int adjacencyIndex)
        {
            throw new NotImplementedException();
        }

        public override Vector CellPosition(int cellIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the number of vertices posessed by the cell at the specifed index
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public override int VertexCount(int cellIndex)
        {
            if (Exists(cellIndex)) return 6;
            else return 0;
        }

        /// <summary>
        /// Get the position of the specifed vertex of the specified cell
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <param name="vertexIndex"></param>
        /// <returns></returns>
        public override Vector CellVertex(int cellIndex, int vertexIndex)
        {
            Vector cP = CellPosition(cellIndex);
            double degAng = 60 * vertexIndex;

            if (Layout.HasAlignedRows()) degAng -= 30;

            Angle angle = Angle.FromDegrees(degAng);
            double size = CellRadius;
            return cP + new Vector(angle) * size;
        }


        #endregion
    }
}
