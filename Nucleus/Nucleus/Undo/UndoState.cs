using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Undo
{
    /// <summary>
    /// Represents a particular state which can be returned to through an undo operation
    /// </summary>
    [Serializable]
    public abstract class UndoState
    {
        /// <summary>
        /// Is this state valid?  i.e. does it represent a restorable state?
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Restore this state
        /// </summary>
        public abstract void Restore();

        /// <summary>
        /// Generate the redo operation that would undo this undo
        /// </summary>
        /// <returns></returns>
        public abstract UndoState GenerateRedo();
    }
}
