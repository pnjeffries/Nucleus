using Nucleus.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Application
{
    /// <summary>
    /// Interface for application hosts.
    /// Application hosts are responsible for initialising the core
    /// instance and for linking to key interaction logic within
    /// the target application - for example selection of objects,
    /// 3D rendering, user interface creation and so on.
    /// </summary>
    public interface IHost
    {
        #region Methods

        /// <summary>
        /// 'Print' a message - displaying it in some form within the host
        /// application. 
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <returns>True if the message was printed successfully.</returns>
        bool Print(string message);

        /// <summary>
        /// Cause the host application to refresh any display elements which
        /// must be manually triggered.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Is the specified object currently not considered to be visible within the host environment?
        /// </summary>
        /// <param name="unique"></param>
        /// <returns></returns>
        bool IsHidden(Unique unique);

        /// <summary>
        /// Select the specified set of items in the host environment, if possible
        /// </summary>
        /// <param name="items">A list of items to select</param>
        /// <returns></returns>
        bool Select(IList items, bool clear = false);

        #endregion
    }
}
