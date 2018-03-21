using Nucleus.Geometry;
using Nucleus.Maths;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// A top-level storage mechanism for all analysis results for a particular
    /// model, keyed by object, then by case, then by type
    /// </summary>
    [Serializable]
    public class ModelResults : ResultsDictionary<Guid, IModelObjectResults>
    {
        #region Properties

        /// <summary>
        /// Get or set the stored results for the specified node.
        /// Will return null if no stored results are found or if they
        /// are not in the correct format.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public NodeResults this[Node node]
        {
            get { return this[node.GUID] as NodeResults; }
            set { this[node.GUID] = value; }
        }

        /// <summary>
        /// Get the stored results (if any) for the specified node and
        /// case.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rCase"></param>
        /// <returns></returns>
        public CaseNodeResults this[Node node, ResultsCase rCase]
        {
            get { return this[node]?[rCase]; }
        }

        /// <summary>
        /// Get the stored result (if any) of the specified type for
        /// the specified node and case.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rCase"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Interval this[Node node, ResultsCase rCase, NodeResultTypes type]
        {
            get { return this[node, rCase]?[type] ?? Interval.Unset; }
        }

        /// <summary>
        /// Get or set the stored results for the specified linear element.
        /// Will return null if no stored results are found for this
        /// element of if they are not in the correct format.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public LinearElementResults this[LinearElement element]
        {
            get { return this[element.GUID] as LinearElementResults; }
            set { this[element.GUID] = value; }
        }

        /// <summary>
        /// Get the stored results (if any) for the specified linear
        /// element and case.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rCase"></param>
        /// <returns></returns>
        public CaseLinearElementResults this[LinearElement element, ResultsCase rCase]
        {
            get { return this[element]?[rCase]; }
        }

        /// <summary>
        /// Get the stored results (if any) of the specified type for the
        /// specified linear element and case.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rCase"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public LinearIntervalDataSet this[LinearElement element, ResultsCase rCase, LinearElementResultTypes type]
        {
            get { return this[element, rCase]?[type]; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the results for the specified node to the store.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="results"></param>
        public void Add(Node node, NodeResults results)
        {
            Add(node.GUID, results);
        }

        /// <summary>
        /// Adds the results for the specified linear element to the store.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="results"></param>
        public void Add(LinearElement element, LinearElementResults results)
        {
            Add(element.GUID, results);
        }

        /// <summary>
        /// Remove from this store any results belonging to the specfied case.
        /// </summary>
        /// <param name="rCase"></param>
        public void ClearCaseResults(ResultsCase rCase)
        {
            foreach (var kvp in this)
            {
                kvp.Value.Remove(rCase);
            }
        }

        /// <summary>
        /// Calculate the deformed centreline geometry of a linear element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public Curve DeformedGeometry(LinearElement element, ResultsCase rCase)
        {
            var sNode = element.StartNode;
            var eNode = element.EndNode;
            Vector sDef = this[sNode, rCase]?.GetMaxDisplacementVector() ?? Vector.Zero;
            Vector eDef = this[eNode, rCase]?.GetMaxDisplacementVector() ?? Vector.Zero;

            // TODO: Add in offsets + rotation
            // TODO: Add in element deflection
            return new Line(sNode.Position + sDef, sNode.Position + eDef);
        }

        #endregion
    }
}
