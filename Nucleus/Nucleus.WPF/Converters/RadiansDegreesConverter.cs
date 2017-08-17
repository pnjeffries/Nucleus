using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Nucleus.WPF.Converters
{
    public class RadiansDegreesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Angle angle = (Angle)value;
            if (angle.IsUndefined) return null;
            else if (angle.IsMulti) return "Multi";
            else return angle.Degrees;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Angle.FromDegrees(double.Parse(value.ToString()));
        }
    }
}
