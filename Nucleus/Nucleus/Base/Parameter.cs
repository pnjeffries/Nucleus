using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Abstract base class for named input or output parameters 
    /// associated with a design option.
    /// </summary>
    [Serializable]
    public abstract class Parameter : Unique
    {
        #region Properties

        /// <summary>
        /// Protected backing field for the Name property
        /// </summary>
        protected string _Name;

        /// <summary>
        /// Get the name of this parameter
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// Private backing field for Group property
        /// </summary>
        private ParameterGroup _Group = null;

        /// <summary>
        /// Get or set the group (if any) to which this parameter belongs.
        /// </summary>
        public ParameterGroup Group
        {
            get { return _Group; }
            set { ChangeProperty(ref _Group, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        protected Parameter() { }

        /// <summary>
        /// Name constructor
        /// </summary>
        /// <param name="name"></param>
        public Parameter(string name)
        {
            _Name = name;
        }

        #endregion
    }

    /// <summary>
    /// Generic class for named input or output parameters 
    /// associated with a design option
    /// </summary>
    /// <typeparam name="T">The type of the parameter</typeparam>
    [Serializable]
    public class Parameter<T> : Parameter
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Value property
        /// </summary>
        private T _Value;

        /// <summary>
        /// The parameter value
        /// </summary>
        public T Value
        {
            get { return _Value; }
            set { ChangeProperty(ref _Value, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new parameter with the default value and no name.
        /// As all parameters require an immutable name, this should not be
        /// used unless you know what you're doing.
        /// </summary>
        internal Parameter() { }

        /// <summary>
        /// Creates a new parameter with the specified name.
        /// </summary>
        /// <param name="name"></param>
        public Parameter(string name) : base(name) { }

        /// <summary>
        /// Creates a new parameter with the specified name and
        /// initial value
        /// </summary>
        /// <param name="name">The name of this parameter</param>
        /// <param name="value">The value of this parameter</param>
        public Parameter(string name, T value) : base(name)
        {
            _Value = value;
        }

        /// <summary>
        /// Creates a new parameter with the specified name, group
        /// and initial value.
        /// </summary>
        /// <param name="name">The name of this parameter</param>
        /// <param name="group">The group within which this parameter
        /// should be displayed</param>
        /// <param name="value">The initial value of the parameter</param>
        public Parameter(string name, ParameterGroup group, T value) 
            : this(name, value)
        {
            Group = group;
        }

        #endregion
    }
}
