using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.ViewModel;
using System.ComponentModel;
using System.Windows.Input;
using Human_resources_managment.Classes;

namespace Human_resources_managment.DepartmentWindow.ViewModel
{
    public class DepartmentDeleteViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        public DepartmentDeleteViewModel(MainViewModel mainViewModel, ObservableCollection<DepartmentDGModel> departmenDGModels) 
        {
            _mainViewModel = mainViewModel;

            _ = InitAsync();

            DeleteCommand = new RelayCommand(ExecuteDelete, () => true);
        }

        public async Task InitAsync()
        {

            var (departmentDG, message) = await DataBaseHelper.GetDepartmentTable();
            if (departmentDG != null)
            {
                DepartmenDGModels = new ObservableCollection<DepartmentDGModel>(departmentDG.ToList());
            }
            else
            {
                MessageBox.Show($"Ошибка: {message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                DepartmenDGModels = new ObservableCollection<DepartmentDGModel>(null);
                return;
            }

        }

        private ObservableCollection<DepartmentDGModel> _departmenDGModels;
        public ObservableCollection<DepartmentDGModel> DepartmenDGModels
        {
            get => _departmenDGModels;
            set => SetProperty(ref _departmenDGModels, value);
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

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
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
            Description = _departmenDGModels.FirstOrDefault(d => d.id == id).description;
            Name = _departmenDGModels.FirstOrDefault(d => d.id == id).name;
        }

        public ICommand DeleteCommand { get; set; }
        private async void ExecuteDelete(object obj)
        {
            if (SelectedProj == Guid.Empty)
            {
                MessageBox.Show("Не выбран отдел!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var (success, message) = await DataBaseHelper.DeleteDepartment(SelectedProj);
            if (success)
            {
                MessageBox.Show(message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedProj = Guid.Empty;
                _mainViewModel.CloseAddView();
                _mainViewModel.RefreshDepartment();
            }
            else
            {
                MessageBox.Show($"{message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
        }
    }
}
