using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// An entry in a log script which defines variations on a message.
    /// </summary>
    [Serializable]
    public class LogScriptEntry
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Variations property
        /// </summary>
        private IList<string> _Variations = new List<string>();

        /// <summary>
        /// The possible variations of log entry message, in raw markup form.
        /// </summary>
        public IList<string> Variations
        {
            get { return _Variations; }
            set { _Variations = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Print a randomised variant of this entry to the specified log
        /// </summary>
        /// <param name="log"></param>
        /// <param name="rng"></param>
        /// <param name="subjects"></param>
        /// <returns></returns>
        public bool PrintToLog(ILog log, Random rng, params object[] subjects)
        {
            return PrintToLog(log, (object)null, null, rng, subjects);
        }

        /// <summary>
        /// Print a randomised variant of this entry to the specified log
        /// </summary>
        /// <param name="log"></param>
        /// <param name="rng"></param>
        /// <param name="subjects"></param>
        /// <returns></returns>
        public bool PrintToLog(ILog log, object author, object reader, Random rng, params object[] subjects)
        {
            if (Variations.Count > 0)
            {
                string markup = Variations[rng.Next(Variations.Count)];
                log.WriteMarkup(markup, author, reader, rng, subjects);
                return true;
            }
            else return false;
        }

        #endregion

    }
}
