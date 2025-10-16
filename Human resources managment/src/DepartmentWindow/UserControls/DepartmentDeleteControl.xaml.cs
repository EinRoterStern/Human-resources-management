using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Human_resources_managment.DepartmentWindow.UserControls
{
    /// <summary>
    /// Логика взаимодействия для DepartmentDeleteControl.xaml
    /// </summary>
    public partial class DepartmentDeleteControl : UserControl
    {
        public DepartmentDeleteControl()
        {
            InitializeComponent();
        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            search.Focus();
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(search.Text) && search.Text.Length > 0)
            {
                textSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                textSearch.Visibility = Visibility.Visible;
            }
        }

        private void textDesc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            desc.Focus();
        }

        private void desc_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(desc.Text) && desc.Text.Length > 0)
            {
                textDesc.Visibility = Visibility.Collapsed;
            }
            else
            {
                textDesc.Visibility = Visibility.Visible;
            }
        }
    }
}
