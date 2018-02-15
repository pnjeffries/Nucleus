using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Rendering
{
    /// <summary>
    /// Abstract generic base class to give a headstart on implementing an ITextureLoader
    /// </summary>
    /// <typeparam name="TTexture"></typeparam>
    public abstract class TextureLoaderBase<TTexture> : ITextureLoader
        where TTexture : ITexture
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Loaded property
        /// </summary>
        private Dictionary<string, TTexture> _Loaded = new Dictionary<string, TTexture>();

        /// <summary>
        /// The cached library of loaded textures
        /// </summary>
        public Dictionary<string, TTexture> Loaded
        {
            get { return _Loaded; }

        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the texture indicated by the specified reference string.
        /// This will be loaded as necessary and possible.
        /// </summary>
        /// <param name="textureReference"></param>
        /// <returns></returns>
        public ITexture GetTexture(string resourceRef)
        {
            if (_Loaded.ContainsKey(resourceRef)) return _Loaded[resourceRef];
            else
            {
                TTexture tex = LoadTexture(resourceRef);
                if (tex != null) _Loaded.Add(resourceRef, tex);
                return tex;
            }
        }

        /// <summary>
        /// Load a texture from a resource.
        /// </summary>
        /// <param name="resourceRef"></param>
        /// <returns></returns>
        public abstract TTexture LoadTexture(string resourceRef);

        #endregion
    }
}
