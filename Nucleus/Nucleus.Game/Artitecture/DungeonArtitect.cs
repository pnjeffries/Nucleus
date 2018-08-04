using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Generator class for dungeons
    /// </summary>
    public class DungeonArtitect
    {
        #region Fields

        /// <summary>
        /// Random number generator
        /// </summary>
        private Random _RNG = new Random();

        #endregion

        #region Properties

        /// <summary>
        /// The map to operate on
        /// </summary>
        public SquareCellMap<MapCell> Map { get; set; }

        #endregion

        #region Constructors

        public DungeonArtitect() { }

        #endregion

        #region Methods

        public bool RecursiveGrowth(int iDoor, int jDoor, RoomTemplate template, CompassDirection direction, bool createDoor, double floorLevel, Room parent)
        {
            bool result = false;



            return result;
        }

        #endregion
    }
}
