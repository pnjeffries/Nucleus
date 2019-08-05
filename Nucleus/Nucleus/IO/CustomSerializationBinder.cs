#if !JS

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            try
            {
                type = Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
            }
            catch (FileNotFoundException ex)
            {
                type = Type.GetType(typeName, AssemblyResolver, null);
            }
            return type;
            //}
            //return null;
        }

        private static System.Reflection.Assembly AssemblyResolver(System.Reflection.AssemblyName assemblyName)
        {
            assemblyName.Version = null;
            return System.Reflection.Assembly.Load(assemblyName);
        }
    }
}

#endif