using Nucleus.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Robot
{
    /// <summary>
    /// Class that contains data on the context of a conversion to or from a robot data type
    /// </summary>
    public class RobotConversionContext : ConversionContext
    {
        #region Properties

        /// <summary>
        /// The ID mapping table
        /// </summary>
        public RobotIDMappingTable IDMap { get; set; }

        /// <summary>
        /// The current conversion options set
        /// </summary>
        public RobotConversionOptions Options { get; set; } = new RobotConversionOptions();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.  Initialises a new RobotConversionContext using the
        /// specififed ID Mapping Table
        /// </summary>
        /// <param name="idMap">The ID mapping table to be used.  May be blank, but should not be null.</param>
        public RobotConversionContext(RobotIDMappingTable idMap)
        {
            IDMap = idMap;
        }

        /// <summary>
        /// Initialises a new RobotConversionContext instance using the specified mapping table and options
        /// </summary>
        /// <param name="idMap"></param>
        /// <param name="options"></param>
        public RobotConversionContext(RobotIDMappingTable idMap, RobotConversionOptions options) 
            : this(idMap)
        {
            Options = options;
        }

        #endregion
    }
}
