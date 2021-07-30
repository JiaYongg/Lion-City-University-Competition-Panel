using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WEB2021Apr_P01_T01.Models;

namespace WEB2021Apr_P01_T01.DAL
{
    public class AoiDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public AoiDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJP_DBConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<AreaInterest> GetAreaInterests()
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM AreaInterest";

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<AreaInterest> aoiList = new List<AreaInterest>();
            while (reader.Read())
            {
                aoiList.Add(
                    new AreaInterest
                    {
                        AreaInterestId = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return aoiList;
        }

        public AreaInterest GetAoi()
        {
            AreaInterest aoi = new AreaInterest();

            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM AreaInterest";

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    aoi.AreaInterestId = reader.GetInt32(0);
                    aoi.Name = reader.GetString(1);
                }
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return aoi;
        }

        public string GetAoiName(int aoiId)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM AreaInterest WHERE AreaInterestId = @selectedId";
            cmd.Parameters.AddWithValue("@selectedId", aoiId);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            string aoiName = "";

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    aoiName = reader.GetString(1);
                }
            }
            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return aoiName;
        }

        public int AddAreaInterest(string aoiName)
        {
            AreaInterest aoi = new AreaInterest();

            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO AreaInterest(Name) OUTPUT INSERTED.AreaInterestID VALUES(@name)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", aoiName);

            conn.Open();

            aoi.AreaInterestId = (int)cmd.ExecuteScalar();

            conn.Close();

            return aoi.AreaInterestId;
        }

        public int Delete(int aoiId)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM AreaInterest WHERE AreaInterestID = @selectAoiID";
            cmd.Parameters.AddWithValue("@selectAoiID", aoiId);

            conn.Open();

            int rowAffected = 0;

            rowAffected += cmd.ExecuteNonQuery();

            conn.Close();

            return rowAffected;
        }

        public bool AoiIsAssigned(int aoiId)
        {
            SqlCommand cmd = conn.CreateCommand();
            SqlCommand cmd2 = conn.CreateCommand();

            cmd.CommandText = @"SELECT COUNT(*) FROM Competition WHERE AreaInterestID = @id";
            cmd2.CommandText = @"SELECT COUNT(*) FROM Judge WHERE AreaInterestID = @id";

            cmd.Parameters.AddWithValue("@id", aoiId);
            cmd2.Parameters.AddWithValue("@id", aoiId);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            int count2 = (int)cmd2.ExecuteScalar();
            conn.Close();

            if (count > 0 || count2 > 0)
            {
                return true;
            }
            return false;
        }
    }
}
