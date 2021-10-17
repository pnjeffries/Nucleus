using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Effect to select the previous item slot in the target's inventory
    /// </summary>
    [Serializable]
    public class SelectPreviousItemEffect : BasicEffect
    {
        public override bool Apply(IActionLog log, EffectContext context)
        {
            var inventory = context.Target?.GetData<Inventory>();
            if (inventory == null) return false;

            inventory.SelectPrevious();
            return true;
        }
    }
}