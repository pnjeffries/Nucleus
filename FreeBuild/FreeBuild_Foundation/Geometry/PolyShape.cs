using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// Abstract base class for PolyShapes - shapes formed of several
    /// basic shapes joined together.
    /// </summary>
    public abstract class PolyShape<TShape, TVertex> : Shape<TVertex>
        where TShape : Shape<TVertex>
        where TVertex : IVertex
    {

        #region Properties

        /// <summary>
        /// The constituant sub-shapes that form this overall shape
        /// </summary>
        public ShapeCollection<TShape> Parts { get; } = new ShapeCollection<TShape>();

        #endregion
    }
}
