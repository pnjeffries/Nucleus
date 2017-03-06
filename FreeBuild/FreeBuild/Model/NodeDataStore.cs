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
    public class NodeDataStore : DataStore<INodeDataComponent, NodeDataType>
    {

        #region Methods

        protected override Type GetRepresentedType(NodeDataType typeEnum)
        {
            return typeEnum.RepresentedType();
        }

        #endregion

    }
}
