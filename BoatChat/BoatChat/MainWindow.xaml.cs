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
using System.Net;
using MaterialDesignThemes;

namespace BoatChat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow LOGINWINDOW = null;
        public ServerConnct serverConnct;
        public string username;
        public string password;
        public MainWindow()
        {
            serverConnct = new ServerConnct();
            InitializeComponent();
            LOGINWINDOW = this;
            IDInput.Text = "2016011498";
            PsdInput.Password = "net2018";
        }

        private void LoginBtn__Login__Click(object sender, RoutedEventArgs e)
        {
            username = IDInput.Text.ToString();
            password = PsdInput.Password;
            if(serverConnct.ServerQuery(username + '_' + password) == "lol")
            {
                ChatWindow window = new ChatWindow(serverConnct);
                window.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Wrong Username or Password.");
            }
        }

        private void LoginWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
