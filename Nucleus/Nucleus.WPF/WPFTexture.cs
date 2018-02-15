using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nucleus.WPF
{
    /// <summary>
    /// A class which wraps a WPF ImageSource as a Nucleus ITexture
    /// </summary>
    public class WPFTexture : ITexture
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the ImageSource property
        /// </summary>
        private ImageSource _ImageSource;

        /// <summary>
        /// The texture ImageSource
        /// </summary>
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a WPFTexture to wrap an ImageSource
        /// </summary>
        /// <param name="imageSource"></param>
        public WPFTexture(ImageSource imageSource)
        {
            _ImageSource = imageSource;
        }

        #endregion
    }
}
