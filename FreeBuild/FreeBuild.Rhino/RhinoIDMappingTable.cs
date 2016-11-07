using FreeBuild.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Rhino
{
    /// <summary>
    /// An ID mapping table from FreeBuild objects to Rhino objects
    /// </summary>
    [Serializable]
    public class RhinoIDMappingTable : IDMappingTable<Guid,Guid>
    {
        #region Properties

        /// <summary>
        /// The name of the category under which Element Geometry Curves are stored
        /// </summary>
        public string ElementCategory { get { return "Elements"; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Mapping Table Constructor
        /// </summary>
        /// <param name="mappedIDName">The name of the first ID set</param>
        public RhinoIDMappingTable(string mappedIDName = "FreeBuild") : base(mappedIDName, "Rhino") { }

        #endregion
    }
}
