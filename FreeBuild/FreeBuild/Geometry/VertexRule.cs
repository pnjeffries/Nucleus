using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Abstract base class for rules which are applied to vertices in
    /// order to define their position by reference to some other object
    /// and/or criteria.
    /// </summary>
    [Serializable]
    public abstract class VertexRule
    {
        /// <summary>
        /// Apply this rule to the vertex.  This may result in the specified vertex being
        /// moved or deleted.
        /// </summary>
        /// <param name="toVertex">The vertex that the rule is to be applied to.</param>
        /// <returns></returns>
        public abstract bool ApplyRule(Vertex toVertex);
    }
}
