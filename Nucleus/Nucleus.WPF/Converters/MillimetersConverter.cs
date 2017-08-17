using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Nucleus.WPF.Converters
{
    /// <summary>
    /// Value converter to convert between distances stored in m but represented in mm
    /// </summary>
    public class MillimetersConverter : IValueConverter
    {
        /// <summary>
        /// Convert from m to mm
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (double)value;
            return v * 1000;
        }

        /// <summary>
        /// Convert from mm to m
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = double.Parse(value.ToString());
            return v / 1000;
        }
    }
}
