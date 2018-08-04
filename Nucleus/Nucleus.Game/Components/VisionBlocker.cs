using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An element data component that can be used to indicate that a cell blocks vision
    /// </summary>
    public class VisionBlocker : Unique, IElementDataComponent
    {
        /// <summary>
        /// Does this block LOS to the specified element
        /// </summary>
        /// <param name="toElement"></param>
        /// <returns></returns>
        public bool IsTransparent(Element toElement)
        {
            return false;
        }
    }
}
