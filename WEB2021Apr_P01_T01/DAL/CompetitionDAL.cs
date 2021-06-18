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
    public class CompetitionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        // Constructor
        public CompetitionDAL()
        {
            // Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJP_DBConnectionString");

            // Instantiate a SqlConnection object with the connection string read.
            conn = new SqlConnection(strConn);
        }

        public List<Competition> GetCurrentCompetitions()
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement

            cmd.CommandText = @"SELECT * FROM Competition WHERE EndDate > @today ORDER BY CompetitionId";
            cmd.Parameters.AddWithValue("@today", DateTime.Today);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Competition> competitionList = new List<Competition>();
            while (reader.Read())
            {
                competitionList.Add(
                    new Competition
                    {
                        CompetitionId = reader.GetInt32(0),
                        Aoi = reader.GetInt32(1),
                        CompetitionName = reader.GetString(2),
                        StartDate = reader.GetDateTime(3),
                        EndDate = reader.GetDateTime(4),
                        ResultsReleaseDate = reader.GetDateTime(5),
                        SubmissionList = new List<CompetitionSubmission>()
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return competitionList;
        }

        public List<Competition> GetPastCompetitions()
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement

            cmd.CommandText = @"SELECT * FROM Competition WHERE EndDate < @today ORDER BY CompetitionId";
            cmd.Parameters.AddWithValue("@today", DateTime.Today);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Competition> pastCompetitionList = new List<Competition>();
            while (reader.Read())
            {
                pastCompetitionList.Add(
                    new Competition
                    {
                        CompetitionId = reader.GetInt32(0),
                        Aoi = reader.GetInt32(1),
                        CompetitionName = reader.GetString(2),
                        StartDate = reader.GetDateTime(3),
                        EndDate = reader.GetDateTime(4),
                        ResultsReleaseDate = reader.GetDateTime(5),
                        SubmissionList = new List<CompetitionSubmission>()
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return pastCompetitionList;
        }
    }
}
