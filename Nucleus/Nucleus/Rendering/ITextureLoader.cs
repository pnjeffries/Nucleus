using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// Interface for classes which can be used to load textures
    /// </summary>
    public interface ITextureLoader
    {
        /// <summary>
        /// Get the texture indicated by the specified reference string
        /// </summary>
        /// <param name="textureReference"></param>
        /// <returns></returns>
        ITexture GetTexture(string resourceRef);
    }

    public static class TextureLoader
    {
        #region Static Properties

        /// <summary>
        /// Get or set the current texture loader.
        /// </summary>
        public static ITextureLoader Current { get; set; } = null;

        #endregion
    }
}
