using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Logs
{
    /// <summary>
    /// A message in an action log
    /// </summary>
    [Serializable]
    public class ActionLogMessage : Unique
    {
        private string _Text;

        /// <summary>
        /// The text of the message
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { ChangeProperty(ref _Text, value); }
        }

        public ActionLogMessage(string text)
        {
            _Text = text;
        }
    }
}
