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
        /// Kilograms, the SI unit of measurement for mass
        /// </summary>
        public static readonly MeasurementUnit Kilograms = new MeasurementUnit("kilograms", "kg");

        /// <summary>
        /// Celsius, the SI unit of measurement for temperature
        /// </summary>
        public static readonly MeasurementUnit Celsius = new MeasurementUnit("degrees Celsius", "°C");

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
