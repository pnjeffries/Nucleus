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

        public EffectContext(Element actor, Element target, GameState state, GameStage stage)
        {
            Actor = actor;
            Target = target;
            Stage = stage;
            State = state;
        }

        public EffectContext(Element actor, Element target, RLState state)
        {
            Actor = actor;
            Target = target;
            State = state;
            Stage = state.Stage;
        }

        public EffectContext(Element actor, Element target, RLState state, Vector direction) : this(actor, target, state)
        {
            Direction = direction;
        }

        public EffectContext(Element actor, GameState state, GameStage stage)
        {
            Actor = actor;
            Stage = stage;
            State = state;
        }

        public EffectContext(Element actor, RLState state)
        {
            Actor = actor;
            State = state;
            Stage = state.Stage;
        }

        public EffectContext(Element actor, RLState state, Vector direction) : this(actor, state)
        {
            Direction = direction;
        }

        #endregion

        #region Methods

        ///// <summary>
        ///// Remove the specified element from the state Elements collection.
        ///// Note that the removal will not take place 
        ///// </summary>
        ///// <param name="element"></param>
        //public void RemoveElementFromState(Element element)
        //{

        //}

        #endregion
    }
}
