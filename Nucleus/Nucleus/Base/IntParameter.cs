using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// A named input or output parameter which holds an integer value
    /// </summary>
    [Serializable]
    public class IntParameter : Parameter<int>
    {
        #region Constructors

        /// <summary>
        /// Creates a copy of another IntParameter
        /// </summary>
        /// <param name="other"></param>
        public IntParameter(IntParameter other) : base(other)
        {

        }

        /// <summary>
        /// Creates a new parameter with the specified name.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="units">The units in which the parameter is expressed</param>
        public IntParameter(string name, MeasurementUnit units = null) : base(name, units)
        {
        }

        /// <summary>
        /// Creates a new parameter with the specified name and
        /// initial value
        /// </summary>
        /// <param name="name">The name of this parameter</param>
        /// <param name="value">The value of this parameter</param>
        /// <param name="units">The units in which the parameter is expressed</param>
        public IntParameter(string name, int value, MeasurementUnit units = null) : base(name, value, units)
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
        public IntParameter(string name, ParameterGroup group, int value, MeasurementUnit units = null) : base(name, group, value, units)
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
        /// <param name="metadata">A dictionary containing any extra data related to this parameter.</param>
        /// <param name="units">The units in which the parameter is expressed</param>
        public IntParameter(string name, ParameterGroup group, int value, IDictionary<string, string> metadata, MeasurementUnit units = null) : base(name, group, value, metadata, units)
        {
        }

        /// <summary>
        /// Creates a new parameter with the default value and no name.
        /// As all parameters require an immutable name, this should not be
        /// used unless you know what you're doing.
        /// </summary>
        internal IntParameter()
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
        public IntParameter(string name, ParameterGroup group, int value, string description, MeasurementUnit units = null) : base(name, group, value, description, units)
        {
        }

        #endregion

        #region Methods

        protected override IFastDuplicatable FastDuplicate_Implementation()
        {
            return new IntParameter(this);
        }

        #endregion
    }
}
