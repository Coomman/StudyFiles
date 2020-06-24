using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Win32;
using StudyFiles.Core;
using StudyFiles.DAL.DataProviders;
using StudyFiles.DTO;

namespace StudyFiles.GUI
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly Facade _supplier = new Facade();
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

        public int Level 
            => _catalog.Count;

        public ApplicationViewModel()
        {
            Models = new ObservableCollection<IEntityDTO>(UniversityDataProviderMock.GetUniversities());
        }

        public void GetNextItemList()
        {
            _catalog.Add(SelectedModel);

            Models = _supplier.GetModelsList(_catalog.Count, SelectedModel.ID);

            SelectedModel = null;
        }
        public void GetPrevItemList()
        {
            _catalog.RemoveAt(Level - 1);

            Models = _catalog.Any() 
                ? _supplier.GetModelsList(Level, _catalog[^1].ID) 
                : _supplier.GetModelsList(Level);

            SelectedModel = null;
        }

        public void AddItem(string name)
        {
            if(Level != 4)
                Models.RemoveAt(Models.Count - 1);

            Models.Add(_supplier.AddNewModel(_catalog.Count, name));
        }
        public void AddFile()
        {
            if (Level != 4)
                Models.Add(new NullDTO());
            else
            {
                var fd = new OpenFileDialog
                {
                    Title = "Select a file",

                    Filter = "PDF files (*.pdf)|*.pdf|" +
                             "Microsoft Word (*.doc;*.docx)|*.doc;*.docx|" +
                             "Text files (*.txt)|*.txt"
                };

                if (fd.ShowDialog() == true)
                    AddItem(fd.FileName);
            }
        }
        public void DeleteItem()
        {
            Models.Remove(SelectedModel);
            SelectedModel = null;
        }
        public void ShowFile()
        {
            _catalog.Add(SelectedModel);

            Models = new ObservableCollection<IEntityDTO>(new []{ _supplier.ReadFile(SelectedModel.Name)});
        }

        public ICommand AddCommand
            => new DelegateCommand(obj => AddFile(),
                obj => Level != 5);
        public ICommand DeleteCommand 
            => new DelegateCommand(obj => DeleteItem(), 
                obj => SelectedModel != null && Level != 5);
        public ICommand SearchCommand
            => new DelegateCommand(obj => _supplier.FindFiles(_catalog.Count, obj.ToString()));

        public ICommand BackCommand
            => new DelegateCommand(obj => GetPrevItemList(), 
                obj => Level != 0);
        public ICommand WindowMouseClickCommand
            => new DelegateCommand(obj => SelectedModel = null);


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
