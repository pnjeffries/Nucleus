using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Nucleus.WPF.Converters
{
    /// <summary>
    /// A converter to interploate a numeric value to a solid colour brush on a gradient
    /// </summary>
    public class ColourScaleConverter : IValueConverter
    {
        public ColourGradient Gradient { get; set; } = new ColourGradient(0, new Colour(255, 200, 200), 0.5, new Colour(255, 255, 200), 1.0, new Colour(200, 255, 200));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = System.Convert.ToDouble(value);
            return new SolidColorBrush(ToWPF.Convert(Gradient.ValueAt(val)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
