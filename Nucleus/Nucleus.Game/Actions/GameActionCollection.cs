using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A collection of game actions
    /// </summary>
    [Serializable]
    public class GameActionCollection : UniquesCollection<GameAction>
    {
        #region Methods

        /// <summary>
        /// Find the first action in this collection with a matching
        /// input trigger to the one provided
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public GameAction FirstMatch(ActionInputTrigger trigger)
        {
            foreach (var gA in this)
            {
                if (gA.Trigger != null && gA.Trigger.Matches(trigger))
                    return gA;
            }
            return null;
        }

        #endregion
    }
}
