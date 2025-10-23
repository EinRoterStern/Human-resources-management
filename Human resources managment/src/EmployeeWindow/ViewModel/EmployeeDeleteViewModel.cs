using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Human_resources_managment.Classes;
using Human_resources_managment.EmployeeWindow.Model;
using Human_resources_managment.ViewModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;

namespace Human_resources_managment.EmployeeWindow.ViewModel
{
    public class EmployeeDeleteViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainWindowViewModel;
        private readonly ObservableCollection<EmployeeDGModel> _employeeDGModels;

        public EmployeeDeleteViewModel(MainViewModel mainView, ObservableCollection<EmployeeDGModel> employeeDGs)
        {
            _employeeDGModels = employeeDGs;
            _mainWindowViewModel = mainView;

            _ = InitAsync();

            DeleteCommand = new RelayCommand(ExecuteDelete, () => true);
        }

        public async Task InitAsync()
        {

            if (_employeeDGModels == null)
            {
                MessageBox.Show("Не удалось получить таблицу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FilteredProject = CollectionViewSource.GetDefaultView("Не удалось получить список сотрудников");
                return;
            }
            if (_employeeDGModels.Count > 0)
            {
                FilteredProject = CollectionViewSource.GetDefaultView(_employeeDGModels.Select(d => d.FIO));
                FilteredProject.Filter = FilterProject;
            }
            else
                FilteredProject = CollectionViewSource.GetDefaultView("Не удалось получить список сотрудников");

        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        private string _fio;
        public string FIO
        {
            get => _fio;
            set
            {
                _fio = value;
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
                if (_selectedProj != null && _selectedProj.ToString() != "Не удалось получить список сотрудников")
                {
                    LoadEmployee(_selectedProj.ToString());
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

        private void LoadEmployee(string emp)
        {
            FIO = _employeeDGModels.FirstOrDefault(d => d.FIO == emp).FIO;
            Email = _employeeDGModels.FirstOrDefault(d => d.FIO == emp).email;
            Phone = _employeeDGModels.FirstOrDefault(d => d.FIO == emp).phone;
        }

        public ICommand DeleteCommand { get; set; }
        private async void ExecuteDelete(object obj)
        {
            if (SelectedProj == "Не удалось получить список сотрудников" || SelectedProj == null)
            {
                MessageBox.Show("Не выбран сотрудник!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SelectedProj = null;
            _mainWindowViewModel.CloseAddView();
            _mainWindowViewModel.RefreshEmployee();
        }
    }
}
