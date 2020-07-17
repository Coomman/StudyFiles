using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CefSharp;
using CefSharp.Wpf;
using Microsoft.Win32;
using GalaSoft.MvvmLight;
using Microsoft.VisualStudio.PlatformUI;

using StudyFiles.DTO.Files;
using StudyFiles.DTO.Service;
using StudyFiles.GUI.ServiceProxies;

namespace StudyFiles.GUI
{
    class ApplicationViewModel : ViewModelBase
    {
        public event Action OnSearchStateExit;

        private bool _inSearch;
        private string _tempFilePath;
        private string _lastSearchQuery;

        private readonly AuthClient _authClient = AuthClient.GetInstance;
        private readonly DataServiceProxy _serviceProxy = new DataServiceProxy();
        private readonly ObservableCollection<IEntityDTO> _catalog = new ObservableCollection<IEntityDTO>();

        private List<IEntityDTO> _searchResultsCache;

        #region Bindings

        private IWpfWebBrowser _browser;
        public IWpfWebBrowser Browser
        {
            get => _browser;
            set => Set(ref _browser, value);
        }

        private ObservableCollection<IEntityDTO> _models;
        public ObservableCollection<IEntityDTO> Models
        {
            get => _models;
            set => Set(ref _models, value);
        }

        private IEntityDTO _selectedModel;
        public IEntityDTO SelectedModel
        {
            get => _selectedModel;
            set => Set(ref _selectedModel, value);
        }

        #endregion

        #region Helper Methods

        private int Level
            => _catalog.Count;
        private void UpdateModels(IEntityDTO model)
        {
            Models = new ObservableCollection<IEntityDTO>(new[] { model });
        }
        private void UpdateModels(IEnumerable<IEntityDTO> models)
        {
            Models = new ObservableCollection<IEntityDTO>(models);
        }

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

                Filter = "All files (*.pdf;*.doc;*.docx;*.txt)|*.pdf;*.doc;*.docx;*.txt|" +
                         "PDF files (*.pdf)|*.pdf|" +
                         "Microsoft Word (*.doc;*.docx)|*.doc;*.docx|" +
                         "Text files (*.txt)|*.txt"
            };

            if (fd.ShowDialog() == true)
                Models.Add(_serviceProxy.UploadFile(File.ReadAllBytes(fd.FileName),
                    Path.Combine(GetDirectory(), fd.SafeFileName), _catalog.Last().ID));
        }

        #endregion

        public ApplicationViewModel()
        {
            Models = new ObservableCollection<IEntityDTO>(_serviceProxy.GetFolderList(0));
        }

        private void GetNextItemList()
        {
            _catalog.Add(SelectedModel);

            UpdateModels(Level == 4
                ? _serviceProxy.GetFileList(GetDirectory(), SelectedModel.ID)
                : _serviceProxy.GetFolderList(Level, SelectedModel.ID));

            SelectedModel = null;
        }
        private void GetPrevItemList()
        {
            if (_tempFilePath != null)
            {
                File.Delete(_tempFilePath);
                _tempFilePath = null;
            }

            if (_inSearch)
                if (Models.First() is FileViewDTO)
                {
                    UpdateModels(_searchResultsCache);
                    return;
                }
                else
                {
                    OnSearchStateExit?.Invoke();
                    _inSearch = false;
                    _searchResultsCache = null;
                }
            else if (!(Models.First() is FileViewDTO))
                _catalog.RemoveAt(Level - 1);

            UpdateModels(Level < 4
                ? _serviceProxy.GetFolderList(Level, _catalog.LastOrDefault()?.ID ?? -1)
                : _serviceProxy.GetFileList(GetDirectory(), _catalog.Last().ID));

            SelectedModel = null;
        }
        private void GetSearchResultList()
        {
            _inSearch = true;

            _searchResultsCache = _serviceProxy.FindFiles(GetDirectory(), _lastSearchQuery);

            UpdateModels(_searchResultsCache);
        }

        public void AddFolder(string name)
        {
            Models.RemoveAt(Models.Count - 1);

            Models.Add(_serviceProxy.AddNewFolder(Level, name, _catalog.LastOrDefault()?.ID ?? -1));
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
            if(SelectedModel is FileDTO file)
                _serviceProxy.DeleteFile(file.Path);
            else
                _serviceProxy.DeleteFolder(Level, GetDirectory(), SelectedModel.ID);

            Models.Remove(SelectedModel);
            SelectedModel = null;
        }

        private void ShowFile()
        {
            var file = (FileDTO) SelectedModel;
            var fileView = _serviceProxy.GetFile(file.Path, file.Extension);

            _tempFilePath = fileView.InnerText;
            UpdateModels(fileView);
        }
        private void HighlightEntries()
        {
            Browser.Find(
                identifier: 1,
                searchText: _lastSearchQuery,
                forward: true,
                matchCase: false,
                findNext: false);
        }
        private void ShowSearchResult()
        {
            ShowFile();

            //HighlightEntries();
        }

        #region Commands

        public ICommand AddCommand
            => new DelegateCommand(obj => AddItem(),
                obj => Level != 5 && !(Models.LastOrDefault() is NullDTO && !_inSearch));
        public ICommand DeleteCommand
            => new DelegateCommand(obj => DeleteItem(),
                obj => SelectedModel != null && Level != 5 && !_inSearch);
        public ICommand SearchCommand
            => new DelegateCommand<string>(searchQuery =>
                {
                    _lastSearchQuery = searchQuery;

                    if (Models.First() is FileViewDTO)
                        HighlightEntries();
                    else
                        GetSearchResultList();
                },
                searchQuery => !string.IsNullOrEmpty(searchQuery) &&
                               !(Models.Last() is NotFoundDTO) && !(Models.Last() is NullDTO));

        public ICommand OnFolderDoubleClickCommand
            => new DelegateCommand(obj => GetNextItemList());
        public ICommand OnFileDoubleClickCommand
            => new DelegateCommand(obj => ShowFile());
        public ICommand OnSearchResultDoubleClickCommand
            => new DelegateCommand(obj => ShowSearchResult());

        public ICommand BackCommand
            => new DelegateCommand(obj => GetPrevItemList(),
                obj => Level != 0 || _inSearch);
        public ICommand WindowMouseClickCommand
            => new DelegateCommand(obj => SelectedModel = null);

        #endregion
    }
}
