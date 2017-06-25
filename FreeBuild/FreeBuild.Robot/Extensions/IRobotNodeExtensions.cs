using Nucleus.Geometry;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Robot
{
    /// <summary>
    /// Extension methods for the IRobotNode interface
    /// </summary>
    public static class IRobotNodeExtensions
    {
        /// <summary>
        /// Set the position of this node using a Vector
        /// </summary>
        /// <param name="node"></param>
        /// <param name="position"></param>
        public static void SetPosition(this IRobotNode node, Vector position)
        {
            node.X = position.X;
            node.Y = position.Y;
            node.Z = position.Z;
        }
    }
}
