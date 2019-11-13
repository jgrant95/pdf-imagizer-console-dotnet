using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Docnet.Core;

namespace pdf_to_image_console_csharp.Services
{
    public static class DocnetService
    {
        public static void Test()
        {
            var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            using (var library = DocLib.Instance)
            {
                using (var docReader = library.GetDocReader(projectDir + @"\test.pdf", 2362, 3543))
                {
                    for (int i = 0; i < docReader.GetPageCount(); i++)
                    {
                        using (var pageReader = docReader.GetPageReader(i))
                        {
                            var rawBytes = pageReader.GetImage();

                            var width = pageReader.GetPageWidth();
                            var height = pageReader.GetPageHeight();

                            using (var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                            {
                                bmp.AddBytes(rawBytes);

                                using (var stream = new MemoryStream())
                                {
                                    var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);

                                    var encParams = new EncoderParameters(1)
                                    {
                                        Param = { [0] = new EncoderParameter(Encoder.Quality, 100L) }
                                    };

                                    bmp.Save(stream, encoder, encParams);

                                    File.WriteAllBytes($"page_image{i}.jpg", stream.ToArray());

                                    Console.WriteLine($"Page {i} converted...");
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void AddBytes(this Bitmap bmp, byte[] rawBytes)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            //using (var g = Graphics.FromImage(rect))
            //{
            //    g.Clear(Color.White);
            //    g.DrawImageUnscaled(bmp, 0, 0);
            //}

            var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            var pNative = bmpData.Scan0;

            Marshal.Copy(rawBytes, 0, pNative, rawBytes.Length);
            bmp.UnlockBits(bmpData);
        }
    }
}
