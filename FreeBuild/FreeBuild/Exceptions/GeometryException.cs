using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Exceptions
{
    /// <summary>
    /// Represents errors that occur during geometric calculation, typically because
    /// the operation to be performed is not possible with the input data
    /// </summary>
    public class GeometryException : Exception
    {
        public GeometryException()
        {
        }

        public GeometryException(string message)
        : base(message)
        {
        }

        public GeometryException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
