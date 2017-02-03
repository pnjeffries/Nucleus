using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Attribute applied to collection types and fields to determine the correct procedure for dealing
    /// with them when automatically copying their values and their contents to another object.
    /// By default, fields are 'shallow-copied' - i.e. values and references are copied.
    /// It is only necessary to apply this attribute to fields where this behaviour should
    /// be changed.  Attributes applied to types define the default behaviour of that type,
    /// but field-level attributes will override this where present.
    /// </summary>
    public class CollectionCopyAttribute : CopyAttribute
    {
        /// <summary>
        /// The behaviour of the items contained within the annotated field during a copy operation.
        /// By default, properties have their values 'shallow-copied', but may
        /// instead be prevented from being copied or be duplicated instead,
        /// provided the property type is duplicatable.
        /// </summary>
        public CopyBehaviour ItemsBehaviour { get; set; } = CopyBehaviour.COPY;

        public CollectionCopyAttribute(CopyBehaviour behaviour, CopyBehaviour itemsBehaviour) : base(behaviour)
        {
            ItemsBehaviour = itemsBehaviour;
        }
    }
}
