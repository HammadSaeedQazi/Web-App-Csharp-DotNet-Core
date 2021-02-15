using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Online_Grocery_System.Models
{
    class ProductServices
    {
        public bool addProduct(Product p)   //add product p in database table named product
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=online_grocery_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"insert into product(pid,name,price,quantity) values(@id,@n,@pr,@qnt)"; //parameterized query
            SqlParameter p1 = new SqlParameter("id", p.ID);
            SqlParameter p2 = new SqlParameter("n", p.Name);
            SqlParameter p3 = new SqlParameter("pr", p.Price);
            SqlParameter p4 = new SqlParameter("qnt", p.Quantity);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            int insertedRows = cmd.ExecuteNonQuery();
            if(insertedRows >= 1)
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }
        public bool deleteProduct(string dltID)     //deletes a product with ID equal to dltID
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=online_grocery_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"delete from product where pid=@id";    //parameterized query
            SqlParameter p1 = new SqlParameter("id", dltID);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            int deletedRows = cmd.ExecuteNonQuery();
            if (deletedRows >= 1)
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }
        public bool updateProduct(Product p)    //update record in DB whose ID equal to the ID of product P and replace that with p
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=online_grocery_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"update product set quantity=@nq where pid=@id";    //parameterized query
            SqlParameter p1 = new SqlParameter("id", p.ID);
            SqlParameter p2 = new SqlParameter("nq", p.Quantity);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            int deletedRows = cmd.ExecuteNonQuery();
            if (deletedRows >= 1)
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }
        public List<Product> returnProducts()   //return all records of product table currently present in database
        {
            List<Product> newData = new List<Product>();
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=online_grocery_system;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = "select * from product";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                newData.Add(new Product { ID = dr[1].ToString(), Name = dr[2].ToString(), Price = dr[3].ToString(), Quantity = dr[4].ToString() });
            }
            con.Close();
            return newData;
        }
    }
}
