using Nucleus.Extensions;
using Nucleus.Maths;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A data structure representing a map of regular square cells arranged in a orthogonal grid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SquareCellMap<T> : ICellMap<T>
    {
        #region Fields

        /// <summary>
        /// The backing array of cells
        /// </summary>
        private T[] _Cells;

        #endregion

        #region Properties

        /// <summary>
        /// Get the number of cells in this map
        /// </summary>
        public int CellCount
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
        /// Private backing field for CellSize property
        /// </summary>
        private double _CellSize = 1.0;

        /// <summary>
        /// The edge dimension of each cell
        /// </summary>
        public double CellSize
        {
            get { return _CellSize; }
        }

        /// <summary>
        /// Get or set the contents of the cell at the specified cell index
        /// </summary>
        /// <param name="cellIndex">The 1-dimensional cell index</param>
        /// <returns></returns>
        public T this[int cellIndex]
        {
            get
            {
                if (cellIndex >= 0 && cellIndex < _Cells.Length)
                    return _Cells[cellIndex];
                else return default(T);
            }
            set {_Cells[cellIndex] = value; }
        }

        /// <summary>
        /// Get or set the contents of the cell at the specified column
        /// and row index.
        /// </summary>
        /// <param name="columnIndex">The column (x) index</param>
        /// <param name="rowIndex">The row (y) index</param>
        /// <returns></returns>
        public T this[int columnIndex, int rowIndex]
        {
            get { return this[IndexAt((int)columnIndex, (int)rowIndex)]; }
            set { this[IndexAt((int)columnIndex, (int)rowIndex)] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new Square cell map with the specified dimensions
        /// </summary>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="cellSize"></param>
        public SquareCellMap(int sizeX, int sizeY, double cellSize = 1.0)
        {
            _SizeX = sizeX;
            _SizeY = sizeY;
            _CellSize = cellSize;
            _Cells = new T[sizeX * sizeY];
        }

        /// <summary>
        /// Initialise a new square cell map at the given origin with the specified dimensions
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="cellSize"></param>
        public SquareCellMap(Vector origin, int sizeX, int sizeY, double cellSize = 1.0) : this(sizeX, sizeY, cellSize)
        {
            _Origin = origin;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the cell object at the specified index
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        object ICellMap.GetCell(int cellIndex)
        {
            return this.GetCell(cellIndex);
        }

        /// <summary>
        /// Get the cell object at the specified index
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public T GetCell(int cellIndex)
        {
            return this[cellIndex];
        }

        /// <summary>
        /// Get the cell object at the specified column and row indices
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public T GetCell(int columnIndex, int rowIndex)
        {
            return GetCell(IndexAt(columnIndex, rowIndex));
        }

        /// <summary>
        /// Does a cell exist at the specified index?
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public bool Exists(int cellIndex)
        {
            return (cellIndex >= 0 && cellIndex < _Cells.Length);
        }

        /// <summary>
        /// Does a cell exist at the specified indices
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public bool Exists(int i, int j)
        {
            return Exists(IndexAt(i, j));
        }

        /// <summary>
        /// Get the index of the cell at the specified location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public int IndexAt(Vector location)
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
            return IndexAt(ColumnIndexAt(x), ColumnIndexAt(y));
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
        /// Get the row and column indices at the specified location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        public void IndicesAt(Vector location, out int columnIndex, out int rowIndex)
        {
            IndicesAt(location.X, location.Y, out columnIndex, out rowIndex);
        }

        /// <summary>
        /// Get the row and column indices at the specified x and y coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        public void IndicesAt(double x, double y, out int columnIndex, out int rowIndex)
        {
            columnIndex = ColumnIndexAt(x);
            rowIndex = RowIndexAt(y);
        }

        /// <summary>
        /// Get the row and column indices covered by the specified bounding box
        /// </summary>
        /// <param name="box"></param>
        /// <param name="columnRange"></param>
        /// <param name="rowRange"></param>
        public void IndicesWithin(BoundingBox box, out IntInterval columnRange, out IntInterval rowRange)
        {
            columnRange = new IntInterval(ColumnIndexAt(box.MinX), ColumnIndexAt(box.MaxX));
            rowRange = new IntInterval(RowIndexAt(box.MinY), RowIndexAt(box.MaxY));
        }

        /// <summary>
        /// Get the column index at the specified x coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int ColumnIndexAt(double x)
        {
            return (int)((x - _Origin.X) / CellSize);
        }

        /// <summary>
        /// Get the row index at the specified y-coordinate
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int RowIndexAt(double y)
        {
            return (int)((y - _Origin.Y) / CellSize);
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
        public int AdjacencyCount(int cellIndex)
        {
            if (Exists(cellIndex)) return 4;
            else return 0;
        }

        /// <summary>
        /// Get the cell index of the specified adjacent cell to the specified cell
        /// </summary>
        /// <param name="cellIndex">The index of the starting cell</param>
        /// <param name="adjacencyIndex">The adjacency index of the cell to retrieve</param>
        /// <returns></returns>
        public int AdjacentCellIndex(int cellIndex, int adjacencyIndex)
        {
            int i = ColumnIndex(cellIndex);
            int j = RowIndex(cellIndex);

            switch (adjacencyIndex)
            {
                case 0:
                    return IndexAt(i + 1, j);
                case 1:
                    return IndexAt(i, j - 1);
                case 2:
                    return IndexAt(i - 1, j);
                case 3:
                    return IndexAt(i, j + 1);
            }

            return -1;
        }

        /// <summary>
        /// Get the cell index of the cell adjacent to the cell with the specified
        /// index in the specified direction.
        /// </summary>
        /// <param name="cellIndex">The index of the starting cell</param>
        /// <param name="direction">The direction of the cell to retrieve</param>
        /// <returns></returns>
        public int AdjacentCellIndex(int cellIndex, Vector direction)
        {
            int i = ColumnIndex(cellIndex);
            int j = RowIndex(cellIndex);

            if (direction.X.Abs() > direction.Y.Abs()) //X-dominant
                return IndexAt(i + direction.X.Sign(), j);
            else //Y-dominant
                return IndexAt(i, j + direction.Y.Sign());
        }

        /// <summary>
        /// Get the position of the specified cell
        /// </summary>
        /// <param name="cellIndex">The index of the cell to determine the position for</param>
        /// <returns></returns>
        public Vector CellPosition(int cellIndex)
        {
            return new Vector( ColumnIndex(cellIndex) + 0.5 * CellSize + Origin.X, RowIndex(cellIndex) + 0.5 * CellSize + Origin.Y);
        }

        /// <summary>
        /// Get the number of vertices posessed by the cell at the specifed index
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public int VertexCount(int cellIndex)
        {
            if (Exists(cellIndex)) return 4;
            else return 0;
        }

        /// <summary>
        /// Get the position of the specifed vertex of the specified cell
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <param name="vertexIndex"></param>
        /// <returns></returns>
        public Vector CellVertex(int cellIndex, int vertexIndex)
        {
            Vector cP = CellPosition(cellIndex);
            switch(vertexIndex)
            {
                case 0:
                    return cP + new Vector(CellSize / 2, CellSize / 2);
                case 1:
                    return cP + new Vector(-CellSize / 2, CellSize / 2);
                case 2:
                    return cP + new Vector(-CellSize / 2, -CellSize / 2);
                case 3:
                    return cP + new Vector(CellSize / 2, -CellSize / 2);
            }
            return Vector.Unset;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_Cells).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Cells.GetEnumerator();
        }

        /// <summary>
        /// Get a list of the cells in this map within a given range of a given position,
        /// ordered by distance
        /// </summary>
        /// <param name="position"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public IList<int> CellsOrderedByDistance(Vector position, double range)
        {
            IList<int> result = new List<int>(SizeX * SizeY);
            int cellIndex = IndexAt(position);
            int startX = ColumnIndex(cellIndex);
            int startY = RowIndex(cellIndex);
            int maxRingNo = Math.Max(Math.Max(startX, SizeX - startX), Math.Max(startY, SizeY - startY));
            if (range > 0) maxRingNo = (int)Math.Min(maxRingNo, range);
            if (Exists(cellIndex)) result.Add(cellIndex);
            for (int ringNo = 1; ringNo <= maxRingNo; ringNo++)
            {
                int offset = 0;
                while (offset != -ringNo)
                {
                    int offX = offset;
                    int offY = ringNo;
                    for (int n = 0; n < 4; n++)
                    {
                        int index = IndexAt(startX + offX, startY + offY);
                        if (Exists(index)) result.Add(index);

                        int oldOffX = offX;
                        offX = offY;
                        offY = -oldOffX;
                    }
                    offset *= -1;
                    if (offset >= 0) offset += 1;
                }
            }

            return result;
        }

        public ICellMap<E> SpawnNewGrid<E>()
        {
            return new SquareCellMap<E>(Origin, SizeX, SizeY, CellSize);
        }

        #endregion
    }
}
