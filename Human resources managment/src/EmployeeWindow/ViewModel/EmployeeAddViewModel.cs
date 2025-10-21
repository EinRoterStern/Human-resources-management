using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Human_resources_managment.EmployeeWindow.Model;
using Human_resources_managment.ViewModel;

namespace Human_resources_managment.EmployeeWindow.ViewModel
{
    public class EmployeeAddViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainWindowViewModel;
        private readonly ObservableCollection<EmployeeDGModel> _employeeDGModels;
        public EmployeeAddViewModel(MainViewModel mainView, ObservableCollection<EmployeeDGModel> employeeDG) 
        {
            _mainWindowViewModel = mainView;
            _employeeDGModels = employeeDG;
        }
    }
}
