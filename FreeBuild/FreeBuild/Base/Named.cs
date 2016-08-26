using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Abstract base class for unique objects that can be named
    /// </summary>
    [Serializable]
    public abstract class Named : Unique, INamed
    {
        /// <summary>
        /// Private backing field for Name property
        /// </summary>
        private string _Name;

        /// <summary>
        /// The name of this unique object
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }
    }
}
