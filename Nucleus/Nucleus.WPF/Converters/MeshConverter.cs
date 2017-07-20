using Nucleus.Geometry;
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
    /// Convert between Nucleus Mesh geometry and WPF MeshGeometry3D
    /// </summary>
    public class MeshConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Mesh)
            {
                return FBtoWPF.Convert((Mesh)value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
