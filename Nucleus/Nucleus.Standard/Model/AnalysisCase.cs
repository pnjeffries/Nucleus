using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Model.Loading;

namespace Nucleus.Model
{
    /// <summary>
    /// A particular condition to be analysed.
    /// </summary>
    [Serializable]
    public class AnalysisCase : ResultsCase
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Task property
        /// </summary>
        private AnalysisTask _Task = null;

        /// <summary>
        /// The analysis task to which this analysis case belongs
        /// </summary>
        public AnalysisTask Task
        {
            get { return _Task; }
            set { ChangeProperty(ref _Task, value, "Task"); }
        }

        /// <summary>
        /// Private backing member variable for the LoadCase property
        /// </summary>
        private ILoadCase _LoadCase = null;

        /// <summary>
        /// The load case which is to be analysed
        /// </summary>
        public ILoadCase LoadCase
        {
            get { return _LoadCase; }
            set { ChangeProperty(ref _LoadCase, value, "LoadCase"); }
        }

        #endregion

    }
}
