using Nucleus.Geometry;
using Nucleus.Model;
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
        /// Private backing member variable for the Elements property
        /// </summary>
        private ElementCollection _Elements = new ElementCollection();

        /// <summary>
        /// The collection of currently active game elements in this stage
        /// </summary>
        public ElementCollection Elements
        {
            get { return _Elements; }
            set
            {
                _Elements = value;
                NotifyPropertyChanged("Elements");
            }
        }

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
