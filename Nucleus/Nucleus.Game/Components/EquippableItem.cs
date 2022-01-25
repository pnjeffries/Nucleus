using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An element data component which can be used to tag items as
    /// being equippable
    /// </summary>
    [Serializable]
    public class EquippableItem : IElementDataComponent
    {
        private string _SlotKey = null;

        /// <summary>
        /// The key which describes the suitable equipment slots for this item.
        /// </summary>
        public string SlotKey
        {
            get { return _SlotKey; }
            set { _SlotKey = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EquippableItem()
        {

        }

        /// <summary>
        /// Slot key constructor
        /// </summary>
        /// <param name="slotKey"></param>
        public EquippableItem(string slotKey)
        {
            _SlotKey = slotKey;
        }
    }
}
