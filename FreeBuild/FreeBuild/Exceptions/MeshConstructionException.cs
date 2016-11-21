using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Exceptions
{
    /// <summary>
    /// Represents errors that occur during mesh building
    /// </summary>
    public class MeshConstructionException : Exception
    {
        public MeshConstructionException() : base() { }

        public MeshConstructionException(string message) : base(message) { }

        public MeshConstructionException(string message, Exception inner) : base(message, inner) { }
    }
}
