using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DDTree
{
    /// <summary>
    /// Class to store the result of a ray hit test
    /// </summary>
    [Serializable]
    public class RayHit<T>
    {
        #region Properties

        /// <summary>
        /// Private backing field for Item property
        /// </summary>
        private T _Item;

        /// <summary>
        /// The item hit by the ray
        /// </summary>
        public T Item { get { return _Item; } }

        /// <summary>
        /// Private backing field for Parameter property
        /// </summary>
        private double _Parameter;

        /// <summary>
        /// The ray intersection parameter - the multiplication factor
        /// to be applied to the ray direction vector from the ray origin
        /// vector in order to find the intersection point
        /// </summary>
        public double Parameter { get { return _Parameter; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a RayHit result
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parameter"></param>
        public RayHit(T item, double parameter)
        {
            _Item = item;
            _Parameter = parameter;
        }

        #endregion
    }
}
