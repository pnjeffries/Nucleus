using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Class to store angular intervals in a full 360 degree panorama
    /// </summary>
    [Serializable]
    public class AngleIntervals
    {
        #region Properties

        private SortedList<Angle, Angle> _Regions = new SortedList<Angle, Angle>();

        /// <summary>
        /// The angle intervals, stored in the form [Start],[End]
        /// </summary>
        public SortedList<Angle,Angle> Regions
        {
            get { return _Regions; }
        }

        private bool _wraps = false;

        /// <summary>
        /// Does a region wrap around and cross the 360/0 degrees border?
        /// </summary>
        public bool Wraps
        {
            get { return _wraps; }
        }

        /// <summary>
        /// Is the full circle filled?
        /// </summary>
        public bool IsFull
        {
            get
            {
                if (_Regions.Count == 1 && _Regions.ContainsKey(new Angle(0d)))
                {
                    if (_Regions[new Angle(0d)] >= Math.PI * 2 - 0.00001) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Have no angle regions yet been stored?
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _Regions.Count == 0;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AngleIntervals() { }

        /// <summary>
        /// Starting interval constructor
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public AngleIntervals(Angle start, Angle end)
        {
           addRegion(start, end);
        }

        #endregion

        #region Methods

       /// <summary>
       /// Add a new angle interval, automatically merging it with
       /// existing intervals as appropriate
       /// </summary>
       /// <param name="start"></param>
       /// <param name="end"></param>
        public void addRegion(Angle start, Angle end)
        {
            if (start > end)
            {
                //Split into two regions:
                addRegion(0, end);
                addRegion(start, 2 * Math.PI);
                _wraps = true;
            }
            else
            {
                for (int i = _Regions.Count -1; i >= 0; i--)
                { 
                    double rStart = _Regions.Keys[i];
                    double rEnd = _Regions.Values[i];
                    if (start <= rEnd && end >= rStart)
                    {
                        start = Math.Min(start, rStart);
                        end = Math.Max(end, rEnd);
                        _Regions.RemoveAt(i);
                    }
                }
                _Regions.Add(start, end);
            }
        }

        /// <summary>
        /// Is the specified angle within a stored interval?
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public bool isInsideRegion(Angle angle, double tolerance = 0.0001)
        {
            angle = angle.NormalizeTo2PI();
            foreach (var entry in _Regions)
            {
                double min = entry.Key;
                double max = entry.Value;
                if (!_wraps || min > 0.000001) min += tolerance;
                if (!_wraps || max < Math.PI * 2 - 0.000001) max -= tolerance;
                if (min <= angle && max >= angle)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Is the specified range of angles within a stored interval?
        /// </summary>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public CellObscurance isInsideRegion(Angle startAngle, Angle endAngle, double tolerance)
        {
            startAngle = startAngle.NormalizeTo2PI();
            endAngle = endAngle.NormalizeTo2PI();
            bool startInside = false;
            bool endInside = false;
            CellObscurance result = CellObscurance.Unobscured;
            foreach (var entry in _Regions)
            {
                double min = entry.Key;
                double max = entry.Value;
                if (!_wraps || min > 0.000001) min += tolerance;
                if (!_wraps || max < Math.PI * 2 - 0.000001) max -= tolerance;
                if (startAngle >= min && startAngle <= max) startInside = true;
                if (endAngle >= min && endAngle <= max) endInside = true;
                if (startInside && endInside) return null;
                else if (startInside || endInside)
                {
                    if (result == CellObscurance.Unobscured) result = new PartialCellObscurance(min, max);
                    else
                    {
                        //return null;
                        PartialCellObscurance pLC = (PartialCellObscurance)result;
                        if (_wraps && max >= Math.PI * 2 - 0.000001 && pLC.StartAngle < 0.00001)
                        {
                            pLC.StartAngle = min;
                        }
                    }
                    if (!_wraps) return result;
                }
            }
            return result;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create a new set of angle intervals containing all outside the specified
        /// cone
        /// </summary>
        /// <param name="coneAngle"></param>
        /// <param name="coneWidth"></param>
        /// <returns></returns>
        public static AngleIntervals InverseCone(Angle coneAngle, Angle coneWidth)
        {
            Angle coneStart = (coneAngle - coneWidth / 2).NormalizeTo2PI();
            Angle coneEnd = (coneAngle + coneWidth / 2).NormalizeTo2PI();
            var result = new AngleIntervals(coneEnd, coneStart);
            return result;
        }

        #endregion

    }
}
