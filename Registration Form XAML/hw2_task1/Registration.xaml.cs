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
using System.Net.Mail;

namespace hw2_task1
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();

        }

        internal bool stringValidation(string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= 'A' && str[i] <= 'Z') || (str[i] >= 'a' && str[i] <= 'z')) { }
                else
                    return false;
            }
            return true;
        }

        internal void updateData()
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=task1_hw2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"insert into person(fname,lname,email,password) values(@fn,@ln,@em,@ps)";
            SqlParameter p1 = new SqlParameter("fn", fname.Text);
            SqlParameter p2 = new SqlParameter("ln", lname.Text);
            SqlParameter p3 = new SqlParameter("em", email.Text);
            SqlParameter p4 = new SqlParameter("ps", password.Text);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            int insertedRows = cmd.ExecuteNonQuery();
            if (insertedRows >= 1)
                MessageBox.Show("Data Updated !");
            else
                MessageBox.Show("Failed to Update Data !");
            con.Close();
            fname.Text = "";
            lname.Text = "";
            email.Text = "";
            password.Text = "";
            cnfrmpasswd.Text = "";
        }

        internal bool spaceValidationForAll()
        {
            for (int i = 0; i < fname.Text.Length; i++)
                if (fname.Text[i] == ' ')
                    return false;
            for (int i = 0; i < lname.Text.Length; i++)
                if (lname.Text[i] == ' ')
                    return false;
            for (int i = 0; i < email.Text.Length; i++)
                if (email.Text[i] == ' ')
                    return false;
            for (int i = 0; i < password.Text.Length; i++)
                if (password.Text[i] == ' ')
                    return false;
            return true;
        }

        internal bool emailValidation()
        {
            try
            {
                MailAddress ma = new MailAddress(email.Text);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (fname.Text == "" || lname.Text == "" || email.Text == "" || password.Text == "" || cnfrmpasswd.Text == "")
                MessageBox.Show("Empty Field is not Allowed !");
            else if (!spaceValidationForAll())
                MessageBox.Show("No Space Allowed in Any Field !");
            else if (!stringValidation(fname.Text) || !stringValidation(lname.Text))
                MessageBox.Show("Invalid Name Input !");
            else if (!emailValidation())
                MessageBox.Show("Invalid Email Address !");
            else if (password.Text.Length > 49)
                MessageBox.Show("Password Lenght must not exceed 50 characters !");
            else
            {
                if (password.Text == cnfrmpasswd.Text)
                {
                    updateData();
                }
                else
                    MessageBox.Show("Password Does'nt Match !");
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
