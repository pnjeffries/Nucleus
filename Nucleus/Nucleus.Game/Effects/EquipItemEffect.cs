using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Effects
{
    /// <summary>
    /// Effect which will Equip an item
    /// </summary>
    [Serializable]
    public class EquipItemEffect : BasicEffect
    {
        /// <summary>
        /// The item to be equipped
        /// </summary>
        private Element _Item;

        /// <summary>
        /// The item to be equipped
        /// </summary>
        public Element Item
        {
            get { return _Item; }
            set { _Item = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public EquipItemEffect()
        {

        }

        /// <summary>
        /// Item constructor
        /// </summary>
        /// <param name="item"></param>
        public EquipItemEffect(Element item)
        {
            _Item = item;
        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            var inventory = context.Target?.GetData<Inventory>();
            if (inventory == null) return false;

            return inventory.Equip(Item);
        }
    }
}
