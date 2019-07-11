using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A collection of SpriteData
    /// </summary>
    [Serializable]
    public class SpriteDataCollection : ObservableKeyedCollection<string, SpriteData>
    {
        protected override string GetKeyForItem(SpriteData item)
        {
            return item.Name;
        }
    }
}
