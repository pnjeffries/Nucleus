using Nucleus.Units;
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
    /// Note: Sub-classes of Parameter should override the FastDuplicate
    /// function to  return a parameter of the requisite type.
    /// </summary>
    [Serializable]
    public abstract class Parameter : Unique, IFastDuplicatable, INamed
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

        /// <summary>
        /// Private backing field for the Units property
        /// </summary>
        private MeasurementUnit _Units = null;

        /// <summary>
        /// Get or set the units of measurement of the parameter
        /// </summary>
        public MeasurementUnit Units
        {
            get { return _Units; }
            set { _Units = value; }
        }

        /// <summary>
        /// Private backing field for the Description property
        /// </summary>
        private string _Description = null;

        /// <summary>
        /// Get or set the description of this parameter.
        /// May be used as a tooltip.
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        /// <summary>
        /// Private backing field for the Metadata property
        /// </summary>
        private IDictionary<string, string> _Metadata = null;

        /// <summary>
        /// Get or set the metadata of this parameter.
        /// </summary>
        public IDictionary<string, string> Metadata
        {
            get { return _Metadata; }
            set { _Metadata = value; }
        }

        /// <summary>
        /// Private backing field for the Visible property
        /// </summary>
        private bool _Visible = true;

        /// <summary>
        /// Should the parameter be displayed?
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set { ChangeProperty(ref _Visible, value); }
        }

        /// <summary>
        /// Get the type of the value held by this parameter
        /// </summary>
        public abstract Type ValueType { get; }

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

        #region Methods

        /// <summary>
        /// Set the value of the parameter to the specified new value
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns>True if the value was successfully set, false if not.</returns>
        public abstract bool SetValue(object newValue);

        /// <summary>
        /// Get the value of the parameter.
        /// </summary>
        /// <returns></returns>
        public abstract object GetValue();

        /// <summary>
        /// Set the value of this parameter by copying the value of
        /// the specified other parameter (if it is of the appropriate type).
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if the value is successfully set, false if not.</returns>
        public abstract bool SetValueFrom(Parameter other);

        /// <summary>
        /// FastDuplicate implementation
        /// </summary>
        /// <returns></returns>
        IFastDuplicatable IFastDuplicatable.FastDuplicate_Internal()
        {
            return FastDuplicate_Implementation();
        }

        /// <summary>
        /// Protected abstract function to implement FastDuplicate
        /// </summary>
        /// <returns></returns>
        protected abstract IFastDuplicatable FastDuplicate_Implementation();

        /// <summary>
        /// Manually notify this parameter that its value has been changed in some
        /// way other than direct replacement (for example, modification of a geometry
        /// object) and which would not be automatically detected.
        /// Will raise a PropertyChanged event for the Value property.
        /// </summary>
        public abstract void NotifyValueChanged();

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
        [CollectionCopy(CopyBehaviour.DUPLICATE, CopyBehaviour.MAP_OR_DUPLICATE)]
        private T _Value;

        /// <summary>
        /// The parameter value
        /// </summary>
        public virtual T Value
        {
            get { return _Value; }
            set { ChangeProperty(ref _Value, value); }
        }

        /// <summary>
        /// Get the type of the value held by this parameter
        /// </summary>
        public override Type ValueType
        {
            get { return typeof(T); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new parameter with the default value and no name.
        /// As all parameters require an immutable name, this should not be
        /// used unless you know what you're doing.
        /// </summary>
        protected Parameter() { }

        /// <summary>
        /// Creates a new parameter as a copy of another one
        /// </summary>
        /// <param name="other"></param>
        public Parameter(Parameter<T> other) : this()
        {
            _Name = other.Name;
            if (other.Value is IFastDuplicatable) _Value = (T)((IFastDuplicatable)other.Value).FastDuplicate();
            else if (other.Value is IDuplicatable) _Value = (T)((IDuplicatable)other.Value).Duplicate(CopyBehaviour.MAP_OR_DUPLICATE);
            else _Value = other.Value;
            Group = other.Group;
            Units = other.Units;
            Description = other.Description;
            Visible = other.Visible;
        }

        /// <summary>
        /// Creates a new parameter with the specified name.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        public Parameter(string name) : base(name) { }

        /// <summary>
        /// Creates a new parameter with the specified name.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="units">The units in which the parameter is expressed</param>
        public Parameter(string name, MeasurementUnit units) : base(name)
        {
            Units = units;
        }

        /// <summary>
        /// Creates a new parameter with the specified name and
        /// initial value
        /// </summary>
        /// <param name="name">The name of this parameter</param>
        /// <param name="value">The value of this parameter</param>
        /// <param name="units">The units in which the parameter is expressed</param>
        public Parameter(string name, T value, MeasurementUnit units = null) : this(name, units)
        {
            _Value = value;
        }

        /// <summary>
        /// Creates a new parameter with the specified name, group
        /// and initial value.
        /// </summary>
        /// <param name="name">The name of this parameter.</param>
        /// <param name="group">The group within which this parameter
        /// should be displayed.</param>
        /// <param name="value">The initial value of the parameter.</param>
        /// <param name="units">The units in which the parameter is expressed.</param>
        public Parameter(string name, ParameterGroup group, T value, MeasurementUnit units = null) 
            : this(name, value, units)
        {
            Group = group;
        }

        /// <summary>
        /// Creates a new parameter with the specified name, group,
        /// initial value and description.
        /// </summary>
        /// <param name="name">The name of this parameter.</param>
        /// <param name="group">The group within which this parameter
        /// should be displayed.</param>
        /// <param name="value">The initial value of the parameter.</param>
        /// <param name="description">The description of the parameter's function.</param>
        /// <param name="units">The units in which the parameter is expressed.</param>
        public Parameter(string name, ParameterGroup group, T value, string description, MeasurementUnit units = null)
            : this(name, value, units)
        {
            Group = group;
            Description = description;
        }

        /// <summary>
        /// Creates a new parameter with the specified name, group,
        /// initial value and metadata.
        /// </summary>
        /// <param name="name">The name of this parameter.</param>
        /// <param name="group">The group within which this parameter
        /// should be displayed.</param>
        /// <param name="value">The initial value of the parameter.</param>
        /// <param name="metadata">A dictionary containing any extra data related to this parameter.</param>
        /// <param name="units">The units in which the parameter is expressed.</param>
        public Parameter(string name, ParameterGroup group, T value, IDictionary<string, string> metadata, MeasurementUnit units = null)
            : this(name, value, units)
        {
            Group = group;
            Metadata = metadata;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the value of the parameter to the specified new value.
        /// If the types do not match but newValue implements the IConvertible 
        /// interface then automatic conversion will be attempted.
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns>True if the value was successfully set, false if not.</returns>
        public override bool SetValue(object newValue)
        {
            if (newValue is T)
            {
                Value = (T)newValue;
                return true;
            }
            if (newValue is IConvertible)
            {
                try
                {
                    Value = (T)Convert.ChangeType(newValue, typeof(T));
                }
                catch
                {
                }
            }

            return false;
        }

        /// <summary>
        /// Set the value of this parameter by copying the value of
        /// the specified other parameter (if it is of the appropriate type).
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if the value is successfully set, false if not.</returns>
        public override bool SetValueFrom(Parameter other)
        {
            return SetValue(other.GetValue());
        }

        /// <summary>
        /// Get the value of the parameter.
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return Value;
        }

        /// <summary>
        /// FastDuplicate Implementation
        /// </summary>
        /// <returns></returns>
        protected override IFastDuplicatable FastDuplicate_Implementation()
        {
            return new Parameter<T>(this);
        }

        /// <summary>
        /// Manually notify this parameter that its value has been changed in some
        /// way other than direct replacement (for example, modification of a geometry
        /// object) and which would not be automatically detected.
        /// Will raise a PropertyChanged event for the Value property.
        /// </summary>
        public override void NotifyValueChanged()
        {
            NotifyPropertyChanged(nameof(Value));
        }

        #endregion
    }
}
