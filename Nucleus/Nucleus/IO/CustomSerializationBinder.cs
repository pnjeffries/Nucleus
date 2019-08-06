#if !JS

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// A customised serialisation binder to enable backwards compatibility with
    /// files saved from versions of the library prior to the rename.
    /// </summary>
    [Serializable]
    public class CustomSerializationBinder : SerializationBinder
    {
        private readonly string _OldLibraryName = "FreeBuild";
        private readonly string _NewLibraryName = "Nucleus";

        public override Type BindToType(string assemblyName, string typeName)
        {
            //if (assemblyName.Contains(_OldLibraryName))
            //{
            assemblyName = assemblyName.Replace(_OldLibraryName, _NewLibraryName);
            typeName = typeName.Replace(_OldLibraryName, _NewLibraryName);
            Type type;
            string fullName = string.Format("{0}, {1}", typeName, assemblyName);
            try
            {
                type = Type.GetType(fullName, true);
            }
            catch (FileNotFoundException ex)
            {
                try
                {
                    type = Type.GetType(fullName, AssemblyResolver, null, true);
                }
                catch (FileNotFoundException ex2)
                {
                    type = Type.GetType(typeName, AssemblyResolver, TypeResolver, true);
                }
            }
            return type;
            //}
            //return null;
        }

        private static Assembly AssemblyResolver(System.Reflection.AssemblyName assemblyName)
        {
            if (assemblyName.Name == "System.Collections")
            {
                // Horrible hack!
                // If loading something on Framework, doesn't currently have anything beyond v4.0,
                // but stuff saved in Standard or Core could be later...
                assemblyName.Version = new Version(4,0,0,0);
            }
            return Assembly.Load(assemblyName);
        }

        private static Type TypeResolver(Assembly assembly, string typeName, bool throwOnError)
        {
            if (assembly!= null) return assembly.GetType(typeName);
            else return Type.GetType(typeName, throwOnError);
        }
    }
}

#endif