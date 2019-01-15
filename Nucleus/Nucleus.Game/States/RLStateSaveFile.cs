using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.States
{
    /// <summary>
    /// Document which saves an RLState
    /// </summary>
    [Serializable]
    public class RLStateSaveFile : GameSaveFile
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the State property
        /// </summary>
        private RLState _State;

        /// <summary>
        /// The state which is stored in this file
        /// </summary>
        public RLState State
        {
            get { return _State; }
            set { _State = value; }
        }


        #endregion
    }
}
