using FreeBuild.Geometry;
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
    /// <summary>
    /// Convert from FreeBuild objects to text strings and back again.
    /// </summary>
    public class TextConverter : IValueConverter
    {
        #region Properties

        private Model.Model _Model = null;

        /// <summary>
        /// The model with respect to which object numbers and names are referenced
        /// </summary>
        public Model.Model Model
        {
            get { return _Model; }
        }

        #endregion

        #region Converter

        public TextConverter(Model.Model model)
        {
            _Model = model;
        }

        #endregion

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is Angle)
                {
                    return ((Angle)value).Degrees.ToString() + "°";
                }
                else return value.ToString();
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            if (targetType == typeof(string)) return text;
            if (targetType == typeof(double)) return double.Parse(text);
            else if (targetType == typeof(int)) return int.Parse(text);
            else if (targetType == typeof(bool)) return bool.Parse(text);
            else if (targetType == typeof(Vector)) return new Vector(text);
            else if (targetType == typeof(Angle)) return new Angle(text);
            else if (targetType == typeof(long)) return long.Parse(text);
            else if (typeof(Family).IsAssignableFrom(targetType)) return Model?.Families?.FindByName(text);
            else if (typeof(CoordinateSystemReference).IsAssignableFrom(targetType)) return Model?.CoordinateSystems?.GetByKeyword(text);
            // TODO: More types!  Convert units!
            else return text;
        }

        #endregion
    }
}
