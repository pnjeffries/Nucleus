using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// A collection of objects implementing the IUnique interface
    /// Each item must be unique within this collection - duplicate entries are not allowed.
    /// </summary>
    /// <typeparam name="TItem">The type of uniquely identifiable item</typeparam>
    public class UniquesCollection<TItem> : ObservableKeyedCollection<Guid, TItem> where TItem : IUnique
    {
        protected override Guid GetKeyForItem(TItem item)
        {
            return item.GUID;
        }
    }
}
