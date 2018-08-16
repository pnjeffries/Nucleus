using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Abstract base class representing the input conditions under which an action will be triggered by the player
    /// </summary>
    public abstract class ActionInputTrigger
    {
        #region Properties

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

        #endregion

        #region Constructor

        public ActionInputTrigger() { }

        public ActionInputTrigger(InputFunction input)
        {
            _Input = input;
        }

        #endregion
    }
}
