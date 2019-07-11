using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// A brush which paints an area with an image texture
    /// </summary>
    [Serializable]
    public class TextureBrush : DisplayBrush
    {
        #region Properties

        private ITexture _Texture = null;

        /// <summary>
        /// The image to be drawn by the brush
        /// </summary>
        public ITexture Texture
        {
            get { return _Texture; }
            set { _Texture = value; }
        }

        private Rectangle _Region = null;

        /// <summary>
        /// The region of the texture to be displayed.
        /// If null, indicates the full texture is to be used
        /// without cropping.
        /// </summary>
        public Rectangle Region
        {
            get { return _Region; }
            set { _Region = value; }
        }

        public override Colour BaseColour => Colour.White;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a blank TextureBrush
        /// </summary>
        public TextureBrush()
        {

        }

        /// <summary>
        /// Initialise a TextureBrush with the specified texture
        /// </summary>
        /// <param name="texture"></param>
        public TextureBrush(ITexture texture)
        {
            Texture = texture;
        }

        /// <summary>
        /// Initialise a TextureBrush to paint the specified region 
        /// of the specified texture
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="region"></param>
        public TextureBrush(ITexture texture, Rectangle region) : this(texture)
        {
            Region = region;
        }

        /// <summary>
        /// Initialise a TextureBrush to paint the specified region 
        /// of the specified texture
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="region"></param>
        public TextureBrush(ITexture texture, string[] data) : this(texture)
        {
            double posX = 0, posY = 0, width = 0, height = 0;
            if (data.Length > 0)
            {
                posX = double.Parse(data[0]);
                if (data.Length > 1)
                {
                    posY = double.Parse(data[1]);
                    if (data.Length > 2)
                    {
                        width = double.Parse(data[2]);
                        if (data.Length > 3)
                        {
                            height = double.Parse(data[3]);
                        }
                    }
                }
            }
            Region = new Rectangle(posX, posX + width, posY, posY + height);
        }

        #endregion
    }
}
