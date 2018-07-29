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
        }

        #endregion

        #region Constructor

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

        #endregion
    }
}
