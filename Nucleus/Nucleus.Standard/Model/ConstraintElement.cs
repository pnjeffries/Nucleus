using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// An element set out by a point cloud in space
    /// and whose 3D geometry consists of repeated block
    /// instances at each one of those points
    /// </summary>
    [Serializable]
    public class ConstraintElement : Element<Cloud, ConstraintFamily>
    {
        /// <summary>
        /// Get a point in space which nominally describes the position of this element,
        /// to be used for display attachments and the like.
        /// </summary>
        /// <returns></returns>
        public override Vector GetNominalPosition()
        {
            // Returns the position of the first point in the cloud
            return Geometry?.Vertices?.FirstOrDefault()?.Position ?? Vector.Unset;
        }

        public override void OrientateToVector(Vector vector)
        {
            throw new NotImplementedException();
        }
    }
}
