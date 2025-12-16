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
    public class DepartmentEditingViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        //private readonly ObservableCollection<DepartmentDGModel> _departmenDGModels;
        public DepartmentEditingViewModel(MainViewModel mainViewModel, ObservableCollection<DepartmentDGModel> departmenDGModels) 
        {
            //_departmenDGModels = departmenDGModels;
            _mainViewModel = mainViewModel;
            _ = InitAsync();

            SaveCommand = new RelayCommand(ExecuteSave, () => true);
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
                //FilteredProject = new ObservableCollection<DepartmentDGModel>("Не удалось получить список отделов");
                DepartmenDGModels = new ObservableCollection<DepartmentDGModel>(null);
                return;
            }

            //if (_departmenDGModels == null)
            //{
            //    MessageBox.Show("Не удалось получить таблицу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    FilteredProject = CollectionViewSource.GetDefaultView("Не удалось получить список отделов");
            //    return;
            //}
            //if (_departmenDGModels.Count > 0)
            //{
            //    FilteredProject = CollectionViewSource.GetDefaultView(_departmenDGModels.Select(d => d.name));
            //    FilteredProject.Filter = FilterProject;
            //}
            //else
            //    FilteredProject = CollectionViewSource.GetDefaultView("Не удалось получить список отделов");

        }

        private ObservableCollection<DepartmentDGModel> _departmenDGModels;
        public ObservableCollection<DepartmentDGModel> DepartmenDGModels
        {
            get => _departmenDGModels;
            set => SetProperty(ref _departmenDGModels, value);
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

        private string _oldDescription;
        public string OldDescription
        {
            get => _oldDescription;
            set
            { _oldDescription = value; OnPropertyChanged(); }
        }


        private Guid _selectedProj;
        public Guid SelectedProj
        {
            get => _selectedProj;
            set
            {
                _selectedProj = value;
                OnPropertyChanged();
                if (_selectedProj != null && _selectedProj.ToString() != "Не удалось получить список отделов")
                {
                    LoadDepart(_selectedProj);
                }
            }
        }


        private void LoadDepart(Guid id)
        {
            OldDescription = _departmenDGModels.FirstOrDefault(d => d.id == id).description;
            OldName = _departmenDGModels.FirstOrDefault(d => d.id == id).name;
        }

        public ICommand SaveCommand { get; set; }

        private async void ExecuteSave(object obj)
        {
            if (SelectedProj == null)
            {
                MessageBox.Show("Не выбран отдел!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Название отдела не должно быть пустым!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Описание отдела не должно быть пустым!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            SelectedProj = Guid.Empty;
            _mainViewModel.CloseAddView();
            _mainViewModel.RefreshDepartment();
        }

    }
}
