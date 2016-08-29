using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Freebuild.WPF.Converters
{
    /// <summary>
    /// Converter to insert spaces to convert camelCase to camel Case
    /// </summary>
    public class AutoSpaceCamelCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string text = value.ToString();
                return text.AutoSpace();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    
}
