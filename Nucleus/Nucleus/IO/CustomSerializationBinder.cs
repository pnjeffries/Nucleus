using System;
using System.Collections.Generic;
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
                Type type = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
            return type;
            //}
            //return null;
        }
    }
}
