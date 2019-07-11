using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{

    /// <summary>
    /// A generic, general-purpose pairing of objects that raises notify property changed events
    /// when modified.
    /// </summary>
    /// <typeparam name="TFirst">The first type of object in the pair</typeparam>
    /// <typeparam name="TSecond">The second type of object in the pair</typeparam>
    [Serializable]
    public class ObservablePair<TFirst, TSecond> : NotifyPropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// Private backing field for the First property
        /// </summary>
        private TFirst _First;

        /// <summary>
        /// The first item in the pair
        /// </summary>
        public TFirst First
        {
            get { return _First; }
            set { _First = value; NotifyPropertyChanged("First"); }
        }

        public TFirst Key
        {
            get { return _First; }
        }

        /// <summary>
        /// Private backing field for the Second property
        /// </summary>
        private TSecond _Second;

        /// <summary>
        /// The second item in the pair
        /// </summary>
        public TSecond Second
        {
            get { return _Second; }
            set { _Second = value; NotifyPropertyChanged("Second"); }
        }

        public TSecond Value
        {
            get { return _Second; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// Initialises a new, empty pair
        /// </summary>
        public ObservablePair() { }

        /// <summary>
        /// First, Second constructor.
        /// Initialises a new pair containing the specififed values
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public ObservablePair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }

        #endregion
    }

    /// <summary>
    /// Extension methods for ObservablePairs and containers which hold them
    /// </summary>
    public static class ObservablePairExtensions
    {
        /// <summary>
        /// Find a pair by its first value
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <param name="collection"></param>
        /// <param name="first">The item to search for</param>
        /// <returns></returns>
        public static ObservablePair<TFirst,TSecond> FindByFirst<TFirst,TSecond>
            (this IEnumerable<ObservablePair<TFirst,TSecond>> collection, TFirst first)
        {
            foreach (ObservablePair<TFirst,TSecond> pair in collection)
            {
                if (pair.First.Equals(first)) return pair;
            }
            return null;
        }

        /// <summary>
        /// Find a pair by its second value
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <param name="collection"></param>
        /// <param name="second">The item to search for</param>
        /// <returns></returns>
        public static ObservablePair<TFirst, TSecond> FindBySecond<TFirst, TSecond>
            (this IEnumerable<ObservablePair<TFirst, TSecond>> collection, TSecond second)
        {
            foreach (ObservablePair<TFirst, TSecond> pair in collection)
            {
                if (pair.Second.Equals(second)) return pair;
            }
            return null;
        }

        /// <summary>
        /// Add a new pair to this collection.
        /// Shortcut method that automatically generates an ObservablePair from the specififed objects and
        /// adds it to this collection
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <param name="collection"></param>
        /// <param name="first">The first item in the pair</param>
        /// <param name="second">The second item in the pair</param>
        public static void Add<TFirst, TSecond>(this ICollection<ObservablePair<TFirst,TSecond>> collection,
            TFirst first, TSecond second)
        {
            collection.Add(new ObservablePair<TFirst, TSecond>(first, second));
        }

        /// <summary>
        /// Insert a new pair to this list at the specified position.
        /// Shortcut method that automatically generates an ObservablePair from the specififed objects and
        /// adds it to this collection.
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <param name="collection"></param>
        /// <param name="index">The index to insert the new pair at</param>
        /// <param name="first">The first item in the pair</param>
        /// <param name="second">The second item in the pair</param>
        public static void Insert<TFirst, TSecond>(this IList<ObservablePair<TFirst, TSecond>> collection,
            int index, TFirst first, TSecond second)
        {
            collection.Insert(index, new ObservablePair<TFirst, TSecond>(first, second));
        }

        /// <summary>
        /// Count the number of pairs in this set where the first item in the pair is equal to the value
        /// specified.
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <param name="collection"></param>
        /// <param name="equals">The value to check the first item in each pair for equality to</param>
        /// <returns></returns>
        public static int CountWhereFirst<TFirst, TSecond>(this IEnumerable<ObservablePair<TFirst, TSecond>> collection, TFirst equals)
        {
            int count = 0;
            foreach (ObservablePair<TFirst, TSecond> pair in collection)
                if (pair.First.Equals(equals)) count++;
            return count;
        }

        /// <summary>
        /// Count the number of pairs in this set where the second item in the pair is equal to the value
        /// specified.
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <param name="collection"></param>
        /// <param name="equals">The value to check the second item in each pair for equality to</param>
        /// <returns></returns>
        public static int CountWhereSecond<TFirst, TSecond>(this IEnumerable<ObservablePair<TFirst, TSecond>> collection, TSecond equals)
        {
            int count = 0;
            foreach (ObservablePair<TFirst, TSecond> pair in collection)
                if (pair.Second.Equals(equals)) count++;
            return count;
        }
    }
}
