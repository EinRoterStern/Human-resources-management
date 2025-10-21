using Human_resources_managment.Classes;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.EmployeeWindow.Model;
using Human_resources_managment.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Human_resources_managment.EmployeeWindow.ViewModel
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainWindowViewModel;
        public EmployeeViewModel(MainViewModel mainViewModel)
        {
            _mainWindowViewModel = mainViewModel;
            _ = InitAsync();


            BackCommand = new RelayCommand(ExecuteBack, () => true);
            ChangeCommand = new RelayCommand(ExecuteOpenChange, () => true);
        }

        private async Task InitAsync()
        {
            Tables = new ObservableCollection<EmployeeDGModel>
            {
               new EmployeeDGModel{ FIO = "Абдула Али", birthDate = DateOnly.ParseExact("22.04.2004", "dd.MM.yyyy"), hireDate = DateOnly.ParseExact("22.04.2025", "dd.MM.yyyy"), email = "amail@mail.ru", phone = "89999999999" },
               new EmployeeDGModel{ FIO = "Резников Константин Игоревич", birthDate = DateOnly.ParseExact("01.12.2000", "dd.MM.yyyy"), hireDate = DateOnly.ParseExact("22.04.2024", "dd.MM.yyyy"), email = "pamail@mail.ru", phone = "89889999966" },
               new EmployeeDGModel{ FIO = "Игнатьев Валентайн Архипович", birthDate = DateOnly.ParseExact("15.08.1980", "dd.MM.yyyy"), hireDate = DateOnly.ParseExact("01.01.2000", "dd.MM.yyyy"), email = "mamail@mail.ru", phone = "89779999933" }

            };
            _collectionView = CollectionViewSource.GetDefaultView(Tables);
            _collectionView.Filter = FilterProjects;
        }


        private ObservableCollection<EmployeeDGModel> _tables;
        public ObservableCollection<EmployeeDGModel> Tables
        {
            get => _tables;
            set => SetProperty(ref _tables, value);
        }
        private ICollectionView _collectionView;

        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                OnPropertyChanged();
                _collectionView?.Refresh();
            }
        }

        public ICommand BackCommand { get; set; }
        public ICommand ChangeCommand { get; set; }

        private async void ExecuteBack(object parameter)
        {
            _mainWindowViewModel.BackToMainWindow();
        }

        private async void ExecuteOpenChange(object parameter)
        {
            _mainWindowViewModel.OpenChangeEmployee(Tables);
            _mainWindowViewModel.EnableCurrentView = false;
        }

        private bool FilterProjects(object obj)
        {
            if (obj is EmployeeDGModel project)
            {
                if (string.IsNullOrWhiteSpace(Search))
                    return true;

                return project.FIO?.Contains(Search, StringComparison.OrdinalIgnoreCase) == true
                    || project.email?.Contains(Search, StringComparison.OrdinalIgnoreCase) == true
                    || project.phone?.Contains(Search, StringComparison.OrdinalIgnoreCase) == true
                /*|| project.ownerFio?.Contains(Search, StringComparison.OrdinalIgnoreCase) == true*/
                ;
            }
            return false;
        }


    }
}
