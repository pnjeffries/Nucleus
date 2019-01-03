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
    /// Data component which denotes resistance to knockback
    /// </summary>
    [Serializable]
    public class Inertia : NotifyPropertyChangedBase, IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Fixed property
        /// </summary>
        private bool _Fixed = false;

        /// <summary>
        /// Gets or sets whether this component is fixed and immovable
        /// </summary>
        public bool Fixed
        {
            get { return _Fixed; }
            set { _Fixed = value; }
        }

        //TODO: Add knockback resistance (+/- or %)?

        #endregion

        #region Constructors

        public Inertia(bool isFixed)
        {
            _Fixed = isFixed;
        }

        #endregion
    }
}
