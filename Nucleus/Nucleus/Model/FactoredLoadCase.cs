using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A factored load case.  Used to build up load combination cases
    /// </summary>
    [Serializable]
    public class FactoredLoadCase : FactoredCase<LoadCase>
    {
    }
}
