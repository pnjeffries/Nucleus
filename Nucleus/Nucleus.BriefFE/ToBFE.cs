using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFE = BriefFiniteElementNet;
using Nucleus.Model;
using Nucleus.Geometry;
using Nucleus.Base;

namespace Nucleus.BriefFE
{
    /// <summary>
    /// Helper class to convert Nucleus object types to equivalent
    /// Brief FE types
    /// </summary>
    public static class ToBFE
    {
        /// <summary>
        /// Convert a Nucleus position vector to a Brief FE point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static BFE.Point Convert(Vector point)
        {
            return new BFE.Point(point.X, point.Y, point.Z);
        }

        /// <summary>
        /// Convert a boolean to a BFE Constraint enum
        /// </summary>
        /// <param name="boolean"></param>
        /// <returns></returns>
        private static BFE.DofConstraint ToConstraint(bool boolean)
        {
            if (boolean) return BFE.DofConstraint.Fixed;
            else return BFE.DofConstraint.Released;
        }

        /// <summary>
        /// Convert a fixity Bool6D to a Brief FE Constraint struct
        /// </summary>
        /// <param name="fixity"></param>
        /// <returns></returns>
        public static BFE.Constraint Convert(Bool6D fixity)
        {
            return new BFE.Constraint(
                ToConstraint(fixity.X),
                ToConstraint(fixity.Y),
                ToConstraint(fixity.Z),
                ToConstraint(fixity.XX),
                ToConstraint(fixity.YY),
                ToConstraint(fixity.ZZ));
        }

        /// <summary>
        /// Convert a Nucleus node to a BFE one
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static BFE.Node Convert(Node node)
        {
            var result = new BFE.Node(Convert(node.Position))
            {
                Label = node.GUID.ToString()
            };
            if (node.HasData<NodeSupport>())
            {
                var nS = node.GetData<NodeSupport>();
                result.Constraints = Convert(nS.Fixity);
            }
            return result;
        }
    }
}
