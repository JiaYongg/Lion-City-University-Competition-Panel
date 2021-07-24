using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P01_T01.Models;

namespace WEB2021Apr_P01_T01.DAL
{
    public class CompetitionScoreDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        // Constructor
        public CompetitionScoreDAL()
        {
            // Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJP_DBConnectionString");

            // Instantiate a SqlConnection object with the connection string read.
            conn = new SqlConnection(strConn);
        }
        
        public List<CompetitionScore> GetCompetitiorScores(int competitionID, int competitorID)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM CompetitionScore WHERE CompetitionID = @selectedCompetitionID AND CompetitorID = @selectedCompetitorID; ";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorID);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<CompetitionScore> scoreList = new List<CompetitionScore>();
            while (reader.Read())
            {
                scoreList.Add(
                    new CompetitionScore
                    {
                        CriteriaID = reader.GetInt32(0),
                        CompetitorID = reader.GetInt32(1),
                        CompetitionID = reader.GetInt32(2),
                        Score = reader.GetInt32(3),
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return scoreList;
        }

        public List<CompetitionScore> GetCompetitionScores(int competitionID)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM CompetitionScore WHERE CompetitionID = @selectedCompetitionID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<CompetitionScore> scoreList = new List<CompetitionScore>();
            while (reader.Read())
            {
                scoreList.Add(
                    new CompetitionScore
                    {
                        CriteriaID = reader.GetInt32(0),
                        CompetitorID = reader.GetInt32(1),
                        CompetitionID = reader.GetInt32(2),
                        Score = reader.GetInt32(3),
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return scoreList;
        }
    }
}
