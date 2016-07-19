using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Abstract base class implementing the IUnique interface
    /// </summary>
    [Serializable]
    public abstract class Unique : NotifyPropertyChangedBase, IUnique
    {
        /// <summary>
        /// Internal backing member for GUID property
        /// </summary>
        private Guid _GUID = new Guid();

        /// <summary>
        /// The GUID of this object, which can be used to uniquely identify it. 
        /// </summary>
        public Guid GUID { get { return _GUID; } } 
    }
}
