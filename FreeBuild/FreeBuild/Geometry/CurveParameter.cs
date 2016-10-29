using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Represents a parametrically-defined position on a curve.
    /// </summary>
    public class CurveParameter : IShapePosition<Curve>
    {
        #region Properties

        /// <summary>
        /// The parameter on the curve
        /// </summary>
        public double T { get; set; }

        // <summary>
        /// Get the point on the shape that this object
        /// describes as a Vector
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public Vector PointOn(Curve shape)
        {
            return shape.PointAt(T);
        }

        #endregion

        #region Constructor



        #endregion

        #region Methods

        public override int GetHashCode()
        {
            return T.GetHashCode();
        }

        #endregion

    }
}
