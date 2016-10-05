using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A data storage mechanism for elements.
    /// Stores element data keyed by the type of data.
    /// </summary>
    [Serializable]
    public class ElementDataStore : DataStore<IElementData>
    {
    }
}
