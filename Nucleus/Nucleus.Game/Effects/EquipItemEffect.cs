using Nucleus.Base;
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
    public class EquipItemEffect : BasicEffect, IFastDuplicatable
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

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="effect"></param>
        public EquipItemEffect(EquipItemEffect effect) : this(effect.Item)
        {

        }

        public override bool Apply(IActionLog log, EffectContext context)
        {
            var inventory = context.Target?.GetData<Inventory>();
            if (inventory == null) return false;

            return inventory.Equip(Item);
        }

        public IFastDuplicatable FastDuplicate_Internal()
        {
            return new EquipItemEffect(this);
        }
    }
}
