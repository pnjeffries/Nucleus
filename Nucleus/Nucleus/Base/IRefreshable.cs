using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Interface for objects which may be refreshed
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Refresh this object
        /// </summary>
        /// <returns></returns>
        bool Refresh();
    }

    /// <summary>
    /// Extension methods for the IRefreshable interface
    /// </summary>
    public static class IRefreshableExtensions
    {
        /// <summary>
        /// Refresh all of the objects in this collection
        /// </summary>
        /// <typeparam name="TRefreshable"></typeparam>
        /// <param name="refreshables"></param>
        public static void RefreshAll<TRefreshable>(this IEnumerable<TRefreshable> refreshables)
            where TRefreshable : IRefreshable
        {
            foreach (TRefreshable refreshable in refreshables)
                refreshable.Refresh();
        }
    }
}
