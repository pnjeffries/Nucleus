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
    /// Convert from FreeBuild objects to text strings and back again.
    /// </summary>
    public class TextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return value.ToString();
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO!
            throw new NotImplementedException();
        }
    }
}
