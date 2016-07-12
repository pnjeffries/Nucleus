using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Interface for entities which are uniquely identifiable via a GUID
    /// </summary>
    public interface IUnique
    {
        /// <summary>
        /// The GUID of this object, which can be used to uniquely identify it 
        /// </summary>
        Guid GUID { get; }
    }
}
