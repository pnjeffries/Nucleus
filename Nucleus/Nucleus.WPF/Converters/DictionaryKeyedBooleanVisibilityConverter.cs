using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Nucleus.WPF.Converters
{
    public class DictionaryKeyedBooleanVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length >= 2)
            {
                var dict = values[0] as IDictionary;
                object key = values[1];
                if (dict != null && key != null)
                {
                    if (values.Length == 2)
                    {
                        bool value = (bool)dict[key];
                        if (value) return Visibility.Visible;
                    }
                    else
                    {
                        var dict2 = dict[key] as IDictionary;
                        object key2 = values[2];
                        bool value = (bool)dict2[key2];
                        if (value) return Visibility.Visible;
                    }
                }
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
   }
}
