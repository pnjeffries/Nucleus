﻿using Nucleus.Model;
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
                    mD.MapCell.Contents.Remove(element);
                    mD.MapCell.NotifyPropertyChanged("Contents"); //TEMP
                }
                Contents.Add(element);
                mD.MapCell = this;
                NotifyPropertyChanged("Contents"); //TEMP
            }
        }

        #endregion
    }
}
