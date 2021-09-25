using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{ 
    /// <summary>
    /// A data component for use on items which may be used via an action
    /// </summary>
    [Serializable]
    public class ItemActions : IElementDataComponent
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

        /// <summary>
        /// Creates a blank itemaction
        /// </summary>
        public ItemActions()
        {
        }

        /// <summary>
        /// Creates an ItemAction using the default 
        /// WindUpAction as a prototype followed by actions
        /// generated using the specified action factory
        /// </summary>
        public ItemActions(ActionFactory actionFactory)
        {
            Prototype = new WindUpAction(actionFactory);
        }

        public ItemActions(GameAction prototype)
        {
            Prototype = prototype;
        }

        #endregion
    }
}
