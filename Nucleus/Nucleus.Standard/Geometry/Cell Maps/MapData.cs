using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// CellMap data storage for elements 
    /// </summary>
    [Serializable]
    public class MapData : Unique, IElementDataComponent
    {
        private MapCell _MapCell = null;

        /// <summary>
        /// The MapCell the element is currently inside
        /// </summary>
        public MapCell MapCell
        {
            get { return _MapCell; }
            set
            {
                ChangeProperty(ref _MapCell, value, "MapCell");
                NotifyPropertyChanged("Position");
            }
        }

        /// <summary>
        /// Get the position of the element as defined by the
        /// map cell.
        /// </summary>
        public Vector Position
        {
            get
            {
                return MapCell.Position;
            }
        }
    }
}
