using Nucleus.Geometry;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model.Loading
{
    /// <summary>
    /// A point load applied to a linear element
    /// </summary>
    [Serializable]
    public class LinearElementPointLoad : ForceLoad<LinearElementSet, LinearElement>
    {
        #region Properties

        /// <summary>
        /// Private backing field for point load position.
        /// </summary>
        private double _Position = 0.5;

        /// <summary>
        /// The position of the point load along the element.
        /// By default (when Relative = true), this is a relative position 
        /// along the element where
        /// 0 = the element start and 1 = the element end.
        /// Otherwise (when Relative = false) it is an absolute distance
        /// value from the element start.
        /// </summary>
        [AutoUI(650)]
        public double Position
        {
            get { return _Position; }
            set { ChangeProperty(ref _Position, value, "Position"); }
        }

        /// <summary>
        /// Private backing field for 'Relative' property
        /// </summary>
        private bool _Relative = true;

        /// <summary>
        /// Is the position of this point load defined relatively?
        /// If true (default) the Position property refers to a 
        /// relative position along the element where
        /// 0 = the element start and 1 = the element end.
        /// Otherwise (when Relative = false) it is an absolute distance
        /// value from the element start.
        /// </summary>
        [AutoUI(655)]
        public bool Relative
        {
            get { return _Relative; }
            set { ChangeProperty(ref _Relative, value, "Relative"); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the position of this point load to to represent the
        /// closest point on a curve to a point
        /// </summary>
        /// <param name="onCurve"></param>
        /// <param name="closestTo"></param>
        public bool SetPosition(Curve onCurve, Vector closestTo)
        {
            if (onCurve != null)
            {
                Position = onCurve.ClosestParameter(closestTo);
                Relative = true;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Set the position of this point load to represent the
        /// closest point on the specified linear element to
        /// a point
        /// </summary>
        /// <param name="element"></param>
        /// <param name="closestTo"></param>
        public bool SetPosition(LinearElement element, Vector closestTo)
        {
            return SetPosition(element.Geometry, closestTo);
        }

        #endregion
    }
}
