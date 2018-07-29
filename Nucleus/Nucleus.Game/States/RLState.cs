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
    /// A game state for Roguelikes
    /// </summary>
    [Serializable]
    public class RLState : BasicGameState<MapStage>
    {
        #region Constructor

        public RLState()
        {
           

        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the user releases a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        /// <param name="direction">The direction associated with the input,
        /// if applicable.</param>
        public override void InputRelease(InputFunction input, Vector direction)
        {
            base.InputRelease(input, direction);
            Element controlled = Controlled.FirstOrDefault();
            if (controlled != null)
            {

            }
        }

        #endregion
    }
}
