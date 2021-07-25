using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Artitecture
{
    /// <summary>
    /// A table of objects and their weightings.  A roll can be made
    /// on this table to randomly decide on an item from the table
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class WeightedTable<T> : Dictionary<T, double>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WeightedTable() { }

        /// <summary>
        /// Create a table of items with equal weighting of 1 for each.
        /// </summary>
        /// <param name="items"></param>
        public WeightedTable(params T[] items)
        {
            foreach (T item in items)
            {
                Add(item, 1);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculate the total of all weighting values in this table
        /// </summary>
        /// <returns></returns>
        public double SumWeights()
        {
            double result = 0;
            foreach (var kvp in this) result += kvp.Value;
            return result;
        }

        /// <summary>
        /// Roll on this table and randomly return a result with a probability
        /// based on the weighting values
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public T Roll(Random rng)
        {
            double roll = rng.NextDouble() * SumWeights();
            double sum = 0;
            foreach (var kvp in this)
            {
                sum += kvp.Value;
                if (sum >= roll) return kvp.Key;
            }
            return default(T);
        }

        #endregion
    }
}
