using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An obervable, keyed collection of vertices
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    [Serializable]
    public class VertexCollection<TItem> : UniquesCollection<TItem> where TItem : IVertex
    {
    }
}
