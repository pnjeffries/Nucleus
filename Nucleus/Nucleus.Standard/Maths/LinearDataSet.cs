using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Extensions;

namespace Nucleus.Maths
{
    /// <summary>
    /// An interpolatable data set of values along a single axis.
    /// </summary>
    [Serializable]
    public abstract class LinearDataSet<TValue> : SortedList<double, TValue>
    {
        #region Properties

        /// <summary>
        /// Get the range of key values currently stored in this data set
        /// </summary>
        public Interval KeyRange
        {
            get
            {
                Interval result = Interval.Unset;
                foreach (double key in Keys)
                {
                    if (!result.IsValid) result = new Interval(key);
                    else result = result.Include(key);
                }
                return result;
            }
        }

        /// <summary>
        /// Is this data set uniform?  i.e. are all of the assigned values equal?
        /// </summary>
        public bool IsUniform
        {
            get
            {
                if (Count > 0)
                {
                    TValue value = Values[0];
                    for (int i = 1; i < Count; i++)
                        if (!value.Equals(Values[i])) return false;
                }
                return true;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new empty LinearGraph
        /// </summary>
        public LinearDataSet() : base() { }

        /// <summary>
        /// Initialise a new LinearGraph from a list of values.
        /// The values will be plotted against their indices.
        /// </summary>
        /// <param name="values"></param>
        public LinearDataSet(IList<TValue> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                Add(i, values[i]);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a sudden jump between two values.
        /// As two values cannot share the same key this is implemented by adding
        /// value0 at t, and value1 at the next greatest valid double parameter.
        /// </summary>
        /// <param name="t">The parameter at which to add the value jump</param>
        /// <param name="value0">The value before the jump</param>
        /// <param name="value1">The value after the jump</param>
        public void AddJump(double t, TValue value0, TValue value1)
        {
            Add(t, value0);
            Add(t.NextValidValue(), value1);
        }

        /// <summary>
        /// Get (or interpolate) the value at the specified parameter
        /// </summary>
        /// <param name="t">The parameter at which to retrieve or derive a value</param>
        /// <param name="extrapolate">If set to true, values outside the currently stored key range
        /// will be extrapolated from the tangency of the end values.  Otherwise the default value
        /// will be returned instead.</param>
        /// <returns></returns>
        public TValue ValueAt(double t, bool extrapolate = false)
        {
            if (!extrapolate && (t < Keys[0] || t > Keys.Last())) return default(TValue);
            if (ContainsKey(t)) return this[t]; // Shortcut: exact datapoint
            if (Count == 1) return Values[0]; // Shortcut: single value
            else if (Count == 2) return Interpolate(0, 1, t); // Shortcut: only two values

            // Find the appropriate two datapoints to interpolate between:
            int i0 = 0;
            int i1 = 1;
            for (int i = 0; i < Count; i++)
            {
                double key = Keys[i];
                if (key > t)
                {
                    if (i == 0)
                    {
                        i0 = 0;
                        i1 = 1;
                    }
                    else
                    {
                        i0 = i - 1;
                        i1 = i;
                    }
                    break;
                }
                else if (i == Count - 1)
                {
                    // Have got to last entry without exceeding t
                    i0 = i - 1;
                    i1 = i;
                }
            }

            return Interpolate(i0, i1, t);
        }

        /// <summary>
        /// Use straight-line interpolation to derive a value at the parameter t
        /// based on the entries in this list at indices i0 and i1.
        /// </summary>
        /// <param name="i0"></param>
        /// <param name="i1"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        protected TValue Interpolate(int i0, int i1, double t)
        {
            TValue v0 = Values[i0];
            TValue v1 = Values[i1];
            double k0 = Keys[i0];
            double k1 = Keys[i1];
            return Interpolate(v0,v1,(t - k0) / (k1 - k0));
        }

        /// <summary>
        /// Overridable interpolation function to provide
        /// efficient interpolation for the data set type
        /// </summary>
        /// <param name="i0"></param>
        /// <param name="i1"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        protected virtual TValue Interpolate(TValue v0, TValue v1, double factor)
        {
            return Interpolation.Linear.Interpolate(v0, v1, factor);
        }



        #endregion
    }
}
