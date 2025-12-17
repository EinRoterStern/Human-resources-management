using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using Human_resources_managment.Classes;
using Human_resources_managment.Classes.Validate;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.EmployeeWindow.Model;
using Human_resources_managment.PositionWindow.Model;
using Human_resources_managment.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Human_resources_managment.EmployeeWindow.ViewModel
{
    public class EmployeeEditingViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainWindowViewModel;
        //private readonly ObservableCollection<EmployeeDGModel> _employeeDGModels;
        public EmployeeEditingViewModel(MainViewModel mainView, ObservableCollection<EmployeeDGModel> employeeDGs ) 
        {
            _mainWindowViewModel = mainView;
            //_employeeDGModels = employeeDGs;

            _ = InitAsync();

            SaveCommand = new RelayCommand(ExecuteSave, () => true);
        }

        public async Task InitAsync()
        {
            var (employeeDG, messageEmpl) = await DataBaseHelper.GetEmployeeTable();
            if (employeeDG != null)
            {
                EmployeeDGModels = new ObservableCollection<EmployeeDGModel>(employeeDG.ToList());
            }
            else
            {
                MessageBox.Show($"Ошибка: {messageEmpl}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                EmployeeDGModels = new ObservableCollection<EmployeeDGModel>(null);
                return;
            }

            var (departmentDG, message) = await DataBaseHelper.GetDepartmentTable();
            if (departmentDG != null)
            {
                DepartmenDGModels = new ObservableCollection<DepartmentDGModel>(departmentDG.ToList());
            }
            else
            {
                MessageBox.Show($"Ошибка: {message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                DepartmenDGModels = new ObservableCollection<DepartmentDGModel>(null);
                return;
            }

            var (positionDG, messagePos) = await DataBaseHelper.GetPositionTable();
            if (positionDG != null)
            {
                PositionDGModels = new ObservableCollection<PositionDGModel>(positionDG.ToList());
            }
            else
            {
                MessageBox.Show($"Ошибка: {messagePos}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                PositionDGModels = new ObservableCollection<PositionDGModel>(null);
                return;
            }

        }

        private ObservableCollection<EmployeeDGModel> _employeeDGModels;
        public ObservableCollection<EmployeeDGModel> EmployeeDGModels
        {
            get => _employeeDGModels;
            set => SetProperty(ref _employeeDGModels, value);
        }

        private ObservableCollection<DepartmentDGModel> _departmenDGModels;
        public ObservableCollection<DepartmentDGModel> DepartmenDGModels
        {
            get => _departmenDGModels;
            set => SetProperty(ref _departmenDGModels, value);
        }

        private ObservableCollection<PositionDGModel> _positionDGModels;
        public ObservableCollection<PositionDGModel> PositionDGModels
        {
            get => _positionDGModels;
            set => SetProperty(ref _positionDGModels, value);
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
        private Guid _selectedProj;
        public Guid SelectedProj
        {
            get => _selectedProj;
            set
            {
                _selectedProj = value;
                OnPropertyChanged();
                if (_selectedProj != Guid.Empty)
                {
                    LoadEmployee(_selectedProj);
                }
            }
        }

        private void LoadEmployee(Guid id)
        {
            Email = _employeeDGModels.FirstOrDefault(d => d.id == id).email;
            Phone = _employeeDGModels.FirstOrDefault(d => d.id == id).phone;

            var dep = _employeeDGModels.FirstOrDefault(d => d.id == id).departmentId;
            SelectedDepart = (Guid)_departmenDGModels.FirstOrDefault(d => d.id == dep).id;

            var pos = _employeeDGModels.FirstOrDefault(d => d.id == id).positionId;
            SelectedPos = (Guid)_positionDGModels.FirstOrDefault(d => d.id == pos).id;
        }

        // Отделы
        private Guid _selectedDepart;
        public Guid SelectedDepart
        {
            get => _selectedDepart;
            set
            {
                _selectedDepart = value;
                OnPropertyChanged();
            }
        }


        // Должность
        private Guid _selectedPos;
        public Guid SelectedPos
        {
            get => _selectedPos;
            set
            {
                _selectedPos = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; set; }

        private async void ExecuteSave(object obj)
        {
            if (SelectedProj == Guid.Empty)
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

            if(SelectedDepart == Guid.Empty)
            {
                MessageBox.Show("Не выбран отдел", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedPos == Guid.Empty)
            {
                MessageBox.Show("Не выбрана должность", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var (success, message) = await DataBaseHelper.UpdateEmployee(SelectedProj,Email, Phone, SelectedDepart, SelectedPos);
            if (success)
            {
                MessageBox.Show(message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                SelectedProj = Guid.Empty;
                _mainWindowViewModel.CloseAddView();
                _mainWindowViewModel.RefreshEmployee();
            }
            else
            {
                MessageBox.Show($"{message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
        }

    }
}
