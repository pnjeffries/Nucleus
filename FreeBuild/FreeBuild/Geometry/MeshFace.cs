using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Mesh face representation.  Describes a face which connects three or more
    /// vertices in a mesh.  The face is defined as a series of vertex indices in the
    /// parent mesh.
    /// </summary>
    [Serializable]
    public class MeshFace : VertexCollection, IUnique
    {
        #region Properties

        /// <summary>
        /// Internal backing member for GUID property
        /// </summary>
        [Copy(CopyBehaviour.DO_NOT_COPY)]
        private Guid _GUID = Guid.NewGuid();

        /// <summary>
        /// The GUID of this object, which can be used to uniquely identify it. 
        /// </summary>
        public Guid GUID { get { return _GUID; } }

        /// <summary>
        /// Is this mesh face definition valid?
        /// </summary>
        public bool IsValid { get { return Count >= 3 && Count <= 4; } }

        /// <summary>
        /// Is this mesh face triangular?
        /// i.e. does it have three vertices?
        /// </summary>
        public bool IsTri { get { return Count == 3; } }

        /// <summary>
        /// Is this mesg face quadrangular?
        /// i.e. does it have four vertices?
        /// </summary>
        public bool IsQuad { get { return Count == 4; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MeshFace()
        {

        }

        #endregion

        #region Methods

        #endregion

    }
}
