using System;

namespace Nucleus.Units
{
    /// <summary>
    /// Class which may be used to represent a unit of measurement
    /// </summary>
    [Serializable]
    public class MeasurementUnit
    {
        #region Constants

        /// <summary>
        /// Get the set of default length/distance units
        /// </summary>
        public static readonly MeasurementUnit[] LengthUnits = new MeasurementUnit[]
        {
            Meters,
            Millimeters,
            Centimeters,
            Feet,
            Inches
        };

        /// <summary>
        /// Percent, a number or ratio expressed as a fraction of 100
        /// </summary>
        public static readonly MeasurementUnit Percent = new MeasurementUnit("percent", "%", 0.01);

        /// <summary>
        /// Meters, the SI unit of measurement for length
        /// </summary>
        public static readonly MeasurementUnit Meters = new MeasurementUnit("meters","m", 1, DimensionType.Distance);

        /// <summary>
        /// Meters squared, the SI unit of measurement for area
        /// </summary>
        public static readonly MeasurementUnit MetersSquared = new MeasurementUnit("meters squared", "m²", 1, DimensionType.Area);

        /// <summary>
        /// Meters cubed, the SI unit of measurement for volume
        /// </summary>
        public static readonly MeasurementUnit MetersCubed = new MeasurementUnit("meters cubed", "m³", 1, DimensionType.Volume);

        /// <summary>
        /// Millimeters, a length measurement
        /// </summary>
        public static readonly MeasurementUnit Millimeters = new MeasurementUnit("millimeters", "mm", 0.001, DimensionType.Distance);

        /// <summary>
        /// Centimeters, a length measurement
        /// </summary>
        public static readonly MeasurementUnit Centimeters = new MeasurementUnit("centimeters", "cm", 0.01, DimensionType.Distance);

        /// <summary>
        /// Feet, a length measurements
        /// </summary>
        public static readonly MeasurementUnit Feet = new MeasurementUnit("feet", "ft", 0.3048, DimensionType.Distance);

        /// <summary>
        /// Feet Squared, an area measurement
        /// </summary>
        public static readonly MeasurementUnit FeetSquared = new MeasurementUnit("feet squared", "sqft", 0.092903, DimensionType.Area);

        /// <summary>
        /// Inches, a length measurement
        /// </summary>
        public static readonly MeasurementUnit Inches = new MeasurementUnit("inches", "in", 0.0254, DimensionType.Distance);

        /// <summary>
        /// Radians, the SI unit of angle measurement
        /// </summary>
        public static readonly MeasurementUnit Radians = new MeasurementUnit("radians", "rad", 1, DimensionType.Angle);

        /// <summary>
        /// Degrees, an angle measurement
        /// </summary>
        public static readonly MeasurementUnit Degrees = new MeasurementUnit("degrees", "°", Math.PI / 180, DimensionType.Angle);

        /// <summary>
        /// Newtons, the SI unit of force measurement
        /// </summary>
        public static readonly MeasurementUnit Newtons = new MeasurementUnit("newtons", "N", 1, DimensionType.Force);

        /// <summary>
        /// Kilonewtons, a force measurement
        /// </summary>
        public static readonly MeasurementUnit KiloNewtons = new MeasurementUnit("kilonewtons", "kN", 1000, DimensionType.Force);

        /// <summary>
        /// Newton meters, the SI unit of moment
        /// </summary>
        public static readonly MeasurementUnit NewtonMeters = new MeasurementUnit("newton meters", "Nm", 1, DimensionType.Moment);

        /// <summary>
        /// Kilograms, the SI unit of measurement for mass
        /// </summary>
        public static readonly MeasurementUnit Kilograms = new MeasurementUnit("kilograms", "kg", 1, DimensionType.Mass);

        /// <summary>
        /// Kelvin, the SI unit of measurement for temperature
        /// </summary>
        public static readonly MeasurementUnit Kelvin = new MeasurementUnit("Kelvin", "K", 1, DimensionType.Temperature);

