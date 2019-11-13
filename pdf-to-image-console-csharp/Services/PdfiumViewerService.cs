using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using PdfiumViewer;

namespace pdf_to_image_console_csharp.Services
{
    public static class PdfiumViewerService
    {
        public static void Test()
        {
            var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            
            using (var document = PdfDocument.Load(projectDir + @"\test.pdf"))
            {
                var pageCount = document.PageCount;

                for (int i = 0; i < pageCount; i++)
                {
                    var dpi = 300;

                    using (var image = document.Render(i, dpi, dpi, PdfRenderFlags.CorrectFromDpi))
                    {
                        var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);

                        var encParams = new EncoderParameters(1)
                        {
                            Param =
                                {
                                    [0] = new EncoderParameter(Encoder.Quality, 50L)
                                }
                        };

                        image.Save(projectDir + @"\output_" + i + ".jpg", encoder, encParams);

                        Console.WriteLine($"Page {i} converted...");
                    }
                }
            }
        }
    }
}
