using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UI
{
    public class AutoUISliderAttribute : AutoUIAttribute
    {
        #region Properties

        /// <summary>
        /// The slider minimum value
        /// </summary>
        public double Min { get; set; } = 0;

        /// <summary>
        /// The slider maximum value
        /// </summary>
        public double Max { get; set; } = 100;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoUISliderAttribute() { }

        /// <summary>
        /// Order constructor
        /// </summary>
        /// <param name="order"></param>
        public AutoUISliderAttribute(double order)
        {
            Order = order;
        }

        #endregion
    }
}
