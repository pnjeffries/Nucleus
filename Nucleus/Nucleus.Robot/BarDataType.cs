using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Robot
{
    /// <summary>
    /// The types of data that can be extracted for bars
    /// </summary>
    public enum BarDataType
    {
        Bar_ID = 0,
        Bar_Name = 1,
        Case_ID = 10,
        Case_Name = 11,
        Length = 20,
        StartNode = 21,
        EndNode = 22,
        Section_Name = 30,
        Displacement_Ux = 130,
        Displacement_Uy = 131,
        Displacement_Uz = 132,
        Displacement_Rxx = 133,
        Displacement_Ryy = 134,
        Displacmenet_Rzz = 135,
        Force_Fx = 140,
        Force_Fy = 141,
        Force_Fz = 142,
        Force_Mxx = 143,
        Force_Myy = 144,
        Force_Mzz = 145,
        Stress_Axial = 150,
        Stress_Shear_Y = 151,
        Stress_Shear_Z = 152,
        Stress_Max = 153,
        Stress_Max_Myy = 154,
        Stress_Max_Mzz = 155,
        Stress_Min = 156,
        Stress_Min_Myy = 157,
        Stress_Min_Mzz = 158,
        Stress_Torsion = 159
    }

    public static class BarDataTypeExtensions
    {
        /// <summary>
        /// Get the next BarDataType value after the one specified
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static BarDataType Next(this BarDataType type)
        {
            BarDataType[] values = (BarDataType[])Enum.GetValues(typeof(BarDataType));
            int i = Array.IndexOf(values, type) + 1;
            if (i == values.Length) i = 0;
            return values[i];
        }

        /// <summary>
        /// Is the value of this enum a bar data type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBarData(this BarDataType type)
        {
            return (type == BarDataType.Bar_Name || type == BarDataType.StartNode || type == BarDataType.EndNode || type == BarDataType.Length || type == BarDataType.Section_Name);
        }

        /// <summary>
        /// Is this data type positional?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPositional(this BarDataType type)
        {
            return type >= BarDataType.Displacement_Ux;
        }

        /// <summary>
        /// Is this a displacement result type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDisplacement(this BarDataType type)
        {
            return (type >= BarDataType.Displacement_Ux
                && type <= BarDataType.Displacmenet_Rzz);
        }

        /// <summary>
        /// Is this a force result type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsForce(this BarDataType type)
        {
            return (type >= BarDataType.Force_Fx
                && type <= BarDataType.Force_Mzz);
        }

        /// <summary>
        /// Is this a stress result type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStress(this BarDataType type)
        {
            return (type >= BarDataType.Stress_Axial
                && type <= BarDataType.Stress_Torsion);
        }
    }
}
