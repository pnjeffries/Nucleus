using Nucleus.Game.Actions;
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
    /// Temporary ability to drop items
    /// </summary>
    [Serializable]
    public class SlotItemDropAbility : TemporaryAbility
    {
        public override void GenerateActions(TurnContext context, AvailableActions addTo)
        {
            Element actor = context.Element;

            Inventory inventory = actor?.GetData<Inventory>();
            if (inventory == null) return;

            context.Log?.WriteLine();
            context.Log?.WriteScripted("DropItemSlot_Hint");
            ExtractItemActions(inventory.Slots, addTo);

                var abortAction = new AbortAction();
            abortAction.Trigger = new ActionInputTrigger(InputFunction.Abort);
            addTo.Actions.Add(abortAction);
        }

        private void ExtractItemActions<TSlot>(IList<TSlot> slots, AvailableActions addTo)
            where TSlot : ItemSlot
        {
            foreach (ItemSlot slot in slots)
            {
                if (slot.Item != null && slot.HotKey != InputFunction.Undefined)
                {

                    var action = new DropItemAction(slot.Item);
                    action.Trigger = new ActionInputTrigger(slot.HotKey);
                    addTo.Actions.Add(action);
                }
            }
        }
    }
}
