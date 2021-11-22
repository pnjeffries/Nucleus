using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A cell in an ICellMap which acts as a container for other objects
    /// </summary>
    [Serializable]
    public class MapCell : DataOwner<MapCellDataStore, IMapCellDataComponent, MapCell>, IMapCell
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Contents property
        /// </summary>
        private ElementCollection _Contents = new ElementCollection();

        /// <summary>
        /// The contents of this map cell
        /// </summary>
        public ElementCollection Contents
        {
            get { return _Contents; }
            set { _Contents = value; }
        }

        private int _Index;

        /// <summary>
        /// The index of the cell
        /// </summary>
        public int Index
        {
            get
            {
                return _Index;
            }
            set { _Index = value; }
        }

        private ICellMap _Map;

        /// <summary>
        /// The map to which the cell belongs
        /// </summary>
        public ICellMap Map
        {
            get
            {
                return _Map;
            }
            set { _Map = value; }
        }

        /// <summary>
        /// Get the position of this map cell
        /// </summary>
        public Vector Position
        {
            get { return Map?.CellPosition(_Index) ?? Vector.Unset; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MapCell()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="map"></param>
        public MapCell(int index, ICellMap map)
        {
            _Index = index;
            _Map = map;
        }

        #endregion

        #region Methods

        protected override MapCellDataStore NewDataStore()
        {
            return new MapCellDataStore();
        }

        /// <summary>
        /// Move the specified element to this cell.
        /// The element will be removed from any cell it occupies.
        /// TODO: Modify this for multi-cell entities?
        /// </summary>
        /// <param name="element"></param>
        public void PlaceInCell(Element element)
        {
            if (!Contents.Contains(element))
            {
                MapData mD = element.GetData<MapData>(true);
                if (mD.MapCell != null && mD.MapCell.Contents.Contains(element))
                {
                    mD.MapCell.RemoveFromCell(element);
                }
                Contents.Add(element);
                mD.MapCell = this;
                NotifyPropertyChanged("Contents"); //TEMP
            }
        }

        public void RemoveFromCell(Element element)
        {
            Contents.Remove(element);
            NotifyPropertyChanged("Contents");
        }

        #endregion
    }

    /// <summary>
    /// Extension methods for the MapCell class
    /// </summary>
    public static class MapCellExtensions
    {
        /// <summary>
        /// Gets the adjacent cell (of the same type) in the specified direction
        /// </summary>
        /// <typeparam name="TMapCell"></typeparam>
        /// <param name="cell"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static TMapCell AdjacentCellInDirection<TMapCell>(this TMapCell cell, Vector direction)
            where TMapCell : MapCell
        {
            return cell?.Map.GetCell(cell.Map.AdjacentCellIndex(cell.Index, direction)) as TMapCell;
        }

        /// <summary>
        /// Get all cells adjacent to this one.
        /// </summary>
        /// <typeparam name="TMapCell"></typeparam>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static IList<TMapCell> AdjacentCells<TMapCell>(this TMapCell cell)
            where TMapCell : MapCell
        {
            if (cell == null) return null;
            else return cell.Map.AdjacentCells<TMapCell>(cell.Index);
        }

        /// <summary>
        /// Travels through the map in the specified direction until a cell meeting the specified condition
        /// is encountered (or the limits are reached)
        /// </summary>
        /// <typeparam name="TMapCell"></typeparam>
        /// <param name="cell"></param>
        /// <param name="direction"></param>
        /// <param name="condition"></param>
        /// <param name="maxRange">The maximum number of cells which may be visited before terminating</param>
        /// <param name="returnLast">If the condition is not met before the max range or the end of the map is found, return the last cell visited</param>
        /// <returns></returns>
        public static TMapCell FirstCellInDirectionWhere<TMapCell>(this TMapCell cell, Vector direction, Func<TMapCell, bool> condition, int maxRange = int.MaxValue, bool returnLast = false)
            where TMapCell : MapCell
        {
            return cell?.Map.GetCell(cell.Map.FirstCellInDirectionWhere(cell.Index, direction, condition, maxRange, returnLast)) as TMapCell;
        }

        /// <summary>
        /// Does this cell contain any elements with attached data of the specified type?
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TMapCell"></typeparam>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static bool HasContentsWithData<TData, TMapCell>(this TMapCell cell)
            where TMapCell : MapCell
            where TData: class, IElementDataComponent
        {
            foreach (var element in cell.Contents)
                if (element.HasData<TData>()) return true;

            return false;
        }
    }
}
