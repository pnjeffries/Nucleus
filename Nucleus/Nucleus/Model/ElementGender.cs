using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// Data component to represent the gender of an element
    /// </summary>
    [Serializable]
    public class ElementGender : IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Gender property
        /// </summary>
        private Gender _Gender = Gender.Neutral;

        /// <summary>
        /// The gender of the element
        /// </summary>
        public Gender Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise a neutral gender
        /// </summary>
        public ElementGender() { }

        /// <summary>
        /// Initialise with the specified gender
        /// </summary>
        /// <param name="gender"></param>
        public ElementGender(Gender gender)
        {
            _Gender = gender;
        }

        #endregion
    }
}
