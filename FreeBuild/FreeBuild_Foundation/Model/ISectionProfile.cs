using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// An I-Shaped Section Profile.
    /// Not an interface!
    /// </summary>
    public class ISectionProfile : SectionProfile
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Depth property
        /// </summary>
        private double _Depth;

        /// <summary>
        /// The depth of the section
        /// </summary>
        public double Depth
        {
            get { return _Depth; }
            set
            {
                _Depth = value;
                NotifyPropertyChanged("Depth");
            }
        }

        /// <summary>
        /// Private backing member variable for the Width property
        /// </summary>
        private double _Width;

        /// <summary>
        /// The width of the section
        /// </summary>
        public double Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                NotifyPropertyChanged("Width");
            }
        }

        #endregion

    }
}
