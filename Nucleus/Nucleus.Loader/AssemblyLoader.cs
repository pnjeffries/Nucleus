using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Loader
{
    public static class AssemblyLoader
    {
        /// <summary>
        /// Load a nucleus assembly, first of all checking it against the version saved in the local appdata folder
        /// and using the most recent
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string InstallAssembly(string filePath)
        {
            string nucleusFolder = "/Nucleus/";
            string fileName = Path.GetFileName(filePath);
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + nucleusFolder;
            string filePathLocal = local + fileName;
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

    }
}
