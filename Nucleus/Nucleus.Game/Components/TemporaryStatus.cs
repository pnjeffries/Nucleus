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
    /// A base class for status ailments or buffs which are temporary in
    /// nature and will expire after a number of turns.
    /// </summary>
    [Serializable]
    public abstract class TemporaryStatus : Deletable,
        IElementDataComponent, IEndOfTurn
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the LifeSpan property
        /// </summary>
        private int _LifeSpan = 1;

        /// <summary>
        /// The number of turns remaining until this status effect should end.
        /// </summary>
        public int LifeSpan
        {
            get { return _LifeSpan; }
            set { ChangeProperty(ref _LifeSpan, value); }
        }

        #endregion

        #region Constructors


        #endregion

        #region Methods

        public void EndOfTurn(TurnContext context)
        {
            ReduceLifespan();
        }

        public void ReduceLifespan(int amount = 1)
        {
            LifeSpan -= amount;
            if (LifeSpan <= 0)
            {
                Delete();
            }
        }

        #endregion
    }
}
