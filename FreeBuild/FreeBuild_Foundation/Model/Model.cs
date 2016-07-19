using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A self-contained data structure that represents an entire
    /// BIM or analysis model.
    /// </summary>
    [Serializable]
    public class Model : Unique
    {
        #region Properties

        /// <summary>
        /// The collection of elements that form the geometric representation 
        /// of this model.
        /// </summary>
        public ElementCollection Elements { get; }

        /// <summary>
        /// The collection of nodes that belong to this model.
        /// </summary>
        public NodeCollection Nodes { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Model()
        {
            Elements = new ElementCollection();
            Nodes = new NodeCollection();
        }
        
        #endregion

    }
}
