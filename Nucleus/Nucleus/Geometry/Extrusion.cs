using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A solid defined as an extrusion of a planar region along a straight line
    /// </summary>
    [Serializable]
    public class Extrusion : Solid
    {
        #region Properties

        /// <summary>
        /// Backing field for Profile property
        /// </summary>
        [Copy(CopyBehaviour.DUPLICATE)]
        private PlanarRegion _Profile;

        /// <summary>
        /// The cross-sectional profile which is extruded to define the volume
        /// </summary>
        public PlanarRegion Profile
        {
            get { return _Profile; }
            set { ChangeProperty(ref _Profile, value, "Profile"); }
        }

        /// <summary>
        /// Backing field for Path property
        /// </summary>
        private Vector _Path;

        /// <summary>
        /// The extrusion path.  The profile will be extruded along this
        /// vector in order to define the full 3D geometry of the extrusion volume
        /// </summary>
        public Vector Path
        {
            get { return _Path; }
            set { ChangeProperty(ref _Path, value, "Path"); }
        }

        /// <summary>
        /// Does this extrusion have a valid geometric definition?
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return Profile != null && Profile.IsValid && Path.IsValidNonZero();
            }
        }

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// Different shapes will provide different means of editing this collection.
        /// DO NOT directly modify the collection returned from this property unless you are
        /// sure you know what you are doing.
        /// </summary>
        public override VertexCollection Vertices
        {
            get
            {
                return Profile.Vertices;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new Extrusion object with a blank definition
        /// </summary>
        public Extrusion()
        {
        }

        /// <summary>
        /// Initialise a new Extrusion object representing an 
        /// extrusion of the specified profile along the specified path
        /// </summary>
        /// <param name="profile">The profile surface to extrude</param>
        /// <param name="path">The extrusion vector</param>
        public Extrusion(PlanarRegion profile, Vector path)
        {
            _Profile = profile;
            _Path = path;
        }

        /// <summary>
        /// Initialise a new Extrusion object representing an 
        /// extrusion of the specified profile along the specified path
        /// </summary>
        /// <param name="profile">The profile perimeter curve.  Must be planar.</param>
        /// <param name="path">The extrusion vector</param>
        public Extrusion(Curve profile, Vector path)
        {
            _Profile = new PlanarRegion(profile);
            _Path = path;
        }

        #endregion
    }
}
