using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nucleus.Unity
{
    /// <summary>
    /// Extension methods for Unity GameObjects
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Get the bound data context for this object, if it has any attached
        /// data binding scripts.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object GetBindingDataContext(this GameObject obj)
        {
            var binding = obj.GetComponent<BindingBase>();
            if (binding != null) return binding.DataContext;
            else return null;
        }
    }
}
