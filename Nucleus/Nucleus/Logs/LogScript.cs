using Nucleus.Base;
using Nucleus.IO;
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

        /// <summary>
        /// The sequence of characters which indicates that the rest of the line 
        /// is a comment and should be ignored.
        /// </summary>
        public const string COMMENT = "//";

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
        /// Load the script from a resource file via a loader
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="resources"></param>
        /// <returns></returns>
        public bool LoadFrom(FilePath filePath, IResourceLoader resources)
        {
            var text = resources.LoadString(filePath);
            if (text == null) return false;
            Load(text);
            return true;
        }

        /// <summary>
        /// Load the script directly from a text string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void Load(string text)
        {
            var reader = new StringReader(text);
            Load(reader);
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
                    if (line.StartsWith(COMMENT))
                    {
                        // Comment - do not process
                    }
                    else if (line.StartsWith(ENTRY_START))
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

        /// <summary>
        /// Get the default unprocessed text record for the entry under the
        /// specified key.  If no record is found the key itself will be returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetText(string key)
        {
            if (!Entries.ContainsKey(key)) return key;
            var entry = Entries[key];
            if (entry == null) return key;
            return entry.Variations.First(); //TODO: apply markup?
        }

        #endregion
    }
}
