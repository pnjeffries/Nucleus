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
        private GameAction DuplicatePrototype(GameAction prototype)
        {
            if (prototype is IFastDuplicatable fastDup) return (GameAction)fastDup.FastDuplicate();
            else return prototype.Duplicate();
        }

        protected override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            Element actor = context.Element;

            Inventory inventory = actor?.GetData<Inventory>();
            if (inventory != null) ExtractItemActions(inventory.Slots, addTo);

            // Use selected item
            if (inventory.Selected?.Item != null)
            {
                var iA = inventory.Selected.Item.GetData<ItemActions>();
                if (iA?.Prototype != null)
                {
                    var action = DuplicatePrototype(iA.Prototype);
                    action.Trigger = new ActionInputTrigger(InputFunction.UseSelected);
                    addTo.Actions.Add(action);
                }
            }
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
                        var action = DuplicatePrototype(iA);
                        if (action != null)
                        {
                            action.Trigger = new ActionInputTrigger(slot.HotKey);
                            addTo.Actions.Add(action);
                        }
                    }
                }
            }
        }

        private GameAction DuplicatePrototype(ItemActions iA)
        {
            if (iA?.Prototype == null) return null;
            if (iA.Prototype is IFastDuplicatable fastDup) return fastDup.FastDuplicate() as GameAction;
            return iA.Prototype.Duplicate();
        }
    }
}
