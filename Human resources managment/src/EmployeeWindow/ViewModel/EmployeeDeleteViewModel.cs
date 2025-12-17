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
        //private readonly ObservableCollection<EmployeeDGModel> _employeeDGModels;

        public EmployeeDeleteViewModel(MainViewModel mainView, ObservableCollection<EmployeeDGModel> employeeDGs)
        {
           // _employeeDGModels = employeeDGs;
            _mainWindowViewModel = mainView;

            _ = InitAsync();

            DeleteCommand = new RelayCommand(ExecuteDelete, () => true);
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


        }

        private ObservableCollection<EmployeeDGModel> _employeeDGModels;
        public ObservableCollection<EmployeeDGModel> EmployeeDGModels
        {
            get => _employeeDGModels;
            set => SetProperty(ref _employeeDGModels, value);
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
            FIO = _employeeDGModels.FirstOrDefault(d => d.id == id).FIO;
            Email = _employeeDGModels.FirstOrDefault(d => d.id == id).email;
            Phone = _employeeDGModels.FirstOrDefault(d => d.id == id).phone;
        }

        public ICommand DeleteCommand { get; set; }
        private async void ExecuteDelete(object obj)
        {
            if (SelectedProj == Guid.Empty)
            {
                MessageBox.Show("Не выбран сотрудник!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var (success, message) = await DataBaseHelper.DeleteEmployee(SelectedProj);
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
