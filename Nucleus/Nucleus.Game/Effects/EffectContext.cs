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
    /// The context within which an effect is being applied
    /// </summary>
    public class EffectContext
    {
        #region Properties

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
        /// Direction for use in directional effects
        /// </summary>
        public Vector Direction { get; set; }

        /// <summary>
        /// Get the map (retrieved from current stage)
        /// </summary>
        public ICellMap<MapCell> Map { get { return ((MapStage)Stage).Map; } }

        #endregion

        #region Constructors

        public EffectContext() { }

        public EffectContext(Element target, GameState state, GameStage stage)
        {
            Target = target;
            Stage = stage;
            State = state;
        }

        public EffectContext(Element target, RLState state)
        {
            Target = target;
            State = state;
            Stage = state.Stage;
        }

        public EffectContext(Element target, RLState state, Vector direction) : this(target, state)
        {

            Direction = direction;
        }

        #endregion

    }
}
