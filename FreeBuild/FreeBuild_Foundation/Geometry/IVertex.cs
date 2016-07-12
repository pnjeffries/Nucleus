using FreeBuild.Base;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// An interface for vertices - points in space which go to form part of a shape definition
    /// </summary>
    public interface IVertex : IUnique
    {
        /// <summary>
        /// The spatial position of the vertex
        /// </summary>
        Vector Position { get; }
    }
}
