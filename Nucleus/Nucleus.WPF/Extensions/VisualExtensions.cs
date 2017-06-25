using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Nucleus.WPF
{
    /// <summary>
    /// Extension methods for WPF Visual objects
    /// </summary>
    public static class VisualExtensions
    {
        /// <summary>
        /// Render and save the current visual appearance of this object to a .png file
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="filePath">The filepath to save the .png file to</param>
        /// <param name="dpi">The Dots Per Inch quality rating for the output image</param>
        /// <returns></returns>
        public static void SaveAsPNG(this Visual visual, FilePath filePath, int dpi = 96)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(visual);
            
            var drawing = new DrawingVisual();
            using (DrawingContext context = drawing.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(visual);
                context.DrawRectangle(brush, null, new Rect(bounds.Size));
            }

            var bitmap = new RenderTargetBitmap((int)(bounds.Width * dpi/96), (int)(bounds.Height * dpi/96), dpi, dpi, PixelFormats.Pbgra32);
            bitmap.Render(drawing);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (Stream stream = File.Create(filePath))
            {
                encoder.Save(stream);
            }
        }


    }
}
