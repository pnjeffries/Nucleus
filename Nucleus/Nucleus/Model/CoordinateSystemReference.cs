using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A reference to a coordinate system that can be defined and stored within the model,
    /// then used as a definition parameter for geometry, load and result axes
    /// </summary>
    [Serializable]
    public abstract class CoordinateSystemReference : ModelObject
    {
        #region Static Properties

        /// <summary>
        /// Get the object which represents the global coordinate system.
        /// This is a shortcut method to GlobalCoordinateSystem.Instance and will return the same object.
        /// </summary>
        public static GlobalCoordinateSystemReference Global { get { return GlobalCoordinateSystemReference.Instance; } }

        /// <summary>
        /// Get the 'dummy' object which represents the use of the local coordinate system.
        /// This is a shortcut method to LocalCoordinateSystem.Instance and will return the same object.
        /// </summary>
        public static LocalCoordinateSystemReference Local { get { return LocalCoordinateSystemReference.Instance; } }

        #endregion

        #region Properties

        /// <summary>
        /// Does this object represent the global coordinate system
        /// </summary>
        public virtual bool IsGlobal { get { return false; } }

        /// <summary>
        /// Does this object represent the local coordinate system of an element or other object
        /// </summary>
        public virtual bool IsLocal { get { return false; } }

        #endregion

        #region Methods

        /// <summary>
        /// Get the coordinate system defined by this object as applicable to the specified object
        /// </summary>
        /// <param name="onObject">The object to which the coordinate system should relate.</param>
        /// <returns></returns>
        public abstract ICoordinateSystem GetCoordinateSystem(ModelObject onObject);

        /// <summary>
        /// Get the coordinate system defined by this object for the specified position along a linear element
        /// </summary>
        /// <param name="element">The linear element the coordinate system relates to</param>
        /// <param name="t">The position along the linear element that the coordinate system relates to</param>
        /// <returns></returns>
        public abstract ICoordinateSystem GetCoordinateSystem(LinearElement element, double t);

        #endregion
    }
}
