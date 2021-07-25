using Nucleus.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// An interface for message logs to which text can be written.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Write text to the log.
        /// </summary>
        /// <param name="text">The text string to write out</param>
        /// <remarks>It is recommended to use the extension method 'Write' instead
        /// of using this directly as that function also returns a reference to
        /// the log itself to allow chaining.</remarks>
        void WriteText(string text);

        /// <summary>
        /// Get or set whether text written to the log currently will be displayed in bold.
        /// Some log types do not support bold text and setting this property will do nothing.
        /// </summary>
        bool IsBold { get; set; }

        /// <summary>
        /// Get or set whether text written to the log subsequently will be displayed in italics.
        /// Some log types do not support italics and setting this property will do nothing.
        /// </summary>
        bool IsItalicised { get; set; }

    }

    /// <summary>
    /// Extension methods for log objects
    /// </summary>
    public static class ILogExtensions
    {
        /// <summary>
        /// Write text to the log.
        /// </summary>
        /// <param name="text">The text string to write out</param>
        /// <returns>A reference to this instance of the log, to allow chaining.</returns>
        public static TLog Write<TLog>(this TLog log, string text)
            where TLog : ILog
        {
            log.WriteText(text);
            return log;
        }

        /// <summary>
        /// Write a line of text to the log, followed by a carriage return.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static TLog WriteLine<TLog>(this TLog log, string text = "")
            where TLog : ILog
        {
            log.WriteText(text);
            log.WriteText(Environment.NewLine);
            return log;
        }

        /// <summary>
        /// Write text to the log from a string of text containing markup.
        /// Returns a reference to this log to enable chaining.
        /// </summary>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="log"></param>
        /// <param name="markup"></param>
        /// <param name="rng"></param>
        /// <param name="subjects"></param>
        /// <returns></returns>
        public static TLog WriteMarkup<TLog>(this TLog log, string markup, Random rng, params object[] subjects)
            where TLog : ILog
        {
            var parser = new LogScriptMarkupParser(log, subjects, rng);
            parser.WriteToLog(markup);
            return log;
        }

        /// <summary>
        /// Write text to the log from a string of text containing markup.
        /// Returns a reference to this log to enable chaining.
        /// </summary>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="log"></param>
        /// <param name="reader"></param>
        /// <param name="markup"></param>
        /// <param name="rng"></param>
        /// <param name="subjects"></param>
        /// <returns></returns>
        public static TLog WriteMarkup<TLog>(this TLog log, string markup, object reader, Random rng, params object[] subjects)
            where TLog : ILog
        {
            var parser = new LogScriptMarkupParser(log, subjects, rng);
            parser.Reader = reader;
            parser.WriteToLog(markup);
            return log;
        }

        /// <summary>
        /// Write text to the log from a string of text containing markup.
        /// Returns a reference to this log to enable chaining.
        /// </summary>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="log"></param>
        /// <param name="author"></param>
        /// <param name="reader"></param>
        /// <param name="markup"></param>
        /// <param name="rng"></param>
        /// <param name="subjects"></param>
        /// <returns></returns>
        public static TLog WriteMarkup<TLog>(this TLog log, string markup, object author, object reader, Random rng, params object[] subjects)
            where TLog : ILog
        {
            var parser = new LogScriptMarkupParser(log, subjects, rng);
            parser.Author = author;
            parser.Reader = reader;
            parser.WriteToLog(markup);
            return log;
        }

        /// <summary>
        /// Switch whether text subsequently written to the log should be displayed
        /// with a bold weight.  Returns a reference to this log to enable chaining.
        /// </summary>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="log"></param>
        /// <param name="value">If true, subsequent text will be bold, if
        /// false it will not.</param>
        /// <returns></returns>
        public static TLog Bold<TLog>(this TLog log, bool value = true)
            where TLog : ILog
        {
            log.IsBold = value;
            return log;
        }

        /// <summary>
        /// Switch whether text subsequently written to the log should be displayed
        /// in italics.  Returns a reference to this log to enable chaining.
        /// </summary>
        /// <typeparam name="TLog"></typeparam>
        /// <param name="log"></param>
        /// <param name="value">If true, subsequent text will be italicised, if
        /// false it will not.</param>
        /// <returns></returns>
        public static TLog Italics<TLog>(this TLog log, bool value = true)
            where TLog : ILog
        {
            log.IsItalicised = value;
            return log;
        }
    }
}
