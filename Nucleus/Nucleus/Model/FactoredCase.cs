using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Abstract, generic base class for the combination of a case and a scalar factor
    /// </summary>
    /// <typeparam name="TCase"></typeparam>
    [Serializable]
    public abstract class FactoredCase<TCase> : Unique
        where TCase : ResultsCase
    {
        #region Properties

        /// <summary>
        /// Private backing field for Factor property
        /// </summary>
        private double _Factor = 1;

        /// <summary>
        /// The factor by which the case results should be magnified
        /// </summary>
        public double Factor
        {
            get { return _Factor; }
            set { _Factor = value;  NotifyPropertyChanged("Factor"); }
        }

        /// <summary>
        /// Private backing field for Case property
        /// </summary>
        private TCase _Case = null;

        /// <summary>w
        /// The case which is to be factored
        /// </summary>
        public TCase Case
        {
            get { return _Case; }
            set { _Case = value;  NotifyPropertyChanged("Case"); }
        }

        #endregion
    }

    /// <summary>
    /// A results case with a factor applied.
    /// </summary>
    [Serializable]
    public class FactoredCase : FactoredCase<ResultsCase>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new factored case with the specified case and factor.
        /// </summary>
        /// <param name="rCase">The results case</param>
        /// <param name="factor">Optional. The factor.  Default is 1.0</param>
        public FactoredCase(ResultsCase rCase, double factor = 1.0)
        {
            Case = rCase;
            Factor = factor;
        }

        #endregion
    }
}
