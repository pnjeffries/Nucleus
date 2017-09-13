using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Nucleus.WPF.Converters
{
    public class SliderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (targetType.IsAssignableFrom(typeof(string))) return value.ToString();
            return System.Convert.ToDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is FrameworkElement)
            {
                var fE = (FrameworkElement)parameter;
                if (fE.DataContext != null && fE.DataContext is SliderFieldControl)
                {
                    var sFC = (SliderFieldControl)fE.DataContext;
                    targetType = sFC.ValueType;
                }
            }

            if (value == null) return 0;
            else if (targetType == typeof(int))
            {
                if (value is string) return int.Parse((string)value);
                else return System.Convert.ToInt32(value);
            }
            else if (value is string)
            {
                return double.Parse((string)value);
            }
            else return System.Convert.ToDouble(value); ;

        }
    }
}
