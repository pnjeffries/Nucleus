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
        private bool _Opaque = true;

        /// <summary>
        /// Is this vision blocker currently blocking vision?
        /// </summary>
        public bool Opaque
        {
            get { return _Opaque; }
            set { ChangeProperty(ref _Opaque, value); }
        }


        /// <summary>
        /// Does this block LOS to the specified element
        /// </summary>
        /// <param name="toElement"></param>
        /// <returns></returns>
        public bool IsTransparent(Element toElement)
        {
            return !Opaque;
        }
    }
}
