using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// Interface for collections of NamedDataSets
    /// </summary>
    public interface INamedDataSetCollection : IEnumerable
    {
        /// <summary>
        /// Get the complete set of unique keys in all datasets in this collection
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllKeys();

        /// <summary>
        /// Get a NamedDataSet containing the keys and value ranges of all datasets
        /// in this collection
        /// </summary>
        /// <returns></returns>
        NamedDataSet GetValueRanges();

        /// <summary>
        /// Get a NamedDataSet containing the keys and end value ranges of all datasets
        /// in this collection
        /// </summary>
        /// <returns></returns>
        NamedDataSet GetEndValueRanges();
    }
}
