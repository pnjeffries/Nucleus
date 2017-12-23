using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    [Serializable]
    public class LinearDoubleDataSet : LinearDataSet<double>
    {
        #region Properties

        /*
        /// <summary>
        /// Is this data-set flat?  i.e. do all datapoints along
        /// the x-axis have the same y-value?
        /// </summary>
        public bool IsFlat
        {
            get
            {
                if (Count > 0)
                {
                    double value = Values[0];
                    for (int i = 1; i < Count; i++)
                    {
                        double kv = Values[i];
                        if (kv != value) return false; //TODO: Use tolerance?
                    }
                }
                return true;
            }
        }*/

        #endregion

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
        /// Initialise a new data set containing the specified constant value between
        /// 0 and 1.0.
        /// </summary>
        /// <param name="constantValue"></param>
        public LinearDoubleDataSet(double constantValue) : this(0, constantValue, 1, constantValue) { }

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

        /// <summary>
        /// Calculate the signed area under the line, over the specified key range
        /// </summary>
        /// <param name="xRange">The range over which the area is to be calculated</param>
        /// <param name="centroid"></param>
        /// <returns></returns>
        public double CalculateAreaUnder(Interval xRange, out Vector centroid)
        {
            double result = 0;
            centroid = new Vector();

            for (int i = 0; i < Count - 1; i++)
            {
                double x0 = Keys[i];

                if (x0 < xRange.End) //Is segment start < end of range?
                {
                    double x1 = Keys[i + 1];

                    if (x1 > xRange.Start) //Is segment end > start of range?
                    {
                        // Segment falls within specified range
                        double y0, y1;
                        if (xRange.Start > x0) //The range starts within this segment
                        {
                            x0 = xRange.Start;
                            y0 = ValueAt(x0);
                        }
                        else y0 = Values[i];


                        if (xRange.End < x1) //The range ends within this segment
                        {
                            x1 = xRange.End;
                            y1 = ValueAt(x1);
                        }
                        else y1 = Values[i + 1];

                        result += MathsHelper.AreaUnder(x0, y0, x1, y1, ref centroid);
                    }     
                }
                else //Shortcut to end:
                    i = Count;
            }
            centroid /= result;
            return result;
        }

        #endregion
    }
}
