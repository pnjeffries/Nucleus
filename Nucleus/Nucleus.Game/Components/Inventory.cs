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
    /// The inventory of an element
    /// </summary>
    [Serializable]
    public class Inventory : Unique, IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Slots property
        /// </summary>
        private ItemSlotCollection _Slots = new ItemSlotCollection();

        /// <summary>
        /// The items contained within this inventory
        /// </summary>
        public ItemSlotCollection Slots
        {
            get { return _Slots; }
        }

        /// <summary>
        /// Private backing member variable for the Slots property
        /// </summary>
        private EquipmentSlotCollection _Equipped = new EquipmentSlotCollection();

        /// <summary>
        /// The available equipment slots
        /// </summary>
        public EquipmentSlotCollection Equipped
        {
            get { return _Equipped; }
        }

        /// <summary>
        /// Get a list of all the unique items currently eqipped in
        /// an equipment slot.
        /// Generated as-needed.
        /// </summary>
        public ElementCollection EquippedItems
        {
            get
            {
                var result = new ElementCollection();
                foreach (var slot in _Equipped)
                {
                    if (slot.Item != null && !result.Contains(slot.Item.GUID))
                    {
                        result.Add(slot.Item);
                    }
                }
                return result;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Inventory() { }

        /// <summary>
        /// Slots constructor.
        /// Any EquipmentSlots will form part of the Equipped collection,
        /// otherwise it will be used as a standard inventory slot
        /// </summary>
        /// <param name="slots"></param>
        public Inventory(params ItemSlot[] slots)
        {
            foreach (var slot in slots)
            {
                if (slot is EquipmentSlot eSlot)
                {
                    Equipped.Add(eSlot);
                }
                else Slots.Add(slot);
            }
        }



        #endregion

        #region Methods

        /// <summary>
        /// Add an item to the inventory in the next available slot.
        /// Returns true if the attempt was successful.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(Element item)
        {
            ItemSlot slot = Slots.GetFirstAvailableFor(item);
            if (slot != null)
            {
                slot.Item = item;
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
            Equipped.RemoveAllItems();
            NotifyPropertyChanged(nameof(EquippedItems));
            return Slots.DropAll(dropper, context);
        }

        /// <summary>
        /// Drop a specifiec item held in this inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dropper"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool DropItem(Element item, Element dropper, EffectContext context)
        {
            if (Equipped.RemoveItem(item)) NotifyPropertyChanged(nameof(EquippedItems));
            return Slots.DropItem(item, dropper, context);
        }

        /// <summary>
        /// Remove an item from this inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(Element item)
        {
            Equipped.RemoveItem(item);
            NotifyPropertyChanged(nameof(EquippedItems));
            return Slots.RemoveItem(item);
        }

        /// <summary>
        /// Does this inventory contain the specified item?
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ContainsItem(Element item)
        {
            return Slots.ContainsItem(item);
        }

        /// <summary>
        /// Attempt to equip the specified item of equipment.
        /// Returns true if the attempt was successful.
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        public bool Equip(Element equipment)
        {
            ItemSlot slot = Equipped.GetFirstAvailableFor(equipment, true);
            if (slot != null)
            {
                slot.Item = equipment;
                NotifyPropertyChanged(nameof(EquippedItems));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Is the specified item currently equipped?
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        public bool IsEquipped(Element equipment)
        {
            return Equipped.ContainsItem(equipment);
        }

        #endregion
    }
}
