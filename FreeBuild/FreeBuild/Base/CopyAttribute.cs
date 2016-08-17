using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Attribute applied to fields to determine the correct procedure for dealing
    /// with them when automatically copying their values to another object.
    /// By default, fields are 'shallow-copied' - i.e. values and references are copied.
    /// It is only necessary to apply this attribute to fields where this behaviour should
    /// be changed.
    /// </summary>
    public class CopyAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// The behaviour of the annotated field during a copy operation.
        /// By default, properties have their values 'shallow-copied', but may
        /// instead be prevented from being copied or be duplicated instead,
        /// provided the property type is duplicatable.
        /// </summary>
        public CopyBehaviour Behaviour { get; set; } = CopyBehaviour.COPY;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.  Initialise this attribute with the specified copying
        /// behaviour.
        /// </summary>
        /// <param name="behaviour">The copying behaviour of this field.</param>
        public CopyAttribute(CopyBehaviour behaviour)
        {
            Behaviour = behaviour;
        }

        #endregion
    }
}
