using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Exceptions
{
    /// <summary>
    /// Exception that occurs during expression evaluation
    /// </summary>
    [Serializable]
    public class ExpressionException : Exception
    {
        public ExpressionException()
        {
        }

        public ExpressionException(string message) : base(message)
        {
        }

        public ExpressionException(string message, Exception innerException) : base(message, innerException)
        {
        }
#if !JS
        protected ExpressionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}
