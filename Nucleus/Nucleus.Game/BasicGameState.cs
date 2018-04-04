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
        private GameElementCollection _Elements = new GameElementCollection();

        /// <summary>
        /// The collection of currently active game elements
        /// </summary>
        public GameElementCollection Elements
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
        private GameElementCollection _Controlled = new GameElementCollection();

        /// <summary>
        /// The collection of game elements which are currently directly under the player's control
        /// </summary>
        public GameElementCollection Controlled
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
