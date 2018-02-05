using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Nucleus.Rendering;

namespace Nucleus.WPF
{
    /// <summary>
    /// A WPF control which represents an animated 2D Sprite
    /// </summary>
    public class Sprite : Image
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the SpriteData property
        /// </summary>
        private SpriteData _SpriteData = null;

        /// <summary>
        /// The sprite data which defines the animations of this Sprite
        /// </summary>
        public SpriteData SpriteData
        {
            get { return _SpriteData; }
            set { _SpriteData = value; }
        }


        #endregion
    }
}
