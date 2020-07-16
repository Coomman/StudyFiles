using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using StudyFiles.DTO;
using StudyFiles.DAL.DataProviders;

namespace StudyFiles.Core
{
    public class DataSupplier : IDataSupplier
    {
        // Get from config file
        private const string StoragePath = @"res\";

        private readonly IUniversityRepository _uniRep;
        private readonly IFacultyRepository _facRep;
        private readonly IDisciplineRepository _disRep;
        private readonly ICourseRepository _courseRep;
        private readonly IFileRepository _fileRep;

        public DataSupplier(IUniversityRepository uniRep, IFacultyRepository facRep, IDisciplineRepository disRep, ICourseRepository courseRep, IFileRepository fileRep)
        {
            _uniRep = uniRep;
            _facRep = facRep;
            _disRep = disRep;
            _courseRep = courseRep;
            _fileRep = fileRep;

            _getCmd = new Dictionary<int, Func<int, IEnumerable<IEntityDTO>>>
            {
                [0] = id => GetUniversities(),
                [1] = GetFaculties,
                [2] = GetDisciplines,
                [3] = GetCourses
            };

            _addCmd = new Dictionary<int, Func<string, int, IEntityDTO>>
            {
                [0] = (modelName, parentId) => AddUniversity(modelName),
                [1] = AddFaculty,
                [2] = AddDiscipline,
                [3] = AddCourse
            };
        }

        private static void CreateDirectory(string path, string newFolder = null)
        {
            var dirPath = Path.Combine(StoragePath, path);

            if (!(newFolder is null))
                dirPath = Path.Combine(dirPath, newFolder);

            Directory.CreateDirectory(dirPath);
        }

        #region Get

        private readonly Dictionary<int, Func<int, IEnumerable<IEntityDTO>>> _getCmd;

        public List<IEntityDTO> GetFolderList(int depth, int id = -1)
        {
            var result = _getCmd[depth].Invoke(id).ToList();

            if (!result.Any())
                result.Add(new NotFoundDTO());

            return result;
        }

        private IEnumerable<IEntityDTO> GetUniversities()
            => _uniRep.GetUniversities();
        private IEnumerable<IEntityDTO> GetFaculties(int universityID)
            => _facRep.GetFaculties(universityID);
        private IEnumerable<IEntityDTO> GetDisciplines(int facultyID)
            => _disRep.GetDisciplines(facultyID);
        private IEnumerable<IEntityDTO> GetCourses(int disciplineID)
            => _courseRep.GetCourses(disciplineID);

        public IEnumerable<IEntityDTO> GetFileList(int courseID, string filePath)
            => _fileRep.GetFiles(new DirectoryInfo(Path.Combine(StoragePath, filePath)), courseID);
        
        #endregion

        #region Add

        private readonly Dictionary<int, Func<string, int, IEntityDTO>> _addCmd;

        public IEntityDTO AddNewFolder(int depth, string modelName, int parentId)
        {
            return _addCmd[depth].Invoke(modelName, parentId);
        }

        private IEntityDTO AddUniversity(string universityName)
            => _uniRep.AddUniversity(universityName);
        private IEntityDTO AddFaculty(string facultyName, int universityID)
            => _facRep.AddFaculty(facultyName, universityID);
        private IEntityDTO AddDiscipline(string disciplineName, int facultyID)
            => _disRep.AddDiscipline(disciplineName, facultyID);
        private IEntityDTO AddCourse(string teacherName, int disciplineID)
            => _courseRep.AddCourse(teacherName, disciplineID);

        public IEntityDTO UploadFile(byte[] file, string filePath, int courseID)
            => _fileRep.UploadFile(file, Path.Combine(StoragePath, filePath), courseID);

        #endregion

        public IEnumerable<IEntityDTO> FindFiles(int depth, string searchQuery)
        {
            //var searchPath = GetDirectory(depth).ToString();

            //return Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
            //    .AsParallel()
            //    .Select(filePath => (path: filePath, pageEntries: FileReader.PdfSearch(filePath, searchQuery)))
            //    .Where(file => file.pageEntries.Any())
            //    .Select(file => FileRepository.GetSearchResultDTO(new FileInfo(file.path), file.pageEntries, StoragePath))
            //    .AsEnumerable();

            return null;
        }

        public byte[] GetFile(string filePath)
        {
            return _fileRep.GetFile((Path.Combine(StoragePath, filePath)));
        }
    }
}