        /// <summary>
        /// Celsius, a unit of measurement for temperature
        /// </summary>
        public static readonly MeasurementUnit Celsius = new MeasurementUnit("degrees Celsius", "°C", 1, DimensionType.Temperature);

        /// <summary>
        /// Seconds, the SI unit of measurement for time
        /// </summary>
        public static readonly MeasurementUnit Seconds = new MeasurementUnit("seconds", "s", 1, DimensionType.Time);

        /// <summary>
        /// Minutes, a time measurement
        /// </summary>
        public static readonly MeasurementUnit Minutes = new MeasurementUnit("minutes", "min", 60, DimensionType.Time);

        /// <summary>
        /// Hours, a time measurement
        /// </summary>
        public static readonly MeasurementUnit Hours = new MeasurementUnit("hours", "hr", 3600, DimensionType.Time);

        /// <summary>
        /// British Pounds, a currency
        /// </summary>
        public static readonly MeasurementUnit BritishPounds = new MeasurementUnit("British Pounds", "GBP", 1, DimensionType.Currency);

        /// <summary>
        /// United States Dollars, a currency
        /// </summary>
        public static readonly MeasurementUnit USDollars = new MeasurementUnit("United States Dollars", "USD", 1, DimensionType.Currency);

        /// <summary>
        /// Euros, a currency
        /// </summary>
        public static readonly MeasurementUnit Euros = new MeasurementUnit("Euros", "€", 1, DimensionType.Currency);

        /// <summary>
        /// Danish Kroner, a currency
        /// </summary>
        public static readonly MeasurementUnit DanishKrone = new MeasurementUnit("Danish Kroner", "kr", 1, DimensionType.Currency);

        /// <summary>
        /// Norwegian Kroner, a currency
        /// </summary>
        public static readonly MeasurementUnit NorwegianKrone = new MeasurementUnit("Norwegian Kroner", "kr", 1, DimensionType.Currency);

        /// <summary>
        /// Gets an array of all predefined currency units
        /// </summary>
        public static MeasurementUnit[] CurrencyUnits
        {
            get
            {
                return new MeasurementUnit[]
                {
                    USDollars,
                    BritishPounds,
                    Euros
                };
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Private backing field for the Name property
        /// </summary>
        private string _Name;

        /// <summary>
        /// Get the name of this unit of measurement
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// Private backing field for the 
        /// </summary>
        private string _Symbol;

        /// <summary>
        /// Get the symbol used to display this unit
        /// </summary>
        public string Symbol
        {
            get { return _Symbol; }
        }

        /// <summary>
        /// Private backing field for SIFactor property
        /// </summary>
        private double _SIFactor = 1.0;

        /// <summary>
        /// The number of SI equivalent units this unit is
        /// equivalent to.  Multiply a value in these units
        /// by this factor to get the SI equivalent value.
        /// </summary>
        public double SIFactor
        {
            get { return _SIFactor; }
        }

        private DimensionType _Dimension = DimensionType.Dimensionless;

        /// <summary>
        /// The dimension that the unit applies to
        /// </summary>
        public DimensionType Dimension
        {
            get { return _Dimension; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new unit of measurement
        /// </summary>
        /// <param name="name">The name of the unit.  This should be in plural form
        /// and without capitalisation, save in cases where the capitalisation should
        /// always be applied.</param>
        /// <param name="symbol">The symbol of the unit</param>
        /// <param name="siFactor">The multiple of SI units to which this unit is equivalent</param>
        public MeasurementUnit(string name, string symbol, double siFactor = 1.0, DimensionType dimension = DimensionType.Dimensionless)
        {
            _Name = name;
            _Symbol = symbol;
            _SIFactor = siFactor;
            _Dimension = dimension;
        }

        #endregion

        #region Static Methods

        public static bool TryParseLength(string text, out double value, out MeasurementUnit unit)
        {
            int length = 0;
            foreach (var unit in LengthUnits)
            {
                text.Normalize
            }
        }

        #endregion
    }
}
