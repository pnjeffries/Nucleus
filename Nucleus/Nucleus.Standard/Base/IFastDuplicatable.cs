using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Interface for objects which implement a 'FastDuplicate()'
    /// method to create duplicates of objects.
    /// This duplication must be manually implemented and so
    /// there is a lot more implementation work involved than with
    /// the fully automatic IDuplicatable interface but the
    /// resulting duplication may be significantly more efficient
    /// as the use of reflection can be avoided.
    /// </summary>
    public interface IFastDuplicatable : IDuplicatable
    {
        /// <summary>
        /// Method used by the FastDuplicate() extension method.
        /// Implement this explicitly and manually construct a copy of the original type.
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        IFastDuplicatable FastDuplicate_Internal();
    }

    /// <summary>
    /// Extension methods for the IFastDuplicateble 
    /// </summary>
    public static class IFastDuplicatableExtensions
    {
        /// <summary>
        /// Produce a duplicate of this object.
        /// This is an alternative to the more general Duplicate()
        /// method.
        /// Unlike Duplicate() this does not (always) use reflection and must
        /// be manually implemented, meaning that it may be significantly
        /// faster in execution but that not all properties are guaranteed
        /// to be fully duplicated (though key ones should be).
        /// This is typically used to optimise performance-critical code
        /// where the normal duplication framework would be too expensive.
        /// </summary>
        /// <typeparam name="TSelf"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TSelf FastDuplicate<TSelf>(this TSelf obj)
            where TSelf : IFastDuplicatable
        {
            return (TSelf)obj.FastDuplicate_Internal();
        }
    }
}
