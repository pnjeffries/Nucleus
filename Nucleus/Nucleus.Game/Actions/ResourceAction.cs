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
    /// An abstract base action which requires a resource 
    /// </summary>
    [Serializable]
    public abstract class ResourceAction : GameAction
    {
        private Resource _ResourceRequired = null;

        /// <summary>
        /// The resource required to execute this action
        /// </summary>
        public Resource ResourceRequired
        {
            get { return _ResourceRequired; }
            set { _ResourceRequired = value; }
        }

        #region Constructor


        public ResourceAction()
        {
        }

        public ResourceAction(string name, Resource resourceRequired = null) : base(name)
        {
            _ResourceRequired = resourceRequired;
        }

        public ResourceAction(string name, IEffect effect, Resource resourceRequired = null) : base(name, effect)
        {
            _ResourceRequired = resourceRequired;
        }

        public ResourceAction(string name, params IEffect[] effects) : base(name, effects)
        {
        }

        public ResourceAction(string name, Resource resourceRequired, params IEffect[] effects) : base(name, effects)
        {
            _ResourceRequired = resourceRequired;
        }

        #endregion

        #region Methods

        public override bool Attempt(IActionLog log, EffectContext context)
        {
            if (!HasRequiredResources(context.Actor))
            {
                var key = "ResourceAction_Fail";
                if (log.HasScriptFor(key))
                {
                    log.WriteLine();
                    log.WriteScripted(key, context.Actor, _ResourceRequired);
                }
                return false;
            }
            return base.Attempt(log, context);
        }

        protected bool HasRequiredResources(Element actor)
        {
            if (_ResourceRequired != null)
            {
                var inventory = actor?.GetData<Inventory>();
                if (inventory == null) return false;
                if (!inventory.Resources.HasResourceQuantity(ResourceRequired)) return false;
            }
            return true;
        }

        #endregion
    }
}
