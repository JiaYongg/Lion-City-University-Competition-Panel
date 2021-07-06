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
    public class CompetitionSubmissionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        // Constructor
        public CompetitionSubmissionDAL()
        {
            // Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJP_DBConnectionString");

            // Instantiate a SqlConnection object with the connection string read.
            conn = new SqlConnection(strConn);
        }

        public List<CompetitionSubmission> GetCompetitionSubmissions(int competitionId)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT cs.*, c.CompetitorName FROM CompetitionSubmission AS cs INNER JOIN Competitor AS c ON cs.CompetitorID = c.CompetitorID WHERE cs.CompetitionID = @selectedCompetitionID;";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<CompetitionSubmission> submissionsList = new List<CompetitionSubmission>();
            while (reader.Read())
            {
                submissionsList.Add(
                    new CompetitionSubmission
                    {
                        CompetitionId = reader.GetInt32(0),
                        CompetitorId = reader.GetInt32(1),
                        CompetitorName = reader.GetString(7), // to add later
                        FileUrl = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                        FileUploadDateTime = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?) null,
                        Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                        VoteCount = reader.GetInt32(5),
                        Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?) null
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return submissionsList;
        }

        public List<CompetitionSubmission> GetTopThree(int competitionId)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT cs.*, c.CompetitorName FROM CompetitionSubmission AS cs INNER JOIN Competitor AS c ON cs.CompetitorID = c.CompetitorID WHERE CompetitionID = @selectedCompetitionId AND Ranking <= 3 ORDER BY Ranking;";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<CompetitionSubmission> rankList = new List<CompetitionSubmission>();
            while (reader.Read())
            {
                rankList.Add(
                    new CompetitionSubmission
                    {
                        CompetitionId = reader.GetInt32(0),
                        CompetitorId = reader.GetInt32(1),
                        CompetitorName = reader.GetString(7), // to add later
                        FileUrl = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                        FileUploadDateTime = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                        Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                        VoteCount = reader.GetInt32(5),
                        Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return rankList;
        }

        public int Vote(int? competitorId, int? competitionId)
        {
            // Create a SQLCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify an UPDATE SQL Statement
            cmd.CommandText = @"UPDATE CompetitionSubmission SET VoteCount = VoteCount + 1 WHERE CompetitionID = @selectedCompetitionId AND CompetitorID = @selectedCompetitorId";

            // Define the parameters used in SQL statement, value for each parameters is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@selectedCompetitionId", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorId", competitorId);

            // Open a Database Connection
            conn.Open();

            // Execute Query is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();

            // Close the Database connection
            conn.Close();

            return count;
        }

        // Gets the competitor's list of competitions
        public List<CompetitionSubmission> competitorCompetitions(int competitorId)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT cs.*, c.CompetitionName, c.StartDate, c.EndDate, c.ResultReleasedDate FROM CompetitionSubmission AS cs INNER JOIN Competition AS c ON cs.CompetitionID = c.CompetitionID WHERE cs.CompetitorID = @selectedCompetitorId";
            cmd.Parameters.AddWithValue("@selectedCompetitorId", competitorId);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<CompetitionSubmission> competitorList = new List<CompetitionSubmission>();
            while (reader.Read())
            {
                competitorList.Add(
                    new CompetitionSubmission
                    {
                        CompetitionId = reader.GetInt32(0),
                        CompetitionName = reader.GetString(7),
                        StartDate = reader.GetDateTime(8),
                        EndDate = reader.GetDateTime(9),
                        ResultReleasedDate = reader.GetDateTime(10),
                        CompetitorId = reader.GetInt32(1),
                        FileUrl = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                        FileUploadDateTime = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                        Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                        VoteCount = reader.GetInt32(5),
                        Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return competitorList;
        }
    }
}
