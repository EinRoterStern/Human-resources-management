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
    public class PositionEditingViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        //private readonly ObservableCollection<PositionDGModel> _positionDGModels;


        public PositionEditingViewModel(MainViewModel mainView, ObservableCollection<PositionDGModel> positionDGs) 
        {
            _mainViewModel = mainView;
            //_positionDGModels = positionDGs;

            _ = InitAsync();

            SaveCommand = new RelayCommand(ExecuteSave, () => true);
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

            //if (_positionDGModels == null)
            //{
            //    MessageBox.Show("Не удалось получить таблицу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    FilteredProject = CollectionViewSource.GetDefaultView("Не удалось получить список должностей");
            //    return;
            //}
            //if (_positionDGModels.Count > 0)
            //{
            //    FilteredProject = CollectionViewSource.GetDefaultView(_positionDGModels.Select(d => d.name));
            //    FilteredProject.Filter = FilterProject;
            //}
            //else
            //    FilteredProject = CollectionViewSource.GetDefaultView("Не удалось получить список должностей");

        }

        private ObservableCollection<PositionDGModel> _positionDGModels;
        public ObservableCollection<PositionDGModel> PositionDGModels
        {
            get => _positionDGModels;
            set => SetProperty(ref _positionDGModels, value);
        }

        private string _oldName;
        public string OldName
        {
            get => _oldName;
            set
            {
                _oldName = value;
                OnPropertyChanged();
            }
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
                    LoadPosition(_selectedProj);
                }
            }
        }


        private void LoadPosition(Guid id)
        {
            OldName = _positionDGModels.FirstOrDefault(d => d.id == id).name;
        }

        public ICommand SaveCommand { get; set; }

        private async void ExecuteSave(object obj)
        {
            if (SelectedProj == Guid.Empty)
            {
                MessageBox.Show("Не выбрана должность!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Название должности не должно быть пустым!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var (success, message) = await DataBaseHelper.UpdatePosition(SelectedProj, Name);
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
