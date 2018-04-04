using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Extensible storage mechanism for adding tagged data to MapCells
    /// </summary>
    public class MapCellDataStore : DataStore<IMapCellDataComponent, MapCell>
    {
    }
}
