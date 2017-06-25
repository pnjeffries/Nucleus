using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Nucleus.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convert this string to a document paragraph with automatic
        /// hyperlinks formed around any valid URL.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Paragraph ToParagraph(this string message)
        {
            var result = new Paragraph();
            foreach (string substr in message.SplitHyperlinks())
            {
                if (substr.IsURI())
                {
                    var link = new Hyperlink(new Run(substr));
                    link.IsEnabled = true;
                    link.Cursor = Cursors.Hand;
                    link.NavigateUri = new Uri(substr);
                    link.RequestNavigate += (sender, args) => Process.Start(substr);
                    result.Inlines.Add(link);
                }
                else result.Inlines.Add(new Run(substr));
            }
            return result;
        }
    }
}
