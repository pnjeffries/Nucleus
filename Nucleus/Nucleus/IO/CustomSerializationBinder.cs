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
        private readonly string _OldLibraryName = "Nucleus";
        private readonly string _NewLibraryName = "GenOME";

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (assemblyName.StartsWith(_OldLibraryName))
            {
                assemblyName = assemblyName.Replace(_OldLibraryName, _NewLibraryName);
                return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
            }
            return null;
        }
    }
}
