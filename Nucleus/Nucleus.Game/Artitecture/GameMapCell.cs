using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A MapCell used for game map representation
    /// </summary>
    [Serializable]
    public class GameMapCell : MapCell
    {
        private Room _Room = null;

        /// <summary>
        /// The room which contains this cell (if any)
        /// </summary>
        public Room Room
        {
            get { return _Room; }
            set { ChangeProperty(ref _Room, value); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameMapCell() : base() { }
    }
}
