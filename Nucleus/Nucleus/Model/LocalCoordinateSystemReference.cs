using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Geometry;

namespace Nucleus.Model
{
    /// <summary>
    /// A coordinate system reference the use of which indicates that the local coordinate system
    /// of the element (or other object) in question should be used in place of a global or
    /// user-defined coordinate system.
    /// </summary>
    [Serializable]
    public class LocalCoordinateSystemReference : CoordinateSystemReference
    {
        #region Static Fields

        /// <summary>
        /// The single static instance that should be used to refer to the use of the local coordinate system
        /// </summary>
        public static readonly LocalCoordinateSystemReference Instance = new LocalCoordinateSystemReference();

        #endregion

        #region Properties

        /// <summary>
        /// Does this object represent a local coordinate system?
        /// Yes - it does!  Will always return true for this type.
        /// </summary>
        public override bool IsLocal
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Constructors

        private LocalCoordinateSystemReference() : base()
        {
            Name = "Local";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the coordinate system defined by this object for the specified position along a linear element
        /// </summary>
        /// <param name="element">The linear element the coordinate system relates to</param>
        /// <param name="t">The position along the linear element that the coordinate system relates to</param>
        /// <returns></returns>
        public override ICoordinateSystem GetCoordinateSystem(LinearElement element, double t)
        {
            if (element?.Geometry != null)
            {
                return element.Geometry.LocalCoordinateSystem(t, element.Orientation);
            }
            return null;
        }


        public override bool Equals(object obj)
        {
            return obj != null && obj is LocalCoordinateSystemReference;
        }

        #endregion
    }
}
