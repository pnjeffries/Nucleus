using Nucleus.Model;
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
    /// Convert a ModelObject to and from its NumericID
    /// </summary>
    public class NumericIDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ModelObject)
            {
                return ((ModelObject)value).NumericID;
            }
            else return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string && parameter is Model.Model)
            {
                Model.Model model = (Model.Model)parameter;
                var result = model.GetByDescription(targetType, (string)value);
                return result;
            }
            throw new Exception("Conversion failed.  Model parameter may not be set.");
        }
    }
}
