using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nucleus.WPF
{
    /// <summary>
    /// Border which automatically snaps it's width to the nearest number of a preset increment
    /// </summary>
    public class SnapBorder : Border
    {
        /// <summary>
        /// The width snapping increment
        /// </summary>
        public double WidthStep { get; set; } = 18;

        protected override Size MeasureOverride(Size constraint)
        {
            Size unmodified = base.MeasureOverride(constraint);
            return new Size(Math.Ceiling(unmodified.Width / WidthStep) * WidthStep, unmodified.Height);
        }
    }
}
