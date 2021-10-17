using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Components
{
    /// <summary>
    /// A collection of resources
    /// </summary>
    [Serializable]
    public class ResourceCollection : ObservableKeyedCollection<ResourceType, Resource>
    {
        protected override ResourceType GetKeyForItem(Resource item)
        {
            return item.ResourceType;
        }

        /// <summary>
        /// Add the specified resource to the collection.
        /// If an entry of the given type already exists in the collection,
        /// the quantities will be combined instead.  If the maximum resource
        /// is capped the remainder will be returned.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Resource AddResourceQuantity(Resource resource)
        {
            if (Contains(resource.ResourceType))
            {
                var currentResource = this[resource.ResourceType];
                currentResource.Quantity += resource.Quantity;
                //TODO: Cap and return remainder
                return null;
            }
            else
            {
                Add(resource);
                return null;
            }
        }

        /// <summary>
        /// Find the quantity of the specified resourceType held in this collection
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public int ResourceQuantity(ResourceType resourceType)
        {
            if (Contains(resourceType)) return this[resourceType].Quantity;
            else return 0;
        }

        /// <summary>
        /// Does this collection contain at least the specified amount of the
        /// specified resource?
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool HasResourceQuantity(Resource resource)
        {
            return ResourceQuantity(resource.ResourceType) >= resource.Quantity;
        }

        /// <summary>
        /// Reduce the quantity of the specified resource in this collection by the specified
        /// amount.
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="amount"></param>
        /// <returns>True if sufficient quantity of resource existed in the collection to meet the full amount</returns>
        public bool ReduceResourceQuantity(ResourceType resourceType, int amount)
        {
            if (!Contains(resourceType)) return false;
            var resource = this[resourceType];
            if (resource.Quantity < amount)
            {
                resource.Quantity = 0;
                return false;
            }
            resource.Quantity -= amount;
            return true;
        }

        /// <summary>
        /// Drop a specified amount of resource from that held in this collection.
        /// If the full amount cannot be dropped as much as is held will be dropped instead.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="dropper"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public GameElement DropResource(Resource resource, Element dropper, EffectContext context)
        {
            int resourceQuantity = ResourceQuantity(resource.ResourceType);
            if (resourceQuantity <= 0) return null;
            resourceQuantity = Math.Min(resourceQuantity, resource.Quantity);
            ReduceResourceQuantity(resource.ResourceType, resourceQuantity);
            var resourceDrop = new Resource(resource.ResourceType, resourceQuantity);
            var pickup = resourceDrop.CreatePickup();
            if (context.State is MapState) //TODO: Work for other states?
            {
                MapData mD = dropper.GetData<MapData>();
                if (mD.MapCell != null)
                {
                    mD.MapCell.PlaceInCell(pickup);
                }
                context.State.Elements.Add(pickup);
            }
            return pickup;
        }

        /// <summary>
        /// Drop all resources held in this collection
        /// </summary>
        /// <param name="dropper"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void DropAll(Element dropper, EffectContext context)
        {
            foreach (var resource in this)
            {
                DropResource(resource, dropper, context);
            }
        }
    }
}
