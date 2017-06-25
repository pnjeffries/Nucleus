using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Extensible storage mechanism for adding attached data to nodes
    /// </summary>
    [Serializable]
    public class NodeDataStore : DataStore<INodeDataComponent, Node, NodeDataType>
    {

        #region Constructors

        public NodeDataStore(Node owner) : base(owner) { }

        #endregion

        #region Methods

        protected override Type GetRepresentedType(NodeDataType typeEnum)
        {
            return typeEnum.RepresentedType();
        }

        /// <summary>
        /// Merge the data in the specified other data store into this one,
        /// merging existing components of matching types and adding those
        /// which are unique.
        /// </summary>
        /// <param name="other"></param>
        public void Merge(NodeDataStore other)
        {
            // Merge shared component types:
            foreach (INodeDataComponent component in this)
            {
                Type type = component.GetType();
                if (other.Contains(type))
                {
                    component.Merge(other[type]);
                }
            }

            // Add component types exclusive to the other:
            foreach (INodeDataComponent component in other)
            {
                if (!Contains(component.GetType()))
                {
                    Add(component);
                }
            }
        }

        #endregion

        #region Operators

        public static NodeDataStore operator +(NodeDataStore store, INodeDataComponent component)
        {
            store.SetData(component);
            return store;
        }

        #endregion
    }
}
