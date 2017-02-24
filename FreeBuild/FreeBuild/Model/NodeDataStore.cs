using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Extensible storage mechanism for adding attached data to nodes
    /// </summary>
    [Serializable]
    public class NodeDataStore : DataStore<INodeDataComponent>
    {
        #region Properties

        /// <summary>
        /// Get a standard type of node attached data
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public INodeDataComponent this[NodeDataType dataType]
        {
            get { return this[dataType.RepresentedType()]; }
        }

        #endregion
    }
}
