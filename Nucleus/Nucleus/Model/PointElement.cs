using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// An element set out by a single point in space and whose
    /// solid 3D geometry is produced by positioning a block geometry
    /// at that point.
    /// </summary>
    public class PointElement : Element<Point, BlockFamily>
    {
        /// <summary>
        /// Get a point in space which nominally describes the position of this element,
        /// to be used for display attachments and the like.
        /// </summary>
        /// <returns></returns>
        public override Vector GetNominalPosition()
        {
            if (Geometry != null) return Geometry.Position;
            else return Vector.Unset;
        }

        public override void OrientateToVector(Vector vector)
        {
            throw new NotImplementedException();
        }
    }
}
