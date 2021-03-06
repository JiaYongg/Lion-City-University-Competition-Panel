using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WEB2021Apr_P01_T01.Models;
using Microsoft.AspNetCore.Http;

namespace WEB2021Apr_P01_T01.DAL
{
    public class CompetitionSubmissionDAL
    {
        public CriteriaDAL criteriaContext = new CriteriaDAL();
        public CompetitionScoreDAL scoreContext = new CompetitionScoreDAL();
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
                        FileUploadDateTime = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                        Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                        VoteCount = reader.GetInt32(5),
                        Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return submissionsList;
        }
        // Getting the details of specific competition and competitor submission, Kevin
        public CompetitionSubmission GetCompetitionCompetitorSubmission(int competitionId, int competitorId)
        {
            CompetitionSubmission cs = new CompetitionSubmission();

            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT cs.*, c.CompetitorName FROM CompetitionSubmission AS cs INNER JOIN Competitor AS c ON cs.CompetitorID = c.CompetitorID WHERE cs.CompetitionID = @selectedCompetitionID AND cs.competitorId = @selectedCompetitorID;";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorId);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cs.CompetitionId = reader.GetInt32(0);
                cs.CompetitorId = reader.GetInt32(1);
                cs.CompetitorName = reader.GetString(7);
                cs.FileUrl = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                cs.FileUploadDateTime = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null;
                cs.Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                cs.VoteCount = reader.GetInt32(5);
                cs.Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null;
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return cs;
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

        // Gets the competitor's list of competitions, Jia Yong
        public List<CompetitionSubmission> CompetitorCompetitions(int competitorId)
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

        // Joins a competition using competitionId and competitorId, Jia Yong
        public void JoinCompetition(int competitionId, int competitorId)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO CompetitionSubmission (CompetitionID, CompetitorID, VoteCount) VALUES(@competitionId, @competitorId, @vote)";
            cmd.Parameters.AddWithValue("@competitionId", competitionId);
            cmd.Parameters.AddWithValue("@competitorId", competitorId);
            cmd.Parameters.AddWithValue("@vote", 0);

            conn.Open();

            // ExecuteNonQuery is used to retrieve the rows affected in the database
            cmd.ExecuteNonQuery();

            // Close the connection to the database after operations
            conn.Close();

        }

        public void UploadFile(CompetitorSubmissionViewModel csVM)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"UPDATE CompetitionSubmission SET FileSubmitted = @selectedFile, DateTimeFileUpload = @uploadDateTime WHERE CompetitionID = @selectedCompetitionID AND CompetitorID = @selectedCompetitorID";
            cmd.Parameters.AddWithValue("@selectedFile", csVM.FileUrl);
            cmd.Parameters.AddWithValue("@uploadDateTime", csVM.FileUploadDateTime);
            cmd.Parameters.AddWithValue("@selectedCompetitionID", csVM.CompetitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", csVM.CompetitorId);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        // Use competitionId to get various details of CompetitionSubmissionViewModel
        public CompetitorSubmissionViewModel GetCompetitorCompetitionDetails(int competitionId, int competitorId)
        {
            CompetitorSubmissionViewModel csVM = new CompetitorSubmissionViewModel();

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM CompetitionSubmission AS cs INNER JOIN Competition AS compy ON cs.CompetitionID = compy.CompetitionID INNER JOIN Competitor AS c ON cs.CompetitorID = c.CompetitorID WHERE compy.CompetitionID = @selectedCompetitionID AND c.CompetitorID = @selectedCompetitorID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorId);

            var criteriaList = criteriaContext.GetCompetitionCriteria(competitionId);
            List<string> nameList = new List<string>();
            List<int> weightList = new List<int>();
            List<double> scoreList = new List<double>();

            foreach (var item in criteriaList)
            {
                nameList.Add(item.CriteriaName);
                weightList.Add(item.Weightage);
            }

            int i = 0;
            foreach (var score in scoreContext.GetCompetitiorScores(competitionId, competitorId))
            {
                // convert score to percentage out of the weightage score
                double num = score.Score;
                num /= 10;
                num *= weightList[i];
                scoreList.Add(num);
                i++;
            }

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                // Read the record from database
                while (reader.Read())
                {
                    // Fill CompetitorSubmissionViewModel object with values from the data reader
                    csVM.CompetitionId = competitionId;
                    csVM.CompetitorId = reader.GetInt32(1);
                    csVM.FileUrl = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    csVM.FileUploadDateTime = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null;
                    csVM.Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    csVM.CriteriaName = nameList;
                    csVM.Score = scoreList;
                    csVM.Weightage = weightList;
                    csVM.VoteCount = reader.GetInt32(5);
                    csVM.Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null;
                    csVM.CompetitionName = reader.GetString(9);
                    csVM.StartDate = reader.GetDateTime(10);
                    csVM.EndDate = reader.GetDateTime(11);
                    csVM.ResultsReleaseDate = reader.GetDateTime(12);
                    csVM.CompetitorName = reader.GetString(14);
                }
            }
            // Close DataReader
            reader.Close();

            // Close database connection
            conn.Close();

            return csVM;
        }

        public void SubmitAppeal(string appealMessage, int competitorId, int competitionId)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"UPDATE CompetitionSubmission SET Appeal = @appeal WHERE CompetitorID = @selectedCompetitorID AND CompetitionID = @selectedCompetitionID";
            cmd.Parameters.AddWithValue("@appeal", appealMessage);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorId);
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);

            conn.Open();

            // ExecuteNonQuery is used to retrieve the rows affected in the database
            cmd.ExecuteNonQuery();

            // Close the connection to the database after operations
            conn.Close();
        }

        // Adding it back in since Kevin requires the model instead of the view model
        public CompetitionSubmission GetCompetitionDetail(int competitionId)
        {
            CompetitionSubmission cs = new CompetitionSubmission();

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM CompetitionSubmission AS cs INNER JOIN Competition AS compy ON cs.CompetitionID = compy.CompetitionID INNER JOIN Competitor AS c ON cs.CompetitorID = c.CompetitorID WHERE compy.CompetitionID = @selectedCompetitionID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                // Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    cs.CompetitionId = competitionId;
                    cs.CompetitorId = reader.GetInt32(1);
                    cs.FileUrl = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    cs.FileUploadDateTime = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null;
                    cs.Appeal = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    cs.VoteCount = reader.GetInt32(5);
                    cs.Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null;
                    cs.CompetitionName = reader.GetString(9);
                    // Competition Start Date is not necessary as the competitor has already joined the competition.
                    cs.EndDate = reader.GetDateTime(11);
                    cs.ResultReleasedDate = reader.GetDateTime(12);
                    cs.CompetitorName = reader.GetString(14);
                }
            }
            // Close DataReader
            reader.Close();

            // Close database connection
            conn.Close();

            return cs;
        }

        // Update 
        public int UpdateRank(int? rank, int competitionId, int competitiorId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE CompetitionSubmission SET Ranking = @rank WHERE CompetitionID = @selectedCompetitionID AND CompetitorID = @selectedCompetitorID;";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@rank", rank ?? Convert.DBNull);
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitiorId);

            //Open a database connection
            conn.Open();

            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();

            //Close the database connection
            conn.Close();
            return count;
        }
    }
}
