using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nucleus.Base;
using Nucleus.Model;

namespace Nucleus.Game
{
    /// <summary>
    /// A collection of Proficiency records keyed by name
    /// </summary>
    [Serializable]
    public class ProficiencyCollection : ObservableKeyedCollection<string, Proficiency>
    {
        protected override string GetKeyForItem(Proficiency item)
        {
            return item.Name;
        }
    }
}
