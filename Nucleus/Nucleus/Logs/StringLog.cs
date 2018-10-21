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
        #region Properties

        /// <summary>
        /// The backing StringBuilder
        /// </summary>
        private StringBuilder _Builder = new StringBuilder();

        bool ILog.IsBold { get { return false; } set { } }
        bool ILog.IsItalicised { get { return false; } set { } }

        #endregion

        public void WriteText(string text)
        {
            _Builder.Append(text);
        }

        public override string ToString()
        {
            return _Builder.ToString();
        }

        void ILog.WriteText(string text)
        {
            throw new NotImplementedException();
        }
    }
}
