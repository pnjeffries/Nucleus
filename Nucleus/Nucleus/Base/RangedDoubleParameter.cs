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
    /// A named input parameter which contains a double value and has
    /// a standard range of possible values.
    /// </summary>
    [Serializable]
    public class RangedDoubleParameter : DoubleParameter
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Max property
        /// </summary>
        private double _Max = double.NaN;

        /// <summary>
        /// The maximum value of the standard range for this parameter
        /// </summary>
        public double Max
        {
            get { return _Max; }
            set { ChangeProperty(ref _Max, value); }
        }

        /// <summary>
        /// Private backing member variable for the Min property
        /// </summary>
        private double _Min = double.NaN;

        public RangedDoubleParameter(string name, MeasurementUnit units = null) : base(name, units)
        {
        }

        public RangedDoubleParameter(string name, double value, MeasurementUnit units = null) : base(name, value, units)
        {
        }

        public RangedDoubleParameter(string name, ParameterGroup group, double value, MeasurementUnit units = null) : base(name, group, value, units)
        {
        }


        public RangedDoubleParameter(string name, ParameterGroup group, double value, double min, double max, MeasurementUnit units = null) : base(name, group, value, units)
        {
            _Min = min;
            _Max = max;
        }

        internal RangedDoubleParameter()
        {
        }

        public RangedDoubleParameter(string name, ParameterGroup group, double value, string description, MeasurementUnit units = null) : base(name, group, value, description, units)
        {
        }

        /// <summary>
        /// The minimum value of the standard range for this parameter
        /// </summary>
        public double Min
        {
            get { return _Min; }
            set { ChangeProperty(ref _Min, value); }
        }

        /// <summary>
        /// The range of this parameter
        /// </summary>
        public Interval Range
        {
            get { return new Interval(_Min, _Max); }
            set
            {
                Min = value.Min;
                Max = value.Max;
            }
        }

        #endregion
    }
}
