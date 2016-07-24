using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Rendering
{
    /// <summary>
    /// An interface for renderable objects
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Draw this renderable
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool Draw(RenderingParameters parameters);
    }
}
