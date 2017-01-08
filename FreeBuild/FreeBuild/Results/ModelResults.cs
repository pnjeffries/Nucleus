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
    /// Storage dictionary for all the results belonging to a model,
    /// keyed by the analysis or combination case for which they were generated
    /// </summary>
    [Serializable]
    public class ModelResults : ResultsDictionary<ResultsCase, CaseResults>
    {
        #region Properties

        /// <summary>
        /// Get the object results for the specified case and item
        /// </summary>
        /// <param name="rCase"></param>
        /// <param name="itemGuid"></param>
        /// <returns></returns>
        public IModelObjectResults this[ResultsCase rCase, Guid itemGuid]
        {
            get { return this[rCase]?[itemGuid]; }
        }

        /// <summary>
        /// Get the object results for the specififed case and item
        /// </summary>
        /// <param name="rCase"></param>
        /// <param name="mObject"></param>
        /// <returns></returns>
        public IModelObjectResults this[ResultsCase rCase, ModelObject mObject]
        {
            get { return this[rCase]?[mObject]; }
        }

        /// <summary>
        /// Get the node results table for the specified case and node
        /// </summary>
        /// <param name="rCase"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public NodeResults this[ResultsCase rCase, Node node]
        {
            get { return this[rCase]?[node]; }
        }

        /// <summary>
        /// Get the result value(s) for the specified case, node and result type
        /// </summary>
        /// <param name="rCase"></param>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Interval this[ResultsCase rCase, Node node, NodeResultTypes type]
        {
            get
            {
                NodeResults nRes = this[rCase, node];
                if (nRes != null) return nRes[type];
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
        protected ModelResults(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Initialises a new empty ModelResults
        /// </summary>
        public ModelResults() : base() { }

        #endregion
    }
}
