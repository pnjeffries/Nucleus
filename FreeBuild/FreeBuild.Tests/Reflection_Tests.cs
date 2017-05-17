using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Tests
{
    public static class Reflection_Tests
    {
        public static void PrintFields(Type type)
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            IEnumerable<FieldInfo> fields = type.GetAllFields(flags);
            foreach (FieldInfo field in fields)
            {
                Core.Print(field.Name);
            }
        }

        public static void PrintProperties(Type type)
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            IEnumerable<PropertyInfo> fields = type.GetProperties(flags);
            foreach (PropertyInfo field in fields)
            {
                Core.Print(field.Name);
            }
        }

        public static void PrintUnserializableTypes(Assembly assembly)
        {
            var q = from t in assembly.GetTypes()
                    where t.IsClass && ((t.Attributes & TypeAttributes.Serializable) != TypeAttributes.Serializable)
                    select t;
            q.ToList().ForEach(t => Core.Print(t.Name));
        }

        public static void PrintUnserializableTypes(Type toSerialise)
        {
            var types = toSerialise.GetDependencies(true);
            foreach (Type t in types)
            {
                if (t.IsClass && t.GetCustomAttribute(typeof(SerializableAttribute)) == null) Core.Print(t.Name);
            }
        }

        public static void PrintDependencies(Type type)
        {
            var types = type.GetDependencies();
            foreach (Type t in types) if (t.IsClass) Core.Print(t.Name);
        }
    }
}
