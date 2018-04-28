using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// Log class which assembles messages into a single string
    /// via an internal StringBuilder
    /// </summary>
    public class StringLog : ILog
    {
        /// <summary>
        /// The backing StringBuilder
        /// </summary>
        private StringBuilder _Builder = new StringBuilder();

        public void WriteText(string text)
        {
            _Builder.Append(text);
        }

        public override string ToString()
        {
            return _Builder.ToString();
        }
    }
}
