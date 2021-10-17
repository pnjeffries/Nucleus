using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// AI component used to select actions to be performed by NPCs
    /// </summary>
    [Serializable]
    public class ActionSelectionAI : IElementDataComponent
    {
        /// <summary>
        /// Select an action from the specified collection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        public GameAction SelectAction(TurnContext context, GameActionCollection actions)
        {
            double bestScore = 0;
            GameAction bestAction = new WaitAction();
            foreach (var action in actions)
            {
                double score = action.AIScore(context, this) + context.RNG.NextDouble()*0.1; //TODO
                if (score > bestScore)
                {
                    bestScore = score;
                    bestAction = action;
                }
            }
            return bestAction;
        }
    }
}
