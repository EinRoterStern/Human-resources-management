using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Human_resources_managment.Classes;
using Human_resources_managment.DepartmentWindow.Model;
using Human_resources_managment.DepartmentWindow.Model.dtoModel;
using Human_resources_managment.ViewModel;

namespace Human_resources_managment.DepartmentWindow.ViewModel
{
    public class DepartmenAddViewModel : ViewModelBase
    {
        private readonly MainViewModel viewModel;

        private readonly ObservableCollection<DepartmentDGModel> Tables;

        public DepartmenAddViewModel(MainViewModel viewModel, ObservableCollection<DepartmentDGModel> table)
        {
            this.viewModel = viewModel;
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


        public ICommand SaveCommand { get; set; }
        private async void ExecuteSave(object parameter)
        {
            if(string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Название отдела не должно быть пустым!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Описание отдела не должно быть пустым!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Tables.Add(new DepartmentDGModel
            {
                name = Name,
                description = Description
            });

            Name = null;
            Description = null;

            viewModel.CloseAddView();
            viewModel.RefreshDepartment();
        }
    }
}
