using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A collection of target AI records, keyed by target
    /// </summary>
    [Serializable]
    public class TargetAIRecordCollection : ObservableKeyedCollection<Guid, TargetAIRecord>
    {
        protected override Guid GetKeyForItem(TargetAIRecord item)
        {
            return item.Target.GUID;
        }
    }
}
