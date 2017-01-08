using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.UI
{
    /// <summary>
    /// Interface for objects which represent assignable user interactions
    /// </summary>
    public interface IUserInteraction<TParameters>
    {
        /// <summary>
        /// Execute this interaction
        /// </summary>
        /// <param name="sender">The object raising this interaction</param>
        /// <param name="parameters">The interaction parameters</param>
        /// <returns></returns>
        bool Execute(object sender, TParameters parameters);
    }
}
