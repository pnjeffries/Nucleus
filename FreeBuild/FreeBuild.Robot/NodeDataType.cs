using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Robot
{ 
    /// <summary>
    /// Types of data related to nodes that can be extracted
    /// </summary>
    public enum NodeDataType
    {
        Node_ID = 0,
        Case_ID  = 10,
        Case_Name = 11,
        X = 20,
        Y = 21,
        Z = 22,
        Displacement_Ux = 30,
        Displacement_Uy = 31,
        Displacement_Uz = 32,
        Displacement_Rxx = 33,
        Displacement_Ryy = 34,
        Displacmenet_Rzz = 35,
        Reactions_Fx = 40,
        Reactions_Fy = 41,
        Reactions_Fz = 42,
        Reactions_Mxx = 43,
        Reactions_Myy = 44,
        Reactions_Mzz = 45
    }

    public static class NodeDataTypeExtensions
    {
        /// <summary>
        /// Get the next NodeDataType value after the one specified
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static NodeDataType Next(this NodeDataType type)
        {
            NodeDataType[] values = (NodeDataType[])Enum.GetValues(typeof(NodeDataType));
            int i = Array.IndexOf(values, type) + 1;
            if (i == values.Length) i = 0;
            return values[i];
        }

        /// <summary>
        /// Does this data type constitute a property of the node itself?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNodeData(this NodeDataType type)
        {
            return (type == NodeDataType.X || type == NodeDataType.Y || type == NodeDataType.Z);
        }


        /// <summary>
        /// Is this a displacement result data type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDisplacementData(this NodeDataType type)
        {
            return (type >= NodeDataType.Displacement_Ux && type <= NodeDataType.Displacmenet_Rzz);
        }
    }
}
