using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    /// <summary>
    /// Extension methods on types and collections of types
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// The number of levels of inheritance between this type and a type that
        /// is somewhere in its inheritance chain.
        /// </summary>
        /// <param name="type">This type</param>
        /// <param name="ancestorType">A type which is an ancestor of this one</param>
        /// <returns>If the specified type is an ancestor of this one, the number of
        /// inheritance levels between the two types.  If the specified type is this 
        /// type, 0.  If the specified type cannot be found in the inheritance chain,
        /// -1.</returns>
        public static int InheritanceLevelsTo(this Type type, Type ancestorType)
        {
            int count = 0;
            while (type != null && type != ancestorType)
            {
                count++;
                type = type.BaseType;
            }
            if (type == ancestorType) return count;
            else return -1;
        }

        /// <summary>
        /// Find the type in this set of types which is the least number of
        /// inheritance levels above the specified type.
        /// </summary>
        /// <param name="forType">The type to seach for</param>
        /// <param name="inTypes">The collection of types to look within</param>
        /// <returns>The type in this collection that is closest in the inheritance
        /// hierarchy to the specified type.  Or, null if the type does not have an
        /// ancestor in the collection.</returns>
        public static Type ClosestAncestor(IEnumerable<Type> inTypes, Type forType)
        {
            int minDist = -1;
            Type closest = null;
            foreach (Type ancestorType in inTypes)
            {
                int dist = forType.InheritanceLevelsTo(ancestorType);
                if (dist >= 0 && (minDist < 0 || dist < minDist))
                {
                    minDist = dist;
                    closest = ancestorType;
                }
            }
            return closest;
        }
    }
}
