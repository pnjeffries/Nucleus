using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    public class ObservablePairCollection<TKey, TValue> : ObservableKeyedCollection<TKey, ObservablePair<TKey, TValue>>
    {
        protected override TKey GetKeyForItem(ObservablePair<TKey, TValue> item)
        {
            return item.First;
        }

        public void Add(TKey key, TValue value)
        {
            Add(new ObservablePair<TKey, TValue>(key, value));
        }
    }
}
