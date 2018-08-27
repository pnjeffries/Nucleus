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
    /// A data component which attaches to actor elements.
    /// Provides a collection of all available actions which
    /// are available to that actor
    /// </summary>
    [Serializable]
    public class AvailableActions : IElementDataComponent, IEndOfTurn
    {
        #region Properties

        private GameActionCollection _Actions = new GameActionCollection();

        /// <summary>
        /// The collection of actions available to this element
        /// </summary>
        public GameActionCollection Actions { get { return _Actions; } }

        #endregion

        #region Methods

        public void EndOfTurn(TurnContext context)
        {
            // Clear all available actions
            Actions.Clear();
        }

        /// <summary>
        /// Get the action assigned to the specified user input function
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GameAction ActionForInput(InputFunction input)
        {
            var trigger = new ActionInputTrigger(input);
            return Actions.FirstMatch(trigger);
        }

        /// <summary>
        /// Get the action assigned to the specified user input function
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GameAction ActionForInput(InputFunction input, int cellIndex)
        {
            var trigger = new ActionCellInputTrigger(cellIndex, input);
            return Actions.FirstMatch(trigger);
        }


        #endregion
    }
}
