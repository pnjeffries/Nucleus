using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Freebuild.WPF.Converters
{
    /// <summary>
    /// A converter that always returns Visibility.Visible.  Can used in conjection with fallbackvalue when
    /// binding to collapse controls when a property is not present on the viewmodel
    /// </summary>
    public class AlwaysVisibleConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Visible;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
