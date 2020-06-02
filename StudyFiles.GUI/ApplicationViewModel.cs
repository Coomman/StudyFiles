using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyFiles.Core;
using StudyFiles.DAL.DataProviders;
using StudyFiles.DTO;

namespace StudyFiles.GUI
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly Facade _supplier = new Facade();

        public ObservableCollection<IEntityDTO> Catalog = new ObservableCollection<IEntityDTO>();
        public ObservableCollection<IEntityDTO> Models { get; set; }
        public IEntityDTO SelectedModel { get; set; }

        public ApplicationViewModel()
        {
            Models = new ObservableCollection<IEntityDTO>(UniversityDataProviderMock.GetUniversities());
        }

        public void GetNextItemList()
        {
            Catalog.Add(SelectedModel);

            Models = _supplier.GetModelsList(Catalog.Count, SelectedModel.ID);

            SelectedModel = null;
            OnPropertyChanged(nameof(Models));
        }
        public void GetPrevItemList()
        {
            Catalog.RemoveAt(Catalog.Count - 1);

            var id = Catalog.Count == 0 ? Guid.Empty : Catalog[^1].ID;
            Models = _supplier.GetModelsList(Catalog.Count, id);

            SelectedModel = null;
            OnPropertyChanged(nameof(Models));
        }

        public void AddItem(string name)
        {
            Models.RemoveAt(Models.Count - 1);
            Models.Add(_supplier.AddNewModel(Catalog.Count, name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
