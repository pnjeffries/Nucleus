using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A component which tags an object as a gate between different stages
    /// </summary>
    [Serializable]
    public class StageExit : Unique, IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the TravelTo property
        /// </summary>
        private MapStage _TravelTo = null;

        /// <summary>
        /// The stage which this exit allows travel to
        /// </summary>
        public MapStage TravelTo
        {
            get { return _TravelTo; }
            set { _TravelTo = value; }
        }

        /// <summary>
        /// Private backing member variable for the CellIndex property
        /// </summary>
        private int _CellIndex = -1;

        /// <summary>
        /// The index of the cell to move to in the next stage
        /// </summary>
        public int CellIndex
        {
            get { return _CellIndex; }
            set { _CellIndex = value; }
        }


        #endregion

        #region Constructors

        public StageExit(MapStage travelTo, int cellIndex)
        {
            TravelTo = travelTo;
            CellIndex = cellIndex;
        }

        #endregion

    }
}
