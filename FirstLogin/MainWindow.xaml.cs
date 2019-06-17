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
using MySql.Data.MySqlClient;
using System.Data;

namespace FirstLogin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        clsDb dbConn = new clsDb();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window1 win1 = new Window1();
            win1.ShowDialog();
            this.Show();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            int iUserID = dbConn.GetUserID(tbUsername.Text, tbPassword.Text);
            if (iUserID > 0)
            {
                //MessageBox.Show("Ingelogd!");

                Game game = new Game(iUserID);
                game.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ongeldige gebruikersnaam of wachtwoord");
            }

        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text == "Username")
            {
                tbUsername.Text = "";
            }
        }

        private void tbUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text == "")
            {
                tbUsername.Text = "Username";
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
