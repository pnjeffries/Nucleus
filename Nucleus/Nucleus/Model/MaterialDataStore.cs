using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A data storage mechanism for materials
    /// Stores data components keyed by type.
    /// </summary>
    public class MaterialDataStore : DataStore<IMaterialDataComponent, Material>
    {
        #region Constructor

        public MaterialDataStore(Material owner) : base(owner) { }

        public MaterialDataStore() { }

        #endregion
    }
}
