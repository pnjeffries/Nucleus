using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Nucleus.Rendering;
using System.Collections;

namespace Nucleus.WPF.Converters
{
    /// <summary>
    /// Converter which returns a brush for barchart bar segments based on a string key.
    /// This is implemented as a MultiValueConverter where the first value is the key and
    /// the second is the dictionary
    /// </summary>
    public class BarKeyedBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2)
            {
                string key = values[0].ToString();
                IDictionary dict = values[1] as IDictionary;
                if (dict != null)
                {
                    if (!dict.Contains(key))
                    {
                        // Auto-assign a colour
                        if (dict.Count < Colour.RambollPalette.Length)
                        {
                            dict.Add(key, Colour.RambollPalette[dict.Count]);
                        }
                    }

                        if (dict.Contains(key))
                    {
                        object val = dict[key];
                        if (val is Brush) return val;
                        else if (val is Color) return new SolidColorBrush((Color)val);
                        else if (val is Colour) return new SolidColorBrush(ToWPF.Convert((Colour)val));
                        else if (val is DisplayBrush) return ToWPF.Convert((DisplayBrush)val);
                    }

                }
            }
            return Brushes.Red;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
