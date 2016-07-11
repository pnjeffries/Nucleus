using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Represents an infinite plane described in terms of an origin point and a local coordinate system,
    /// where X and Y axes lie on the plane and the Z axis is normal to it.
    /// The coordinate system of the plane is right-handed.
    /// Immutable geometric primitive.
    /// </summary>
    [Serializable]
    public class Plane :
        ICoordinateSystem
    {
        #region Fields

        /// <summary>
        /// The origin point of the plane
        /// </summary>
        public readonly Vector Origin;

        /// <summary>
        /// The unit vector defining the local X-axis of the plane
        /// </summary>
        public readonly Vector X;

        /// <summary>
        /// The unit vector defining the local Y-axis of the plane
        /// </summary>
        public readonly Vector Y;

        /// <summary>
        /// The unit vector normal to the plane that forms the local Z-axis
        /// </summary>
        public readonly Vector Z;

        #endregion

        #region Properties

        #endregion


        #region Constructors

        /// <summary>
        /// Default constructor.  Plane is initialised as a global XY plane.
        /// </summary>
        public Plane()
        {
            Origin = Vector.Zero;
            X = Vector.UnitX;
            Y = Vector.UnitY;
            Z = Vector.UnitZ;
        }

        /// <summary>
        /// Constructor creating a global XY plane with its origin at the specified point
        /// </summary>
        /// <param name="origin">The origin point of the new plane</param>
        public Plane(Vector origin)
        {
            Origin = origin;
            X = Vector.UnitX;
            Y = Vector.UnitY;
            Z = Vector.UnitZ;
        }

        /// <summary>
        /// Constructor creating a plane defined by an origin point and a normal vector.
        /// The X- and Y-axes will be generated perpendicular to the normal vector and with the x-axis orientated as closely as possible to the global x-axis.
        /// </summary>
        /// <param name="origin">The origin point of the plane</param>
        /// <param name="normal">The normal vector to the plane.  Will become the plane's local Z-axis.
        /// Should be a unit vector if consistent scaling is required.</param>
        public Plane(Vector origin, Vector normal)
        {
            Origin = origin;
            Z = normal;
            Y = Z.Cross(Vector.UnitX);
            X = Y.Cross(Z);
        }

        /// <summary>
        /// Constructor creating a plane defined by an origin and two vectors on the plane.
        /// </summary>
        /// <param name="origin">The origin point of the plane</param>
        /// <param name="xAxis">The first vector that lies on the plane.  Will form the plane local x-axis.</param>
        /// <param name="xyVector">A second vector that lies on the plane but that is not coincident with the first.</param>
        public Plane(Vector origin, Vector xAxis, Vector xyVector)
        {
            Origin = origin;
            X = xAxis;
            Z = xAxis.Cross(xyVector);
            Y = Z.Cross(xAxis);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert a vector defined in the global coordinate system into 
        /// one defined in local coordinates of this coordinate system.
        /// </summary>
        /// <param name="vector">A vector in the global coordinate system.</param>
        /// <returns>A vector in local coordinates</returns>
        public Vector GlobalToLocal(Vector vector)
        {
            Vector relV = vector - Origin;
            return new Vector(relV.Dot(X), relV.Dot(Y), relV.Dot(Z));
        }

        /// <summary>
        /// Convert a vector defined in the local coordinate system into
        /// one defined in global coordinates
        /// </summary>
        /// <param name="vector">A vector in the local coordinate system.</param>
        /// <returns>A vector in global coordinates</returns>
        public Vector LocalToGlobal(Vector vector)
        {
            return Origin + X * vector.X + Y * vector.Y + Z * vector.Z;
        }

        #endregion
    }
}
