using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A generic interface for maps of cells
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICellMap<T>
    {
        /// <summary>
        /// Get or set the contents of the cell at the specified cell index
        /// </summary>
        /// <param name="cellIndex">The 1-dimensional cell index</param>
        /// <returns></returns>
        T this[int cellIndex] { get; set; }

        /// <summary>
        /// Get the number of cells in this map
        /// </summary>
        int CellCount { get; }

        /// <summary>
        /// Does a cell exist at the specified index?
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        bool Exists(int cellIndex);

        /// <summary>
        /// Get the index of the cell at the specified location
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        int IndexAt(Vector location);

        /// <summary>
        /// Get the maximum number of possible adjacent cells for the specified
        /// cellIndex.  Note that this is only the maximum possible number and
        /// does not guarantee that all adjacencyIndices up to this number will
        /// return a cell that exists - you should check for this.
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        int AdjacencyCount(int cellIndex);

        /// <summary>
        /// Get the cell index of the specified adjacent cell to the specified cell
        /// </summary>
        /// <param name="cellIndex">The index of the starting cell</param>
        /// <param name="adjacencyIndex">The adjacency index of the cell to retrieve</param>
        /// <returns></returns>
        int AdjacentCellIndex(int cellIndex, int adjacencyIndex);

        /// <summary>
        /// Get the cell index of the cell adjacent to the cell with the specified
        /// index in the specified direction.
        /// </summary>
        /// <param name="cellIndex">The index of the starting cell</param>
        /// <param name="direction">The direction of the cell to retrieve</param>
        /// <returns></returns>
        int AdjacentCellIndex(int cellIndex, Vector direction);

        /// <summary>
        /// Get the position of the specified cell
        /// </summary>
        /// <param name="cellIndex">The index of the cell to determine the position for</param>
        /// <returns></returns>
        Vector CellPosition(int cellIndex);

        /// <summary>
        /// Get the position of the specifed vertex of the specified cell
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <param name="vertexIndex"></param>
        /// <returns></returns>
        Vector CellVertex(int cellIndex, int vertexIndex);

        /// <summary>
        /// Get the number of vertices posessed by the cell at the specifed index
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        int VertexCount(int cellIndex);
    }

    /// <summary>
    /// Extension methods for the ICellMap interface
    /// </summary>
    public static class ICellMapExtensions
    {
        /// <summary>
        /// Get the item in the cell at the specified location
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="location">A point vector which lies within the cell to be retrieved.</param>
        /// <returns></returns>
        public static T CellAt<T>(this ICellMap<T> map, Vector location)
        {
            int i = map.IndexAt(location);
            if (map.Exists(i)) return map[i];
            else return default(T);
        }

        /// <summary>
        /// Get the specified adjacent cell to the specified cell
        /// </summary>
        /// <param name="cellIndex">The index of the starting cell</param>
        /// <param name="adjacencyIndex">The adjacency index of the cell to retrieve</param>
        /// <returns></returns>
        public static T AdjacentCell<T>(this ICellMap<T> map, int cellIndex, int adjacencyIndex)
        {
            if (map.Exists(cellIndex))
            {
                int i = map.AdjacentCellIndex(cellIndex, adjacencyIndex);
                if (map.Exists(i)) return map[i];
            }
            return default(T);
        }

        /// <summary>
        /// Get the cell adjacent to the cell with the specified
        /// index in the specified direction.
        /// </summary>
        /// <param name="cellIndex">The index of the starting cell</param>
        /// <param name="direction">The direction of the cell to retrieve</param>
        /// <returns></returns>
        public static T AdjacentCell<T>(this ICellMap<T> map, int cellIndex, Vector direction)
        {
            if (map.Exists(cellIndex))
            {
                int i = map.AdjacentCellIndex(cellIndex, direction);
                if (map.Exists(i)) return map[i];
            }
            return default(T);
        }

        /// <summary>
        /// Retrieve a list of all cells adjacent to the one with the specified index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public static IList<T> AdjacentCells<T>(this ICellMap<T> map, int cellIndex)
        {
            int aC = map.AdjacencyCount(cellIndex);
            var result = new List<T>(aC);
            for (int i = 0; i < aC; i++)
            {
                int iA = map.AdjacentCellIndex(cellIndex, i);
                if (map.Exists(iA)) result.Add(map[iA]);
            }
            return result;
        }

        /// <summary>
        /// Get the polyline representing the border of the cell with the specified index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public static PolyLine CellBorder<T>(this ICellMap<T> map, int cellIndex)
        {
            int vC = map.VertexCount(cellIndex);
            if (vC > 0)
            {
                var result = new PolyLine();
                for (int i = 0; i < vC; i++)
                {
                    result.Add(map.CellVertex(cellIndex, i));
                }
                result.Close();
                return result;
            }
            return null;
        }

    }
}
