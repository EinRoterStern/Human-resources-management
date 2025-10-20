using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Human_resources_managment.Classes;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.PositionWindow.Model;
using Human_resources_managment.ViewModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Human_resources_managment.PositionWindow.ViewModel
{
    public class PositionEditingViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ObservableCollection<PositionDGModel> _positionDGModels;


        public PositionEditingViewModel(MainViewModel mainView, ObservableCollection<PositionDGModel> positionDGs) 
        {
            _mainViewModel = mainView;
            _positionDGModels = positionDGs;

            _ = InitAsync();

            SaveCommand = new RelayCommand(ExecuteSave, () => true);
        }

        public async Task InitAsync()
        {

            if (_positionDGModels == null)
            {
                MessageBox.Show("Не удалось получить таблицу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FilteredProject = CollectionViewSource.GetDefaultView("Не удалось получить список должностей");
                return;
            }
            if (_positionDGModels.Count > 0)
            {
                FilteredProject = CollectionViewSource.GetDefaultView(_positionDGModels.Select(d => d.name));
                FilteredProject.Filter = FilterProject;
            }
            else
                FilteredProject = CollectionViewSource.GetDefaultView("Не удалось получить список должностей");

        }

        private string _oldName;
        public string OldName
        {
            get => _oldName;
            set
            {
                _oldName = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private object _selectedProj;
        public object SelectedProj
        {
            get => _selectedProj;
            set
            {
                _selectedProj = value;
                OnPropertyChanged();
                if (_selectedProj != null && _selectedProj.ToString() != "Не удалось получить список должностей")
                {
                    LoadPosition(_selectedProj.ToString());
                }
            }
        }

        private ICollectionView _filteredProject;
        public ICollectionView FilteredProject
        {
            get => _filteredProject;
            set { _filteredProject = value; OnPropertyChanged(); }
        }

        private string _filterTextProject;
        public string FilterTextProject
        {
            get => _filterTextProject;
            set
            {
                _filterTextProject = value;
                OnPropertyChanged();
                FilteredProject.Refresh(); // обновляем фильтр
            }
        }

        private bool FilterProject(object obj)
        {
            if (obj is string supply)
            {
                return string.IsNullOrEmpty(FilterTextProject) ||
                       supply.ToLower().Contains(FilterTextProject.ToLower());
            }
            return false;
        }

        private void LoadPosition(string depart)
        {
            OldName = depart;
        }

        public ICommand SaveCommand { get; set; }

        private async void ExecuteSave(object obj)
        {
            if (SelectedProj == "Не удалось получить список должностей" || SelectedProj == null)
            {
                MessageBox.Show("Не выбрана должность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Название должности не должно быть пустым!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            SelectedProj = null;
            _mainViewModel.CloseAddView();
            _mainViewModel.RefreshPosition();
        }
    }
}
