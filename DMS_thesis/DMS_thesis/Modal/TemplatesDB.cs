using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using DMS_thesis.Models;

namespace DMS_thesis.Modal
{
    public class TemplatesDB
    {
        //declare connection string  
        string cs = ConfigurationManager.ConnectionStrings["DMSContext"].ConnectionString;

        //Return list of all Templates  
        public List<Template> ListAll()
        {
            List<Template> templ = new List<Template>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("SelectTemplates", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    templ.Add(new Template
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        Path = rdr["Path"].ToString(),
                    });
                }
                return templ;
            }
        }

        //Method for Adding a Template
        public int Add(Template tp)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("InsertUpdateTemplates", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", tp.Id);
                com.Parameters.AddWithValue("@Name", tp.Name);
                com.Parameters.AddWithValue("@Path", tp.Path);
                com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        //Method for Updating Template  
        public int Update(Template tp)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("InsertUpdateTemplates", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", tp.Id);
                com.Parameters.AddWithValue("@Name", tp.Name);
                com.Parameters.AddWithValue("@Path", tp.Path);
                com.Parameters.AddWithValue("@Action", "Update");
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        //Method for Deleting a Template 
        public int Delete(int ID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("DeleteTemplates", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", ID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
    }
}