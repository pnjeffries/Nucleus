using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// A class to help read CSV files
    /// </summary>
    [Serializable]
    public class CSVReader : MessageRaiser
    {
        #region Properties

        /// <summary>
        /// The delimiting character.  By default, this is a comma.
        /// </summary>
        public char Delimiter { get; set; } = ',';

        #endregion

        #region Methods

        /// <summary>
        /// Read the CSV file at the specified location to a list of lists of strings
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IList<IList<string>> ReadToLists(FilePath filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var result = ReadToLists(reader);
                reader.Close();
                return result;
            }
        }

        /// <summary>
        /// Read the CSV stream to a list of lists of strings
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IList<IList<string>> ReadToLists(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var result = ReadToLists(reader);
                reader.Close();
                return result;
            }
        }

        /// <summary>
        /// Read CSV-format data from a TextReader to a list of lists of strings
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public IList<IList<string>> ReadToLists(TextReader reader)
        {
            var result = new List<IList<string>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                result.Add(line.Split(Delimiter));
            }
            return result;
        }

        #endregion
    }
}
