using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StudyFiles.DAL.Repositories.Catalog;
using StudyFiles.DAL.Repositories.Files;
using StudyFiles.DTO.Service;

namespace StudyFiles.Core.Suppliers
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

            _deleteCmd = new Dictionary<int, Action<string, int>>
            {
                [0] = (path, id) => DeleteUniversity(id),
                [1] = DeleteFaculty,
                [2] = DeleteDiscipline,
                [3] = DeleteCourse
            };

            Directory.CreateDirectory(StoragePath);
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
            => _fileRep.GetFiles(Path.Combine(StoragePath, filePath), courseID);

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

        #region Delete

        private readonly Dictionary<int, Action<string, int>> _deleteCmd;

        public void DeleteFolder(int depth, string path, int id)
        {
            _deleteCmd[depth].Invoke(path, id);
        }

        private void DeleteUniversity(int id)
        {
            _uniRep.DeleteUniversity(id);
            _fileRep.DeleteFolder(Path.Combine(StoragePath, id.ToString()));
        }
        private void DeleteFaculty(string path, int id)
        {
            _facRep.DeleteFaculty(id);
            _fileRep.DeleteFolder(Path.Combine(StoragePath, path, id.ToString()));
        }
        private void DeleteDiscipline(string path, int id)
        {
            _disRep.DeleteDiscipline(id);
            _fileRep.DeleteFolder(Path.Combine(StoragePath, path, id.ToString()));
        }
        private void DeleteCourse(string path, int id)
        {
            _courseRep.DeleteCourse(id);
            _fileRep.DeleteFolder(Path.Combine(StoragePath, path, id.ToString()));
        }


        public void DeleteFile(string filePath)
        {
            _fileRep.DeleteFile(filePath);
        }

        #endregion

        public IEnumerable<IEntityDTO> FindFiles(string path, string searchQuery)
        {
            var searchPath = Path.Combine(StoragePath, path);

            if(!Directory.Exists(searchPath))
                return Array.Empty<IEntityDTO>();

            return Directory.GetFiles(searchPath, "*.*", SearchOption.AllDirectories)
                .AsParallel()
                .Where(filePath => _fileRep.InFileSearch(filePath, searchQuery))
                .Select(filePath => _fileRep.GetSearchResultDTO(new FileInfo(filePath)))
                .AsEnumerable();
        }

        public byte[] GetFile(string filePath)
        {
            return _fileRep.GetFile(filePath);
        }
    }
}
