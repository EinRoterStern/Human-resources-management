using Human_resources_managment.Classes;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.PositionWindow.Model;
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

namespace Human_resources_managment.PositionWindow.ViewModel
{
    public class PositionViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainWindowViewModel;
        public PositionViewModel(MainViewModel mainViewModel) 
        {
            _mainWindowViewModel = mainViewModel;

            _ = InitAsync();


            BackCommand = new RelayCommand(ExecuteBack, () => true);
            ChangeCommand = new RelayCommand(ExecuteOpenChange, () => true);
        }

        private async Task InitAsync()
        {
            Tables = new ObservableCollection<PositionDGModel>
            {
               new PositionDGModel{ name = "Инженер" },
               new PositionDGModel{ name = "Инженер-программист" },
               new PositionDGModel{ name = "Бухгалтер" }
                
            };
            _collectionView = CollectionViewSource.GetDefaultView(Tables);
            _collectionView.Filter = FilterProjects;
        }

        private ObservableCollection<PositionDGModel> _tables;
        public ObservableCollection<PositionDGModel> Tables
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
            _mainWindowViewModel.OpenChangePosition(Tables);
            _mainWindowViewModel.EnableCurrentView = false;
        }

        private bool FilterProjects(object obj)
        {
            if (obj is PositionDGModel project)
            {
                if (string.IsNullOrWhiteSpace(Search))
                    return true;

                return project.name?.Contains(Search, StringComparison.OrdinalIgnoreCase) == true
                /*|| project.ownerFio?.Contains(Search, StringComparison.OrdinalIgnoreCase) == true*/
                ;
            }
            return false;
        }
    }
}
