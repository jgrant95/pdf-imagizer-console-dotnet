using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using pdf_to_image_console_csharp.Services;

namespace pdf_to_image_console_csharp
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pdf To Image Converter...");

            try
            {
                //DocnetService.Test();
                PdfiumViewerService.Test();


            }
            catch (Exception e)
            {
                Console.WriteLine("An error has occurred... " + e.Message);
            }

            Console.WriteLine("Conversion finished...");
            Console.ReadLine();
        }
        
        
    }
}
