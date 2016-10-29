using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    //SPECULATIVE - Could we replace putting nodes on shape vertices with something like this?
    //HMMMMM... Maybe stick with current scheme until we come across a compelling reason to change?

    /// <summary>
    /// A position along an element
    /// </summary>
    public class ElementVertex : Unique, IOwned<Element>
    {
        #region Properties

        /// <summary>
        /// Private backing field for Element property
        /// </summary>
        private Element _Element;

        /// <summary>
        /// The element that this vertex belongs to
        /// </summary>
        public Element Element { get { return _Element; } }

        Element IOwned<Element>.Owner { get { return _Element; } }

        private Node _Node;

        public Node Node { get { return _Node; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.  Initialises a new ElementVertex belonging to the specified element
        /// </summary>
        /// <param name="element"></param>
        public ElementVertex(Element element)
        {
            _Element = element;
        }

        #endregion
    }
}
