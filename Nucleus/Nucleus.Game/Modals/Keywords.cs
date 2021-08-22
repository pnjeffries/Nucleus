using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Modals
{
    /// <summary>
    /// A paragraph which displays a list of keywords or tags
    /// </summary>
    [Serializable]
    public class Keywords : Paragraph
    {
        /// <summary>
        /// Text constructor
        /// </summary>
        /// <param name="text"></param>
        public Keywords(string text) : base(text)
        {
        }
    }
}
