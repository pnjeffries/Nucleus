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
    /// files saved prior to renaming or refactoring of types and namespaces.
    /// </summary>
    [Serializable]
    public class CustomSerializationBinder : SerializationBinder
    {
        #region Constants

        private const string _OldLibraryName = "FreeBuild";
        private const string _NewLibraryName = "Nucleus";

        #endregion

        #region Properties

        private IDictionary<string, string> _TypeNameReplacements = null;

        /// <summary>
        /// A dictionary of type-name replacements to be utilised in the case that
        /// a particular type fails deserialization.  If a TypeLoadException occurs
        /// then a string.replace operation will be performed for each entry in this 
        /// dictionary, replacing the key string with the value string in the typeName.
        /// </summary>
        public IDictionary<string, string> TypeNameReplacements
        {
            get { return _TypeNameReplacements; }
            set { _TypeNameReplacements = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomSerializationBinder()
        {
            //TEMP:
            _TypeNameReplacements = new Dictionary<string, string>();
            _TypeNameReplacements.Add("SiteSolve.CoreSizer+", "SiteSolve.CorePacking.");
            _TypeNameReplacements.Add("SiteSolve.CorePacking.CoreLayout", "SiteSolve.CoreLayout");
            _TypeNameReplacements.Add("SiteSolve.CorePacking.ShaftType", "SiteSolve.ShaftType");
            _TypeNameReplacements.Add("System.Collections.Generic.List`1[[SiteSolve.InternalSpace,", "System.Collections.Generic.List`1[[SiteSolve.LayoutSpace,");
            _TypeNameReplacements.Add("SiteSolve.InternalSpace", "SiteSolve.LayoutSpace");
        }

        #endregion

        #region Methods

        public override Type BindToType(string assemblyName, string typeName)
        {
            Type type;
            try
            {
                string fullName = string.Format("{0}, {1}", typeName, assemblyName);
                type = Type.GetType(fullName, true);
            }
            catch (SystemException ex)
            {
                assemblyName = assemblyName.Replace(_OldLibraryName, _NewLibraryName);
                typeName = typeName.Replace(_OldLibraryName, _NewLibraryName);
                if (_TypeNameReplacements != null)
                {
                    foreach (var kvp in _TypeNameReplacements)
                    {
                        typeName = typeName.Replace(kvp.Key, kvp.Value);
                    }
                }
                string fullName = string.Format("{0}, {1}", typeName, assemblyName);
                try
                {
                    type = Type.GetType(fullName, true);
                }
                catch (FileNotFoundException ex2)
                {
                    try
                    {
                        type = Type.GetType(fullName, AssemblyResolver, null, true);
                    }
                    catch (FileNotFoundException ex3)
                    {
                        type = Type.GetType(typeName, AssemblyResolver, TypeResolver, true);
                    }
                }
            }
            return type;
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
            else if (assemblyName.Name == "mscorlib")
            {
                // Same as above, but in reverse...
                // Try this...
                assemblyName.Name = "System.Collections";
                assemblyName.Version = new Version(4, 0, 0, 0);
            }
            return Assembly.Load(assemblyName);
        }

        private static Type TypeResolver(Assembly assembly, string typeName, bool throwOnError)
        {
            if (assembly!= null) return assembly.GetType(typeName);
            else return Type.GetType(typeName, throwOnError);
        }

        #endregion
    }
}

#endif