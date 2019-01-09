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
    /// A data component to store the currently equipped items on a character
    /// </summary>
    [Serializable]
    public class Equipped : Unique, IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Slots property
        /// </summary>
        private EquipmentSlotCollection _Slots = new EquipmentSlotCollection();

        /// <summary>
        /// The available equipment slots
        /// </summary>
        public EquipmentSlotCollection Slots
        {
            get { return _Slots; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Equipped() { }

        public Equipped(params EquipmentSlot[] slots)
        {
            foreach (var slot in slots)
                Slots.Add(slot);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempt to equip the specified item of equipment.
        /// Returns true if the attempt was successful.
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        public bool Equip(Element equipment)
        {
            EquipmentSlot slot = Slots.GetFirstAvailableFor(equipment);
            if (slot != null)
            {
                slot.Item = equipment;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Drop all items in this
        /// </summary>
        /// <param name="dropper"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool DropAll(Element dropper, EffectContext context)
        {
            bool result = false;
            foreach (var slot in Slots)
            {
                if (slot.DropItem(dropper, context)) result = true;
            }
            return result;
        }

        #endregion


    }
}
