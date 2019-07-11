using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Alerts
{
    /// <summary>
    /// Enumerated value to represent different levels of alerts
    /// </summary>
    public enum AlertLevel
    {
        /// <summary>
        /// The alert relays information
        /// </summary>
        Information = 0,

        /// <summary>
        /// The alert is a warning drawing attention to a potential problem
        /// in the process but which does not necessarily invalidate the results
        /// </summary>
        Warning = 100,

        /// <summary>
        /// The alert draws attention to a major problem which may need
        /// the process to be halted.
        /// </summary>
        Error = 200,

        /// <summary>
        /// The alert indicates a condition has been passed
        /// </summary>
        Pass = 500,

        /// <summary>
        /// The alert indicates that a condition has been failed
        /// </summary>
        Fail = 510
    }
}
