using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model.Loading
{
    /// <summary>
    /// A Gravity load which is applied to elements and generates a force
    /// based on an acceleration and the individual mass of those elements
    /// </summary>
    public class GravityLoad : ForceLoad<ElementSet, Element>
    {
        /// <summary>
        /// Initialise a new gravity load with the default properties
        /// </summary>
        public GravityLoad()
        {
            this.Value = -9.8;
        }

        public override string GetValueUnits()
        {
            return "m/s/s";
        }
    }
}
