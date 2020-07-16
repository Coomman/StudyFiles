using System.IO;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using Microsoft.Win32;
using Microsoft.VisualStudio.PlatformUI;

using StudyFiles.DTO;
using StudyFiles.GUI.ServiceProxies;

namespace StudyFiles.GUI
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        //TODO:
        // 1) Refactor this class
        // 2) Fix several queries for file displaying error
        // 3) Fix search
        // 4) Add .doc and .docx formats support

        private string _tempFilePath;

        private readonly DataServiceProxy _serviceProxy = new DataServiceProxy();
        private readonly ObservableCollection<IEntityDTO> _catalog = new ObservableCollection<IEntityDTO>();

        private ObservableCollection<IEntityDTO> _models;
        public ObservableCollection<IEntityDTO> Models
        {
            get => _models;
            set
            {
                _models = value;
                OnPropertyChanged();
            }
        }

        private IEntityDTO _selectedModel;
        public IEntityDTO SelectedModel
        {
            get => _selectedModel;
            set
            {
                _selectedModel = value;
                OnPropertyChanged(nameof(SelectedModel));
            }
        }

        private int Level 
            => _catalog.Count;
        private string GetDirectory()
        {
            var path = string.Empty;

            for (int i = 0; i < Level; i++)
                path = Path.Combine(path, _catalog[i].ID.ToString());

            return path;
        }
        private void DisplayFileDialog()
        {
            var fd = new OpenFileDialog
            {
                Title = "Select a file",

                Filter = "PDF files (*.pdf)|*.pdf|" +
                         "Microsoft Word (*.doc;*.docx)|*.doc;*.docx|" +
                         "Text files (*.txt)|*.txt"
            };

            if (fd.ShowDialog() == true)
                Models.Add(_serviceProxy.UploadFile(File.ReadAllBytes(fd.FileName), 
                    Path.Combine(GetDirectory(), fd.SafeFileName), _catalog.Last().ID));
        }

        public ApplicationViewModel()
        {
            Models = new ObservableCollection<IEntityDTO>(_serviceProxy.GetFolderList(0));
        }

        public void GetNextItemList()
        {
            _catalog.Add(SelectedModel);

            Models = new ObservableCollection<IEntityDTO>(Level == 4
                ? _serviceProxy.GetFileList(GetDirectory(), SelectedModel.ID)
                : _serviceProxy.GetFolderList(Level, SelectedModel.ID));

            SelectedModel = null;
        } //++
        public void GetPrevItemList()
        {
            if (_tempFilePath != null)
            {
                File.Delete(_tempFilePath);
                _tempFilePath = null;
            }

            if(!(Models.Last() is FileViewDTO))
                _catalog.RemoveAt(Level - 1);

            Models = new ObservableCollection<IEntityDTO>(Level < 4
                ? _serviceProxy.GetFolderList(Level, _catalog.LastOrDefault()?.ID ?? -1)
                : _serviceProxy.GetFileList(GetDirectory(), _catalog.Last().ID));

            SelectedModel = null;
        } //++
        public void GetSearchResult(string query)
        {
            if (Models.First() is FileViewDTO)
            {
                Models = new ObservableCollection<IEntityDTO>(new[] {_serviceProxy.GetFile(Models[0].InnerText, ".pdf")});
                return;
            }

            Models = new ObservableCollection<IEntityDTO>(_serviceProxy.FindFiles(GetDirectory(), query));

            _catalog.Add(new NullDTO());
        }

        public void AddFolder(string name)
        {
            Models.RemoveAt(Models.Count - 1);

            Models.Add(_serviceProxy.AddNewFolder(Level, name, _catalog.Last().ID));
        }
        public void AddItem()
        {
            if(Models.First() is NotFoundDTO)
                Models.RemoveAt(0);

            if (Level != 4)
                Models.Add(new NullDTO());
            else
                DisplayFileDialog();
        }

        public void DeleteItem()
        {
            Models.Remove(SelectedModel);
            SelectedModel = null;
        }

        public void ShowFile(string searchQuery = null)
        {
            if (SelectedModel is SearchResultDTO searchResult)
                Models = new ObservableCollection<IEntityDTO>(new[]
                    {_serviceProxy.GetFile($"{searchResult.Path}\\{searchResult.InnerText}", ".pdf")});

            if (SelectedModel is FileDTO file)
            {
                var fileView = _serviceProxy.GetFile(Path.Combine(GetDirectory(), SelectedModel.InnerText),
                    file.Extension);

                _tempFilePath = fileView.InnerText;
                Models = new ObservableCollection<IEntityDTO>(new[] {fileView});
            }
        }

        #region Commands

        public ICommand AddCommand
            => new DelegateCommand(obj => AddItem(),
                obj => Level != 5 && !(Models.LastOrDefault() is NullDTO)); //TODO: Add In-Search state
        public ICommand DeleteCommand
            => new DelegateCommand(obj => DeleteItem(),
                obj => SelectedModel != null && Level != 5); //TODO: Add In-Search state
        public ICommand SearchCommand
            => new DelegateCommand(obj => GetSearchResult(obj.ToString()),
                obj => Models.Any() && !string.IsNullOrEmpty(obj.ToString())); //TODO: Add searchQuery domains

        public ICommand OnFolderDoubleClickCommand
            => new DelegateCommand(obj => GetNextItemList());
        public ICommand OnFileDoubleClickCommand
            => new DelegateCommand(obj => ShowFile());
        public ICommand BackCommand
            => new DelegateCommand(obj => GetPrevItemList(),
                obj => Level != 0);
        public ICommand WindowMouseClickCommand
            => new DelegateCommand(obj => SelectedModel = null);

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
