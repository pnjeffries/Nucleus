using Nucleus.Extensions;
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
    /// Converter that takes a list of strings and converts them into a single flat string separated by newline
    /// characters.
    /// </summary>
    public class FlatStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IList<string> values = value as IList<string>;
            return values.ToFlatString(Environment.NewLine);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
