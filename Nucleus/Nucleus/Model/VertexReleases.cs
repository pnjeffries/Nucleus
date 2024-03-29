﻿using Nucleus.Base;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Data component which stores the translational and rotational
    /// releases of a vertex which forms part of an element definition,
    /// for use during structural analysis, physics simulation etc.
    /// If 
    /// </summary>
    [Serializable]
    [Copy(CopyBehaviour.DUPLICATE)]
    public class VertexReleases : Unique, IVertexDataComponent
    {
        #region Constants

        /// <summary>
        /// Get a new VertexReleases component set up to represent full fixity in all directions.
        /// This is the default condition which will be assumed if no VertexReleases component is
        /// attached to the vertex, so you can simply remove any previous releases component from
        /// the vertex rather than use this value.
        /// </summary>
        public static VertexReleases Fixed { get { return new VertexReleases(new Bool6D(false)); } }

        /// <summary>
        /// Get a new VertexReleases component set up to represent full moment release of the vertex.
        /// </summary>
        public static VertexReleases Pinned { get { return new VertexReleases(new Bool6D(false, false, false, true, true, true)); } }

        /// <summary>
        /// Get a new VertexReleases component set up to represent release in the XX and YY axes only.
        /// </summary>
        public static VertexReleases MomentsReleased { get { return new VertexReleases(new Bool6D(false, false, false, true, true, false)); } }

        /// <summary>
        /// Get a new VertexReleases component set up to represent full rotational and translational release.
        /// </summary>
        public static VertexReleases Free { get { return new VertexReleases(new Bool6D(true)); } }

        #endregion

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

        /// <summary>
        /// Private backing field for Stiffness property
        /// </summary>
        private SixVector _Stiffness = new SixVector();

        /// <summary>
        /// The stiffnesses in the translational and rotational degrees of freedom
        /// of this release.  By default this stiffness is 0 in all directions.
        /// This property is only considered when the release in the relevent direction
        /// is set to true - otherwise the element is taken to be fully restrained in that
        /// axis and the stiffness is effectively infinite.
        /// Expressed in N/m.
        /// </summary>
        public SixVector Stiffness
        {
            get { return _Stiffness; }
            set { ChangeProperty(ref _Stiffness, value, "Stiffness"); }
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

        /// <summary>
        /// Initialise a new vertex releases component with the specified
        /// releases and stiffnesses.  Note that the stiffness values will
        /// typically be ignored in directions which are not also released.
        /// </summary>
        /// <param name="releases"></param>
        /// <param name="stiffness"></param>
        public VertexReleases(Bool6D releases, SixVector stiffness) : this (releases)
        {
            _Stiffness = stiffness;
        }

        #endregion
    }
}
