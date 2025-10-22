using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Human_resources_managment.DepartmentWindow.Model;
using System.Windows.Input;
using System.Windows;
using Human_resources_managment.EmployeeWindow.Model;
using Human_resources_managment.ViewModel;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections;
using Human_resources_managment.Classes;
using Human_resources_managment.Classes.Validate;
using Human_resources_managment.Models.ValueObjectModels;

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

            SaveCommand = new RelayCommand(ExecuteSave, () => true);
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

            Tables.Add(new EmployeeDGModel
            {
                FIO = FIO,
                birthDate = SelectedBirthData,
                hireDate = SelectedHireData,
                phone = Phone,
                email = Email,
            });

            //FIO = null;
            //Phone = null;

            _mainWindowViewModel.CloseAddView();
            _mainWindowViewModel.RefreshEmployee();
        }

    }
}
