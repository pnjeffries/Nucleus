using Nucleus.Base;
using Nucleus.Game.Components;
using Nucleus.Logs;
using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A specific quantity of a specific type of collectable resource
    /// </summary>
    [Serializable]
    public class Resource : NotifyPropertyChangedBase, IInventoryContainer
    {
        private ResourceType _ResourceType;

        /// <summary>
        /// Get the type of resource this represents
        /// </summary>
        public ResourceType ResourceType
        {
            get { return _ResourceType; }
        }

        private int _Quantity = 1;

        /// <summary>
        /// The quantity of the resource
        /// </summary>
        public int Quantity
        {
            get { return _Quantity; }
            set { ChangeProperty(ref _Quantity, value); }
        }

        public Resource(ResourceType resourceType, int quantity)
        {
            _ResourceType = resourceType;
            _Quantity = quantity;
        }

        public Resource(Resource resource) : this(resource.ResourceType, resource.Quantity)
        {

        }

        /// <summary>
        /// Create a game element to represent a pickup for this resource
        /// </summary>
        /// <returns></returns>
        public GameElement CreatePickup()
        {
            var result = new ResourceElement(ResourceType.Name);
            result.SetData(
                new PickUp(),
                new ResourcePickUp(new Resource(this)),
                new LogDescription("few <color=#FF00FD>", "</color>"),
                new ASCIIStyle(ResourceType.Symbol));
            return result;
        }
    }
}
