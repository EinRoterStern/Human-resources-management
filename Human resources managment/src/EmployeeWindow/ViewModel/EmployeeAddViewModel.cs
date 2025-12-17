using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Human_resources_managment.Classes;
using Human_resources_managment.Classes.Validate;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.EmployeeWindow.Model;
using Human_resources_managment.Models.ValueObjectModels;
using Human_resources_managment.PositionWindow.Model;
using Human_resources_managment.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Human_resources_managment.EmployeeWindow.ViewModel
{
    public class EmployeeAddViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainWindowViewModel;
        private readonly ObservableCollection<EmployeeDGModel> _employeeDGModels;

        private readonly ObservableCollection<EmployeeDGModel> Tables;

        public EmployeeAddViewModel(MainViewModel mainView, ObservableCollection<EmployeeDGModel> employeeDG) 
        {
            _mainWindowViewModel = mainView;
            _employeeDGModels = employeeDG;

            _ = InitAsync();

            SaveCommand = new RelayCommand(ExecuteSave, () => true);
        }

        public async Task InitAsync()
        {

            var (departmentDG, message) = await DataBaseHelper.GetDepartmentTable();
            if (departmentDG != null)
            {
                DepartmenDGModels = new ObservableCollection<DepartmentDGModel>(departmentDG.ToList());
            }
            else
            {
                MessageBox.Show($"Ошибка: {message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //FilteredProject = new ObservableCollection<DepartmentDGModel>("Не удалось получить список отделов");
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

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email == value) return;
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone == value) return;
                _phone = value;
                OnPropertyChanged();
            }
        }

        private DateOnly? _selectedBirthData;
        public DateOnly? SelectedBirthData
        {
            get => _selectedBirthData;
            set
            {
                _selectedBirthData = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _selectedDateForPickerBirth;
        public DateTime? SelectedDateForPickerBirth
        {
            get => _selectedDateForPickerBirth;
            set
            {
                _selectedDateForPickerBirth = value;

                // Преобразуем в DateOnly при установке даты
                if (value.HasValue)
                    SelectedBirthData = DateOnly.FromDateTime(value.Value);
                else
                    SelectedBirthData = null;

                OnPropertyChanged();
            }
        }


        private DateOnly? _selectedHireData;
        public DateOnly? SelectedHireData
        {
            get => _selectedHireData;
            set
            {
                _selectedHireData = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _selectedDateForPickerHire;
        public DateTime? SelectedDateForPickerHire
        {
            get => _selectedDateForPickerHire;
            set
            {
                _selectedDateForPickerHire = value;

                // Преобразуем в DateOnly при установке даты
                if (value.HasValue)
                    SelectedHireData = DateOnly.FromDateTime(value.Value);
                else
                    SelectedHireData = null;

                OnPropertyChanged();
            }
        }

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
        private async void ExecuteSave(object parameter)
        {
            if (string.IsNullOrWhiteSpace(FIO))
            {
                MessageBox.Show("ФИО не должно быть пустым!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var parts = FIO.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2)
            {
                MessageBox.Show("ФИО должно содержать хотя бы фамилию и имя!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (!ValidateDate.IsValidDate(SelectedDateForPickerBirth))
            {
                string error = ValidateDate.GetValidationError(SelectedDateForPickerBirth);
                MessageBox.Show(error, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidateDate.IsValidDate(SelectedDateForPickerHire))
            {
                string error = ValidateDate.GetValidationError(SelectedDateForPickerHire);
                MessageBox.Show(error, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            if (SelectedDepart == Guid.Empty)
            {
                MessageBox.Show("Отдел не должен быть пустой!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (SelectedPos == Guid.Empty)
            {
                MessageBox.Show("Должность не должна быть пустой!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            //Tables.Add(new EmployeeDGModel
            //{
            //    FIO = FIO,
            //    birthDate = SelectedBirthData,
            //    hireDate = SelectedHireData,
            //    phone = Phone,
            //    email = Email,
            //});

            //FIO = null;
            //Phone = null;

            var (firstName, lastName, middleName) = DataBaseHelper.ParseFio(FIO);

            var (success, message) = await DataBaseHelper.AddEmployee(firstName, lastName, middleName, SelectedBirthData, SelectedHireData, Email, Phone, SelectedPos, SelectedDepart);
            if (success)
            {
                MessageBox.Show(message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
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
