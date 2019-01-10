using Nucleus.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Nucleus.WPF
{
    [Serializable]
    public class WPFTextureLoader : TextureLoaderBase<WPFTexture>
    {
        public override WPFTexture LoadTexture(string resourceRef)
        {
            return new WPFTexture(new BitmapImage(new Uri(resourceRef)));
        }
    }
}
