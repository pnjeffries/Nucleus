using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Artitecture
{
    /// <summary>
    /// A cell in a stage map blueprint
    /// </summary>
    [Serializable]
    public class BlueprintCell : IMapCellDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the GenerationType property
        /// </summary>
        private CellGenerationType _GenerationType = CellGenerationType.Untouched;

        /// <summary>
        /// The generation type of the cell
        /// </summary>
        public CellGenerationType GenerationType
        {
            get { return _GenerationType; }
            set { _GenerationType = value; }
        }

        /// <summary>
        /// Private backing member variable for the Room property
        /// </summary>
        private Room _Room = null;

        /// <summary>
        /// The room that the cell occupies
        /// </summary>
        public Room Room
        {
            get { return _Room; }
            set { _Room = value; }
        }

        #endregion
    }
}
