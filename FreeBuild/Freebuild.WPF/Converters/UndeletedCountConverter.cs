using FreeBuild.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FreeBuild.WPF.Converters
{
    /// <summary>
    /// A MultiValueConverter used to determine the number of items in a collection which
    /// are not marked as deleted
    /// </summary>
    public class UndeletedCountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is ICollection)
            {
                int count = 0;
                ICollection col = (ICollection)values[0];
                foreach (object obj in col)
                {
                    if (obj is IDeletable)
                    {
                        if (!((IDeletable)obj).IsDeleted) count++;
                    }
                    else count++;
                }
                return count;
            }
            return values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
