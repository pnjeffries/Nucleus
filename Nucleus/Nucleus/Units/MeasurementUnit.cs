using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Percent, a number or ratio expressed as a fraction of 100
        /// </summary>
        public static readonly MeasurementUnit Percent = new MeasurementUnit("percent", "%", 0.01);

        /// <summary>
        /// Meters, the SI unit of measurement for length
        /// </summary>
        public static readonly MeasurementUnit Meters = new MeasurementUnit("meters","m");

        /// <summary>
        /// Meters squared, the SI unit of measurement for area
        /// </summary>
        public static readonly MeasurementUnit MetersSquared = new MeasurementUnit("meters squared", "m²");

        /// <summary>
        /// Meters cubed, the SI unit of measurement for volume
        /// </summary>
        public static readonly MeasurementUnit MetersCubed = new MeasurementUnit("meters cubed", "m³");

        /// <summary>
        /// Millimeters, a length measurement
        /// </summary>
        public static readonly MeasurementUnit Millimeters = new MeasurementUnit("millimeters", "mm", 0.001);

        /// <summary>
        /// Centimeters, a length measurement
        /// </summary>
        public static readonly MeasurementUnit Centimeters = new MeasurementUnit("centimeters", "cm", 0.01);

        /// <summary>
        /// Radians, the SI unit of angle measurement
        /// </summary>
        public static readonly MeasurementUnit Radians = new MeasurementUnit("radians", "rad");

        /// <summary>
        /// Degrees, an angle measurement
        /// </summary>
        public static readonly MeasurementUnit Degrees = new MeasurementUnit("degrees", "°", Math.PI / 180);

        /// <summary>
        /// Newtons, the SI unit of force measurement
        /// </summary>
        public static readonly MeasurementUnit Newtons = new MeasurementUnit("newtons", "N");

        /// <summary>
        /// Kilonewtons, a force measurement
        /// </summary>
        public static readonly MeasurementUnit KiloNewtons = new MeasurementUnit("kilonewtons", "kN", 1000);

        /// <summary>
        /// Kilograms, the SI unit of measurement for mass
        /// </summary>
        public static readonly MeasurementUnit Kilograms = new MeasurementUnit("kilograms", "kg");

        /// <summary>
        /// Celsius, the SI unit of measurement for temperature
        /// </summary>
        public static readonly MeasurementUnit Celsius = new MeasurementUnit("degrees Celsius", "°C");

        /// <summary>
        /// Seconds, the SI unit of measurement for time
        /// </summary>
        public static readonly MeasurementUnit Seconds = new MeasurementUnit("seconds", "s");

        /// <summary>
        /// Minutes, a time measurement
        /// </summary>
        public static readonly MeasurementUnit Minutes = new MeasurementUnit("minutes", "min", 60);

        /// <summary>
        /// Hours, a time measurement
        /// </summary>
        public static readonly MeasurementUnit Hours = new MeasurementUnit("hours", "hr", 3600);

        /// <summary>
        /// British Pounds, a currency
        /// </summary>
        public static readonly MeasurementUnit BritishPounds = new MeasurementUnit("British Pounds", "GBP");

        /// <summary>
        /// United States Dollars, a currency
        /// </summary>
        public static readonly MeasurementUnit USDollars = new MeasurementUnit("United States Dollars", "USD");

        /// <summary>
        /// Euros, a currency
        /// </summary>
        public static readonly MeasurementUnit Euros = new MeasurementUnit("Euros", "€");

        /// <summary>
        /// Gets an array of all predefined length units
        /// </summary>
        public static MeasurementUnit[] LengthUnits
        {
            get
            {
                return new MeasurementUnit[]
                {
                    Meters,
                    Millimeters,
                    Centimeters
                };
            }
        }

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
            set { _SIFactor = value; }
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
        public MeasurementUnit(string name, string symbol, double siFactor = 1.0)
        {
            _Name = name;
            _Symbol = symbol;
            _SIFactor = siFactor;
        }

        #endregion
    }
}
