using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components
{
    /// <summary>
    /// An item which has a limited number of uses, after which
    /// it is destroyed.
    /// </summary>
    public class ConsumableItem : Unique, IElementDataComponent
    {
        private int _Uses = 1;

        /// <summary>
        /// The remaining number of uses of the consumable
        /// </summary>
        public int Uses
        {
            get { return _Uses; }
            set { ChangeProperty(ref _Uses, value); }
        }

        public ConsumableItem(int uses)
        {
            _Uses = uses;
        }
    }
}
