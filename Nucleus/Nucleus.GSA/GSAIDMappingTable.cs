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
    public class GSAIDMappingTable : IDMappingTable<string, string>
    {
        public GSAIDMappingTable() : base("Nucleus", "GSA")
        {
            TypeCategories.Add(typeof(Element), "ELEMENT");
            TypeCategories.Add(typeof(SectionFamily), "SECTION");
            TypeCategories.Add(typeof(BuildUpFamily), "BUILDUP");
        }
    }
}
