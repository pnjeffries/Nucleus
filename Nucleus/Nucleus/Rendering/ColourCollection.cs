using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A collection of colours
    /// </summary>
    public class ColourCollection : ObservableKeyedCollection<int, Colour>
    {
        #region Methods

        protected override int GetKeyForItem(Colour item)
        {
            return item.GetHashCode();
        }

        #endregion
    }
}
