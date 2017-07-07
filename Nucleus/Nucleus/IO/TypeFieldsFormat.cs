using Nucleus.Base;
using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// A record of a type's aliases and fields to be used during
    /// serialisation and deserialisation
    /// </summary>
    public class TypeFieldsFormat
    {
        #region Properties

        /// <summary>
        /// The alias of the type
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// The type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// The type's serializable fields, in order of storage
        /// </summary>
        public IList<FieldInfo> Fields { get; set; }


        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new type fields' format record
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fields"></param>
        public TypeFieldsFormat(string alias, Type type, IList<FieldInfo> fields)
        {
            Alias = alias;
            Type = type;
            Fields = fields;
        }

        public TypeFieldsFormat(string alias, Type type)
        {
            Alias = alias;
            Type = type;
            Fields = type.GetAllFields(true, FilterField);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determine whether or not the specified field is suitable
        /// </summary>
        /// <param name="fI"></param>
        /// <returns></returns>
        public bool FilterField(FieldInfo fI)
        {
            if (fI.MemberType == MemberTypes.Event || typeof(MulticastDelegate).IsAssignableFrom(fI.FieldType))
            {
                return false;
            }
            if (fI.DeclaringType.IsStandardDictionary())
            {
                return false;
            }
            if (fI.Name == "_GUID" && typeof(IUnique).IsAssignableFrom(fI.DeclaringType))
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
