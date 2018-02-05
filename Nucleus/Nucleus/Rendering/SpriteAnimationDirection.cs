using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    public class SpriteAnimationDirection
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Frames property
        /// </summary>
        private IList<TextureBrush> _Frames = new List<TextureBrush>();

        /// <summary>
        /// The frames that make up this animation direction
        /// </summary>
        public IList<TextureBrush> Frames
        {
            get { return _Frames; }
        }

            #endregion
        }
    }
