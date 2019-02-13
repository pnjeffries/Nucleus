using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Base class for components which have a data context
    /// </summary>
    public abstract class DataContextOwner : MonoBehaviour, IDataContext
    {
        /// <summary>
        /// The data context of the component
        /// </summary>
        public abstract object DataContext { get; set; }
    }

    /// <summary>
    /// Extension methods for and to deal with Binding components
    /// </summary>
    public static class DataContextOwnerExtensions
    {
        /// <summary>
        /// Set the data context of 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dataContext"></param>
        public static void SetDataContext(this GameObject obj, object dataContext)
        {
            // TODO: This may need some more work...
            var bindings = obj.GetComponents<IDataContext>();
            foreach (var binding in bindings) binding.DataContext = dataContext;

            // Children:
            var childBindings = obj.transform.GetComponentsInChildren<DataContextOwner>();
            foreach (var binding in childBindings) binding.DataContext = dataContext;
        }
    }
}
