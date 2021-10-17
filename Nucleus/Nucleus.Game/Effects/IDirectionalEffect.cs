using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Interface for effects which act in a particular direction
    /// </summary>
    public interface IDirectionalEffect : IEffect
    {
        /// <summary>
        /// The direction the effect acts in
        /// </summary>
        Vector Direction { get; set; }
    }
}
