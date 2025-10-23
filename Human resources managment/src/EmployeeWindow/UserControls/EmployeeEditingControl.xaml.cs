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
using Human_resources_managment.EmployeeWindow.ViewModel;

namespace Human_resources_managment.EmployeeWindow.UserControls
{
    /// <summary>
    /// Логика взаимодействия для EmployeeEditingControl.xaml
    /// </summary>
    public partial class EmployeeEditingControl : UserControl
    {
        public EmployeeEditingControl()
        {
            InitializeComponent();

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
