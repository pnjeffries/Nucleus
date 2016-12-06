using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Rendering
{
    /// <summary>
    /// A brush that uses a single solid colour
    /// </summary>
    public class ColourBrush : DisplayBrush
    {
        #region Properties

        /// <summary>
        /// The colour of the brush
        /// </summary>
        public Colour Colour { get; set; } = Colour.Black;

        #endregion

        #region Constructors

        /// <summary>
        /// Colour constructor
        /// </summary>
        /// <param name="colour"></param>
        public ColourBrush(Colour colour)
        {
            Colour = colour;
        }

        #endregion
    }
}
