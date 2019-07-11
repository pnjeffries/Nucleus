using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// An object which represents a script which is to be drawn upon
    /// to automatically populate log messages
    /// </summary>
    [Serializable]
    public class LogScript
    {
        #region Constants

        /// <summary>
        /// The sequence of characters which depicts an entry key when 
        /// placed at the start of a line.
        /// </summary>
        public const string ENTRY_START = "~~~";

        #endregion

        #region Properties

        /// <summary>
        /// Private backing member variable for the Entries property
        /// </summary>
        private Dictionary<string, LogScriptEntry> _Entries = new Dictionary<string, LogScriptEntry>();

        /// <summary>
        /// The dictionary of entries, keyed by a reference string
        /// </summary>
        public Dictionary<string, LogScriptEntry> Entries
        {
            get { return _Entries; }
            set { _Entries = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load the script from a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool LoadFrom(FilePath filePath)
        {
            if (filePath.Exists)
            {
                var reader = new StreamReader(filePath);
                Load(reader);
                reader.Close();
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Load the script via a text reader.
        /// </summary>
        /// <param name="reader"></param>
        public void Load(TextReader reader)
        {
            LogScriptEntry currentEntry = null;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (line.StartsWith(ENTRY_START))
                    {
                        // New entry record
                        string key = line.Substring(ENTRY_START.Length);
                        if (!Entries.ContainsKey(key))
                        {
                            currentEntry = new LogScriptEntry();
                            Entries.Add(key, currentEntry);
                        }
                        else currentEntry = Entries[key];
                    }
                    else if (currentEntry != null)
                    {
                        currentEntry.Variations.Add(line);
                    }
                }
            }
        }
        #endregion
    }
}
