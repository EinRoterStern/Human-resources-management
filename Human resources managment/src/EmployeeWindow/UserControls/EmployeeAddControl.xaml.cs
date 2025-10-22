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

namespace Human_resources_managment.EmployeeWindow.UserControls
{
    /// <summary>
    /// Логика взаимодействия для EmployeeAddControl.xaml
    /// </summary>
    public partial class EmployeeAddControl : UserControl
    {
        public EmployeeAddControl()
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

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            email.Focus();
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(email.Text) && email.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }

        private void textPhone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            phone.Focus();
        }

        private void phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(phone.Text) && phone.Text.Length > 0)
            {
                textPhone.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPhone.Visibility = Visibility.Visible;
            }
        }
    }
}
