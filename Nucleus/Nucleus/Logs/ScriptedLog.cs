using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// Log class which assembles messages based on a LogScript
    /// </summary>
    [Serializable]
    public abstract class ScriptedLog : IScriptedLog
    {
        #region Properties

        /// <summary>
        /// Private backer for the Script property
        /// </summary>
        private LogScript _Script;

        /// <summary>
        /// The script used to generate log messages
        /// </summary>
        public LogScript Script
        {
            get { return _Script; }
            set { _Script = value; }
        }

        /// <summary>
        /// Private backer for the RNG property
        /// </summary>
        private Random _RNG = new Random();

        /// <summary>
        /// The random number generator used to assemble script varients
        /// </summary>
        public Random RNG
        {
            get { return _RNG; }
            set { _RNG = value; }
        }

        public abstract bool IsBold { get; set; }
        public abstract bool IsItalicised { get; set; }


        private object _Author = null;

        /// <summary>
        /// The object representing the author of this log (if any).
        /// Used to automatically determine when first-person variations should be employed.
        /// </summary>
        public object Author
        {
            get { return _Author; }
            set { _Author = value; }
        }

        private object _Reader = null;

        /// <summary>
        /// The object representing the reader of this log (if any).
        /// Used to automatically determine when second-person variations should be employed.
        /// </summary>
        public object Reader
        {
            get { return _Reader; }
            set { _Reader = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.  Initialise from a log script
        /// </summary>
        /// <param name="script"></param>
        public ScriptedLog(LogScript script)
        {
            _Script = script;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Write a pre-scripted entry to the log based on a key and a set of subject objects
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subjects"></param>
        public void WriteScripted(string key, params object[] subjects)
        {
            var entry = Script.Entries[key];
            if (entry == null)
            {
                WriteText("KEY '" + key + "' NOT FOUND!");
            }
            else entry.PrintToLog(this, Author, Reader, RNG, subjects);
        }

        public abstract void WriteText(string text);

        /// <summary>
        /// Does this log have a pre-defined script for the specified key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasScriptFor(string key)
        {
            return Script.Entries.ContainsKey(key);
        }

        #endregion

    }
}
