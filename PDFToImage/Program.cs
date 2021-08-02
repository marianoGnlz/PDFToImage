using System;

namespace PDFToImage
{
    class Program
    {
        static void Main(string[] args)
        {
            var pdfToImage = new PDFToImage(args[0]);

            pdfToImage.ToImage();
        }
    }
}
