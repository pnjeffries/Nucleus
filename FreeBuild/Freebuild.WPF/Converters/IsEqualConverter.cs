using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FreeBuild.WPF.Converters
{
    /// <summary>
    /// Converter that returns whether or not the value is the same as the parameter
    /// as a boolean
    /// </summary>
    public class IsEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value.Equals(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value == true) return parameter;
            else throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter that returns whether or not the value is not the same as the parameter
    /// as a boolean
    /// </summary>
    public class IsNotEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!value.Equals(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
