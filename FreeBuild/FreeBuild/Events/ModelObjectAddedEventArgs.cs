using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Events
{
    /// <summary>
    /// Event arguments used when an object has been added to the model
    /// </summary>
    public class ModelObjectAddedEventArgs : EventArgs
    {
        /// <summary>
        /// The object added to the model
        /// </summary>
        public Unique Added { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="added"></param>
        public ModelObjectAddedEventArgs(Unique added)
        {
            Added = added;
        }
    }
}
