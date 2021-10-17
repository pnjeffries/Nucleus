using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Modals
{
    /// <summary>
    /// A back-end representation of a modal window.
    /// This allows for window content and functionality to be defined
    /// within the 'business logic', however the display and implementation
    /// of the window itself is the responsibility of the front-end
    /// </summary>
    [Serializable]
    public class ModalWindow : Named
    {
        private UniquesCollection _Contents = new UniquesCollection();

        /// <summary>
        /// The main contents of the window
        /// </summary>
        public UniquesCollection Contents
        {
            get { return _Contents; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModalWindow() { }

        /// <summary>
        /// Title constructor
        /// </summary>
        /// <param name="title"></param>
        public ModalWindow(string title) : base(title) { }
    }
}
