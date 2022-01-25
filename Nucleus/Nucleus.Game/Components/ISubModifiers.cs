using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for objects which may have sub-objects which act to modify a value
    /// </summary>
    public interface ISubModifiers
    {
        /// <summary>
        /// Apply modifiers to a value from sub-objects posessing a particular interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="value"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        T ApplyModifiers<T, TInterface>(T value, Func<T, TInterface, T> function);
    }

    /// <summary>
    /// Extension methods relating to the ISubModifiersExtensions interface
    /// </summary>
    public static class ISubModifiersExtensions
    {
        /// <summary>
        /// Apply modifiers to a specified value based on the components of an appropriate interface stored in this data store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static T ApplyModifiers<T, TInterface> (this ElementDataStore data, T value, Func<T, TInterface, T> function)
        {
            foreach (var component in data)
            {
                if (component is TInterface iComp) value = function.Invoke(value, iComp);
                else if (component is ISubModifiers iSub)
                {
                    value = iSub.ApplyModifiers(value, function);
                }
            }
            return value;
        }
    }
}
