using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Rendering
{
    /// <summary>
    /// A set of parameters passed in to renderable objects to allow them to be drawn.
    /// Actual implementation will be application-specific
    /// </summary>
    public abstract class RenderingParameters
    {
        /// <summary>
        /// The current position of the 3D cursor during dynamically-drawn selection operations
        /// </summary>
        public Vector CursorPosition { get; set; } = Vector.Unset;
    }
}
