﻿using System.Collections.Generic;

namespace StudyFiles.DTO
{
    public class SearchResultDTO : IEntityDTO
    {
        private readonly FileDTO _fileInfo;

        public int ID => _fileInfo.ID;
        public string InnerText => _fileInfo.InnerText;
        public string Size => _fileInfo.Size;
        public int CourseID => _fileInfo.CourseID;
        public string CreationTime => _fileInfo.CreationTime;

        public string Path { get; }
        public string BreadCrumb { get; }
        public List<int> PageEntries { get; }

        public SearchResultDTO(FileDTO fileInfo, string path, string breadCrumb, List<int> pageEntries)
        {
            _fileInfo = fileInfo;
            Path = path;
            BreadCrumb = breadCrumb;
            PageEntries = pageEntries;
        }
    }
}