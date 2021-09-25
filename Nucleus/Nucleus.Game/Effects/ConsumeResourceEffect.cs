using Nucleus.Base;
using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// Effect which consumes a resource
    /// </summary>
    [Serializable]
    public class ConsumeResourceEffect : BasicEffect, IFastDuplicatable
    {
        private Resource _Resource;

        public Resource Resource
        {
            get { return _Resource; }
            set { _Resource = value; }
        }

        public ConsumeResourceEffect(Resource resource)
        {
            _Resource = resource;
        }

        public ConsumeResourceEffect(ConsumeResourceEffect other) : this(other.Resource) { }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            var target = context.Target;
            var inventory = target?.GetData<Inventory>();
            if (inventory == null) return false;
            if (Resource == null) return false;
            return inventory.RemoveResource(Resource);
        }

        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return new ConsumeResourceEffect(this);
        }
    }
}
