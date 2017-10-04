using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;

namespace Nucleus.Model
{
    /// <summary>
    /// A coordinate system reference that is used to indicate that the global coordinate system
    /// should be used.
    /// </summary>
    [Serializable]
    public class GlobalCoordinateSystemReference : CoordinateSystemReference
    {
        #region Static Fields

        /// <summary>
        /// The single static instance that should be used to refer to the global coordinate system
        /// </summary>
        public static readonly GlobalCoordinateSystemReference Instance = new GlobalCoordinateSystemReference();

        #endregion

        #region Properties

        /// <summary>
        /// Does this object represent the global coordinate system?
        /// Yes, it does - will always return true.
        /// </summary>
        public override bool IsGlobal
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor
        /// </summary>
        private GlobalCoordinateSystemReference() : base()
        {
            Name = "Global";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the coordinate system defined by this object as applicable to the specified object
        /// </summary>
        /// <param name="onObject">The object to which the coordinate system should relate.</param>
        /// <returns></returns>
        public override ICoordinateSystem GetCoordinateSystem(ModelObject onObject)
        {
            return CartesianCoordinateSystem.Global;
        }

        /// <summary>
        /// Get the coordinate system defined by this object for the specified position along a linear element
        /// </summary>
        /// <param name="element">The linear element the coordinate system relates to</param>
        /// <param name="t">The position along the linear element that the coordinate system relates to</param>
        /// <returns></returns>
        public override ICoordinateSystem GetCoordinateSystem(LinearElement element, double t)
        {
            return CartesianCoordinateSystem.Global;
        }

        /// <summary>
        /// Any instance of a global coordinate system will evaluate as equal to any other.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj != null && obj is GlobalCoordinateSystemReference;
        }

        #endregion
    }
}
