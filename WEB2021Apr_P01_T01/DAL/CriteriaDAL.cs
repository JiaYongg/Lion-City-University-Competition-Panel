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
    public class CriteriaDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public CriteriaDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJP_DBConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Criteria> GetCompetitionCriteria(int competitionId)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM Criteria WHERE CompetitionID = @selectedCompetitionID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Criteria> compyCriteriaList = new List<Criteria>();
            while (reader.Read())
            {
                compyCriteriaList.Add(
                    new Criteria
                    {
                       CriteriaID = reader.GetInt32(0),
                       CompetitionID = !reader.IsDBNull(1) ? reader.GetInt32(1) : (int?) null,
                       CriteriaName = reader.GetString(2),
                       Weightage = reader.GetInt32(3)
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return compyCriteriaList;
        }
    }
}
