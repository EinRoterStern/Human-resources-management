using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Human_resources_managment.ViewModel;

namespace Human_resources_managment.Models
{
    public class TabItemModel : ViewModelBase
    {
        public string Header { get; set; }
        public object ContentViewModel { get; set; }
        public string IconName { get; set; } = string.Empty;
    }
}
