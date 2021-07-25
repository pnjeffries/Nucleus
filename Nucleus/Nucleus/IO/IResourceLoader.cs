using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// Interface for loading resources
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// Load a string from a resource at the specified path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string LoadString(FilePath filePath);
    }
}
