using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Enum used to describe the behaviour of a field during a
    /// data copying operation
    /// </summary>
    public enum CopyBehaviour
    {
        /// <summary>
        /// Copy the value or reference assigned to the field
        /// - i.e. create a 'shallow' copy.  This is the default
        /// behaviour, you do not need to specify this value.
        /// </summary>
        COPY = 0,
        
        /// <summary>
        /// The field should not be copied at all - the original
        /// value of the field should not be overwritten.
        /// Use when a field should survive a copy operation without
        /// being modified - for example the GUID of the object.
        /// </summary>
        DO_NOT_COPY = 1,
        
        /// <summary>
        /// Duplicate the object assigned to this field, creating
        /// a 'deep copy'.  The object type of this field *must* itself
        /// implement the IDuplicatable interface in order for this
        /// value to be valid.
        /// </summary>
        DUPLICATE = 2,

        /// <summary>
        /// Map the reference assigned to this field to the duplication of
        /// that object created during this same copy operation.
        /// If no such object has been included in this duplication thus far,
        /// do not copy anything.  Use this when you have a back-reference
        /// to a parent object and want that reference to kept to the *new*
        /// parent when copied, unless the child object is being copied on
        /// its own and will not *have* a parent.
        /// </summary>
        MAP = 3,

        /// <summary>
        /// Map the reference assigned to this field to the duplication of
        /// that object created during this same copy operation.
        /// If no such object has been included in this duplication thus far,
        /// copy the reference itself.
        /// </summary>
        MAP_OR_COPY = 4,

        /// <summary>
        /// Map the reference assigned to this field to the duplication of
        /// that object created during this same copy operation.
        /// If no such object has been included in this duplication thus far,
        /// create that duplicate.   The object type of this field *must* itself
        /// implement the IDuplicatable interface in order for this
        /// behaviour to be valid.
        /// </summary>
        MAP_OR_DUPLICATE = 4
    }
}
