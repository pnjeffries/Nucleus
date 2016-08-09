using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Exceptions
{
    /// <summary>
    /// Represents errors that occur during automated type conversion
    /// </summary>
    public class ConversionFailedException : Exception
    {
        public ConversionFailedException() : base() { }

        public ConversionFailedException(string message) : base(message) { }

        public ConversionFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
