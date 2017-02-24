using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Enum to represent the standard built-in data types
    /// intended for use as attached data on nodes
    /// </summary>
    public enum NodeDataType
    {
        NodeSupport
    }

    /// <summary>
    /// Static extension methods for the NodeDataType enum
    /// </summary>
    public static class NodeDataTypeExtensions
    {
        /// <summary>
        /// Get the type that this enum value represents
        /// </summary>
        /// <param name="nDT"></param>
        /// <returns></returns>
        public static Type RepresentedType(this NodeDataType nDT)
        {
            if (nDT == NodeDataType.NodeSupport) return typeof(NodeSupport);
            else return null;
        }
    }
}
