﻿namespace StudyFiles.DAL.DataProviders
{
    public interface IFileReader
    {
        public bool FileSearch(string filePath, string searchQuery);
        public string SaveFile(string filePath, byte[] data);
    }
}
