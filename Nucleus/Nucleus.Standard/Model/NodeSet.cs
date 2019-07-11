using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A parametrically-defined set of Nodes.  
    /// Allows node collections to be defined via a base collection and a set of logical 
    /// filters which act upon that collection.
    /// </summary>
    [Serializable]
    public class NodeSet : ModelObjectSet<
        Node, 
        NodeCollection, 
        ISetFilter<Node>, 
        SetFilterCollection<ISetFilter<Node>, Node>,
        NodeSet, ModelObjectSetCollection<NodeSet>>
    {
        #region Constructors

        public NodeSet()
        {
        }

        public NodeSet(NodeCollection baseCollection) : base(baseCollection)
        {
        }

        public NodeSet(ISetFilter<Node> filter) : base(filter)
        {
        }

        public NodeSet(Node item) : base(item)
        {
        }

        public NodeSet(bool all) : base(all)
        {
        }

        #endregion

        #region Methods

        protected override NodeCollection GetItemsInModel()
        {
            return Model?.Nodes;
        }

        #endregion
    }

}

