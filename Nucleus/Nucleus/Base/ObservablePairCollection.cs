using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    public class ObservablePairCollection<TKey, TValue> : ObservableKeyedCollection<TKey, ObservablePair<TKey, TValue>>, IDictionary
    {
        object IDictionary.this[object key]
        {
            get
            {
                if (!(key is TKey)) throw new KeyNotFoundException();
                ObservablePair<TKey, TValue> value = this[(TKey)key] as ObservablePair<TKey, TValue>;
                if (value != null) return value.Second;
                else return null;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        protected override TKey GetKeyForItem(ObservablePair<TKey, TValue> item)
        {
            return item.First;
        }

        public void Add(TKey key, TValue value)
        {
            Add(new ObservablePair<TKey, TValue>(key, value));
        }

        void IDictionary.Add(object key, object value)
        {
            Add((TKey)key, (TValue)value);
        }
    }
}
