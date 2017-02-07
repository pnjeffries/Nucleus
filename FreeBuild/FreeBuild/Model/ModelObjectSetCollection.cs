using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A generic collection of ModelObjectSet objects
    /// </summary>
    /// <typeparam name="TSet"></typeparam>
    [Serializable]
    public class ModelObjectSetCollection<TSet> : ModelObjectCollection<TSet>
        where TSet : ModelObjectSetBase
    {
    }
}
