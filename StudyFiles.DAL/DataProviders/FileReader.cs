using System;
using System.IO;
using System.Linq;

using Spire.Pdf;
using Aspose.Words;

namespace StudyFiles.DAL.DataProviders
{
    public class FileReader : IFileReader
    {
        private static bool TxtSearch(string filePath, string searchQuery)
        {
            return File.ReadAllText(filePath).Contains(searchQuery);
        }
        private static bool PdfSearch(string filePath, string searchQuery)
        {
            using var doc = new PdfDocument(filePath);

            return doc.Pages
                .Cast<PdfPageBase>()
                .Select(page => page.ExtractText())
                .AsParallel()
                .Any(pageText => pageText.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool FileSearch(string fileExtension, string filePath, string searchQuery)
        {
            return fileExtension == ".txt" 
                ? TxtSearch(filePath, searchQuery) 
                : PdfSearch(filePath, searchQuery);
        }
        public void SaveAsPdf(string filePath)
        {
            var doc = new Document(filePath);

            doc.Save("...");
        }
    }
}
