using FreeBuild.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A transform is an operation which can be applied to vertex geometry
    /// in order to change its position or orientation.
    /// Represented as a 4 x 4 transformation matrix
    /// </summary>
    public class Transform : ArrayMatrix
    {
        #region Constructors

        /// <summary>
        /// Default constructor - creates a new identity transform.
        /// Equates to a 4x4 matrix with the main diagonal set to 1.0.
        /// </summary>
        public Transform() : base(4,4)
        {
            SetMainDiagonal(1.0);
        }

        /// <summary>
        /// Translation constructor.  Creates a translation transform along the specified vector
        /// </summary>
        /// <param name="translationVector">The vector of the translation</param>
        public Transform(Vector translationVector) : this()
        {
            this[0, 3] = translationVector.X;
            this[1, 3] = translationVector.Y;
            this[2, 3] = translationVector.Z;
        }

        #endregion
    }
}
