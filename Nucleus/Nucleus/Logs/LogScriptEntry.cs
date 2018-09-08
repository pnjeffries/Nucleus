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

        public bool PrintToLog(ILog log, Random rng, params object[] subjects)
        {
            if (Variations.Count > 0)
            {
                string markup = Variations[rng.Next(Variations.Count)];

                return true;
            }
            else return false;
        }



        #endregion

    }
}
