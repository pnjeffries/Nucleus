using Nucleus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    /// <summary>
    /// Immutable struct used to store a calculation variable with additional metadata.
    /// </summary>
    public struct CalcVariable<TDimension>
        where TDimension : IDimension
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Value property
        /// </summary>
        private double _Value;

        /// <summary>
        /// The value of the variable
        /// </summary>
        public double Value
        {
            get { return _Value; }
        }

        /// <summary>
        /// Private backing member variable for the Name property
        /// </summary>
        private string _Name;

        /// <summary>
        /// The name of the variable
        /// </summary>
        public string Name
        {
            get { return _Name; }

        }

        /// <summary>
        /// Private backing member variable for the Dimension property
        /// </summary>
        private DimensionType _Dimension;

        /// <summary>
        /// The dimensional type of the variable
        /// </summary>
        public DimensionType Dimension
        {
            get { return _Dimension; }

        }


        #endregion

        #region Constructor

        /// <summary>
        /// Initialise a calculation variable with a value but no name or dimension
        /// </summary>
        /// <param name="value"></param>
        public CalcVariable(double value)
        {
            _Value = value;
            _Name = null;
            _Dimension = DimensionType.Dimensionless;
        }

        /// <summary>
        /// Initialise a dimensionless calculation variable with the specified name
        /// and value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public CalcVariable(string name, double value)
        {
            _Name = name;
            _Value = value;
            _Dimension = DimensionType.Dimensionless;
        }

        /// <summary>
        /// Initialise a calculation variable with the specified name, value and dimension type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dimension"></param>
        public CalcVariable(string name, double value, DimensionType dimension)
        {
            _Name = name;
            _Value = value;
            _Dimension = dimension;
        }

        /// <summary>
        /// Initialise an unnamed calculation variable with the specified value and dimension type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dimension"></param>
        public CalcVariable(double value, DimensionType dimension)
        {
            _Name = null;
            _Value = value;
            _Dimension = dimension;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implicit conversion from CalcVariable to double
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator double(CalcVariable<TDimension> value) => value.Value;

        /// <summary>
        /// Implicit conversion from double to CalcVariable
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator CalcVariable<TDimension>(double value) => new CalcVariable<TDimension>(value);

        #endregion
    }
}
