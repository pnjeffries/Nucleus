using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Extensible storage mechanism for adding attached data to families
    /// </summary>
    [Serializable]
    public class FamilyDataStore : DataStore<IFamilyDataComponent>
    {
    }
}
