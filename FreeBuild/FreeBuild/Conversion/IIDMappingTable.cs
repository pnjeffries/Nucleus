using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// Interface for tables which map from one application's unique identifier system
    /// to another.
    /// </summary>
    public interface IIDMappingTable
    {
        /// <summary>
        /// The name of the first set of IDs to be contained within this table
        /// </summary>
        string FirstIDName { get; }

        /// <summary>
        /// The name of the second set of IDs to be contained within this table
        /// </summary>
        string SecondIDName { get; }

        /// <summary>
        /// The file path of the file that this mapping was written to or read from,
        /// if any.
        /// </summary>
        FilePath FilePath { get; set; }

        /// <summary>
        /// The last time that this mapping table was used
        /// </summary>
        DateTime LastUsed { get; }
    }
}
