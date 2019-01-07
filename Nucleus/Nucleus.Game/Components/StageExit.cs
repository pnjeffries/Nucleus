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

        #endregion

    }
}
