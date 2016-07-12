using FreeBuild.Base;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An abstract base class for shapes.
    /// Shapes are geometry defined by and containing a set of vertices.
    /// </summary>
    /// <typeparam name="TVertex">The type of vertex used to define this shape</typeparam>
    /// <typeparam name="TParameter">The type of the parameter used to indicate a specific position </typeparam>
    public abstract class Shape<TVertex> : Unique, IShape
        where TVertex : IVertex
    {
        #region Properties

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape
        /// </summary>
        public abstract VertexCollection<TVertex> Vertices { get; }

        /// <summary>
        /// Is the definition of this shape valid?
        /// i.e. does it have the correct number of vertices, are all parameters within acceptable limits, etc.
        /// </summary>
        public abstract bool IsValid { get; }

        #endregion
    }
}
