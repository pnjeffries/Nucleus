using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for the Assembly class
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Get a list of all non-static types in this assembly which have not been tagged
        /// with the [Serializable] attribute.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IList<Type> GetUnserializableTypes(this Assembly assembly)
        {
            var result = new List<Type>();
            var q = from t in assembly.GetTypes()
                    where t.IsClass && !(t.IsAbstract && t.IsSealed) &&
                    ((t.Attributes & TypeAttributes.Serializable) != TypeAttributes.Serializable)
                    && !t.Name.StartsWith("<")
                    && (t.InheritanceLevelsTo(typeof(Attribute)) < 0)
                    select t;
            q.ToList().ForEach(t => result.Add(t));
            return result;
        }

        /// <summary>
        /// Get a list of all unique namespaces in this assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetNamespaces(this Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            return types.Select(type => type.Namespace)
                        .Distinct()
                        .Where(name => name != null);
        }

        /// <summary>
        /// Get all types in the specified namespace in this assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="namespace">The namespace to search for</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesInNamespace(this Assembly assembly, string @namespace)
        {
            return assembly.GetTypes().Where(type => type.Namespace.EqualsIgnoreCase(@namespace));
        }
    }
}
