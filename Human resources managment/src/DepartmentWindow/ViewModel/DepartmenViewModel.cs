using Human_resources_managment.Classes;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.ViewModel;
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

namespace Human_resources_managment.DepartmentWindow.ViewModel
{
    public class DepartmenViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainWindowViewModel;
        public DepartmenViewModel(MainViewModel mainViewModel)
        {
            _mainWindowViewModel = mainViewModel;
            _ = InitAsync();


            BackCommand = new RelayCommand(ExecuteBack, () => true);
            ChangeCommand = new RelayCommand(ExecuteOpenChange, () => true);
        }

        private async Task InitAsync()
        {
            Tables = new ObservableCollection<DepartmenDGModel>
            {
               new DepartmenDGModel{ name = "IT", description = "Технический отдел, отвечающий за все компьютеры в офисе" },
               new DepartmenDGModel{ name = "Бухгалтерский", description = "Бухгалтерский отдел, отвечающий за деньги" },
               new DepartmenDGModel{ name = "ИЦ", description = "Испытательный центр, испытывает все нововедения" }

            };
            _collectionView = CollectionViewSource.GetDefaultView(Tables);
            _collectionView.Filter = FilterProjects;
        }

        private ObservableCollection<DepartmenDGModel> _tables;
        public ObservableCollection<DepartmenDGModel> Tables
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
            _mainWindowViewModel.OpenChangeDepartment(Tables);
            _mainWindowViewModel.EnableCurrentView = false;
        }

        private bool FilterProjects(object obj)
        {
            if (obj is DepartmenDGModel project)
            {
                if (string.IsNullOrWhiteSpace(Search))
                    return true;

                return project.name?.Contains(Search, StringComparison.OrdinalIgnoreCase) == true
                    || project.description?.Contains(Search, StringComparison.OrdinalIgnoreCase) == true
                /*|| project.ownerFio?.Contains(Search, StringComparison.OrdinalIgnoreCase) == true*/
                ;
            }
            return false;
        }

    }
}
