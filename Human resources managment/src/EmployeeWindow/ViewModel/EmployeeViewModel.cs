using Human_resources_managment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.EmployeeWindow.ViewModel
{
    public class EmployeeViewModel
    {
        private readonly MainViewModel _mainWindowViewModel;
        public EmployeeViewModel(MainViewModel mainViewModel)
        {
            _mainWindowViewModel = mainViewModel;
        }
    }
}
