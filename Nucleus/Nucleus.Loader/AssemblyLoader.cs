using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Loader
{
    /// <summary>
    /// Helper class to aid with installing and loading the most recent version of shared DLLs and resource files
    /// in order to avoid clashes when multiple Nucleus-using applications or plugins are active within the same
    /// process.
    /// </summary>
    public static class AssemblyLoader
    {

        /// <summary>
        /// Install a nucleus assembly to the shared install location, if it is a more recent
        /// version than any existing assembly with the same name already saved in the same location.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string InstallAssembly(string filePath)
        {
            string filePathLocal = AssemblyInstallLocation(filePath);
            if (File.Exists(filePath))
            {
                if (!File.Exists(filePathLocal))
                {
                    File.Copy(filePath, filePathLocal);
                    return "Assembly installed to '" + filePathLocal + "'.";
                }
                else
                {
                    FileInfo fInfo = new FileInfo(filePath);
                    FileInfo fInfoLocal = new FileInfo(filePathLocal);
                    if (fInfo.LastWriteTimeUtc > fInfoLocal.LastWriteTimeUtc)
                    {
                        File.Copy(filePath, filePathLocal); //Will this overwrite?  In theory yes...
                        return "Assembly updated at '" + filePathLocal + "'.";
                    }
                }
            }

            return "Error: Assembly '" + filePath + "' could not be found!";
        }

        /// <summary>
        /// Get the install location for the specified DLL or resource
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string AssemblyInstallLocation(string filePath)
        {
            string nucleusFolder = "/Nucleus/";
            string fileName = Path.GetFileName(filePath);
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + nucleusFolder;
            string filePathLocal = local + fileName;
            return filePathLocal;
        }

    }
}
