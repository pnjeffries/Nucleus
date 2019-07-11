using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Abstract base class for Options.  Represents one potential
    /// design configuration which is under consideration.
    /// </summary>
    [Serializable]
    public abstract class Option : Named
    {
        #region Constructors

        public Option() : base() { }

        public Option(string name) : base(name) { }

        #endregion
    }

    /// <summary>
    /// Abstract generic base class for Options.  Represents one potential
    /// design configuration which is under consideration.
    /// </summary>
    /// <typeparam name="TDesign">The type of data that represents 
    /// the design within this option</typeparam>
    [Serializable]
    public abstract class Option<TDesign> : Option
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Design property
        /// </summary>
        private TDesign _Design;

        /// <summary>
        /// The design of this option
        /// </summary>
        public TDesign Design
        {
            get { return _Design; }
            set
            {
                _Design = value;
                NotifyPropertyChanged("Design");
            }
        }

        #endregion

        #region Constructors

        public Option() : base() { }

        public Option(string name) : base(name) { }

        #endregion

    }
}
