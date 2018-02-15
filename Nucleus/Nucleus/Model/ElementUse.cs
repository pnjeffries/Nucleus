using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A description of the usage of an element.
    /// A tag used to indicate what a model element represents
    /// </summary>
    [Serializable]
    public struct ElementUse : IEquatable<ElementUse>
    {
        #region Constants

        /// <summary>
        /// The element represents a structural beam
        /// </summary>
        public static readonly ElementUse Beam = "Beam";

        /// <summary>
        /// The element represents a structural column
        /// </summary>
        public static readonly ElementUse Column = "Column";

        /// <summary>
        /// The element represents structural bracing
        /// </summary>
        public static readonly ElementUse Brace = "Brace";

        /// <summary>
        /// The element represents a structural slab
        /// </summary>
        public static readonly ElementUse Slab = "Slab";

        /// <summary>
        /// The element represents a structural wall
        /// </summary>
        public static readonly ElementUse Wall = "Wall";

        #endregion

        #region Properties

        /// <summary>
        /// Private backing member variable for the Name property
        /// </summary>
        private string _Name;

        /// <summary>
        /// The name of the ElementUse tag.  Two ElementUse tags are assumed to be equal if their names match.
        /// </summary>
        public string Name
        {
            get { return _Name; }

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initalise a new ElementUse with the specified name
        /// </summary>
        /// <param name="name"></param>
        public ElementUse(string name)
        {
            _Name = name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// IEquatable Equals implementation.
        /// Two ElementUses are equal if they share the same name.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ElementUse other)
        {
            return Name == other.Name;
        }

        #endregion

        #region Operators

        public static implicit operator ElementUse(string str)
        {
            return new ElementUse(str);
        }

        #endregion
    }
}
