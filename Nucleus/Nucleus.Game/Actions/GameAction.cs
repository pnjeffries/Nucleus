using Nucleus.Base;
using Nucleus.Logs;
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
    [Serializable]
    public class GameAction : Named
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Trigger property
        /// </summary>
        private ActionInputTrigger _Trigger;

        /// <summary>
        /// The input combination which will trigger this action.
        /// </summary>
        public ActionInputTrigger Trigger
        {
            get { return _Trigger; }
            set { _Trigger = value; }
        }

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

        #region Constructors

        public GameAction() { }

        public GameAction(string name) : base(name) { }

        public GameAction(string name, IEffect effect) : this(name)
        {
            Effects.Add(effect);
        }

        public GameAction(string name, params IEffect[] effects) : this(name)
        {
            foreach (var effect in effects)
                Effects.Add(effect);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempt the action.  Returns true if successful, false if not.
        /// Should be overridden to check skill levels etc.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool Attempt(IEffectLog log, EffectContext context)
        {
            return true;
        }

        /// <summary>
        /// Enact this action
        /// </summary>
        /// <param name="log"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool Enact(IEffectLog log, EffectContext context)
        {
            // TODO: Populate context
            if (Attempt(log, context))
            {
                // Apply effects:
                foreach (var effect in Effects)
                {
                    effect.Apply(log, context);
                }
                return true;
            }
            return false;
        }

        #endregion

    }
}
