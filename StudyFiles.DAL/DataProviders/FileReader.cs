using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Spire.Pdf;
using Aspose.Words;

namespace StudyFiles.DAL.DataProviders
{
    public sealed class FileReader : IFileReader
    {
        private readonly Regex _wordFileRegex = new Regex(@".*docx*");

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

        public bool FileSearch(string filePath, string searchQuery)
        {
            return Path.GetExtension(filePath) == ".pdf" 
                ? PdfSearch(filePath, searchQuery) 
                : TxtSearch(filePath, searchQuery);
        }

        private static string ConvertWordToPdf(string filePath)
        {
            var doc = new Document(filePath);

            var newFilePath = Path.ChangeExtension(filePath, ".pdf");
            doc.Save(newFilePath);
            File.Delete(filePath);

            return newFilePath;
        }

        public string SaveFile(string filePath, byte[] data)
        {
            using (var fileStream = File.Create(filePath))
                fileStream.Write(data);

            if (_wordFileRegex.IsMatch(filePath))
                filePath = ConvertWordToPdf(filePath);

            return filePath;
        }
    }
}
