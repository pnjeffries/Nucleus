using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Meshing
{
    /// <summary>
    /// An enum describing different ways of remapping points into
    /// a coordinate system.
    /// </summary>
    public enum CoordinateSystemRemappingOption
    {
        None = 0,
        RemapYZ = 1,
        RemapNegYZ = 2
    }
}
