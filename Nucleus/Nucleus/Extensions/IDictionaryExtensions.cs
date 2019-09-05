using Nucleus.Base;
using Nucleus.IO;
using Nucleus.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Extension methods for the IDictionary interface
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Get the range of keys in this dictionary
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static Interval KeyRange<TValue>(this IDictionary<double, TValue> dictionary)
        {
            Interval result = Interval.Unset;
            foreach (KeyValuePair<double, TValue> kvp in dictionary)
            {
                if (!result.IsValid) result = new Maths.Interval(kvp.Key);
                else result = result.Include(kvp.Key);
            }
            return result;
        }

        /// <summary>
        /// Load in string data from a CSV file and use it to populate this dictionary.
        /// The keys are read from the first column and the corresponding value from the
        /// second.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="filePath">The path of the file to read from.</param>
        /// <param name="delimiter">The delimiter character used to separate columns.</param>
        /// <param name="startAtLine">Start reading from the specifed line.  This
        /// may be used to ignore column titles</param>
        public static void PopulateFromCSV(this IDictionary<string, string> dictionary, FilePath filePath, char delimiter = ',', 
            int startAtLine = 0)
        {
            var csvReader = new CSVReader();
            csvReader.Delimiter = delimiter;
            var data = csvReader.ReadToLists(filePath);
            foreach (IList<string> line in data)
            {
                if (line.Count > 1)
                {
                    string key = line[0];
                    string value = line[1];
                    dictionary[key] = value;
                }
            }
        }

        /// <summary>
        /// Load in string data from a CSV file and use it to populate this dictionary.
        /// The keys are read from the first column and the corresponding value from the
        /// second.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="delimiter">The delimiter character used to separate columns.</param>
        /// <param name="startAtLine">Start reading from the specifed line.  This
        /// may be used to ignore column titles</param>
        public static void PopulateFromCSV(this IDictionary<string, string> dictionary, System.IO.Stream stream, char delimiter = ',',
            int startAtLine = 0)
        {
            var csvReader = new CSVReader();
            csvReader.Delimiter = delimiter;
            var data = csvReader.ReadToLists(stream);
            foreach (IList<string> line in data)
            {
                if (line.Count > 1)
                {
                    string key = line[0];
                    string value = line[1];
                    dictionary[key] = value;
                }
            }
        }
    }
}
