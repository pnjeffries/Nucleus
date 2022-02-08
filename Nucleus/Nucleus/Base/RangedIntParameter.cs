using Nucleus.Maths;
using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// A named input parameter which contains an int value and has
    /// a standard range of possible values.
    /// </summary>
    [Serializable]
    public class RangedIntParameter : IntParameter
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Max property
        /// </summary>
        private int _Max;

        /// <summary>
        /// The maximum value of the standard range for this parameter
        /// </summary>
        public int Max
        {
            get { return _Max; }
            set { ChangeProperty(ref _Max, value); }
        }

        /// <summary>
        /// Private backing member variable for the Min property
        /// </summary>
        private int _Min;


        /// <summary>
        /// The minimum value of the standard range for this parameter
        /// </summary>
        public int Min
        {
            get { return _Min; }
            set { ChangeProperty(ref _Min, value); }
        }

        /// <summary>
        /// The range of this parameter
        /// </summary>
        public IntInterval Range
        {
            get { return new IntInterval(_Min, _Max); }
            set
            {
                Min = value.Min;
                Max = value.Max;
            }
        }

        #endregion

        #region Constructors

        public RangedIntParameter(RangedIntParameter other) : base(other)
        {
            _Min = other.Min;
            _Max = other.Max;
        }

        public RangedIntParameter(string name, MeasurementUnit units = null) : base(name, units)
        {
        }

        public RangedIntParameter(string name, int value, MeasurementUnit units = null) : base(name, value, units)
        {
        }

        public RangedIntParameter(string name, ParameterGroup group, int value, MeasurementUnit units = null) : base(name, group, value, units)
        {
        }


        public RangedIntParameter(string name, ParameterGroup group, int value, int min, int max, MeasurementUnit units = null) : base(name, group, value, units)
        {
            _Min = min;
            _Max = max;
        }

        internal RangedIntParameter()
        {
        }

        public RangedIntParameter(string name, ParameterGroup group, int value, string description, MeasurementUnit units = null) : base(name, group, value, description, units)
        {
        }

        #endregion

        #region Methods

        protected override IFastDuplicatable FastDuplicate_Implementation()
        {
            return new RangedIntParameter(this);
        }

        #endregion
    }
}
