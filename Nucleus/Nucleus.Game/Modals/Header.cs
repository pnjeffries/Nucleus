using Nucleus.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Modals
{
    /// <summary>
    /// A header-style segment of text
    /// </summary>
    [Serializable]
    public class Header : Paragraph
    {
        /// <summary>
        /// Text constructor
        /// </summary>
        /// <param name="text"></param>
        public Header(string text) : base(text)
        {
        }
    }
}
