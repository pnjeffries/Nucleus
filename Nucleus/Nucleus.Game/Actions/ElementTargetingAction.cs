using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An action which targets an element
    /// </summary>
    [Serializable]
    public class ElementTargetingAction : TargetedAction<Element>
    {
        #region Constructor

        public ElementTargetingAction()
        {
        }

        public ElementTargetingAction(string name) : base(name)
        {
        }

        public ElementTargetingAction(string name, IEffect effect) : base(name, effect)
        {
        }

        public ElementTargetingAction(string name, params IEffect[] effects) : base(name, effects)
        {
        }

        public ElementTargetingAction(string name, Element target, params IEffect[] effects) : base(name, target, effects)
        {
        }

        #endregion

        #region Method

        protected override EffectContext PrePopulateContext(EffectContext context)
        {
            // Overwrite the default target with the one defined by this action:
            context.Target = Target;
            return base.PrePopulateContext(context);
        }

        #endregion
    }
}
