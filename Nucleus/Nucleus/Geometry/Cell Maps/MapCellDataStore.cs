using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    public class MapCellDataStore<TMapCell> : DataStore<IMapCellDataComponent, TMapCell>
        where TMapCell : IDataOwner
    {
    }
}
