#if !JS

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Undo
{
    /// <summary>
    /// A collection of undo states
    /// </summary>
    [Serializable]
    public class UndoStageCollection : ObservableCollection<UndoStage>
    {
    }
}

#endif
