using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model.Loading
{
    /// <summary>
    /// A load acting on an element due to a change in temperature
    /// </summary>
    [Serializable]
    public class ThermalLoad : Load<ElementSet, Element>
    {
        public override string GetValueUnits()
        {
            return "°C";
        }
    }
}
