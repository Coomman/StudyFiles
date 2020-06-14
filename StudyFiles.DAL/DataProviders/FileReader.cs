using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;

namespace StudyFiles.DAL.DataProviders
{
    public static class FileReader
    {
        private static readonly Dictionary<string, Func<string, string>> ReadCmd = new Dictionary<string, Func<string, string>>
        {
            [".txt"] = ReadTxt,
            [".pdf"] = ReadPdf,
            [".doc"] = ReadDocs,
            [".docx"] = ReadDocs
        };

        public static string ReadFile(FileInfo fileInfo)
        {
            return ReadCmd[fileInfo.Extension].Invoke(fileInfo.FullName);
        }

        private static string ReadTxt(string filePath)
        {
            return File.ReadAllText(filePath);
        }
        private static string ReadPdf(string filePath)
        {
            var doc = new PdfDocument(filePath);

            var text = new StringBuilder();

            foreach (PdfPageBase page in doc.Pages)
            {
                text.Append(page.ExtractText());
            }

            return text.ToString();
        }
        private static string ReadDocs(string filePath)
        {
            //using var wordDocument = DocX.Load(filePath);
            //return wordDocument.Text;

            return null;
        }

        public static List<(int pageNum, List<PointF>)> PdfSearch(string filePath, string searchQuery)
        {
            var doc = new PdfDocument(filePath);

            //return doc.Pages
            //    .AsParallel()
            //    .Cast<PdfPageBase>()
            //    .Select((page, pageNum) => (pageNum, page.FindText(searchQuery, TextFindParameter.IgnoreCase).Finds
            //        .AsParallel()
            //        .SelectMany(entry => entry.Positions)
            //        .ToList()))
            //    .ToList();

            doc.Pages
                .AsParallel()
                .Cast<PdfPageBase>()
                .ForAll(page => page.FindText(searchQuery, TextFindParameter.IgnoreCase).Finds
                    .AsParallel()
                    .ForAll(entry => entry.ApplyHighLight()));


            var image = doc.SaveAsImage(0, PdfImageType.Bitmap);

            return null;
        }
    }
}
