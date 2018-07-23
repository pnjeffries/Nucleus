using Nucleus.Base;
using Nucleus.Extensions;
using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// Class to hold the definition of a sprite
    /// </summary>
    public class SpriteData : Named
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Animations property
        /// </summary>
        private Dictionary<string, SpriteAnimation> _Animations = new Dictionary<string, SpriteAnimation>();

        /// <summary>
        /// The animations of this sprite
        /// </summary>
        public Dictionary<string, SpriteAnimation> Animations
        {
            get { return _Animations; }
        }

        #endregion

        #region Constructors

        public SpriteData() { }

        #endregion

        #region Methods

        /// <summary>
        /// Get the default animation for this sprite
        /// </summary>
        /// <returns></returns>
        public SpriteAnimation GetDefaultAnimation()
        {
            if (_Animations.ContainsKey("Idle")) return _Animations["Idle"];
            else return _Animations.Values.FirstOrDefault();
        }

        /// <summary>
        /// Get the animation on this Sprite with the specified name, if one exists.
        /// Otherwise, will return the default (usually, 'Idle') animation.
        /// </summary>
        /// <param name="animationName"></param>
        /// <returns></returns>
        public SpriteAnimation GetAnimation(string animationName)
        {
            if (_Animations.ContainsKey(animationName)) return _Animations[animationName];
            else return GetDefaultAnimation();
        }

        /// <summary>
        /// Get the closest direction of the animation with the specified name (or,
        /// the default animation if no animation with this name exists).
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public SpriteAnimationDirection GetDirection(string animationName, Angle orientation)
        {
            return GetAnimation(animationName)?.GetDirection(orientation);
        }

        /// <summary>
        /// Get the appropriate frame of the appropriate direction of the animation with the
        /// specified name (or the default animation if no animation with the given name exists)
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="orientation"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public TextureBrush GetFrame(string animationName, Angle orientation, double t)
        {
            return GetDirection(animationName, orientation)?.GetFrame(t);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Load sprite data from a sprite sheet
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="textureLoader"></param>
        /// <returns></returns>
        public SpriteDataCollection LoadSpriteSheet(FilePath filePath, ITextureLoader textureLoader)
        {
            var result = new SpriteDataCollection();
            using (var br = new StreamReader(filePath))
            {
                //Read line-by-line:
                SpriteData currentSprite = null;
                SpriteAnimation currentAnimation = null;
                SpriteAnimationDirection currentDirection = null;
                ITexture image = null;

                String line = br.ReadLine();
                if (line != null)
                {
                    //First line gives image filename
                    image = textureLoader.GetTexture(filePath.Directory + line);
                }
                while (line != null)
                {
                    line = br.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        //Store and clear current sprite data:
                        if (currentSprite != null)
                        {
                            result.Add(currentSprite);
                        }
                        currentSprite = null;
                        currentAnimation = null;
                    }
                    else
                    {
                        String[] splitLine = line.Split('\t'); // Split on tabs
                        if (currentSprite == null)
                        {
                            //Initialise sprite:
                            currentSprite = new SpriteData();
                            currentSprite.Name = line;
                            //currentSprite.Texture(image);
                        }
                        else if (currentAnimation == null || !splitLine.IsNumeric())
                        {
                            currentAnimation = new SpriteAnimation();
                            currentSprite.Animations.Add(line, currentAnimation);
                            currentDirection = null;
                        }
                        else if (currentDirection == null || splitLine.Length <= 1)
                        {
                            currentDirection = new SpriteAnimationDirection();
                            currentAnimation.Directions.Add(double.Parse(line), currentDirection);
                        }
                        else
                        {
                            TextureBrush frame = new TextureBrush(image, splitLine);
                            currentDirection.Frames.Add(frame);
                        }

                    }

                }
            }
            return result;
        }

        #endregion
    }
}
