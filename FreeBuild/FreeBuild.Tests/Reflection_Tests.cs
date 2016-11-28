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
    }
}
