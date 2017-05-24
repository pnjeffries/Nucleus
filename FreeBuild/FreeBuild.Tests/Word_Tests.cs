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

        public static void WriteDocTest()
        {
            Core.Print("Writing to Word...");
            var word = new WordController();
            word.NewDocument();
            word.Title("Hello Wor(l)d!");
            word.WriteLine("This is a test of writing to Word.");
            word.WriteImage("C:/Users/PJEFF/Pictures/SalamanderDomeInRobot.png");
            word.WriteLine();
            word.WriteLine("Here is a bit more text... I hope you like it!");
            word.WriteTable(new string[,] { { "A", "1" }, { "B", "2" }, { "C", "3" } }, true, true, true);
            word.WriteLine("Hungry hungry pile caps!");
            word.Write("This text includes a").WriteSuper("Superscript").Write(" and a").WriteSub("Subscript").WriteLine(".");
            word.Underline().Write("For my next trick").Underline(false).Write(", I will ").Bold().Write("Bold out a section of text").Bold(false).Write(" and then ");
            word.Italics().WriteLine("italicise some more.").Italics(false);
            word.WriteBulletPoints(new string[] { "Item A.", "Item B.", "The third item.", "Bananarama" });
            word.SaveDocument("C:/Temp/WordTest4.docx");
            word.ReleaseWord();
            Core.Print("Complete!");
        }
    }
}
