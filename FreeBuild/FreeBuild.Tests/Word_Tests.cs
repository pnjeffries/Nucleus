using FreeBuild.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Tests
{
    public static class Word_Tests
    {
        public static void ReadDocTest()
        {
            var word = new WordController(@"\\ukramlonfiler01\adm\0000-0999\00020\Business Service Areas\Buildings and Designs\Ramboll Computational Design\Case Studies\Case Study Template Draft.docx");
            Core.Print(word.GetDocumentText());
        }
    }
}
