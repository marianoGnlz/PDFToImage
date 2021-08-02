using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PDFToImage
{
    class PDFToImage
    {
        public PDFToImage(string path)
        {
            PathOfImage = path;
        }

        private string PathOfImage { get; set; }

        public void ToImage ()
        {
            
            System.IO.DirectoryInfo dir = new DirectoryInfo(this.PathOfImage);


            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.pdf", SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery =
                from file in fileList
                orderby file.Name
                select file;

            if (fileQuery.Any())
            {
                foreach (var file  in fileQuery)
                {
                    if (!Directory.Exists(file.DirectoryName + "\\Images"))
                    {
                        Directory.CreateDirectory(file.DirectoryName + "\\Images");
                    }

                    var stream = File.Open(file.FullName, FileMode.Open);
                    var rasterizer = new GhostscriptRasterizer();
                        rasterizer.Open(
                            stream, 
                            GhostscriptVersionInfo.GetLastInstalledVersion(
                                GhostscriptLicense.GPL | 
                                GhostscriptLicense.AFPL, 
                                GhostscriptLicense.GPL), 
                            true
                            );


                    string outputLowPNGPath = Path.Combine(file.DirectoryName + "\\Images", string.Format("{0}.jpeg", file.Name.Replace(".pdf", "LOW")));

                    string outputHighPNGPath = Path.Combine(file.DirectoryName + "\\Images", string.Format("{0}.jpeg", file.Name.Replace(".pdf", "High")));

                    var pdf2Low = rasterizer.GetPage(30, 1);
                    var pdf2High = rasterizer.GetPage(70, 1);


                    pdf2Low.Save(outputLowPNGPath, ImageFormat.Jpeg);
                    pdf2High.Save(outputHighPNGPath, ImageFormat.Jpeg);

                    Console.WriteLine("Saved " + outputLowPNGPath);
                    Console.WriteLine("Saved " + outputHighPNGPath);

                    rasterizer.Close();
                    stream.Close();
                    }
            }
        }

    }
}
