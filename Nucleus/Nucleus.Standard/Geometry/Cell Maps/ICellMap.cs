using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A base interface for maps of cells
    /// </summary>
    public interface ICellMap : IDuplicatable
    {

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
        /// Get the cell object at the specified index
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        object GetCell(int cellIndex);

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

        /// <summary>
        /// Get a list of the cells in this map within a given range of a given position,
        /// ordered by distance
        /// </summary>
        /// <param name="position"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        IList<int> CellsOrderedByDistance(Vector position, double range);

        /// <summary>
        /// Spawn a new grid of the same type and size but with a different
        /// generic type
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        ICellMap<E> SpawnNewGrid<E>();
    }

    /// <summary>
    /// A generic interface for maps of cells
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICellMap<T> : ICellMap, IEnumerable<T>
    {
        /// <summary>
        /// Get or set the contents of the cell at the specified cell index
        /// </summary>
        /// <param name="cellIndex">The 1-dimensional cell index</param>
        /// <returns></returns>
        T this[int cellIndex] { get; set; }
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
        /// Get the items in the cells at the specified locations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        public static IList<T> CellsAt<T>(this ICellMap<T> map, IList<Vector> locations)
        {
            var result = new List<T>(locations.Count);
            foreach (Vector location in locations)
            {
                int i = map.IndexAt(location);
                if (map.Exists(i)) result.Add(map[i]);
            }
            return result;
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

        /// <summary>
        /// Set a cell in this map.  This will back-register the map and index
        /// with the cell itself.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="index"></param>
        /// <param name="cell"></param>
        public static void SetCell<T>(this ICellMap<T> map, int index, T cell)
            where T:IMapCell
        {
            map[index] = cell;
            cell.Index = index;
            cell.Map = map;
        }

        /// <summary>
        /// Set all cells in this map to the same value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="value"></param>
        public static void SetAll<T>(this ICellMap<T> map, T value)
        {
            for (int i = 0; i < map.CellCount; i++)
            {
                map[i] = value;
            }
        }

        /// <summary>
        /// Populate this map with cells
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        public static void InitialiseCells<T>(this ICellMap<T> map)
            where T:IMapCell, new() 
        {
            for (int i = 0; i < map.CellCount; i++)
            {
                T cell = new T();
                map.SetCell(i, cell);
            }
        }

      /// <summary>
      /// Generate a field of view within this cell map
      /// </summary>
      /// <param name="fromPoint"></param>
      /// <param name="maxRange"></param>
      /// <param name="transparencyCondition"></param>
      /// <param name="outGrid"></param>
      /// <param name="visibleValue">The value to set in the output grid to indicate visibility</param>
        public static void FieldOfView<E,T>(this ICellMap<T> grid, Vector fromPoint, double maxRange, Func<T, bool> transparencyCondition,
                ICellMap<E> outGrid, E visibleValue)
        {
            double tolerance = 0.00001;
            double maxRangeSqd = maxRange * maxRange;
            AngleIntervals shadows = new AngleIntervals();
            IList<int> cIs = grid.CellsOrderedByDistance(fromPoint, Math.Ceiling(maxRange));
            int current = grid.IndexAt(fromPoint);

            foreach (int index in cIs)
            {
                Vector centroid = grid.CellPosition(index);
                double distSqd = fromPoint.DistanceToSquared(centroid);
                if (distSqd > maxRangeSqd) return; //Shortcut, reached end of range (?)
                T item = grid[index];
                bool opaque = !transparencyCondition.Invoke(item);

                if ((opaque || !shadows.IsEmpty) && index != current)
                {

                    double angle = fromPoint.AngleTo(centroid).NormalizeTo2PI();
                    if (!shadows.isInsideRegion(angle, tolerance))
                    {
                        //Is visible:
                        if (!opaque)outGrid[index] = visibleValue;
                    }

                    if (opaque)
                    {
                        //Add to shadows:
                        Angle mindA = 0;
                        Angle maxdA = 0;
                        for (int i = 0; i < grid.VertexCount(index); i++)
                        {
                            Vector vertex = grid.CellVertex(index, i);
                            Angle dA = (fromPoint.AngleTo(vertex) - angle).Normalize();
                            if (dA < mindA) mindA = dA;
                            if (dA > maxdA) maxdA = dA;
                        }
                        mindA *= 0.9;
                        maxdA *= 0.9;
                        Angle start = (angle + mindA).NormalizeTo2PI();
                        Angle end = (angle + maxdA).NormalizeTo2PI();
                        shadows.addRegion(start, end);

                        //Shortcut: end if full:
                        if (shadows.IsFull) return;
                    }
                }
                else
                {
                    outGrid[index] = visibleValue;
                }

            }
        }

        /// <summary>
        /// Generate a Dijkstra Map to aid AI navigation through this map
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="startingValue"></param>
        /// <param name="targetCondition"></param>
        /// <param name="passCondition"></param>
        /// <param name="blockedValue"></param>
        /// <returns></returns>
        public static ICellMap<int> GenerateDijkstraMap<T>(this ICellMap<T> map, Func<T,bool> targetCondition, Func<T,bool> passCondition, 
            int startingValue = 1000000, int blockedValue = -1)
        {
            ICellMap<int> dMap = map.SpawnNewGrid<int>();
            dMap.SetAll(startingValue);
            for (int i = 0; i < map.CellCount; i++)
            {
                T cellValue = map[i];
                if (targetCondition(cellValue))
                {
                    dMap[i] = 0;
                }
                else if (!passCondition(cellValue))
                {
                    dMap[i] = blockedValue;
                }
            }

            while (IterateDijkstraMap(dMap, blockedValue))
            {
            }

            return dMap;
        }

        /// <summary>
        /// Run one iteration of a dijkstra map update.  Each cell is set to the lowest
        /// adjacent value - 1.
        /// </summary>
        /// <param name="dMap"></param>
        /// <param name="blockedValue"></param>
        /// <returns></returns>
        public static bool IterateDijkstraMap(this ICellMap<int> dMap, int blockedValue = -1)
        {
            bool changed = false;
            for (int i = 0; i < dMap.CellCount; i++)
            {
                int cellValue = dMap[i];
                if (cellValue != blockedValue)
                {
                    int lowestValue = cellValue;
                    for (int ai = 0; ai <= dMap.AdjacencyCount(i); ai++)
                    {
                        int adjacentCellIndex = dMap.AdjacentCellIndex(i, ai);
                        if (dMap.Exists(adjacentCellIndex))
                        {
                            int adjacentValue = dMap[adjacentCellIndex];
                            if (adjacentValue < lowestValue) lowestValue = adjacentValue;
                        }
                    }
                    if (lowestValue < cellValue - 1)
                    {
                        dMap[i] = lowestValue + 1;
                        changed = true;
                    }
                }
            }
            return changed;
        }
    }
}
