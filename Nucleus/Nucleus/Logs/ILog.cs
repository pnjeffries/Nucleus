using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// An interface for message logs
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Write text to the log.
        /// </summary>
        /// <param name="text">The text string to write out</param>
        void WriteText(string text);
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
    }
}
