using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Robot
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
        Displacement_Ux = 30,
        Displacement_Uy = 31,
        Displacement_Uz = 32,
        Displacement_Rxx = 33,
        Displacement_Ryy = 34,
        Displacmenet_Rzz = 35,
        Force_Fx = 40,
        Force_Fy = 41,
        Force_Fz = 42,
        Force_Mxx = 43,
        Force_Myy = 44,
        Force_Mzz = 45,
        Stress_Axial = 50,
        Stress_Shear_Y = 51,
        Stress_Shear_Z = 52,
        Stress_Max = 53,
        Stress_Max_Myy = 54,
        Stress_Max_Mzz = 55,
        Stress_Min = 56,
        Stress_Min_Myy = 57,
        Stress_Min_Mzz = 58,
        Stress_Torsion = 59
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
            return (type == BarDataType.Bar_Name || type == BarDataType.StartNode || type == BarDataType.EndNode || type == BarDataType.Length);
        }

        /// <summary>
        /// Is this data type positional?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPositional(this BarDataType type)
        {
            return !(type == BarDataType.Bar_ID || type == BarDataType.Bar_Name || type == BarDataType.Case_ID || type == BarDataType.Case_Name ||
                type == BarDataType.EndNode || type == BarDataType.Length || type == BarDataType.StartNode);
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
