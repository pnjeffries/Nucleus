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
            Equipped equipped = actor?.GetData<Equipped>();
            if (equipped != null)
            {
                foreach (EquipmentSlot slot in equipped.Slots)
                {
                    if (slot.Item != null)
                    {
                        var iA = slot.Item.GetData<ItemAction>();
                        if (iA?.Prototype != null)
                        {
                            var action = iA.Prototype.Duplicate();
                            addTo.Actions.Add(new WindUpAction(slot.HotKey));
                        }
                    }
                }
            }
            //TODO: Base on items
            //addTo.Actions.Add(new WindUpAction(InputFunction.Ability_1));
        }
    }
}
