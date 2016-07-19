using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Interface for element vertices.
    /// Element vertices are positions in space which form part of an element's geometric definition.
    /// Each element vertex is unique to a particular element.
    /// The IElementVertex interface provides a simple way of interacting with element vertices without 
    /// needing to define generic parameters.
    /// </summary>
    public interface IElementVertex : IVertex
    {
        /// <summary>
        /// The parent element of this vertex
        /// </summary>
        IElement Element { get; }

    }
}
