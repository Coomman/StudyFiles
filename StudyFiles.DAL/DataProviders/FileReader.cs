using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Spire.Pdf;
using Spire.Pdf.General.Find;

namespace StudyFiles.DAL.DataProviders
{
    public static class FileReader
    {
        public static List<int> PdfSearch(string filePath, string searchQuery)
        {
            using var doc = new PdfDocument(filePath);

            return doc.Pages
                .Cast<PdfPageBase>()
                .Select((page, pageNum) => (pageText: page.ExtractText(), pageNum))
                .AsParallel()
                .Where(page => page.pageText.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
                .Select(page => page.pageNum)
                .ToList();

            //return doc.Pages
            //    .AsParallel()
            //    .Cast<PdfPageBase>()
            //    .Select((page, pageNum) => (pageNum, page.FindText(searchQuery, TextFindParameter.IgnoreCase).Finds
            //        .AsParallel()
            //        .SelectMany(entry => entry.Positions)
            //        .ToList()))
            //    .ToList();
        }

        private static void HighlightEntries(PdfDocument doc, string searchQuery, List<int> pageEntries)
        {
            pageEntries
                .ForEach(pageNum => doc.Pages[pageNum].FindText(searchQuery, TextFindParameter.IgnoreCase)
                    .Finds
                    .ToList()
                    .ForEach(entry => entry.ApplyHighLight()));
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

        public static Image[] GetPdfImages(string filePath, string searchQuery, List<int> pageEntries)
        {
            using var doc = new PdfDocument(filePath);

            if(searchQuery != null)
                if (pageEntries == null)
                    HighlightEntries(doc, searchQuery);
                else
                    HighlightEntries(doc, searchQuery, pageEntries);

            var images = new Image[doc.Pages.Count];

            for (int i = 0; i < doc.Pages.Count; i++)
                images[i] = doc.SaveAsImage(i);

            return images;
        }
    }
}
