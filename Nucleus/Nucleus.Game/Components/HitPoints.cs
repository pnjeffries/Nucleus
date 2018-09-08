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
    /// A data component to hold the hit points value of destructable elements
    /// </summary>
    [Serializable]
    public class HitPoints : Unique, IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing field for Value property
        /// </summary>
        private double _Value = 1;

        /// <summary>
        /// The current value of the hitpoints of the element
        /// </summary>
        public double Value
        {
            get { return _Value; }
            set
            {
                if (value > _Maximum) value = _Maximum; //Limit max
                ChangeProperty(ref _Value, value, "Value");
            }
        }

        /// <summary>
        /// Private backing field for Maximum property
        /// </summary>
        private double _Maximum = 1;

        /// <summary>
        /// The maximum value of the hitpoints of the element
        /// </summary>
        public double Maximum
        {
            get { return _Maximum; }
            set { ChangeProperty(ref _Maximum, value, "Maximum"); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public HitPoints() { }

        /// <summary>
        /// Constructor setting the value and maximum of the hitpoints value
        /// </summary>
        /// <param name="value"></param>
        public HitPoints(double value)
        {
            _Value = value;
            _Maximum = value;
        }

        #endregion
    }
}
