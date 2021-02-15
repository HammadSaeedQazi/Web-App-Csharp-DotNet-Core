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
namespace hw2_task2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void login_Click_1(object sender, RoutedEventArgs e)
        {
            string usrnm = username.Text.ToLower();
            string passwd = hidden_text.Password.ToLower();
            if (usrnm == "admin" && passwd == "admin")
            {
                MessageBox.Show("Signed In");
                username.Text = "";
                hidden_text.Password = "";
                //shown_text.Text = "";
            }
            else
                MessageBox.Show("Wrong Credentials");
        }

        private void show_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            shown_text.Text = hidden_text.Password.ToString();
            hidden_text.Visibility = Visibility.Collapsed;
            shown_text.Visibility = Visibility.Visible;
        }

        private void show_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            shown_text.Visibility = Visibility.Collapsed;
            hidden_text.Visibility = Visibility.Visible;
        }
    }
}
