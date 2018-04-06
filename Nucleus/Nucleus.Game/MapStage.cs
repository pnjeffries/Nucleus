using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A game stage which represents level geometry via a map
    /// </summary>
    [Serializable]
    public class MapStage : GameStage
    {
        /// <summary>
        /// Private backing member variable for the Map property
        /// </summary>
        private ICellMap<MapCell> _Map;

        /// <summary>
        /// The spatial map of the stage
        /// </summary>
        public ICellMap<MapCell> Map
        {
            get { return _Map; }
            set
            {
                _Map = value;
                NotifyPropertyChanged("Map");
            }
        }

    }
}
