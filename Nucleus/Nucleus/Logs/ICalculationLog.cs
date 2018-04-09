using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// A message log object that can be used to output records of calculations
    /// </summary>
    public interface ICalculationLog : ILog
    {
        /// <summary>
        /// Write text to the log as a superscript
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        ICalculationLog Superscript(string text);

        /// <summary>
        /// Write text to the log as a subscript
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        ICalculationLog Subscript(string text);

        /// <summary>
        /// Write a symbol to the log
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICalculationLog Symbol(string name);

        /// <summary>
        /// Add a diagram image to the log
        /// </summary>
        /// <param name="resourceURI"></param>
        /// <returns></returns>
        ICalculationLog Diagram(string resourceURI);
    }
}
