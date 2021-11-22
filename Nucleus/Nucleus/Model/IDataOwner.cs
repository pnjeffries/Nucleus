using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// An interface for objects which own data stores
    /// </summary>
    public interface IDataOwner
    {
        /// <summary>
        /// Notify this owner that a property of a data component has been changed.
        /// This may then be 'bubbled' upwards with a new event.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="propertyName"></param>
        void NotifyComponentPropertyChanged(object component, string propertyName);

        /// <summary>
        /// Get a flat list of all data components currently attached to this object
        /// </summary>
        /// <returns></returns>
        IList AllAttachedDataComponents();
    }

    /// <summary>
    /// Extension methods for the IDataOwner interface
    /// </summary>
    public static class IDataOwnerExtensions
    {
        /// <summary>
        /// Get a flat list containing all unique data components attached to objects in this collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IList AllAttachedDataComponents<T>(this IEnumerable<T> collection)
            where T : IDataOwner
        {
            var result = new HashSet<object>();
            foreach (T item in collection)
            {
                foreach (object component in item.AllAttachedDataComponents())
                {
                    if (!result.Contains(component))
                        result.Add(component);
                }
            }
            return result.ToList();
        }

        /// <summary>
        /// Get a flat list containing all unique types of data components attached to the
        /// objects in this collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<Type> AllAttachedDataTypes<T>(this IEnumerable<T> collection)
            where T : IDataOwner
        {
            var result = new HashSet<Type>();
            foreach (T item in collection)
            {
                foreach (object component in item.AllAttachedDataComponents())
                {
                    Type type = component.GetType();
                    if (!result.Contains(type))
                        result.Add(type);
                }
            }
            return result.ToList();
        }

        /// <summary>
        /// Does this data owner have any attached data of the given generic type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static bool HasAttachedDataType<T>(this IDataOwner owner)
        {
            var attached = owner.AllAttachedDataComponents();
            foreach (var data in attached)
            {
                if (data is T) return true;
            }
            return false;
        }

        /// <summary>
        /// Does any of the objects in this collection have an attached of the specified type
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TDataOwner"></typeparam>
        /// <param name="owners"></param>
        /// <returns></returns>
        public static bool ContainsAttachedData<TData, TDataOwner>(this IEnumerable<TDataOwner> owners)
            where TDataOwner : IDataOwner
        {
            foreach (var owner in owners)
            {
                if (owner.HasAttachedDataType<TData>()) return true;
            }
            return false;
        }
    }
}
