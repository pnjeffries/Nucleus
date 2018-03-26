using Nucleus.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Model.SectionProfiles;
using Nucleus.Model;

namespace Nucleus.GSA
{
    public class GSAConversionContext : ConversionContext
    {
        /// <summary>
        /// The map of Nucleus-GSA IDs
        /// </summary>
        public GSAIDMappingTable IDMap { get; set; }

        /// <summary>
        /// The set of mapping options
        /// </summary>
        public GSAConversionOptions Options { get; set; }

        /// <summary>
        /// The section library
        /// </summary>
        public SectionProfileLibrary SectionLibrary { get; set; }

        public GSAConversionContext(GSAIDMappingTable idMap, GSAConversionOptions options, SectionProfileLibrary sectLib = null)
        {
            IDMap = idMap;
            Options = options;
            SectionLibrary = sectLib;
        }
        
    }
}
