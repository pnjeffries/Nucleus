using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A data component that represents they styling of an
    /// ASCII visual representation
    /// </summary>
    [Serializable]
    public class ASCIIStyle : AvatarStyle, IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Symbol property
        /// </summary>
        private string _Symbol = "@";

        /// <summary>
        /// The ASCII symbol used to represent the element
        /// </summary>
        public string Symbol
        {
            get { return _Symbol; }
            set
            {
                _Symbol = value;
                NotifyPropertyChanged("Symbol");
            }
        }

        #endregion

        #region Constructors

        public ASCIIStyle() { }

        public ASCIIStyle(string symbol)
        {
            _Symbol = symbol;
        }

        #endregion
    }
}
