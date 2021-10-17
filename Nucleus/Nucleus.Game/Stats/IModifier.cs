using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interfaces for components, status effects and items which modify stats
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModifier<T>
        where T:Stat
    {
        /// <summary>
        /// Modify the specified stat
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        T Modify(T stat);
    }
}
