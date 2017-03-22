using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
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
    }
}
