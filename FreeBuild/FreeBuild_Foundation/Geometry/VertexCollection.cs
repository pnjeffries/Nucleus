using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An obervable, keyed collection of vertices
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [Serializable]
    public class VertexCollection : OwnedCollection<Vertex, Shape>
    {
        
        #region Constructor

        /// <summary>
        /// Owner constructor
        /// </summary>
        /// <param name="owner"></param>
        public VertexCollection(Shape owner): base(owner)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the owning geometry of the specified vertex
        /// </summary>
        /// <param name="item"></param>
        protected override void SetItemOwner(Vertex item)
        {
            if (Owner != null)
            {
                if (item.Owner != null && item.Owner != Owner)
                {
                    throw new Exception("Vertex already has an owner.  The same vertex cannot belong to more than one piece of geometry.");
                }
                else
                    item.Owner = Owner;
            }
        }

        /// <summary>
        /// Clears the owning geometry of the specified
        /// </summary>
        /// <param name="item"></param>
        protected override void ClearItemOwner(Vertex item)
        {
            if (item.Owner == Owner) item.Owner = null;
        }

        #endregion
    }
}
