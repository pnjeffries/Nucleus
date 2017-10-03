using Nucleus.Conversion;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.GSA
{
    /// <summary>
    /// ID mapping table for conversion to and from GSA
    /// </summary>
    [Serializable]
    public class GSAIDMappingTable : IDMappingTable<string, string>
    {
        /// <summary>
        /// Initialise a new empty GSA ID Mapping Table
        /// </summary>
        public GSAIDMappingTable() : base("Nucleus", "GSA")
        {
            // Specify type category aliases
            TypeCategories.Add(typeof(Node), "NODE");
            TypeCategories.Add(typeof(Element), "EL");
            TypeCategories.Add(typeof(SectionFamily), "PROP_SEC");
            TypeCategories.Add(typeof(BuildUpFamily), "PROP_2D");
        }
    }
}
