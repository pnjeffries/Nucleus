using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// Class to parse markup tags in log script entries
    /// </summary>
    public class LogScriptMarkupParser
    {
        #region Constants

        /// <summary>
        /// The character which depicts the start of a markup expression
        /// </summary>
        public const char MARKUP_OPEN = '<';

        /// <summary>
        /// The character which depicts the end of a markup expression
        /// </summary>
        public const char MARKUP_CLOSE = '>';

        /// <summary>
        /// The character which depicts the start of a set of function arguments
        /// </summary>
        public const char ARGUMENT_OPEN = '[';

        /// <summary>
        /// The character which depicts the end of a set of function arguments
        /// </summary>
        public const char ARGUMENT_CLOSE = ']';

        /// <summary>
        /// The character which separates 
        /// </summary>
        public const char ARGUMENT_SEPARATOR = '|';

        #endregion

        #region Properties

        /// <summary>
        /// Private backing member variable for the Log property
        /// </summary>
        private ILog _Log;

        /// <summary>
        /// The log to write to
        /// </summary>
        public ILog Log
        {
            get { return _Log; }
            set { _Log = value; }
        }

        /// <summary>
        /// Private backing member variable for the Subjects property
        /// </summary>
        private object[] _Subjects;

        /// <summary>
        /// The collection of objects to be used as source data
        /// </summary>
        public object[] Subjects
        {
            get { return _Subjects; }
            set { _Subjects = value; }
        }

        /// <summary>
        /// Private backing member variable for the RNG property
        /// </summary>
        private Random _RNG;

        /// <summary>
        /// The random number generator used to randomly determine variations
        /// </summary>
        public Random RNG
        {
            get
            {
                if (_RNG == null) _RNG = new Random();
                return _RNG;
            }
            set { _RNG = value; }
        }

        #endregion

        #region Constructors

        public LogScriptMarkupParser(ILog log, object[] subjects)
        {
            Log = log;
            Subjects = subjects;
        }

        #endregion

        #region Methods

        public void WriteToLog(string markup)
        {
            int i = 0;
            
            while (i < markup.Length)
            {
                int iS = i;
                string nextMarkup = markup.NextBracketed(ref i, MARKUP_OPEN, MARKUP_CLOSE);

                // Write preceding text:
                string text = markup.Substring(iS, i - iS - 1);
                if (!string.IsNullOrEmpty(text))
                    Log.Write(text);

                if (nextMarkup != null)
                {
                    string expanded = ParseTag(nextMarkup, 1);

                    i += nextMarkup.Length + 1;
                }

            }
        }

        public string ParseTag(string markup, int pass)
        {
            int iS = 0;
            string argumentBit = markup.NextBracketed(ref iS, ARGUMENT_OPEN, ARGUMENT_CLOSE);
            string functionName = markup.Substring(0, iS);

            // In the first pass, tags with arguments will be processed:
            if (argumentBit != null)
            {
                string[] arguments = argumentBit.Split(ARGUMENT_SEPARATOR);
                for (int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = ParseArgument(arguments[i]);
                }
            }
        }

        public string ParseArgument(string argument)
        {
            string subTag = argument.Next
        }

        #endregion


    }
}
