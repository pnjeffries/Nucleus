using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Undo
{
    /// <summary>
    /// An undo stage.  A collection of recorded states resulting
    /// from an undoable operation and which will all be reverted
    /// at once.
    /// </summary>
    public class UndoStage : List<UndoState>
    {
        /// <summary>
        /// Revert this stage by restoring the states stored
        /// within this collection
        /// </summary>
        /// <returns>The redo stage necessary to redo this undo.</returns>
        public UndoStage Undo()
        {
            var redo = new UndoStage();
            for (int i = Count - 1; i >= 0; i--)
            {
                UndoState state = this[i];
                if (state.IsValid)
                {
                    try
                    {
                        redo.Add(state.GenerateRedo());
                        state.Restore();
                    }
                    catch { }
                    
                }
            }
            return redo;
        }
    }
}
