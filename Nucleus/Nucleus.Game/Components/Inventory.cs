using Nucleus.Base;
using Nucleus.Game.Components;
using Nucleus.Geometry;
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
    public class Inventory : Unique, IElementDataComponent, ISubModifiers
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

        [NonSerialized]
        private ItemSlot _Selected = null;

        /// <summary>
        /// The currently selected item slot
        /// </summary>
        public ItemSlot Selected
        {
            get { return _Selected; }
            set { ChangeProperty(ref _Selected, value); }
        }

        private ResourceCollection _Resources = new ResourceCollection();

        /// <summary>
        /// The resources within this inventory.  These are stackable items such 
        /// as money and ammunition which do not take up an inventory slot.
        /// </summary>
        public ResourceCollection Resources
        {
            get { return _Resources; }
        }

        private ElementCollection _Keys = new ElementCollection();

        /// <summary>
        /// The collection of key items held within this inventory.
        /// </summary>
        public ElementCollection Keys
        {
            get { return _Keys; }
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
        public Inventory(params IInventoryContainer[] slots)
        {
            foreach (var slot in slots)
            {
                if (slot is EquipmentSlot eSlot)
                {
                    Equipped.Add(eSlot);
                }
                else if (slot is ItemSlot iSlot) Slots.Add(iSlot);
                else if (slot is Resource resource)
                {
                    Resources.AddResourceQuantity(resource);
                }
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
            // If a key:
            var key = item.GetData<Key>();
            if (key != null)
            {
                Keys.Add(item);
                return true;
                // Could keys also be other things?
            }
            // If a resource:
            var rPU = item.GetData<ResourcePickUp>();
            if (rPU != null)
            {
                Resources.AddResourceQuantity(rPU.Resource);
                return true;
            }
            // If not:
            ItemSlot slot = Slots.GetFirstAvailableFor(item);
            if (slot != null)
            {
                slot.Item = item;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Drop all items and resourrces in this inventory
        /// </summary>
        /// <param name="dropper"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool DropAll(Element dropper, EffectContext context)
        {
            Equipped.RemoveAllItems();
            NotifyPropertyChanged(nameof(EquippedItems));
            bool result = Slots.DropAll(dropper, context);
            Resources.DropAll(dropper, context);
            DropAllKeys(dropper, context);
            return result;
        }

        /// <summary>
        /// Drop a specified item held in this inventory
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
        /// Drop a quantity of a specified resource
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="dropper"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool DropResource(Resource resource, Element dropper, EffectContext context)
        {
            return Resources.DropResource(resource, dropper, context) != null;
        }

        /// <summary>
        /// Drop all keys held in this inventory
        /// </summary>
        /// <param name="dropper"></param>
        /// <param name="context"></param>
        public void DropAllKeys(Element dropper, EffectContext context)
        {
            for (int i = Keys.Count - 1; i >= 0; i--)
            {
                var key = Keys[i];
                DropKey(key, dropper, context);
            }
        }

        /// <summary>
        /// Drop the specified key item.
        /// </summary>
        /// <param name="dropper">The element dropping the item</param>
        /// <param name="context">The current context</param>
        /// <returns></returns>
        private bool DropKey(Element key, Element dropper, EffectContext context)
        {
            if (key != null)
            {
                if (key.HasData<PickUp>() && context.State is MapState) //TODO: Work for other states?
                {
                    MapData mD = dropper.GetData<MapData>();
                    if (mD.MapCell != null)
                    {
                        mD.MapCell.PlaceInCell(key);
                    }
                    context.State.Elements.Add(key);
                }
                Keys.Remove(key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove the specified amount of a resource from this inventory (if possible)
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool RemoveResource(Resource resource)
        {
            return Resources.ReduceResourceQuantity(resource.ResourceType, resource.Quantity);
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

        /// <summary>
        /// Select the next item slot after the currently selected one
        /// </summary>
        public void SelectNext()
        {
            if (Selected == null) Selected = Slots.FirstOrDefault();
            else if (Slots.Count > 0)
            {
                int i = Slots.IndexOf(Selected) + 1;
                if (i >= Slots.Count) i = 0;
                var slot = Slots[i];
                Selected = slot;
            }
        }

        /// <summary>
        /// Select the previous item slot before the currently selected one
        /// </summary>
        public void SelectPrevious()
        {
            if (Selected == null) Selected = Slots.LastOrDefault();
            else if (Slots.Count > 0)
            {
                int i = Slots.IndexOf(Selected) - 1;
                if (i < 0) i = Slots.Count - 1;
                var slot = Slots[i];
                Selected = slot;
            }
        }

        /// <summary>
        /// Get the key from this inventory with the specified keyCode,
        /// if one is currently held.  Else will return null.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public Element GetKey(string keyCode)
        {
            foreach (var item in Keys)
            {
                var key = item.GetData<Key>();
                if (key != null && key.KeyCode == keyCode) return item;
            }
            return null;
        }

        /// <summary>
        /// Apply value modifiers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="value"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public T ApplyModifiers<T, TInterface>(T value, Func<T, TInterface, T> function)
        {
            foreach (var item in EquippedItems)
            {
                value = item.Data.ApplyModifiers(value, function);
            }
            return value;
        }

        #endregion
    }
}
