using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Human_resources_managment.Classes;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.PositionWindow.Model;
using Human_resources_managment.ViewModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Human_resources_managment.PositionWindow.ViewModel
{
    public class PositionDeleteViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        //private readonly ObservableCollection<PositionDGModel> _positionDGModels;
        public PositionDeleteViewModel(MainViewModel mainViewModel, ObservableCollection<PositionDGModel> positionDGs) 
        {
            _mainViewModel = mainViewModel;
            //_positionDGModels = positionDGs;

            _ = InitAsync();

            DeleteCommand = new RelayCommand(ExecuteDelete, () => true);
        }

        public async Task InitAsync()
        {
            var (positionDG, message) = await DataBaseHelper.GetPositionTable();
            if (positionDG != null)
            {
                PositionDGModels = new ObservableCollection<PositionDGModel>(positionDG.ToList());
            }
            else
            {
                MessageBox.Show($"Ошибка: {message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                PositionDGModels = new ObservableCollection<PositionDGModel>(null);
                return;
            }


        }

        private ObservableCollection<PositionDGModel> _positionDGModels;
        public ObservableCollection<PositionDGModel> PositionDGModels
        {
            get => _positionDGModels;
            set => SetProperty(ref _positionDGModels, value);
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
                    LoadDepart(_selectedProj);
                }
            }
        }

       

        private void LoadDepart(Guid id)
        {
            Name = _positionDGModels.FirstOrDefault(d => d.id == id).name;
        }

        public ICommand DeleteCommand { get; set; }
        private async void ExecuteDelete(object obj)
        {
            if (SelectedProj == Guid.Empty)
            {
                MessageBox.Show("Не выбрана должность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var (success, message) = await DataBaseHelper.DeletePosition(SelectedProj);
            if (success)
            {
                MessageBox.Show(message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedProj = Guid.Empty;
                _mainViewModel.CloseAddView();
                _mainViewModel.RefreshPosition();
            }
            else
            {
                MessageBox.Show($"{message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

    }
}
