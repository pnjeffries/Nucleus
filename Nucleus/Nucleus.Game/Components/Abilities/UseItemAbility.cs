using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Ability which enables the use of items
    /// </summary>
    [Serializable]
    public class UseItemAbility : Ability
    {
        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            Element actor = context.Element;

            Inventory inventory = actor?.GetData<Inventory>();
            if (inventory != null) ExtractItemActions(inventory.Slots, addTo);
        }

        private void ExtractItemActions<TSlot>(IList<TSlot> slots, AvailableActions addTo)
            where TSlot : ItemSlot
        {
            foreach (ItemSlot slot in slots)
            {
                if (slot.Item != null && slot.HotKey != InputFunction.Undefined)
                {
                    var iA = slot.Item.GetData<ItemActions>();
                    if (iA?.Prototype != null)
                    {
                        var action = iA.Prototype.Duplicate();
                        action.Trigger = new ActionInputTrigger(slot.HotKey);
                        addTo.Actions.Add(action);
                    }
                }
            }
        }
    }
}
