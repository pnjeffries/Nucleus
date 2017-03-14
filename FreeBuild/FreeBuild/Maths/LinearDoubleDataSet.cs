using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Maths
{
    public class LinearDoubleDataSet : LinearDataSet<double>
    {

        #region Constructors

        /// <summary>
        /// Initialise a new blank data set
        /// </summary>
        public LinearDoubleDataSet() : base() { }

        /// <summary>
        /// Initialise a new data set containing the specified values.
        /// Each value will be plotted against it's index in the list
        /// </summary>
        /// <param name="values"></param>
        public LinearDoubleDataSet(IList<double> values) : base(values) { }

        /// <summary>
        /// Initialise a new data set containing the specified initial pairing
        /// of values
        /// </summary>
        /// <param name="firstKey"></param>
        /// <param name="firstValue"></param>
        public LinearDoubleDataSet(double firstKey, double firstValue) : this()
        {
            Add(firstKey, firstValue);
        }

        /// <summary>
        /// Initialise a new data set containing the two specified initial pairings
        /// </summary>
        /// <param name="firstKey"></param>
        /// <param name="firstValue"></param>
        /// <param name="secondKey"></param>
        /// <param name="secondValue"></param>
        public LinearDoubleDataSet(double firstKey, double firstValue, double secondKey, double secondValue)
            : this(firstKey, firstValue)
        {
            Add(secondKey, secondValue);
        }

        #endregion]

        #region Methods

        protected override double Interpolate(int i0, int i1, double t)
        {
            return Values[i0].Interpolate(Values[i1], t);
        }

        /// <summary>
        /// Convert this dataset of doubles to an equivalent one of Intervals
        /// </summary>
        /// <returns></returns>
        public LinearIntervalDataSet ToIntervals()
        {
            return new Maths.LinearIntervalDataSet(this);
        }

        #endregion
    }
}
