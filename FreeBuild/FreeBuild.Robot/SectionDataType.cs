using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Robot
{
    /// <summary>
    /// The types of data that can be extracted for Sections
    /// </summary>
    public enum SectionDataType
    {
        Name = 1,
        Material = 2,
        Type = 3,
        ShapeType = 4,

        Depth = 100,
        Width = 101,
        Width_2 = 102,
        Flange_Thickness = 110,
        Flange_Thickness_2 = 111,
        Web_Thickness = 120,
        Fillet_Radius = 130,
        Gamma_Angle = 190,

        Weight = 200,
        Perimeter = 210,
        Area_X = 220,
        Area_Y = 221,
        Area_Z = 222,
        I_XX = 230,
        I_YY = 231,
        I_ZZ = 232
    }

    public static class SectionDataTypeExtensions
    {
        /// <summary>
        /// Get the next NodeDataType value after the one specified
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SectionDataType Next(this SectionDataType type)
        {
            SectionDataType[] values = (SectionDataType[])Enum.GetValues(typeof(SectionDataType));
            int i = Array.IndexOf(values, type) + 1;
            if (i == values.Length) i = 0;
            return values[i];
        }
    }
}
