using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Undo
{
    /// <summary>
    /// A collection of undo states
    /// </summary>
    public class UndoStageCollection : ObservableCollection<UndoStage>
    {
    }
}
