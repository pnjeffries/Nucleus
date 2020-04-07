// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Units
{
    /// <summary>
    /// Enum to represent different physical dimensions
    /// </summary>
    [Serializable]
    public enum DimensionType
    {
        /// <summary>
        /// A quantity to which no physical dimension is applicable.
        /// </summary>
        Dimensionless = 0,

        /// <summary>
        /// A distance, or length, measurement.
        /// Default unit: m
        /// </summary>
        Distance = 1,

        /// <summary>
        /// An angle or rotation measurement.
        /// Default unit: Radians
        /// </summary>
        Angle = 2,

        /// <summary>
        /// An area measurement
        /// </summary>
        Area = 10,

        /// <summary>
        /// A volume measurement
        /// </summary>
        Volume = 11,

        /// <summary>
        /// A measurement of force.
        /// Default unit: N
        /// </summary>
        Force = 100,

        /// <summary>
        /// A measurement of moments
        /// Default unit: Nm
        /// </summary>
        Moment = 101,
       
        /// <summary>
        /// A measurement of mass.
        /// Default unit: kg
        /// </summary>
        Mass = 200,

        /// <summary>
        /// A measurement of time.
        /// </summary>
        Time = 300,

        /// <summary>
        /// A measurement of temperature
        /// </summary>
        Temperature = 400,

        /// <summary>
        /// A measurement of currency
        /// </summary>
        Currency = 1000,

        //TODO: Add more dimension types as and when needed
    }

    /// <summary>
    /// Extension methods for the DimensionType attribute
    /// </summary>
    public static class DimensionTypeExtensions
    {
        /// <summary>
        /// Get the standard SI unit for this dimension type (if one is defined).
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static MeasurementUnit SIUnit(this DimensionType dimension)
        {
            switch (dimension)
            {
                case DimensionType.Angle: 
                    return MeasurementUnit.Radians;
                case DimensionType.Area:
                    return MeasurementUnit.MetersSquared;
                case DimensionType.Distance:
                    return MeasurementUnit.Meters;
                case DimensionType.Force:
                    return MeasurementUnit.Newtons;
                case DimensionType.Mass:
                    return MeasurementUnit.Kilograms;
                case DimensionType.Moment:
                    return MeasurementUnit.NewtonMeters;
                case DimensionType.Temperature:
                    return MeasurementUnit.Kelvin;
                case DimensionType.Time:
                    return MeasurementUnit.Seconds;
                case DimensionType.Volume:
                    return MeasurementUnit.MetersCubed;
                default:
                    return null;
            }
        }
    }
}
