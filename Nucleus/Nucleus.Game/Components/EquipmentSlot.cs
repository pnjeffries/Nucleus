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
    /// A slot which can hold an item of equipment
    /// </summary>
    [Serializable]
    public class EquipmentSlot : Named
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
        public EquipmentSlot(string name, InputFunction hotKey, Element item = null) : base(name)
        {
            HotKey = hotKey;
            Item = item;
        }

        #endregion

        /// <summary>
        /// Can this slot hold the specified item?
        /// </summary>
        /// <returns></returns>
        public bool CanHold(Element item)
        {
            // TODO: Some equipment slots only hold certain types of item?
            if (item.HasData<EquippableItem>()) return true;
            else return false;
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
}
