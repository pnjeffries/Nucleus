using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An action is a process that may be performed by a game element
    /// which has one or more effects
    /// </summary>
    public class GameAction : Named
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Effects property
        /// </summary>
        private EffectCollection _Effects = new EffectCollection();

        /// <summary>
        /// The effects of this action
        /// </summary>
        public EffectCollection Effects
        {
            get { return _Effects; }
        }

        #endregion
    }
}
