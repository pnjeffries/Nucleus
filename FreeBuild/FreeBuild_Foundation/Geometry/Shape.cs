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
    /// An abstract base class for shapes.
    /// Shapes are geometry defined by and containing a set of vertices.
    /// Different types of shapes will require different numbers and types of vertices
    /// and will interpret them in different ways, however the basic data structure is 
    /// always the same.
    /// </summary>
    /// <typeparam name="TVertex">The type of vertex used to define this shape</typeparam>
    /// <typeparam name="TParameter">The type of the parameter used to indicate a specific position </typeparam>
    [Serializable]
    public abstract class Shape : Unique
    {
        #region Properties

        /// <summary>
        /// The collection of vertices which are used to define the geometry of this shape.
        /// Different shapes will provide different means of editing this collection.
        /// DO NOT directly modify the collection returned from this property unless you are
        /// sure you know what you are doing.
        /// </summary>
        public abstract VertexCollection Vertices { get; }

        /// <summary>
        /// Is the definition of this shape valid?
        /// i.e. does it have the correct number of vertices, are all parameters within acceptable limits, etc.
        /// </summary>
        public abstract bool IsValid { get; }

        #endregion

        #region Methods


        #endregion


    }
}
