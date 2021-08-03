using Nucleus.Game.Components;
using Nucleus.Geometry;
using Nucleus.Model;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// The context within which an effect is being applied
    /// </summary>
    public class EffectContext
    {
        #region Properties

        /// <summary>
        /// The element (if any) which is the root cause of this effect 
        /// </summary>
        public Element Actor { get; set; }

        /// <summary>
        /// The element that is the primary target of the effect
        /// </summary>
        public Element Target { get; set; }

        /// <summary>
        /// The current stage
        /// </summary>
        public GameStage Stage { get; set; }

        /// <summary>
        /// The current state
        /// </summary>
        public GameState State { get; set; }

        /// <summary>
        /// The object used to trigger special effects in the front-end
        /// </summary>
        public SFXSupervisor SFX { get { return GameEngine.Instance.SFX; } }

        /// <summary>
        /// Direction for use in directional effects
        /// </summary>
        public Vector Direction { get; set; }

        /// <summary>
        /// Get the map (retrieved from current stage)
        /// </summary>
        public ICellMap<MapCell> Map { get { return ((MapStage)Stage).Map; } }

        /// <summary>
        /// The random number generator to be used to resolve stochastic effects
        /// </summary>
        public Random RNG { get; set; }

        /*
        /// <summary>
        /// Private backing member variable for the ToBeRemoved property
        /// </summary>
        private ElementCollection _ToBeRemoved = null;

        /// <summary>
        /// The elements which are to be removed from the state elements collection
        /// </summary>
        public ElementCollection ToBeRemoved
        {
            get
            {
                if (_ToBeRemoved == null) _ToBeRemoved = new ElementCollection();
                return _ToBeRemoved;
            }
        }

        /// <summary>
        /// Are there any elements in the to be removed collection?
        /// </summary>
        public bool AnyElementsToRemove
        {
            get { return _ToBeRemoved != null; }
        }
        */

        #endregion

        #region Constructors

        public EffectContext() { }

        public EffectContext(Element actor, Element target, GameState state, GameStage stage, Random rng)
        {
            Actor = actor;
            Target = target;
            Stage = stage;
            State = state;
            RNG = rng;
        }

        public EffectContext(Element actor, Element target, RLState state)
        {
            Actor = actor;
            Target = target;
            State = state;
            Stage = state.Stage;
            RNG = state.RNG;
        }

        public EffectContext(Element actor, Element target, RLState state, Vector direction) : this(actor, target, state)
        {
            Direction = direction;
        }

        public EffectContext(Element actor, GameState state, GameStage stage, Random rng)
        {
            Actor = actor;
            Stage = stage;
            State = state;
            RNG = rng;
        }

        public EffectContext(Element actor, RLState state)
        {
            Actor = actor;
            State = state;
            Stage = state.Stage;
            RNG = state.RNG;
        }

        public EffectContext(Element actor, RLState state, Vector direction) : this(actor, state)
        {
            Direction = direction;
        }

        public EffectContext(EffectContext other)
        {
            Actor = other.Actor;
            Direction = other.Direction;
            RNG = other.RNG;
            Stage = other.Stage;
            State = other.State;
            Target = other.Target;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a clone of this EffectContext but with the target element replaced with another
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public EffectContext CloneWithTarget(Element target)
        {
            var result = new EffectContext(this);
            result.Target = target;
            return result;
        }

        public void ElementMovedOutOfTurn(Element element)
        {
            ElementMovedOutOfTurn(element, Stage as MapStage);
        }

        public void ElementMovedOutOfTurn(Element element, MapStage newStage) //TODO: pass in log?
        {
            var turnContext = new TurnContext(State, newStage, element, RNG, null);
            foreach (var component in element.Data)
            {
                if (component is IOutOfTurnMove outMove)
                {
                    outMove.OutOfTurnMove(turnContext);
                }
            }
        }


        /// <summary>
        /// Is the player aware of the status of the specified element?
        /// </summary>
        public bool IsPlayerAwareOf(Element element)
        {
            var bState = State as MapState;
            if (bState != null)
            {
                // Is the element the player themselves?
                if (element == bState.Controlled) return true;

                // Check if element is item in player inventory
                if (element.HasData<PickUp>())
                {
                    var inventory = bState.Controlled.GetData<Inventory>();
                    if (inventory != null && inventory.ContainsItem(element)) return true;
                }

                // Check if the player can see it
                var awareness = bState.Controlled.GetData<MapAwareness>();
                return awareness.AwareOf(element, true);
            }
            return false;
        }

        #endregion
    }
}
