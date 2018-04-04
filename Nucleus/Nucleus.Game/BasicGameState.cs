using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A basic game state that implements the fundamental mechanics
    /// of a game in a fairly general way
    /// </summary>
    public class BasicGameState : GameState
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Elements property
        /// </summary>
        private ElementCollection _Elements = new ElementCollection();

        /// <summary>
        /// The collection of currently active game elements
        /// </summary>
        public ElementCollection Elements
        {
            get { return _Elements; }
            set
            {
                _Elements = value;
                NotifyPropertyChanged("Elements");
            }
        }

        /// <summary>
        /// Private backing member variable for the Controlled property
        /// </summary>
        private ElementCollection _Controlled = new ElementCollection();

        /// <summary>
        /// The collection of game elements which are currently directly under the player's control
        /// </summary>
        public ElementCollection Controlled
        {
            get { return _Controlled; }
            set
            {
                _Controlled = value;
                NotifyPropertyChanged("Controlled");
            }
        }

        #endregion
    }
}
