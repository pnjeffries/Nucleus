using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Wraps a generic Comparison delegate in an IComparer to make it easy
    /// to use a lambda expression for methods that take an IComparer or IComparer
    /// </summary>
    /// <typeparam name="T">The type to be compared.</typeparam>
    [Serializable]
    public class ComparisonComparer<T> : IComparer<T>, IComparer
    {
        /// <summary>
        /// The comparison being wrapped
        /// </summary>
        private readonly Comparison<T> _Comparison;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comparison">The comparison being wrapped</param>
        public ComparisonComparer(Comparison<T> comparison)
        {
            _Comparison = comparison;
        }

        /// <summary>
        /// Compare two objects
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            return _Comparison(x, y);
        }

        /// <summary>
        /// Compare two objects
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public int Compare(object o1, object o2)
        {
            return _Comparison((T)o1, (T)o2);
        }
    }
    
}
