using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace hw2_task1
{
    /// <summary>
    /// Interaction logic for OutputGrid.xaml
    /// </summary>
    public partial class OutputGrid : Window
    {
        public OutputGrid()
        {
            InitializeComponent();
            List<User> userList = new List<User>();
            userList = retrieveData();
            showUsers.ItemsSource = userList;
        }

        internal void updateDummyData()
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=task1_hw2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"insert into person(fname,lname,email,password) values('Hammad', 'Dummy', 'itsdummy1@gmail.com', 'iamdummy1')";
            SqlCommand cmd = new SqlCommand(query, con);
            int insertedRows = cmd.ExecuteNonQuery();
            query = $"insert into person(fname,lname,email,password) values('Majid', 'Dummy', 'itsdummy2@gmail.com', 'iamdummy2')";
            cmd = new SqlCommand(query, con);
            insertedRows = cmd.ExecuteNonQuery();
            query = $"insert into person(fname,lname,email,password) values('Umar', 'Dummy', 'itsdummy3@gmail.com', 'iamdummy3')";
            cmd = new SqlCommand(query, con);
            insertedRows = cmd.ExecuteNonQuery();
            con.Close();
        }
        internal List<User> retrieveData()
        {
            label1:
            List<User> newData = new List<User>();
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=task1_hw2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = "select * from person";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                con.Close();
                updateDummyData();
                goto label1;
            }
            while (dr.Read())
            {
                newData.Add(new User { Fname = dr[1].ToString(), Lname = dr[2].ToString(), Email = dr[3].ToString(), Password=dr[4].ToString() });
            }
            con.Close();
            return newData;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void mainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}