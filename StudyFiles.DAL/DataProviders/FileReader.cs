using System;
using System.Drawing;
using System.Linq;

using Spire.Pdf;
using Spire.Pdf.General.Find;

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
        }

        private static void HighlightEntries(PdfDocument doc, string searchQuery)
        {
            doc.Pages
                .AsParallel()
                .Cast<PdfPageBase>()
                .ToList()
                .ForEach(page => page.FindText(searchQuery, TextFindParameter.IgnoreCase).Finds
                    .ToList()
                    .ForEach(entry => entry.ApplyHighLight()));
        }

        public static Image[] GetPdfImages(string filePath, string searchQuery)
        {
            using var doc = new PdfDocument(filePath);

            if(searchQuery != null)
                HighlightEntries(doc, searchQuery);

            var images = new Image[doc.Pages.Count];

            for (int i = 0; i < doc.Pages.Count; i++)
                images[i] = doc.SaveAsImage(i);

            return images;
        }
    }
}
