using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FreeBuild.WPF.Converters
{
    public class ModelTableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ModelObject && parameter is Type)
            {
                var mO = (ModelObject)value;
                var type = (Type)parameter;
                if (mO.Model != null) return mO.Model.GetTableFor(type);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
