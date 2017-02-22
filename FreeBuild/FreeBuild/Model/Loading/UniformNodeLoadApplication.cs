using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model.Loading
{
    /// <summary>
    /// A load application which applies the load value directly to a collection
    /// of nodes
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class UniformNodeLoadApplication : LoadApplication
    {
        #region Properties

        /// <summary>
        /// Private backing field for Nodes property
        /// </summary>
        private NodeSet _Nodes = null;

        /// <summary>
        /// The nodes this load is applied to
        /// </summary>
        public NodeSet Nodes
        {
            get
            {
                if (_Nodes == null) _Nodes = new NodeSet();
                return _Nodes;
            }
            set
            {
                ChangeProperty(ref _Nodes, value, "Nodes");
            }
        }

        #endregion
    }
}
