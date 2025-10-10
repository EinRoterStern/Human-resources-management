using Human_resources_managment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.DepartmentWindow.ViewModel
{
    public class DepartmenViewModel
    {
        private readonly MainViewModel _mainWindowViewModel;
        public DepartmenViewModel(MainViewModel mainViewModel)
        {
            _mainWindowViewModel = mainViewModel;
        }
    }
}
