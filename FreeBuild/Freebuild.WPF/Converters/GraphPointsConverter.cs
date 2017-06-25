using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Nucleus.WPF.Converters
{
    /// <summary>
    /// Convert a LinearIntervalGraph into a collection of points suitable for binding to a Polyline Points property.
    /// If the parameter is set to boolean true then the keys will be used as the y coordinate, else it will be used as
    /// the x coordinate.
    /// </summary>
    public class GraphPointsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flipXY = false;
            if (parameter != null && parameter is bool) flipXY = (bool)parameter;
            PointCollection result = new PointCollection();
            if (value is LinearIntervalDataSet)
            {
                var graph = (LinearIntervalDataSet)value;
                foreach (KeyValuePair<double, Interval> kvp in graph)
                {
                    System.Windows.Point pt;
                    if (!flipXY) pt = new System.Windows.Point(kvp.Key, kvp.Value.End);
                    else pt = new System.Windows.Point(kvp.Value.End, kvp.Key);
                    result.Add(pt);
                }
                if (graph.IsEnvelope)
                {
                    foreach (KeyValuePair<double, Interval> kvp in graph.Reverse())
                    {
                        System.Windows.Point pt;
                        if (!flipXY) pt = new System.Windows.Point(kvp.Key, kvp.Value.Start);
                        else pt = new System.Windows.Point(kvp.Value.Start, kvp.Key);
                        result.Add(pt);
                    }
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
