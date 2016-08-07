using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Exceptions
{
    /// <summary>
    /// Represents errors that occur when a single geometry object is assigned to
    /// more than one element as its key set-out geometry
    /// </summary>
    public class NonExclusiveGeometryException : Exception
    {
        public NonExclusiveGeometryException()
        {
        }

        public NonExclusiveGeometryException(string message)
        : base(message)
        {
        }

        public NonExclusiveGeometryException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
