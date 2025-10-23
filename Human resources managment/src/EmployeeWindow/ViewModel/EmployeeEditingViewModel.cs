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
using Human_resources_managment.Classes.Validate;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Human_resources_managment.EmployeeWindow.ViewModel
{
    public class EmployeeEditingViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainWindowViewModel;
        private readonly ObservableCollection<EmployeeDGModel> _employeeDGModels;
        public EmployeeEditingViewModel(MainViewModel mainView, ObservableCollection<EmployeeDGModel> employeeDGs ) 
        {
            _mainWindowViewModel = mainView;
            _employeeDGModels = employeeDGs;

            _ = InitAsync();

            SaveCommand = new RelayCommand(ExecuteSave, () => true);
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


            // Отдел
            if (_employeeDGModels == null)
            {
                MessageBox.Show("Не удалось получить таблицу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FilteredDepart = CollectionViewSource.GetDefaultView("Не удалось получить список отделов");
                return;
            }
            if (_employeeDGModels.Count > 0)
            {
                FilteredDepart = CollectionViewSource.GetDefaultView(_employeeDGModels.Select(d => d.departmentName));
                FilteredDepart.Filter = FilterDepart;
            }
            else
                FilteredDepart = CollectionViewSource.GetDefaultView("Не удалось получить список отделов");

            // Должность
            if (_employeeDGModels == null)
            {
                MessageBox.Show("Не удалось получить таблицу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FilteredPos = CollectionViewSource.GetDefaultView("Не удалось получить список должностей");
                return;
            }
            if (_employeeDGModels.Count > 0)
            {
                FilteredPos = CollectionViewSource.GetDefaultView(_employeeDGModels.Select(d => d.positionName));
                FilteredPos.Filter = FilterPos;
            }
            else
                FilteredPos = CollectionViewSource.GetDefaultView("Не удалось получить список должностей");

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


        // Сотрудник
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

        private void LoadEmployee(string employee)
        {
            Email = _employeeDGModels.FirstOrDefault(d => d.FIO == employee).email;
            Phone = _employeeDGModels.FirstOrDefault(d => d.FIO == employee).phone;
            SelectedDepart = _employeeDGModels.FirstOrDefault(d => d.FIO == employee).departmentName;
            SelectedPos = _employeeDGModels.FirstOrDefault(d => d.FIO == employee).positionName;
        }

        // Отделы
        private object _selectedDepart;
        public object SelectedDepart
        {
            get => _selectedDepart;
            set
            {
                _selectedDepart = value;
                OnPropertyChanged();

                if (value != null)
                    FilterTextDepart = value.ToString();

            }
        }

        private ICollectionView _filteredDepart;
        public ICollectionView FilteredDepart
        {
            get => _filteredDepart;
            set { _filteredDepart = value; OnPropertyChanged(); }
        }

        private string _filterTextDepart;
        public string FilterTextDepart
        {
            get => _filterTextDepart;
            set
            {
                _filterTextDepart = value;
                OnPropertyChanged();
                FilteredDepart.Refresh(); // обновляем фильтр
            }
        }

        private bool FilterDepart(object obj)
        {
            if (obj is string supply)
            {
                return string.IsNullOrEmpty(FilterTextDepart) ||
                       supply.ToLower().Contains(FilterTextDepart.ToLower());
            }
            return false;
        }

        // Должность
        private object _selectedPos;
        public object SelectedPos
        {
            get => _selectedPos;
            set
            {
                _selectedPos = value;
                OnPropertyChanged();

                if (value != null)
                    FilterTextPos = value.ToString();

            }
        }

        private ICollectionView _filteredPos;
        public ICollectionView FilteredPos
        {
            get => _filteredPos;
            set { _filteredPos = value; OnPropertyChanged(); }
        }

        private string _filterTextPos;
        public string FilterTextPos
        {
            get => _filterTextPos;
            set
            {
                _filterTextPos = value;
                OnPropertyChanged();
                FilteredPos.Refresh(); // обновляем фильтр
            }
        }

        private bool FilterPos(object obj)
        {
            if (obj is string supply)
            {
                return string.IsNullOrEmpty(FilterTextPos) ||
                       supply.ToLower().Contains(FilterTextPos.ToLower());
            }
            return false;
        }

        public ICommand SaveCommand { get; set; }

        private async void ExecuteSave(object obj)
        {
            if (SelectedProj == "Не удалось получить список сотрудников" || SelectedProj == null)
            {
                MessageBox.Show("Не выбран сотрудник!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ValidateEmail.IsValidEmail(Email))
            {
                string error = ValidateEmail.GetValidationError(Email);
                MessageBox.Show(error, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidatePhone.IsValidPhone(Phone))
            {
                string error = ValidatePhone.GetValidationError(Phone);
                MessageBox.Show(error, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(SelectedDepart == null || SelectedDepart == "Не удалось получить список отделов")
            {
                MessageBox.Show("Не выбран отдел", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedPos == null || SelectedPos == "Не удалось получить список должностей")
            {
                MessageBox.Show("Не выбрана должность", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _mainWindowViewModel.CloseAddView();
            _mainWindowViewModel.RefreshEmployee();
        }

    }
}
