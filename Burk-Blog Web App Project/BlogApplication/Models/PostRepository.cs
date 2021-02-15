using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    static public class PostRepository
    {
        static public bool AddPost(Post p)  //add post p in DB.
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBofBlogApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"insert into Posts(title,content,date,usr) values(@t,@c,@d,@u)";
            SqlParameter p1 = new SqlParameter("t", p.Title);
            SqlParameter p2 = new SqlParameter("c", p.Content);
            SqlParameter p3 = new SqlParameter("d", p.Date);
            SqlParameter p4 = new SqlParameter("u", p.Usr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            int insertedRows = cmd.ExecuteNonQuery();
            if (insertedRows >= 1)
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }
        static public bool RemovePost(int id)   //remove post from DB with the given id.
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBofBlogApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"delete from Posts where Id=@i";
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
        static public bool UpdatePost(Post p)   //update post p with the data of p and id of p.
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBofBlogApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = $"update Posts set title=@t, content=@c, usr=@u where Id=@i";
            SqlParameter p1 = new SqlParameter("t", p.Title);
            SqlParameter p2 = new SqlParameter("c", p.Content);
            SqlParameter p3 = new SqlParameter("u", p.Usr);
            SqlParameter p4 = new SqlParameter("i", p.Id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            int updatedRows = cmd.ExecuteNonQuery();
            if (updatedRows >= 1)
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }
        static public List<Post> ReturnPosts()  //returns all post present currently in DB.
        {
            List<Post> newData = new List<Post>();
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBofBlogApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            string query = "select * from Posts";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                newData.Add(new Post { Id = System.Convert.ToInt32(dr[0]), Title = dr[1].ToString(), Content = dr[2].ToString(), Date = dr[3].ToString(), Usr = dr[4].ToString() });
            }
            con.Close();
            return newData;
        }

    }
}
