using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Human_resources_managment.Classes;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.PositionWindow.Model;
using Human_resources_managment.ViewModel;

namespace Human_resources_managment.PositionWindow.ViewModel
{
    public class PositionAddViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        private readonly ObservableCollection<PositionDGModel> Tables;

        public PositionAddViewModel(MainViewModel mainViewModel, ObservableCollection<PositionDGModel> table) 
        {
            this._mainViewModel = mainViewModel;
            this.Tables = table;

            SaveCommand = new RelayCommand(ExecuteSave, () => true);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; set; }
        private async void ExecuteSave(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Название должности не должно быть пустым!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Tables.Add(new PositionDGModel
            {
                name = Name
            });

            Name = null;

            _mainViewModel.CloseAddView();
            _mainViewModel.RefreshPosition();
        }

    }
}
