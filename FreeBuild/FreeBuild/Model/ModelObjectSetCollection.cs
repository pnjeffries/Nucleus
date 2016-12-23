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
    public class ModelObjectSetCollection<TSet> : ModelObjectCollection<TSet>
        where TSet : ModelObjectSet
    {
    }
}
