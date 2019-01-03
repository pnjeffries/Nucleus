using Nucleus.Base;
using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// The character which depicts the start of 
        /// a formatting markup expression
        /// </summary>
        public const char MARKUP_OPEN = '<';

        /// <summary>
        /// The character which depicts the end of 
        /// a formatting markup expression
        /// </summary>
        public const char MARKUP_CLOSE = '>';

        /// <summary>
        /// The character which depicts the start of
        /// a function markup expression
        /// </summary>
        public const char FUNCTION_OPEN = '{';

        /// <summary>
        /// The character which depicts the end of
        /// a function markup expression
        /// </summary>
        public const char FUNCTION_CLOSE = '}';

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

        public LogScriptMarkupParser(ILog log, object[] subjects, Random rng) : this(log,subjects)
        {
            RNG = rng;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parse and write a markup string to the log
        /// </summary>
        /// <param name="markup"></param>
        public void WriteToLog(string markup)
        {
            string expanded = ExpandMarkup(markup);

            int i = 0;
            while (i < expanded.Length)
            {
                int iS = i;
                string nextMarkup = expanded.NextBracketed(ref i, MARKUP_OPEN, MARKUP_CLOSE);
                if (nextMarkup == null) i = expanded.Length;

                // Write preceding text:
                int length = i - iS;
                string text = null;
                if (length > 0)  text = expanded.Substring(iS, length);
                if (!string.IsNullOrEmpty(text))
                    Log.Write(text);

                if (nextMarkup != null)
                {
                    ExecuteTag(nextMarkup, 1);
                    i += nextMarkup.Length + 2;
                }
            }
        }

        /// <summary>
        /// Expand any expandable tags within a markup string
        /// </summary>
        /// <param name="markup"></param>
        /// <returns></returns>
        public string ExpandMarkup(string markup)
        {
            int i = 0;
            var sb = new StringBuilder();

            while (i < markup.Length)
            {
                int iS = i;
                string nextMarkup = markup.NextBracketed(ref i, FUNCTION_OPEN, FUNCTION_CLOSE);
                if (nextMarkup == null) i = markup.Length;

                // Write preceding text:
                string text = markup.Substring(iS, i - iS);
                if (!string.IsNullOrEmpty(text))
                    sb.Append(text);

                if (nextMarkup != null)
                {
                    string expanded = ExecuteTag(nextMarkup, 0);
                    sb.Append(expanded);
                    i += nextMarkup.Length + 2;
                }

            }
            return sb.ToString();
        }

        /// <summary>
        /// Execute a markup tag.  In the first pass (pass = 0) expandable
        /// tags will be expanded.  In the second executable tags will executed.
        /// </summary>
        /// <param name="markup"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public string ExecuteTag(string markup, int pass)
        {
            int iS = 0;
            string argumentBit = markup.NextBracketed(ref iS, ARGUMENT_OPEN, ARGUMENT_CLOSE);
            string functionName = markup.Substring(0, iS).ToUpper().Replace('/', '_');

            // First we check if any method with that name exist and if so its return type
            MethodInfo method = GetType().GetMethod(functionName, new Type[] { });

            // Don't invoke method if it is not expandable (doesn't return a string) & this is the
            // first pass.
            if (method == null || (method.ReturnType.IsAssignableFrom(typeof(string)) || pass > 0))
            {
                // Refine the function call by checking the arguments:

                string[] arguments = null;

                // In the first pass, tags with arguments will be processed:
                if (argumentBit != null)
                {
                    arguments = argumentBit.Split(ARGUMENT_SEPARATOR);
                    Type[] types = new Type[arguments.Length];
                    for (int i = 0; i < arguments.Length; i++)
                    {
                        arguments[i] = ExpandMarkup(arguments[i]);
                        types[i] = typeof(string);
                    }

                    method = GetType().GetMethod(functionName, types);
                }
                else
                {
                    method = GetType().GetMethod(functionName, Type.EmptyTypes);
                }

                if (method != null)
                {
                    return method.Invoke(this, arguments)?.ToString();
                }
            }

            return markup;
        }

        #endregion

        #region Markup Tag Methods

        /// <summary>
        /// Bold on
        /// </summary>
        public void B()
        {
            Log.Bold(true);
        }

        /// <summary>
        /// Bold off
        /// </summary>
        public void _B()
        {
            Log.Bold(false);
        }

        /// <summary>
        /// Italics on
        /// </summary>
        public void I()
        {
            Log.Italics(true);
        }

        /// <summary>
        /// Italics off
        /// </summary>
        public void _I()
        {
            Log.Italics(false);
        }

        /// <summary>
        /// Select a random value
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public string RANDOM(string arg1, string arg2)
        {
            return RANDOMInternal(arg1, arg2);
        }

        /// <summary>
        /// Select a random value
        /// </summary>
        /// <returns></returns>
        public string RANDOM(string arg1, string arg2, string arg3)
        {
            return RANDOMInternal(arg1, arg2, arg3);
        }

        /// <summary>
        /// Select a random value
        /// </summary>
        /// <returns></returns>
        public string RANDOM(string arg1, string arg2, string arg3, string arg4)
        {
            return RANDOMInternal(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Select a random value
        /// </summary>
        /// <returns></returns>
        public string RANDOM(string arg1, string arg2, string arg3, string arg4, string arg5)
        {
            return RANDOMInternal(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Select a random value
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private string RANDOMInternal(params string[] args)
        {
            return args[RNG.Next(args.Length)];
        }

        /// <summary>
        /// Retrieve a value from the specified subject
        /// </summary>
        /// <param name="index"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string SUBJECT(string index, string path)
        {
            try
            {
                int i = int.Parse(index);
                var obj = Subjects[i];
                return obj.GetFromPath(path).ToString();
            }
            catch { }
            return "!ERROR!"; //TODO: Review - fallback to ""?
        }

        /// <summary>
        /// Select a variation based on the gender of a subject
        /// </summary>
        /// <param name="index"></param>
        /// <param name="neutral"></param>
        /// <param name="masculine"></param>
        /// <param name="feminine"></param>
        /// <returns></returns>
        public string GENDER(string index, string neutral, string masculine, string feminine)
        {
            try
            {
                int i = int.Parse(index);
                Gender gender = Gender.Neutral;
                if (i >= 0 && i < Subjects.Length)
                {
                    object subject = Subjects[i];
                    gender = GenderHelper.GenderOf(subject);
                }
                if (gender == Gender.Masculine) return masculine;
                else if (gender == Gender.Feminine) return feminine;
            }
            catch { }

            return neutral;
        }

        #endregion

    }
}
