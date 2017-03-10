using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for structural and physical load types that act in a particular
    /// direction
    /// </summary>
    /// <typeparam name="TAppliedTo"></typeparam>
    public abstract class ForceLoad<TAppliedTo> : Load<TAppliedTo>
        where TAppliedTo : ModelObjectSetBase, new()
    {
        #region Properties

        /// <summary>
        /// Private backing field for Direction property
        /// </summary>
        private Direction _Direction = Direction.Z;

        /// <summary>
        /// The axis (of the coordinate system specified by the Axes property)
        /// along which or about which this force is applied
        /// </summary>
        public Direction Direction
        {
            get { return _Direction; }
            set { ChangeProperty(ref _Direction, value, "Direction"); }
        }

        /// <summary>
        /// Private backing field for Axes property
        /// </summary>
        private CoordinateSystemReference _Axes = CoordinateSystemReference.Global;

        /// <summary>
        /// The reference system used to resolve the application of the load
        /// in 3D space.  The direction property determines which axis of this
        /// system is used.
        /// </summary>
        public CoordinateSystemReference Axes
        {
            get { return _Axes; }
            set { ChangeProperty(ref _Axes, value, "Axes"); }
        }

        #endregion
    }
}
