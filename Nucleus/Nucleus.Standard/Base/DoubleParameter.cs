using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Units;

namespace Nucleus.Base
{
    /// <summary>
    /// A named input or output parameter which holds a double value
    /// </summary>
    [Serializable]
    public class DoubleParameter : Parameter<double>
    {
        #region Properties

        /// <summary>
        /// Get or set the value of this parameter in
        /// SI units.  If the Units property of this
        /// parameter is set then the base value of this
        /// parameter will be converted using the SIFactor
        /// property of the unit to return a value in
        /// the equivalent SI units.
        /// </summary>
        public double SIValue
        {
            get
            {
                if (Units == null) return Value;
                else return Value * Units.SIFactor;
            }
            set
            {
                if (Units == null) Value = value;
                else Value = (value / Units.SIFactor);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new parameter with the specified name.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="units">The units in which the parameter is expressed</param>
        public DoubleParameter(string name, MeasurementUnit units = null) : base(name, units)
        {
        }

        /// <summary>
        /// Creates a new parameter with the specified name and
        /// initial value
        /// </summary>
        /// <param name="name">The name of this parameter</param>
        /// <param name="value">The value of this parameter</param>
        /// <param name="units">The units in which the parameter is expressed</param>
        public DoubleParameter(string name, double value, MeasurementUnit units = null) : base(name, value, units)
        {
        }

        /// <summary>
        /// Creates a new parameter with the specified name, group
        /// and initial value.
        /// </summary>
        /// <param name="name">The name of this parameter</param>
        /// <param name="group">The group within which this parameter
        /// should be displayed</param>
        /// <param name="value">The initial value of the parameter</param>
        /// <param name="units">The units in which the parameter is expressed</param>
        public DoubleParameter(string name, ParameterGroup group, double value, MeasurementUnit units = null) : base(name, group, value, units)
        {
        }

        /// <summary>
        /// Creates a new parameter with the default value and no name.
        /// As all parameters require an immutable name, this should not be
        /// used unless you know what you're doing.
        /// </summary>
        internal DoubleParameter()
        {
        }

        /// <summary>
        /// Creates a new parameter with the specified name, group,
        /// initial value and description.
        /// </summary>
        /// <param name="name">The name of this parameter.</param>
        /// <param name="group">The group within which this parameter
        /// should be displayed.</param>
        /// <param name="value">The initial value of the parameter.</param>
        /// <param name="description">The description of the parameter's function.</param>
        /// <param name="units">The units in which the parameter is expressed.</param>
        public DoubleParameter(string name, ParameterGroup group, double value, string description, MeasurementUnit units = null) : base(name, group, value, description, units)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the current value of this parameter, expressed in the
        /// specified units.
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        public double GetValueIn(MeasurementUnit units)
        {
            double toSI = 1;
            double fromSI = 1;
            if (Units != null) toSI = Units.SIFactor;
            if (units != null) fromSI = units.SIFactor;
            return Value * toSI / fromSI;
        }

        #endregion
    }
}
