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
    /// A collection of equipment slots
    /// </summary>
    [Serializable]
    public class EquipmentSlotCollection : UniquesCollection<EquipmentSlot>
    {
        #region Methods

        /// <summary>
        /// Get the first equipment slot that is not currently occupied
        /// </summary>
        /// <returns></returns>
        public EquipmentSlot GetFirstEmpty()
        {
            foreach (var slot in this)
                if (slot.Item == null) return slot;
            return null;
        }

        /// <summary>
        /// Get the first equipment slot that is not currently occupied
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public EquipmentSlot GetFirstAvailableFor(Element item)
        {
            foreach (var slot in this)
            {
                if (slot.Item == null && slot.CanHold(item)) return slot;
            }
            return null;
        }

        #endregion
    }
}
