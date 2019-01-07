using Nucleus.Base;
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
    /// An action is a process that may be performed by a game element
    /// which has one or more effects
    /// </summary>
    [Serializable]
    public class GameAction : Deletable, IDuplicatable, IUniqueWithModifiableGUID
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
        [CollectionCopy(CopyBehaviour.DUPLICATE, CopyBehaviour.DUPLICATE)]
        private EffectCollection _Effects = new EffectCollection();

        /// <summary>
        /// The effects of this action on the target element(s)
        /// </summary>
        public EffectCollection Effects
        {
            get { return _Effects; }
        }

        /// <summary>
        /// Private backing member variable for the Effects property
        /// </summary>
        private EffectCollection _SelfEffects = new EffectCollection();

        /// <summary>
        /// The effects of this action on the element performing the action
        /// </summary>
        public EffectCollection SelfEffects
        {
            get { return _SelfEffects; }
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
        public virtual bool Attempt(IActionLog log, EffectContext context)
        {
            return true;
        }

        /// <summary>
        /// Enact this action
        /// </summary>
        /// <param name="log"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool Enact(IActionLog log, EffectContext context)
        {
            // Populate context:
            context = PrePopulateContext(context);

            if (Attempt(log, context))
            {
                // Apply effects:
                ApplyEffects(log, context);
                ApplySelfEffects(log, context);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Populate the context data before attempting to execute the action
        /// </summary>
        /// <param name="context"></param>
        protected virtual EffectContext PrePopulateContext(EffectContext context)
        {
            return context;
        }

        /// <summary>
        /// Apply the effects of the action to the target
        /// </summary>
        /// <param name="log"></param>
        /// <param name="context"></param>
        protected virtual void ApplyEffects(IActionLog log, EffectContext context)
        {
            foreach (var effect in Effects)
            {
                effect.Apply(log, context);
            }
        }

        /// <summary>
        /// Apply the self-effects of the action, targetting the actor performing
        /// the action
        /// </summary>
        /// <param name="log"></param>
        /// <param name="context"></param>
        protected virtual void ApplySelfEffects(IActionLog log, EffectContext context)
        {
            context.Target = context.Actor;
            foreach (var effect in SelfEffects)
            {
                effect.Apply(log, context);
            }
        }

        /// <summary>
        /// Generate a score for this action based on the specified
        /// set of weightings
        /// </summary>
        /// <param name="weights"></param>
        /// <returns></returns>
        public virtual double AIScore(TurnContext context, ActionSelectionAI weights)
        {
            return 1.0;
        }

        /// <summary>
        /// Can this action target the specified element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual bool CanTarget(Element element)
        {
            return true;
        }

        #endregion

    }
}
