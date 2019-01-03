using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An action which targets particular element or place
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    [Serializable]
    public abstract class TargetedAction<TTarget> : GameAction
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Target property
        /// </summary>
        private TTarget _Target;
       
        /// <summary>
        /// The target of the action
        /// </summary>
        public TTarget Target
        {
            get { return _Target; }
            set { _Target = value; }
        }

        #endregion

        #region Constructors

        public TargetedAction()
        {
        }

        public TargetedAction(string name) : base(name)
        {
        }

        public TargetedAction(string name, IEffect effect) : base(name, effect)
        {
        }

        public TargetedAction(string name, params IEffect[] effects) : base(name, effects)
        {
        }

        public TargetedAction(string name, TTarget target, params IEffect[] effects) : base(name, effects)
        {
            Target = target;
        }

        #endregion
    }
}
