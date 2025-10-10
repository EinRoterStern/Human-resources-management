using Human_resources_managment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_resources_managment.PositionWindow.ViewModel
{
    public class PositionViewModel
    {
        private readonly MainViewModel _mainWindowViewModel;
        public PositionViewModel(MainViewModel mainViewModel) 
        {
            _mainWindowViewModel = mainViewModel;
        }
    }
}
