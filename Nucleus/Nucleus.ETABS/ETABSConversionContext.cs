using Nucleus.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.ETABS
{
    /// <summary>
    /// Object storing data about the context of data conversion between
    /// .Nucleus and ETABS
    /// </summary>
    public class ETABSConversionContext : ConversionContext
    {
        #region Properties

        /// <summary>
        /// The ID mapping table
        /// </summary>
        public ETABSIDMappingTable IDMap { get; set; }

        /// <summary>
        /// The current conversion options set
        /// </summary>
        public ETABSConversionOptions Options { get; set; } = new ETABSConversionOptions();

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new ETABSConversionContext with the specified ID Map and options
        /// </summary>
        /// <param name="idMap"></param>
        /// <param name="options"></param>
        public ETABSConversionContext(ETABSIDMappingTable idMap, ETABSConversionOptions options)
        {
            IDMap = idMap;
            Options = options;
        }

        #endregion
    }
}
