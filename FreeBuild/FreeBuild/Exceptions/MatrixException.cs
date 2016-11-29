using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Exceptions
{
    /// <summary>
    /// Represents errors that occur during matrix mathematics operations
    /// </summary>
    public class MatrixException : Exception
    {
        public MatrixException()
        {
        }

        public MatrixException(string message)
        : base(message)
        {
        }

        public MatrixException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
