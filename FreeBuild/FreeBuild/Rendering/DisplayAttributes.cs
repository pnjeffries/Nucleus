using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Rendering
{
    /// <summary>
    /// A set of attributes which determine how an object should be rendered
    /// </summary>
    [Serializable]
    public class DisplayAttributes
    {
        /// <summary>
        /// The brush which determines how this geometry should be drawn.
        /// </summary>
        public DisplayBrush Brush { get; set; } = null;

    }
}
