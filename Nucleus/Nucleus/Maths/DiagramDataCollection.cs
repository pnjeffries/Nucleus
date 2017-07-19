using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// A collection of diagram data
    /// </summary>
    public class DiagramDataCollection : ObservableKeyedCollection<string, DiagramData>
    {
        protected override string GetKeyForItem(DiagramData item)
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
    }
}
