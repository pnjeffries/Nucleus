using Nucleus.Logs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Logs
{
    /// <summary>
    /// A log which describe in-game player actions
    /// </summary>
    [Serializable]
    public class ActionLog : ScriptedLog, IActionLog
    {
        private ObservableCollection<ActionLogMessage> _Messages = new ObservableCollection<ActionLogMessage>();

        public ObservableCollection<ActionLogMessage> Messages
        {
            get { return _Messages; }
        }

        public override bool IsBold { get => false; set { } }
        public override bool IsItalicised { get => false; set { } }

        /// <summary>
        /// The message currently being written to.
        /// </summary>
        [NonSerialized]
        private ActionLogMessage _CurrentMessage = null;

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="script"></param>
        public ActionLog(LogScript script, object author = null, object reader = null) : base(script)
        {
            Author = author;
            Reader = reader;
        }

        /// <summary>
        /// Write text to the log
        /// </summary>
        /// <param name="text"></param>
        public override void WriteText(string text)
        {
            var tokens = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for(int i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                if (i > 0) _CurrentMessage = null;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    if (_CurrentMessage == null)
                    {
                        _CurrentMessage = new ActionLogMessage(token);
                        Messages.Add(_CurrentMessage);
                    }
                    else
                    {
                        // Automatically add a space if the current text exists. (Review?)
                        if (!string.IsNullOrWhiteSpace(_CurrentMessage.Text)) _CurrentMessage.Text += " ";

                        _CurrentMessage.Text += token;
                    }
                }
            }
        }
    }
}
