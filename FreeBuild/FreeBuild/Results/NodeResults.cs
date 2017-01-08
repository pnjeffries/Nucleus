using FreeBuild.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Results
{
    /// <summary>
    /// Results storage table for nodes
    /// </summary>
    [Serializable]
    public class NodeResults : ModelObjectResults<NodeResultTypes, Interval>
    {
        #region Constructors

        /// <summary>
        /// Deserialisation constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected NodeResults(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Initialises a new empty NodeResults dictionary
        /// </summary>
        public NodeResults() : base() { }

        #endregion
    }
}
