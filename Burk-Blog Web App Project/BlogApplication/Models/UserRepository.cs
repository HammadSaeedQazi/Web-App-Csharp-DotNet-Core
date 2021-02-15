using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    static public class UserRepository
    {
        static public bool AddUser(RegularUser usr) //adds user usr in DB.
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBofBlogApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"insert into RegularUsers(username,password,email) values(@u,@p,@e)";
            SqlCommand cmd = new SqlCommand(query, con);
            if (!string.IsNullOrEmpty(usr.picAddress))
            {
                query = $"insert into RegularUsers (username,email,password,picaddress) values(@u,@e,@p,@pa)";
                cmd = new SqlCommand(query, con);
                SqlParameter p4 = new SqlParameter("pa", usr.picAddress);
                cmd.Parameters.Add(p4);
            }
            SqlParameter p1 = new SqlParameter("u", usr.Username);
            SqlParameter p2 = new SqlParameter("p", usr.Password);
            SqlParameter p3 = new SqlParameter("e", usr.Email);
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
        static public bool RemoveUser(int id)   //remove user form DB with the given id.
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBofBlogApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"delete from RegularUsers where Id=@i";
            SqlParameter p1 = new SqlParameter("i", id);
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
        static public bool UpdateUser(RegularUser usr)  //update user usr with the id equal to usr.id with its attributes 
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBofBlogApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"update RegularUsers set username=@u, email=@e, password=@p where Id=@i";
            SqlCommand cmd = new SqlCommand(query, con);
            if (!string.IsNullOrEmpty(usr.picAddress))
            {
                query = $"update RegularUsers set username=@u, email=@e, password=@p, picaddress=@pa where Id=@i";
                cmd = new SqlCommand(query, con);
                SqlParameter p4 = new SqlParameter("pa", usr.picAddress);
                cmd.Parameters.Add(p4);
            }
            SqlParameter p1 = new SqlParameter("u", usr.Username);
            SqlParameter p2 = new SqlParameter("e", usr.Email);
            SqlParameter p3 = new SqlParameter("p", usr.anotherPassword);
            SqlParameter p5 = new SqlParameter("i", usr.Id);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p5);
            int updatedRows = cmd.ExecuteNonQuery();
            if (updatedRows >= 1)
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }

        static public List<RegularUser> ReturnUsers()   //return all users currently present in DB.
        {
            List<RegularUser> newData = new List<RegularUser>();
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBofBlogApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = "select * from RegularUsers";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                newData.Add(new RegularUser { Id = System.Convert.ToInt32(dr[0]), Username = dr[1].ToString(), Password = dr[2].ToString(), Email = dr[3].ToString(), picAddress = dr[4].ToString() });
            }
            con.Close();
            return newData;
        }
    }
}
