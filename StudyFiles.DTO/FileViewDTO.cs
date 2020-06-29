using System.Collections.Generic;
using System.Drawing;

namespace StudyFiles.DTO
{
    public class FileViewDTO : IEntityDTO
    {
        public int ID { get; }
        public string InnerText { get; }
        public IList<Image> Images { get; }

        public FileViewDTO(string path, Image[] images)
        {
            InnerText = path;
            Images = images;
        }
    }
}
