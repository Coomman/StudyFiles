using System;
using System.Drawing;
using System.Linq;

using Spire.Pdf;

namespace StudyFiles.DAL.DataProviders
{
    public static class FileReader
    {
        public static bool PdfSearch(string filePath, string searchQuery)
        {
            using var doc = new PdfDocument(filePath);

            return doc.Pages
                .Cast<PdfPageBase>()
                .Select(page => page.ExtractText())
                .AsParallel()
                .Any(pageText => pageText.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase));

            //return doc.Pages
            //    .AsParallel()
            //    .Cast<PdfPageBase>()
            //    .Select((page, pageNum) => (pageNum, page.FindText(searchQuery, TextFindParameter.IgnoreCase).Finds
            //        .AsParallel()
            //        .SelectMany(entry => entry.Positions)
            //        .ToList()))
            //    .ToList();

            //doc.Pages
            //    .AsParallel()
            //    .Cast<PdfPageBase>()
            //    .ForAll(page => page.FindText(searchQuery, TextFindParameter.IgnoreCase).Finds
            //        .AsParallel()
            //        .ForAll(entry => entry.ApplyHighLight()));


            //var image = doc.SaveAsImage(0, PdfImageType.Bitmap);

            //return null;
        }

        public static Image[] PdfHighlight(string filePath, string searchQuery)
        {
            using var doc = new PdfDocument(filePath);

            var images = new Image[doc.Pages.Count];

            for (int i = 0; i < doc.Pages.Count; i++)
                images[i] = doc.SaveAsImage(i);

            return images;
        }
    }
}
