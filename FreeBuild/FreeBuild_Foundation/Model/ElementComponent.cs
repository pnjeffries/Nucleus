using FreeBuild.Base;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract base class for objects which form part of an element definition.
    /// Element components belong to a specific element and cannot survive on their own.
    /// </summary>
    public abstract class ElementComponent<TElement> : Unique where TElement : IElement
    {
        #region Properties

        /// <summary>
        /// The parent element of this component
        /// </summary>
        public TElement Element { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Element constructor.
        /// Derived classes must call this constructor in order to specify the element this
        /// component is part of.  
        /// </summary>
        /// <param name="element"></param>
        protected ElementComponent(TElement element)
        {
            Element = element;
        }

        #endregion
    }
}
