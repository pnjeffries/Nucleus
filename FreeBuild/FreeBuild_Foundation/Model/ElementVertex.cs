using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Generic abstract base class for element vertices.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public abstract class ElementVertex<TElement> : ElementComponent<TElement>, IElementVertex where TElement : IElement
    {
        #region Properties

        /// <summary>
        /// Interface Element redirector
        /// </summary>
        IElement IElementVertex.Element { get { return Element; } }

        /// <summary>
        /// The current position of this vertex
        /// </summary>
        public abstract Vector Position { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Base constructor.  Initialises this vertex as part of an element.
        /// Constructors on derived types must invoke this constructor.
        /// Element vertices cannot be created without specifying an owning element.
        /// </summary>
        /// <param name="element"></param>
        protected ElementVertex(TElement element) : base(element) { }

        #endregion
    }
}
