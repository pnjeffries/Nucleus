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

        #region Methods

        public TextureBrush GetFrame(double t)
        {
            if (_Frames.Count > 0)
            {
                int i = (int)Math.Floor(t * _Frames.Count);
                while (i >= _Frames.Count)
                {
                    i -= _Frames.Count;
                }
                return _Frames[i];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
