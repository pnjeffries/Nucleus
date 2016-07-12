using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An observable, keyed collection of shapes
    /// </summary>
    /// <typeparam name="TShape"></typeparam>
    [Serializable]
    public class ShapeCollection<TShape> : UniquesCollection<TShape>
        where TShape : IShape
    {
    }
}
