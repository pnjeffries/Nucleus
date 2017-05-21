using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.IO
{
    /// <summary>
    /// A record of a type's aliases and fields to be used during
    /// serialisation and deserialisation
    /// </summary>
    public class TypeFieldsFormat
    {
        #region Properties

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
        public TypeFieldsFormat(Type type, IList<FieldInfo> fields)
        {
            Type = type;
            Fields = fields;
        }

        public TypeFieldsFormat(Type type)
        {
            Type = type;
            Fields = type.GetAllFields(true);
        }


        #endregion
    }
}
