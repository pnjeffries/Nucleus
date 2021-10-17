using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Pickup item which represents a quantity of a certain resource
    /// </summary>
    [Serializable]
    public class ResourcePickUp : IElementDataComponent
    {
        private Resource _Resource;

        /// <summary>
        /// The resource represented by this pickup
        /// </summary>
        public Resource Resource
        {
            get { return _Resource; }
        }

        public ResourcePickUp(Resource resource)
        {
            _Resource = resource;
        }
    }
}
