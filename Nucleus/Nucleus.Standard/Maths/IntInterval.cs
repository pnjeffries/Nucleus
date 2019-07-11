using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Maths
{
    /// <summary>
    /// A numeric interval between two integers
    /// </summary>
    [Serializable]
    public struct IntInterval
    {
        #region Fields

        /// <summary>
        /// The minimum, or start, value of this interval.
        /// </summary>
        public readonly int Start;

        /// <summary>
        /// The maximum, or end, value of this interval.
        /// </summary>
        public readonly int End;

        #endregion

        #region Properties

        /// <summary>
        /// Get the size, or length, of this interval
        /// </summary>
        public int Size { get { return End - Start; } }

        /// <summary>
        /// Does this interval represent a range of values which increases from start to finish
        /// </summary>
        public bool IsIncreasing
        {
            get { return Start < End; }
        }

        /// <summary>
        /// Does this interval represent a range of
        /// </summary>
        public bool IsDecreasing
        {
            get { return Start > End; }
        }

        /// <summary>
        /// Is this a singularity?  i.e. are the maximum and minimum values the same?
        /// </summary>
        /// <returns></returns>
        public bool IsSingularity
        {
            get { return Start == End; }
        }

        /// <summary>
        /// Get the minimum value encompassed by this interval
        /// (This will be the lesser of the Start and End fields)
        /// </summary>
        public int Min { get { return Math.Min(Start, End); } }

        /// <summary>
        /// Get the maximum value encompassed by this interval
        /// (This will be the greater of the Start and End fields)
        /// </summary>
        public int Max { get { return Math.Max(Start, End); } }

        /// <summary>
        /// Get the signed value of the greatest absolute value in this interval.
        /// This will return whichever of Max and Min has the largest (unsigned) magnitude.
        /// </summary>
        public int AbsMax { get { return Math.Abs(End) > Math.Abs(Start) ? End : Start; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor creating a singularity interval with
        /// maximum and minimum set to the specified value
        /// </summary>
        /// <param name="value"></param>
        public IntInterval(int value)
        {
            Start = value;
            End = value;
        }

        /// <summary>
        /// Constructor creating an interval from maximum and minimum values.
        /// The minimum value should be lower than or equal to the maximum, or else
        /// this interval will not be valid.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public IntInterval(int start, int end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Constructor creating an interval surrounding the specified set of 
        /// values.  The maximum and minimum will be automatically determined.
        /// </summary>
        /// <param name="list"></param>
        public IntInterval(int val1, int val2, int val3)
        {
            if (val1 < val2)
            {
                Start = Math.Min(val1, val3);
                End = Math.Max(val2, val3);
            }
            else
            {
                Start = Math.Min(val2, val3);
                End = Math.Max(val1, val3);
            }
        }

        /// <summary>
        /// Constructor creating an interval surrounding the specified set of 
        /// values.  The maximum and minimum will be automatically determined
        /// </summary>
        /// <param name="list"></param>
        public IntInterval(params int[] list)
        {
            int min = list[0];
            int max = list[1];
            for (int i = 0; i < list.Length; i++)
            {
                int val = list[i];
                if (val < min) min = val;
                if (val > max) max = val;
            }
            Start = min;
            End = max;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generate a random integer within this interval
        /// </summary>
        /// <param name="rng">The random number generator to use</param>
        /// <returns></returns>
        public int Random(Random rng)
        {
            return rng.Next(Start, End);
        }

        /// <summary>
        /// Create a new interval with the same start value as this one
        /// but with the new specified end value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IntInterval WithEnd(int end)
        {
            return new IntInterval(Start, end);
        }

        public override string ToString()
        {
            if (IsSingularity) return End.ToString();
            else return "[" + Start.ToString() + ";" + End.ToString() + "]";
        }

        #endregion
    }
}
