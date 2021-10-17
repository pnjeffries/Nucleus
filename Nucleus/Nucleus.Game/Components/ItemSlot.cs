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
    /// A slot which can hold an inventory item
    /// </summary>
    [Serializable]
    public class ItemSlot : Named, IInventoryContainer
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the HotKey property
        /// </summary>
        private InputFunction _HotKey;

        /// <summary>
        /// The button bound to this equipment slot
        /// </summary>
        public InputFunction HotKey
        {
            get { return _HotKey; }
            set { _HotKey = value; }
        }

        /// <summary>
        /// Private backing member variable for the Item property
        /// </summary>
        private Element _Item = null;

        /// <summary>
        /// The item of equipment contained within this slot
        /// </summary>
        public Element Item
        {
            get { return _Item; }
            set { ChangeProperty(ref _Item, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new equipment slot with the specified name, hotkey and starting item
        /// </summary>
        /// <param name="hotKey"></param>
        /// <param name="item"></param>
        public ItemSlot(string name, InputFunction hotKey = InputFunction.Undefined, Element item = null) : base(name)
        {
            HotKey = hotKey;
            Item = item;
        }

        #endregion

        /// <summary>
        /// Can this slot hold the specified item?
        /// </summary>
        /// <returns></returns>
        public virtual bool CanHold(Element item)
        {
            return true;
        }

        /// <summary>
        /// Drop the item held in this slot.
        /// This will remove the item from the slot.  If the
        /// item is a PickUp it will be added back to the map.
        /// </summary>
        /// <param name="dropper">The element dropping the item</param>
        /// <param name="context">The current context</param>
        /// <returns></returns>
        public bool DropItem(Element dropper, EffectContext context)
        {
            if (Item != null)
            {
                if (Item.HasData<PickUp>() && context.State is MapState) //TODO: Work for other states?
                {
                    MapData mD = dropper.GetData<MapData>();
                    if (mD.MapCell != null)
                    {
                        mD.MapCell.PlaceInCell(Item);
                    }
                    context.State.Elements.Add(Item);  
                }
                Item = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get a text description of this slot
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ": " + Item?.Name;
        }

    }

    /// <summary>
    /// Extension methods for item slots and collections thereof
    /// </summary>
    public static class ItemSlotExtensions
    {
        /// <summary>
        /// Get the first equipment slot that is not currently occupied
        /// </summary>
        /// <returns></returns>
        public static TSlot GetFirstEmpty<TSlot>(this IList<TSlot> slots)
            where TSlot : ItemSlot
        {
            foreach (var slot in slots)
                if (slot.Item == null) return slot;
            return null;
        }

        /// <summary>
        /// Get the first equipment slot that is not currently occupied
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="item">he item</param>
        /// <param name="replaceCurrent">If false, the slot will not be considered available if an item is
        /// already held there.</param>
        /// <returns></returns>
        public static TSlot GetFirstAvailableFor<TSlot>(this IList<TSlot> slots, Element item, bool replaceCurrent = false)
            where TSlot : ItemSlot
        {
            foreach (var slot in slots)
            {
                if ((replaceCurrent || slot.Item == null) && slot.CanHold(item)) return slot;
            }
            return null;
        }

        /// <summary>
        /// Drop all items in this collection
        /// </summary>
        /// <param name="dropper"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool DropAll<TSlot>(this IList<TSlot> slots, Element dropper, EffectContext context)
            where TSlot : ItemSlot
        {
            bool result = false;
            foreach (var slot in slots)
            {
                if (slot.DropItem(dropper, context)) result = true;
            }
            return result;
        }

        /// <summary>
        /// Drop a specific item
        /// </summary>
        /// <typeparam name="TSlot"></typeparam>
        /// <param name="slots"></param>
        /// <param name="item"></param>
        /// <param name="dropper"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool DropItem<TSlot>(this IList<TSlot> slots, Element item, Element dropper, EffectContext context)
            where TSlot: ItemSlot
        {
            bool dropped = false;
            foreach (var slot in slots)
            {
                if (slot.Item == item)
                {
                    if (!dropped && slot.DropItem(dropper, context)) dropped = true;
                    else slot.Item = null;
                }
            }
            return dropped;
        }

        /// <summary>
        /// Remove the specified item from any slot which contains it in this collection
        /// </summary>
        /// <typeparam name="TSlot"></typeparam>
        /// <param name="slots"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool RemoveItem<TSlot>(this IList<TSlot> slots, Element item)
            where TSlot : ItemSlot
        {
            bool result = false;
            foreach (var slot in slots)
            {
                if (slot.Item == item)
                {
                    slot.Item = null;
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Clear items from all slots in this collection
        /// </summary>
        public static void RemoveAllItems<TSlot>(this IList<TSlot> slots)
            where TSlot:ItemSlot
        {
            foreach (var slot in slots) slot.Item = null;
        }

        /// <summary>
        /// Do any of the slots in this collection contain the specified item?
        /// </summary>
        /// <typeparam name="TSlot"></typeparam>
        /// <param name="slots"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool ContainsItem<TSlot>(this IList<TSlot> slots, Element item)
            where TSlot: ItemSlot
        {
            foreach (var slot in slots)
            {
                if (slot.Item == item) return true;
            }
            return false;
        }
    }
}
