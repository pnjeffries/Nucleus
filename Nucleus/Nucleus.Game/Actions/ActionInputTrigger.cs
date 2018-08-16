using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// The input conditions under which an action will be triggered by the player
    /// </summary>
    public class ActionInputTrigger
    {
        /// <summary>
        /// Private backing member variable for the TargetCell property
        /// </summary>
        private int _TargetCell = -1;

        /// <summary>
        /// The index of the cell that is targeted for this action
        /// </summary>
        public int TargetCell
        {
            get { return _TargetCell; }
            set { _TargetCell = value; }
        }

        /// <summary>
        /// Private backing member variable for the Input property
        /// </summary>
        private InputFunction _Input = InputFunction.Undefined;

        /// <summary>
        /// The input function which will trigger this action.
        /// </summary>
        public InputFunction Input
        {
            get { return _Input; }
            set { _Input = value; }
        }
    }
}
