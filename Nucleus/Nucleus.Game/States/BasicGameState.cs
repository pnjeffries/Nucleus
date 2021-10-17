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
    public abstract class BasicGameState<TStage> : ModalState
        where TStage : GameStage
    {
        #region Properties

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
        public virtual TStage Stage
        {
            get { return _Stage; }
            set
            {
                ChangeProperty(ref _Stage, value);
                NotifyPropertyChanged("Elements");
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Change the current stage.  This may be overridden to provide
        /// more specific logic on how stages should be changed.
        /// </summary>
        /// <param name="newStage"></param>
        public virtual void ChangeStage(TStage newStage)
        {
            Stage = newStage;
        }

        #endregion
    }
}
