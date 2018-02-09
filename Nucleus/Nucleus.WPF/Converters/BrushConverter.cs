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
    /// A converter class to convert Nucleus DisplayBrush objects to WPF form.
    /// The parameter represents a limiting alpha value.
    /// </summary>
    public class BrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte alphaLimit = 255;
            if (parameter != null) alphaLimit = (byte)(int)parameter;
            if (value is DisplayBrush)
            {
                return ToWPF.Convert((DisplayBrush)value, alphaLimit);
            }
            else if (value is Colour)
            {
                return new SolidColorBrush(ToWPF.Convert((Colour)value, alphaLimit));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
