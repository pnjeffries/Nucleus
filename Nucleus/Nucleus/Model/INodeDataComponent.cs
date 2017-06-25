using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Interface for data components which can be attached to nodes
    /// </summary>
    public interface INodeDataComponent : IDataComponent
    {
        /// <summary>
        /// Merge the specified other data component of the same type into this component.
        /// Called when two nodes are merged together.
        /// </summary>
        /// <param name="other"></param>
        void Merge(INodeDataComponent other);
    }
}
