using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Units;

namespace Nucleus.Base
{
    /// <summary>
    /// A named input or output parameter that holds a string value
    /// </summary>
    [Serializable]
    public class StringParameter : Parameter<string>
    {
        public StringParameter(string name, MeasurementUnit units = null) : base(name, units)
        {
        }

        public StringParameter(string name, string value, MeasurementUnit units = null) : base(name, value, units)
        {
        }

        public StringParameter(string name, ParameterGroup group, string value, MeasurementUnit units = null) : base(name, group, value, units)
        {
        }

        public StringParameter(string name, ParameterGroup group, string value, string description, MeasurementUnit units = null) : base(name, group, value, description, units)
        {
        }

        internal StringParameter()
        {
        }
    }
}
