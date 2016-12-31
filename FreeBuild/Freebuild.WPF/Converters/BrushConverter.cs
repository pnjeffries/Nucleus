using FreeBuild.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FreeBuild.WPF.Converters
{
    /// <summary>
    /// A converter class to convert FreeBuild DisplayBrush objects to WPF form
    /// </summary>
    public class BrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DisplayBrush)
            {
                return FBtoWPF.Convert((DisplayBrush)value);
            }
            else if (value is Colour)
            {
                return new SolidColorBrush(FBtoWPF.Convert((Colour)value));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
