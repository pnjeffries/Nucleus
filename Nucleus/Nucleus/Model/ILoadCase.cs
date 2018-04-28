using Nucleus.Base;
using Nucleus.Model.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Interface for objects which represent a loading case
    /// </summary>
    public interface ILoadCase : IUnique
    {
        /// <summary>
        /// Does this case contain the specified load?
        /// </summary>
        /// <param name="load"></param>
        /// <returns></returns>
        bool Contains(Load load);

    }
}
