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
    [Serializable]
    public class BasicGameState<TStage> : GameState
        where TStage : GameStage
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
        private Element _Controlled = null;

        /// <summary>
        /// The game element which is currently directly under the player's control
        /// </summary>
        public Element Controlled
        {
            get { return _Controlled; }
            set
            {
                _Controlled = value;
                NotifyPropertyChanged("Controlled");
            }
        }

        /// <summary>
        /// Private backing member variable for the Stage property
        /// </summary>
        private TStage _Stage = null;

        /// <summary>
        /// The current stage
        /// </summary>
        public TStage Stage
        {
            get { return _Stage; }
            set
            {
                _Stage = value;
                NotifyPropertyChanged("Stage");
            }
        }


        #endregion
    }
}
