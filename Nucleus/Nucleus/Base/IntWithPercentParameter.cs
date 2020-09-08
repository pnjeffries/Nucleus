using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Units;

namespace Nucleus.Base
{
    /// <summary>
    /// A specialised IntParameter which also stores an equivalent
    /// percentage for display purposes
    /// </summary>
    [Serializable]
    public class IntWithPercentParameter : IntParameter
    {
        #region Properties

        /// <summary>
        /// The parameter value
        /// </summary>
        public override int Value 
        {
            get { return base.Value; } 
            set
            {
                base.Value = value;
                NotifyPropertyChanged(nameof(ValueDisplayString));
            }
        }

        /// <summary>
        /// Private backing member variable for the Percentage property
        /// </summary>
        private double _Percentage = 0;

               /// <summary>
        /// The percentage value, out of 100
        /// </summary>
        public double Percentage
        {
            get { return _Percentage; }
            set
            {
                ChangeProperty(ref _Percentage, value);
                NotifyPropertyChanged(nameof(ValueDisplayString));
            }
        }

        /// <summary>
        /// Get the string to be displayed which combines the integer value with 
        /// its accompaying percentage
        /// </summary>
        public string ValueDisplayString
        {
            get { return string.Format("{0} ({1:0.#}%)", Value, Percentage); }
        }

        #endregion

        #region Constructors

        public IntWithPercentParameter(IntWithPercentParameter other) : base(other) { }

        public IntWithPercentParameter(string name, MeasurementUnit units = null) : base(name, units)
        {
        }

        public IntWithPercentParameter(string name, int value, MeasurementUnit units = null) : base(name, value, units)
        {
        }

        public IntWithPercentParameter(string name, ParameterGroup group, int value, double percentage = 0, MeasurementUnit units = null) : base(name, group, value, units)
        {
            _Percentage = percentage;
        }

        public IntWithPercentParameter(string name, ParameterGroup group, int value, string description, MeasurementUnit units = null) : base(name, group, value, description, units)
        {
        }

        internal IntWithPercentParameter()
        {
        }

        #endregion

        #region Methods

        protected override IFastDuplicatable FastDuplicate_Implementation()
        {
            return new IntWithPercentParameter(this);
        }

        public override bool SetValueFrom(Parameter other)
        {
            if (other is IntWithPercentParameter pcOther)
            {
                Percentage = pcOther.Percentage;
            }
            return base.SetValueFrom(other);
        }

        #endregion
    }
}
