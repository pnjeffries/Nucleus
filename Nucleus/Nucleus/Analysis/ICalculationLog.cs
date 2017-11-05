using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    /// <summary>
    /// A message log object that can be used to output records of calculations
    /// </summary>
    public interface ICalculationLog
    {
        /// <summary>
        /// Write text to the log.
        /// </summary>
        /// <param name="text">The text string to write out</param>
        /// <returns>A reference to this instance of the log, to allow chaining.</returns>
        ICalculationLog Write(string text);

        /// <summary>
        /// Write a line of text to the log, followed by a carriage return.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        ICalculationLog WriteLine(string text = "");
    }
}
