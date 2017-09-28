using Nucleus.Conversion;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.GSA
{
    public class GSAConversionOptions : ModelConversionOptions, IAutoUIHostable
    {
        /// <summary>
        /// If true, all text data will be output
        /// </summary>
        public bool PrintAll { get; set; } = true;
    }
}
