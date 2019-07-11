using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Base;
using Nucleus.Model.Loading;

namespace Nucleus.Model
{
    /// <summary>
    /// A combination of load cases
    /// </summary>
    [Serializable]
    public class LoadCombinationCase : FactoredLoadCaseCollection, ILoadCase
    {
        /// <summary>
        /// Private backing field for GUID property
        /// </summary>
        [Copy(CopyBehaviour.DO_NOT_COPY)]
        private Guid _GUID = Guid.NewGuid();

        /// <summary>
        /// The GUID of this object, which can be used to uniquely identify it. 
        /// </summary>
        public Guid GUID { get { return _GUID; } }

        public bool Contains(Load load)
        {
            foreach (var fL in this)
            {
                if (fL.Contains(load)) return true;
            }
            return false;
        }
    }
}
