using Human_resources_managment.Classes;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.DepartmentWindow.ViewModel;
using Human_resources_managment.EmployeeWindow.ViewModel;
using Human_resources_managment.PositionWindow.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Human_resources_managment.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel() 
        {
            OpenDepartmentCommand = new RelayCommand(ExecuteOpenDepartment, () => true);
            OpenEmployeeCommand = new RelayCommand(ExecuteOpenEmployee, () => true);
            OpenPositionCommand = new RelayCommand(ExecuteOpenPosition, () => true);
        }


        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        private object _currentCenterControl;
        public object CurrentCenterControl
        {
            get => _currentCenterControl;
            set
            {
                if (_currentCenterControl != value)
                {
                    _currentCenterControl = value;
                    OnPropertyChanged();
                }
            }
        }


        private Visibility _mainButton = Visibility.Visible;
        public Visibility MainButton
        {
            get => _mainButton;
            set
            {
                _mainButton = value;
                OnPropertyChanged();
            }
        }

        private bool _enableCurrentView = true;
        public bool EnableCurrentView
        {
            get => _enableCurrentView;
            set
            {
                if (_enableCurrentView != value)
                {
                    _enableCurrentView = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand OpenDepartmentCommand { get; set; }
        public ICommand OpenEmployeeCommand { get; set; }
        public ICommand OpenPositionCommand { get; set; }

        private async void ExecuteOpenDepartment(object parameter)
        {
            MainButton = Visibility.Collapsed;
            var refVM = new DepartmenViewModel(this);
            CurrentView = refVM;
        }

        public async void RefreshDepartment()
        {
            var refVM = new DepartmenViewModel(this);
            CurrentView = refVM;
        }

        private async void ExecuteOpenEmployee(object parameter)
        {
            MainButton = Visibility.Collapsed;
            var refVM = new EmployeeViewModel(this);
            CurrentView = refVM;
        }

        private async void ExecuteOpenPosition(object parameter)
        {
            MainButton = Visibility.Collapsed;
            var refVM = new PositionViewModel(this);
            CurrentView = refVM;
        }


        public void BackToMainWindow()
        {
            CurrentView = null;
            MainButton = Visibility.Visible;
        }

        public void OpenChangeDepartment(ObservableCollection<DepartmenDGModel> table)
        {
            var VM = new DepartmentChangeViewModel(this, table);
            CurrentCenterControl = VM;
        }

        public void CloseAddView()
        {
            EnableCurrentView = true;
            CurrentCenterControl = null;
        }


    }
}
