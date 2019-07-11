using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// A utility structure to contain two related objects
    /// </summary>
    /// <typeparam name="TFirst">The type of the first object</typeparam>
    /// <typeparam name="TSecond">The type of the second object</typeparam>
    [Serializable]
    public struct Pair<TFirst, TSecond>
    {
        #region Fields

        /// <summary>
        /// The first field
        /// </summary>
        public TFirst First { get; }

        /// <summary>
        /// The second field
        /// </summary>
        public TSecond Second { get; }

        #endregion

        #region Constructors

        public Pair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }

        #endregion
    }

    /// <summary>
    /// Static helper functions relating to the Pair structure
    /// </summary>
    public static class Pair
    {
        /// <summary>
        /// Static helper function to create a Pair structure containing two objects.
        /// This can be used to rapidly instantiate new pairs without the necessity to
        /// explicitly specify the type parameters each time.
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Pair<TFirst, TSecond> Create<TFirst, TSecond>(TFirst first, TSecond second)
        {
            return new Pair<TFirst, TSecond>(first, second);
        }
    }
}
