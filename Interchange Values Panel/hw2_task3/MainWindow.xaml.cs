using System.Collections.ObjectModel;
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

namespace hw2_task3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> leftList = new ObservableCollection<string>();
        ObservableCollection<string> rightList = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            leftListBox.ItemsSource = leftList;
            rightListBox.ItemsSource = rightList;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void addLeft_Click(object sender, RoutedEventArgs e)
        {
            if (leftValue.Text != "")
            {
                leftList.Add(leftValue.Text);
                leftValue.Text = "";
            }
            else
                MessageBox.Show("Empty Input Is Not Allowed !");
        }

        private void addRight_Click(object sender, RoutedEventArgs e)
        {
            if (rightValue.Text != "")
            {
                rightList.Add(rightValue.Text);
                rightValue.Text = "";
            }
            else
                MessageBox.Show("Empty Input Is Not Allowed !");
        }

        private void singleToRight_Click(object sender, RoutedEventArgs e)
        {
            if (leftList.Count == 0)
                MessageBox.Show("No Item Exist !");
            else if (leftListBox.SelectedItem != null)
            {
                rightList.Add(leftListBox.SelectedItem as string);
                leftList.Remove(leftListBox.SelectedItem as string);
            }
            else
                MessageBox.Show("No Item Has Been Selected !");
        }

        private void singleToLeft_Click(object sender, RoutedEventArgs e)
        {
            if (rightList.Count == 0)
                MessageBox.Show("No Item Exist !");
            else if (rightListBox.SelectedItem != null)
            {
                leftList.Add(rightListBox.SelectedItem as string);
                rightList.Remove(rightListBox.SelectedItem as string);
            }
            else
                MessageBox.Show("No Item Has Been Selected !");
        }

        private void allToRight_Click(object sender, RoutedEventArgs e)
        {
            if (leftList.Count > 0)
            {
                foreach (string obj in leftList)
                {
                    rightList.Add(obj);
                }
                leftList.Clear();
            }
            else
                MessageBox.Show("No Item Exist !");
        }

        private void allToLeft_Click(object sender, RoutedEventArgs e)
        {
            if (rightList.Count > 0)
            {
                foreach (string obj in rightList)
                {
                    leftList.Add(obj);
                }
                rightList.Clear();
            }
            else
                MessageBox.Show("No Item Exist !");
        }
    }
}
