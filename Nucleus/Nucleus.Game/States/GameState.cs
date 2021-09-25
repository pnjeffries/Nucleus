using Nucleus.Base;
using Nucleus.Game.Debug;
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
    /// Abstract base class for states within a game (levels, menus etc.)
    /// </summary>
    [Serializable]
    public abstract class GameState : Unique
    {
        #region Properties

        /// <summary>
        /// The collection of currently active game elements
        /// </summary>
        public abstract ElementCollection Elements { get; }

        /// <summary>
        /// Private backing field for DebugModeOn
        /// </summary>
        private bool _DebugModeOn = false;

        /// <summary>
        /// Is debug mode currently active?  Debug mode enables extra developer functionality
        /// to aid with testing and debugging
        /// </summary>
        public bool DebugModeOn
        {
            get { return _DebugModeOn; }
            set { ChangeProperty(ref _DebugModeOn, value); }
        }

        /// <summary>
        /// Get the current library of debug commands
        /// </summary>
        protected virtual DebugCommandLibrary DebugCommands
        {
            get { return new DebugCommandLibrary(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform state initialisation
        /// </summary>
        public virtual void StartUp()
        {

        }

        /// <summary>
        /// Called every frame update
        /// </summary>
        public virtual void Update(UpdateInfo info)
        {

        }
        
        /// <summary>
        /// Called when the user presses a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        /// <param name="direction">The direction associated with the input,
        /// if applicable.</param>
        public virtual void InputPress(InputFunction input, Vector direction)
        {

        }

        /// <summary>
        /// Called when the user releases a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        /// <param name="direction">The direction associated with the input,
        /// if applicable.</param>
        public virtual void InputRelease(InputFunction input, Vector direction)
        {
            if (input == InputFunction.Debug) DebugModeOn = !DebugModeOn; //Toggle debug mode
        }

        /// <summary>
        /// Run a debug command
        /// </summary>
        /// <param name="command"></param>
        public virtual void RunDebugCommand(string command)
        {
            DebugCommands.RunCommand(command);
        }

        #endregion

    }
}
