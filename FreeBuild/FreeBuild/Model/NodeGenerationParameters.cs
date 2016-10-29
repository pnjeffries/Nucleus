using FreeBuild.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A set of parameters used in the generation of nodes
    /// </summary>
    public class NodeGenerationParameters
    {
        /// <summary>
        /// The execution info for the operation that the node generation is taking part as part of
        /// </summary>
        public ExecutionInfo ExInfo { get; set; } = null;

        /// <summary>
        /// The distance tolerance for creating connections
        /// </summary>
        public double ConnectionTolerance { get; set; } = 0;
    }
}
