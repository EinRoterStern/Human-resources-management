using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Human_resources_managment.Classes;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.DepartmentWindow.ViewModel;
using Human_resources_managment.EmployeeWindow.Model;
using Human_resources_managment.Models;
using Human_resources_managment.ViewModel;

namespace Human_resources_managment.EmployeeWindow.ViewModel
{
    public class EmployeeChangeViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainWindowViewModel;
        private readonly ObservableCollection<EmployeeDGModel> _employeeDGModels;

        public ObservableCollection<TabItemModel> Tabs { get; set; }

        public EmployeeChangeViewModel(MainViewModel mainWindowViewModel, ObservableCollection<EmployeeDGModel> table)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _employeeDGModels = table;

            Tabs = new ObservableCollection<TabItemModel>
            {
                new TabItemModel { Header = "Добавить" },
                new TabItemModel { Header = "Изменить" },
                new TabItemModel { Header = "Удалить" }
            };

            SelectedTab = Tabs[0]; // <-- задаём первую вкладку
            SelectedTab.ContentViewModel = new EmployeeAddViewModel(_mainWindowViewModel, table);

            CloseCommand = new RelayCommand(ExecuteClose, () => true);
        }

        private TabItemModel _selectedTab;
        public TabItemModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    OnPropertyChanged();

                    if (_selectedTab != null)
                    {
                        LoadTabContent(_selectedTab);
                    }
                }
            }
        }

        public ICommand CloseCommand { get; set; }

        private async void ExecuteClose(object parameter)
        {
            _mainWindowViewModel.CloseAddView();
        }

        public void LoadTabContent(TabItemModel tab)
        {
            if (tab.Header == "Добавить")
                tab.ContentViewModel = new EmployeeAddViewModel(_mainWindowViewModel, _employeeDGModels);
            if (tab.Header == "Изменить")
                tab.ContentViewModel = new EmployeeEditingViewModel(_mainWindowViewModel, _employeeDGModels);
            if (tab.Header == "Удалить")
                tab.ContentViewModel = new EmployeeDeleteViewModel(_mainWindowViewModel, _employeeDGModels);
        }


    }
}
