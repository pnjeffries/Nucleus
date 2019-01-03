using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components
{
    /// <summary>
    /// The inventory of an element
    /// </summary>
    [Serializable]
    public class Inventory : IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Contents property
        /// </summary>
        private ElementCollection _Contents = new ElementCollection();

        /// <summary>
        /// The items contained within this inventory
        /// </summary>
        public ElementCollection Contents
        {
            get { return _Contents; }
        }

        #endregion
    }
}
