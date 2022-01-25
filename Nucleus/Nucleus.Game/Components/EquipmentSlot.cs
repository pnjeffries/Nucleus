using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An item slot to which items may be equipped to put them into use
    /// </summary>
    public class EquipmentSlot : ItemSlot
    {
        public EquipmentSlot(string name, InputFunction hotKey = InputFunction.Undefined, Element item = null) : base(name, hotKey, item)
        {
        }

        public EquipmentSlot(string name, Element item) : base(name, InputFunction.Undefined, item)
        {
        }

        /// <summary>
        /// Can this slot hold the specified item?
        /// </summary>
        /// <returns></returns>
        public override bool CanHold(Element item)
        {
            // TODO: Some equipment slots only hold certain types of item?
            var equippable = item.GetData<EquippableItem>();
            if (equippable != null && equippable.SlotKey == null || equippable.SlotKey == Name) return true;
            else return false;
        }
    }
}
