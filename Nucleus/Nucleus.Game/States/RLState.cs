using Nucleus.Geometry;
using Nucleus.Logs;
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
        #region Properties

        /// <summary>
        /// Private backing member variable for the Active property
        /// </summary>
        private Element _Active = null;

        /// <summary>
        /// The element who is currently activated for a turn.
        /// </summary>
        public Element Active
        {
            get { return _Active; }
            set
            {
                _Active = value;
                NotifyPropertyChanged("Active");
            }
        }

        /// <summary>
        /// Private backing member variable for the Log property
        /// </summary>
        private IActionLog _Log = null;

        /// <summary>
        /// The logger which stores and displays messages to the user.  May be null, in which case no messages will be displayed.
        /// </summary>
        public IActionLog Log
        {
            get { return _Log; }
            set
            {
                _Log = value;
                NotifyPropertyChanged("Log");
            }
        }


        #endregion

        #region Constructor

        public RLState()
        {
           

        }

        #endregion

        #region Methods

        public override void StartUp()
        {
            base.StartUp();
            EndTurnOf(Controlled);
        }

        /// <summary>
        /// Called when the user releases a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        /// <param name="direction">The direction associated with the input,
        /// if applicable.</param>
        public override void InputRelease(InputFunction input, Vector direction)
        {
            base.InputRelease(input, direction);
            Element controlled = Controlled;
            if (controlled != null && controlled == Active)
            {
                var aA = controlled.GetData<AvailableActions>();

                if (direction.IsValidNonZero())
                {
                    //Find targetted cell:
                    MapData mD = controlled.GetData<MapData>();
                    if (mD != null && mD.MapCell != null)
                    {
                        MapCell newCell = Stage.Map.AdjacentCell(mD.MapCell.Index, direction);
                        var tAction = aA.ActionForInput(input, newCell.Index);
                        if (tAction != null)
                        {
                            tAction.Enact(Log, new EffectContext(controlled, this, direction));
                            EndTurnOf(controlled);
                            return;
                        }
                        /*if (newCell != null && (controlled.GetData<MapCellCollider>()?.CanEnter(newCell) ?? true))
                            newCell.PlaceInCell(controlled);
                        EndTurnOf(controlled);*/
                    }
                }

                // Haven't found a targeted action; fallback to:
                var action = aA.ActionForInput(input.ToTopLevel());
                if (action != null)
                {
                    action.Enact(Log, new EffectContext(controlled, this));
                    EndTurnOf(controlled);
                }

            }
        }

        /// <summary>
        /// Process the start of the turn
        /// </summary>
        /// <param name="element"></param>
        public void StartTurnOf(Element element)
        {
            var context = new TurnContext(this, Stage, element);
            Active = element;
            foreach (IElementDataComponent dC in element.Data)
            {
                if (dC is IStartOfTurn)
                {
                    ((IStartOfTurn)dC).StartOfTurn(context);
                }
            }
        }

        /// <summary>
        /// Process the end of the turn
        /// </summary>
        public void EndTurnOf(Element element)
        {
            var context = new TurnContext(this, Stage, element);
            foreach (IElementDataComponent dC in element.Data)
            {
                if (dC is IEndOfTurn)
                {
                    ((IEndOfTurn)dC).EndOfTurn(context);
                }
            }
            NextTurn();
        }

        /// <summary>
        /// Progress to the next turn
        /// </summary>
        public void NextTurn()
        {
            // Find element with next turn:
            Element next = null;
            int lowest = 0;
            foreach (Element element in Elements)
            {
                var tC = element.GetData<TurnCounter>();
                if (tC != null && next == null || lowest > tC.CountDown)
                {
                    next = element;
                    lowest = tC.CountDown;
                }
            }
            // Adjust turn counters for everything:
            foreach (Element element in Elements)
            {
                var tC = element.GetData<TurnCounter>();
                if (tC != null)
                {
                    tC.CountDown -= lowest;
                }
            }
            // TODO: Switch to more efficient system?

            if (next != null)
            {
                StartTurnOf(next);
            }
        }

        #endregion
    }
}
