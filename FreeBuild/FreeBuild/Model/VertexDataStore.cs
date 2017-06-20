using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A data component store belonging to a Vertex - can be used to attach
    /// data components to said vertex.
    /// </summary>
    [Serializable]
    public class VertexDataStore : DataStore<IVertexDataComponent, Vertex>
    {
    }
}
