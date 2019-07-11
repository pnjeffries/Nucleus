using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A set of attributes which determine how an object should be rendered
    /// </summary>
    [Serializable]
    public class DisplayAttributes : NotifyPropertyChangedBase, IDuplicatable
    {
        /// <summary>
        /// Private backing field for the Brush property
        /// </summary>
        private DisplayBrush _Brush = null;

        /// <summary>
        /// The brush which determines how this geometry should be drawn.
        /// </summary>
        public DisplayBrush Brush
        {
            get { return _Brush; }
            set { ChangeProperty(ref _Brush, value, "Brush"); }
        }

        /// <summary>
        /// Private backing field for the Thickness property
        /// </summary>
        private double _Weight = 1.0;

        /// <summary>
        /// The scaling factor applied to the thickness of curves and the
        /// size of points when drawn.
        /// </summary>
        public double Weight
        {
            get { return _Weight; }
            set { ChangeProperty(ref _Weight, value, "Weight"); }
        }

    }
}
