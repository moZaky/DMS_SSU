using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using DMS_thesis.Models;

namespace DMS_thesis.Modal
{
    public class DepartmentDB
    {
        //declare connection string  
        string cs = ConfigurationManager.ConnectionStrings["DMSContext"].ConnectionString;

        //Return list of all Employees  
        public List<Department> ListAll()
        {
            List<Department> lst = new List<Department>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("SelectDepartment", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Department
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                    Name = rdr["Name"].ToString(),
                        
                    });
                }
                return lst;
            }
        }

        //Method for Adding an Employee  
        public int Add(Department dp)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("InsertUpdateDepartment", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", dp.Id);
                com.Parameters.AddWithValue("@Name", dp.Name);
                com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        //Method for Updating Employee record  
        public int Update(Department dp)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("InsertUpdateDepartment", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", dp.Id);
                com.Parameters.AddWithValue("@Name", dp.Name);
                com.Parameters.AddWithValue("@Action", "Update");
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        //Method for Deleting an Employee  
        public int Delete(int ID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("DeleteDepartment", con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", ID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
    }
}