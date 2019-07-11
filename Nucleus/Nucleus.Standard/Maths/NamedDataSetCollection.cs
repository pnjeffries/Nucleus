using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// A collection of named data sets
    /// </summary>
    [Serializable]
    public class NamedDataSetCollection<T> : ObservableKeyedCollection<string, T>, INamedDataSetCollection
        where T : NamedDataSet
    {
        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }

        /// <summary>
        /// Get a combined list of all the key names in the data sets in this collection
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllKeys()
        {
            var result = new HashSet<string>();

            foreach (var dataSet in this)
            {
                foreach (string key in dataSet.Data.Keys)
                {
                    if (!result.Contains(key)) result.Add(key);
                }
            }

            return result.ToList();
        }

        /// <summary>
        /// Get a NamedDataSet containing the keys and value ranges of all datasets
        /// in this collection
        /// </summary>
        /// <returns></returns>
        public NamedDataSet GetValueRanges()
        {
            NamedDataSet result = new NamedDataSet();

            foreach (var dataSet in this)
            {
                foreach (string key in dataSet.Data.Keys)
                {
                    if (!result.Data.ContainsKey(key))
                    {
                        result.Data.Add(key, dataSet.Data[key]);
                    }
                    else
                    {
                        result.Data[key] = result.Data[key].Union(dataSet.Data[key]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get a NamedDataSet containing the keys and value ranges of all datasets
        /// in this collection
        /// </summary>
        /// <returns></returns>
        public NamedDataSet GetEndValueRanges()
        {
            NamedDataSet result = new NamedDataSet();

            foreach (var dataSet in this)
            {
                foreach (string key in dataSet.Data.Keys)
                {
                    if (!result.Data.ContainsKey(key))
                    {
                        result.Data.Add(key, new Interval(dataSet.Data[key].End));
                    }
                    else
                    {
                        result.Data[key] = result.Data[key].Include(dataSet.Data[key].End);
                    }
                }
            }

            return result;
        }
    }

    /// <summary>
    /// A collection of named data sets
    /// </summary>
    [Serializable]
    public class NamedDataSetCollection : NamedDataSetCollection<NamedDataSet>
    { }
}
