using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for collections of coordinate system references
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [Serializable]
    public abstract class CoordinateSystemReferenceCollection<TItem> : ModelObjectCollection<TItem>
        where TItem : CoordinateSystemReference
    {
    }

    /// <summary>
    /// A collection of coordinate system references
    /// </summary>
    [Serializable]
    public class CoordinateSystemReferenceCollection : CoordinateSystemReferenceCollection<CoordinateSystemReference>
    {
    }
}
