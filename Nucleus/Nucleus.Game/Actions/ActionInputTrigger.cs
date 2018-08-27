using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Base class representing the input conditions under which an action will be triggered by the player
    /// </summary>
    public class ActionInputTrigger
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

        #region Methods

        /// <summary>
        /// Does this input trigger match the given one?
        /// For the purposes of matching, top level input
        /// functions are taken to be equivalent to any of
        /// their sub-inputs
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Matches(ActionInputTrigger other)
        {
            if (other.Input.IsTopLevel())
                return _Input.ToTopLevel() == other.Input;
            else if (_Input.IsTopLevel())
                return _Input == other.Input.ToTopLevel();
            else return _Input == other.Input;
        }

        #endregion
    }
}
