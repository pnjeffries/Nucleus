using FreeBuild.Maths;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Results
{
    /// <summary>
    /// A table of results for objects under a particular analysis case.
    /// Keyed by object GUID.
    /// </summary>
    [Serializable]
    public class CaseResults : ResultsDictionary<Guid, IModelObjectResults>
    {
        #region Properties

        /// <summary>
        /// Get or set the results for the specified object
        /// </summary>
        /// <param name="mObject"></param>
        /// <returns></returns>
        public IModelObjectResults this[ModelObject mObject]
        {
            get { return this[mObject.GUID]; }
            set { this[mObject.GUID] = value; }
        }

        /// <summary>
        /// Get or set the results table for the specified node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public NodeResults this[Node node]
        {
            get { return this[node.GUID] as NodeResults; }
            set { this[node.GUID] = value; }
        }

        /// <summary>
        /// Get the maximum and minimum stored results of the specified type for the specified node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Interval this[Node node, NodeResultTypes type]
        {
            get
            {
                NodeResults res = this[node];
                if (res != null) return res[type];
                else return Interval.Unset;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Deserialisation constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CaseResults(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Initialises a new empty CaseResults
        /// </summary>
        public CaseResults() : base() { }

        #endregion
    }
}
