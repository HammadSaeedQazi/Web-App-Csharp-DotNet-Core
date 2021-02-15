using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
namespace Online_Grocery_System.Models
{
    class CustomerServices
    {
        public bool addCustomer(Customer c)     //add customer c in database table named as customer
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=online_grocery_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"insert into customer(username,password,phone) values(@u,@p,@ph)"; //parameterized query for efficiency
            SqlParameter p1 = new SqlParameter("u", c.UserName);
            SqlParameter p2 = new SqlParameter("p", c.Password);
            SqlParameter p3 = new SqlParameter("ph", c.PhoneNo);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            int insertedRows = cmd.ExecuteNonQuery();
            if (insertedRows >= 1)
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }
        public List<Customer> returnCustomers()     //returns all customer records currently present in database
        {
            List<Customer> newData = new List<Customer>();
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=online_grocery_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = "select * from customer";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                newData.Add(new Customer { UserName = dr[1].ToString(), Password = dr[2].ToString(), PhoneNo = dr[3].ToString() });
            }
            con.Close();
            return newData;
        }
    }
}
