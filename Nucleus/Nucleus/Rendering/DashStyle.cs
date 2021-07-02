using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A class for defining the dash style of a line brush
    /// </summary>
    [Serializable]
    public class DashStyle
    {
        #region Properties

        /// <summary>
        /// The size of each dash segment
        /// </summary>
        public double Size = 1f;

        /// <summary>
        /// The spacing between each dash segment
        /// </summary>
        public double Spacing = 1f;

        /// <summary>
        /// The offset of the dash pattern
        /// </summary>
        public double Offset = 0f;

        #endregion

        #region Constructors

        public DashStyle(double size, double spacing)
        {
            Size = size;
            Spacing = spacing;
        }

        public DashStyle(double size, double spacing, double offset)
        {
            Size = size;
            Spacing = spacing;
            Offset = offset;
        }

        #endregion

    }
}
