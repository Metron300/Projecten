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
using System.Windows.Shapes;

namespace FirstLogin
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        clsDb dbConn = new clsDb();
        public Window1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            dbConn.AddUser(tbUserName.Text, tbPassword.Text);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void tbUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbUserName.Text == "Username")
            {
                tbUserName.Text = "";
            }
        }

        private void tbUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbUserName.Text == "")
            {
                tbUserName.Text = "Username";
            }
        }

        private void tbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Text == "Password")
            {
                tbPassword.Text = "";
            }
        }

        private void tbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Text == "")
            {
                tbPassword.Text = "Password";
            }
        }
    }
}
