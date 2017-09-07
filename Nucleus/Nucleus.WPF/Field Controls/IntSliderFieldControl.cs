using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.WPF
{
    public class IntSliderFieldControl : SliderFieldControl
    {
        /// <summary>
        /// The type of the control's value
        /// </summary>
        public override Type ValueType
        {
            get { return typeof(int); }
        }

        #region Constructors

        public IntSliderFieldControl() : base()
        {
            TickFrequency = 1;
        }

        /// <summary>
        /// Minimum, maximum constructor
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public IntSliderFieldControl(int minimum, int maximum) : base(minimum, maximum)
        {
            TickFrequency = 1;
        }

        /// <summary>
        /// Minimum, maximum, tick frequency constructor
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="tickFrequency"></param>
        public IntSliderFieldControl(int minimum, int maximum, int tickFrequency) : base(minimum, maximum, tickFrequency) { }

        #endregion
    }
}
