using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model.Loading
{
    /// <summary>
    /// A force load that is applied along linear elements
    /// </summary>
    [Serializable]
    public class LinearElementLoad : ForceLoad<LinearElementSet, LinearElement>
    {
        #region Properties

        private LinearDoubleDataSet _Distribution;

        /// <summary>
        /// The distribution of the applied load along each element,
        /// stored as a multiplication factor for the base value mapped against a
        /// normalised length along the element.
        /// By default, this distribution contains a single value of 1.0, representing
        /// a uniformly distributed load with a scaling factor of 1.
        /// </summary>
        /// <remarks>Currently not possible to have sudden stops... fix?</remarks>
        public LinearDoubleDataSet Distribution
        {
            get
            {
                if (_Distribution == null) _Distribution = new LinearDoubleDataSet(1.0);
                return _Distribution;
            }
            set { ChangeProperty(ref _Distribution, value, "Distribution"); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set this load to have a uniform distribution along the element
        /// </summary>
        /// <param name="scalingFactor"></param>
        public void SetUniform(double scalingFactor = 1.0)
        {
            Distribution = new Maths.LinearDoubleDataSet(0, scalingFactor);
        }

        #endregion
    }
}
