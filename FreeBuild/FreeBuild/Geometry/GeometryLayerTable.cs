using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A table of geometry layer objects, keyed by name
    /// </summary>
    public class GeometryLayerTable : ObservableKeyedCollection<string, GeometryLayer>
    {
        protected override string GetKeyForItem(GeometryLayer item)
        {
            return item.Name;
        }
    }
}
