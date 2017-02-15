using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Maths
{
    /// <summary>
    /// A linear graph that plots numeric intervals along a single axis
    /// </summary>
    public class LinearIntervalGraph : LinearGraph<Interval>
    {
        #region Methods

        protected override Interval Interpolate(int i0, int i1, double t)
        {
            return Values[i0].Interpolate(Values[i1], Keys[i0], Keys[i1], t);
        }

        /// <summary>
        /// Envelope this plot with another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public LinearIntervalGraph Envelope(LinearIntervalGraph other)
        {
            var result = new LinearIntervalGraph();
            int iA = 0;
            int iB = 0;
            double tA = Keys[0];
            double tB = other.Keys[0];
            while (iA < Count || iB < Count)
            {
                // Find the next lowest key in either list and envelope the values
                // at that position
                if (tA < tB)
                {
                    result.Add(tA, Values[iA].Union(other.ValueAt(tA)));
                    iA++;
                    if (iA < Count) tA = Keys[iA];
                    else tA = double.MaxValue;
                }
                else if (tB < tA)
                {
                    result.Add(tB, ValueAt(tB).Union(other.Values[iB]));
                    iB++;
                    if (iB < Count) tB = other.Keys[iB];
                    else tB = double.MaxValue;
                }
                else //tA == tB
                {
                    result.Add(tA, Values[iA].Union(other.Values[iB]));
                    iA++;
                    iB++;
                    if (iA < Count) tA = Keys[iA];
                    else tA = double.MaxValue;
                    if (iB < Count) tB = Keys[iB];
                    else tB = double.MaxValue;
                }
                //Hmmm... may fail if either contains a key at infinity...?
            }
            return result;

        }

        #endregion

    }
}
