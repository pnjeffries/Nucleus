using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Data component which stores the translational and rotational
    /// releases of a vertex which forms part of an element definition.
    /// </summary>
    [Serializable]
    public class VertexReleases : Unique, IVertexDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing field for Releases property
        /// </summary>
        private Bool6D _Releases = new Bool6D();

        /// <summary>
        /// The released directions.  These are stored relative
        /// to the local coordinate system of the owning element.
        /// </summary>
        public Bool6D Releases
        {
            get { return _Releases; }
            set { ChangeProperty(ref _Releases, value, "Releases"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new Vertex releases component with default values.
        /// </summary>
        public VertexReleases() { }

        /// <summary>
        /// Initialise a new vertex releases component with the specified 
        /// translational and rotational releases
        /// </summary>
        /// <param name="releases"></param>
        public VertexReleases(Bool6D releases)
        {
            _Releases = releases;
        }

        #endregion
    }
}
