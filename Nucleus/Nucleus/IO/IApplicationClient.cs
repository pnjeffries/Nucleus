using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// Interface for classes which control an external application and provide access to the
    /// file handling functionality of that application.
    /// </summary>
    public interface IApplicationClient
    {
        /// <summary>
        /// Open the file at the specified filepath in the target application
        /// </summary>
        /// <param name="filePath">The filepath to open.  (Note that this can be expressed as a string)</param>
        /// <returns>True if the specified file could be opened, false if this was prevented in some way.</returns>
        bool Open(FilePath filePath);

        /// <summary>
        /// Open a new file in the target application
        /// </summary>
        /// <returns></returns>
        bool New();

        /// <summary>
        /// Save the currently open file to the specified file location
        /// </summary>
        /// <param name="filePath">The filepath to save to</param>
        /// <returns></returns>
        bool Save(FilePath filePath);

        /// <summary>
        /// Close the currently open file in the target application
        /// </summary>
        void Close();
    }
}
