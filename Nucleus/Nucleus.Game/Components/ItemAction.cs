using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{ 
    /// <summary>
    /// A data component for use on items which 
    /// </summary>
    [Serializable]
    public class ItemAction : IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Prototype property
        /// </summary>
        private GameAction _Prototype;

        /// <summary>
        /// The prototype of the item action.
        /// </summary>
        public GameAction Prototype
        {
            get { return _Prototype; }
            set { _Prototype = value; }
        }

        #endregion

        #region Constructor

        public ItemAction() { }

        public ItemAction(GameAction prototype)
        {
            Prototype = prototype;
        }

        #endregion
    }
}
