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
        private int _LifeSpan = 0;

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
            LifeSpan -= 1;
            if (LifeSpan < 0)
            {
                EndStatus(context);
                Delete();
            }
        }

        /// <summary>
        /// End the status effect, performing any necessary clearup
        /// </summary>
        /// <param name="context"></param>
        public virtual void EndStatus(TurnContext context)
        {

        }

        #endregion
    }
}
