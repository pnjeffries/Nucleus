using Nucleus.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.ETABS
{
    /// <summary>
    /// A mapping table for Nucleus ids to ETABS ones
    /// </summary>
    [Serializable]
    public class ETABSIDMappingTable : IDMappingTable<string, string>
    {
        public ETABSIDMappingTable() : base("Nucleus", "ETABS")
        {
        }
    }
}
