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

            if(reader.HasRows)
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
    }
}
