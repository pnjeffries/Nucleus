using Nucleus.Base;
using Nucleus.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.UI
{
    /// <summary>
    /// Class to store data about the display names and tooltips
    /// associated with a set of named parameters or tools
    /// </summary>
    [Serializable]
    public class TextDisplayData
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Delimiter property
        /// </summary>
        private char _Delimiter = '\t';

        /// <summary>
        /// The delimiter used when reading display data in from a text file
        /// </summary>
        public char Delimiter
        {
            get { return _Delimiter; }
            set { _Delimiter = value; }
        }

        /// <summary>
        /// Private backing member variable for the DisplayNames property
        /// </summary>
        private IDictionary<string, string> _DisplayNames = new Dictionary<string, string>();

        /// <summary>
        /// The dictionary of parameter names to display name equivalents
        /// </summary>
        public IDictionary<string, string> DisplayNames
        {
            get { return _DisplayNames; }

        }

        /// <summary>
        /// Private backing member variable for the ToolTips property
        /// </summary>
        private IDictionary<string, string> _ToolTips = new Dictionary<string, string>();

        /// <summary>
        /// The dictionary of parameter names to tooltips
        /// </summary>
        public IDictionary<string, string> ToolTips
        {
            get { return _ToolTips; }

        }

        #endregion

        #region Methods

        /// <summary>
        /// Add an entry to the data store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="displayName"></param>
        /// <param name="toolTip"></param>
        public void Add(string key, string displayName, string toolTip)
        {
            DisplayNames[key] = displayName;
            ToolTips[key] = toolTip;
        }

        /// <summary>
        /// Clear all previous records
        /// </summary>
        public void Clear()
        {
            DisplayNames.Clear();
            ToolTips.Clear();
        }

        /// <summary>
        /// Load this display data from a text file, separated by the delimiter
        /// character specifed by the Delimiter property
        /// in the format KEY [DELIMITER] DISPLAY NAME [DELIMITER] TOOLTIP
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadFromFile(FilePath filePath)
        {
            var csvReader = new CSVReader();
            csvReader.Delimiter = Delimiter; 
            var data = csvReader.ReadToLists(filePath);
            foreach (IList<string> line in data)
            {
                if (line.Count > 1)
                {
                    string key = line[0];
                    string displayName = line[1];
                    if (!string.IsNullOrWhiteSpace(displayName))
                    {
                        DisplayNames[key] = displayName;
                    }
                    if (line.Count > 2)
                    {
                        string tooltip = line[2];
                        ToolTips[key] = tooltip;
                    }
                }
            }
        }

        /// <summary>
        /// Convert this data set to a delimiter-separated string,
        /// suitable for writing out to a text file.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            // Title bar
            sb.Append("[KEY NAME]");
            sb.Append(Delimiter);
            sb.Append("[DISPLAY NAME]");
            sb.Append(Delimiter);
            sb.Append("[TOOLTIP]");
            sb.AppendLine();

            foreach (var kvp in DisplayNames)
            {
                sb.Append(kvp.Key);
                sb.Append(Delimiter);
                sb.Append(kvp.Value);
                if (ToolTips.ContainsKey(kvp.Key))
                {
                    sb.Append(Delimiter);
                    sb.Append(ToolTips[kvp.Key]);
                }
                sb.AppendLine();
            }
            // Write any tooltips that were missed
            foreach (var kvp in ToolTips)
            {
                if (!DisplayNames.ContainsKey(kvp.Key))
                {
                    sb.Append(kvp.Key);
                    sb.Append(Delimiter);
                    sb.Append(Delimiter);
                    sb.Append(kvp.Value);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get the display name for the specified key name.
        /// If no display name is stored for the key the
        /// key value itself will be returned.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDisplayName(string key)
        {
            if (DisplayNames.ContainsKey(key)) return DisplayNames[key];
            else return key;
        }

        /// <summary>
        /// Get the display name for the specifed parameter.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetDisplayName(Parameter param)
        {
            return GetDisplayName(param.Name);
        }

        /// <summary>
        /// Get the tooltip for the specified key name.
        /// Will return null if no tooltip is stored for the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetToolTip(string key)
        {
            if (ToolTips.ContainsKey(key)) return ToolTips[key];
            else return null;
        }

        /// <summary>
        /// Get the stored tooltip for the specified parameter
        /// (if one exists).  Will return null if no tooltip is
        /// stored for this parameter.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetToolTip(Parameter param)
        {
            return GetToolTip(param.Name);
        }

        #endregion
    }
}
