using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// Element data component which controls how the element should be described in scripted log entries
    /// </summary>
    [Serializable]
    public class LogDescription : Unique, IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backer for SubjectPrefix
        /// </summary>
        private string _SubjectPrefix = null;

        /// <summary>
        /// The prefix to be applied before subject names
        /// </summary>
        public string SubjectPrefix
        {
            get { return _SubjectPrefix; }
            set { _SubjectPrefix = value; }
        }

        /// <summary>
        /// Private backer for SubjectPrefix
        /// </summary>
        private string _SubjectSuffix = null;

        /// <summary>
        /// The prefix to be applied before subject names
        /// </summary>
        public string SubjectSuffix
        {
            get { return _SubjectSuffix; }
            set { _SubjectSuffix = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogDescription() { }

        public LogDescription(string prefix, string suffix)
        {
            SubjectPrefix = prefix;
            SubjectSuffix = suffix;
        }

        #endregion
    }
}
