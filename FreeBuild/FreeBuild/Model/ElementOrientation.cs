using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Base class for objects which describe the orientation
    /// of the local coordinate system of an element,
    /// which in turn may influence how the volumetric property
    /// is applied to the set-out geometry to create the full
    /// solid representation of the object
    /// </summary>
    public abstract class ElementOrientation
    {
        #region Operator

        /// <summary>
        /// Implicit conversion from a double to an ElementOrientationAngle
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ElementOrientation(double value)
        {
            return new ElementOrientationAngle(value);
        }

        #endregion
    }
}
