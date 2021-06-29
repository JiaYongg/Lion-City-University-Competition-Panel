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
        private AoiDAL aoiContext = new AoiDAL();

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
                        AoiId = reader.GetInt32(1),
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
                        AoiId = reader.GetInt32(1),
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

        public Competition GetCompetitionDetails(int competitionId)
        {
            Competition compy = new Competition();

            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL statement that retrieves all attributes of a competition records.
            cmd.CommandText = @"SELECT * FROM Competition WHERE CompetitionId = @selectedCompetitionID";

            // Define the parameter used in SQL Statement, value for the parameter is retrieved from the method parameter "competitionId".
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);

            // Open a database connection.
            conn.Open();

            // Execute SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                // Read the retrieved record from the database.
                while (reader.Read())
                {
                    // Fill Competition object with values from the data reader
                    compy.CompetitionId = competitionId;
                    compy.AoiId = reader.GetInt32(1);
                    compy.AoiName = aoiContext.GetAoiName(compy.AoiId);
                    compy.CompetitionName = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    compy.StartDate = reader.GetDateTime(3);
                    compy.EndDate = reader.GetDateTime(4);
                    compy.ResultsReleaseDate = reader.GetDateTime(5);
                    compy.SubmissionList = new List<CompetitionSubmission>();

                    // To add loop to call CompetitionSubmissionDAL to get competitionSubmissions and add into list.
                }
            }

            // Close Data Reader
            reader.Close();

            // Close Database Connection
            conn.Close();

            return compy;
        }

        public int AddCompetition(Competition competition, int aoiId)
        {
            string compName = competition.CompetitionName;
            int startDay = competition.StartDate.Day;
            int startMonth = competition.StartDate.Month;
            int startYear = competition.StartDate.Year;

            DateTime start = new DateTime(startYear, startMonth, startDay);

            int endDay = competition.EndDate.Day;
            int endMonth = competition.EndDate.Month;
            int endYear = competition.EndDate.Year;

            DateTime end = new DateTime(endYear, endMonth, endDay);

            int resultDay = competition.ResultsReleaseDate.Day;
            int resultMonth = competition.ResultsReleaseDate.Month;
            int resultYear = competition.ResultsReleaseDate.Year;

            DateTime result = new DateTime(resultYear, resultMonth, resultDay);

            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Competition (AreaInterestID, CompetitionName, StartDate, EndDate, ResultReleasedDate)
                OUTPUT INSERTED.CompetitionID
                VALUES(@aoi, @name, @start, @end, @results)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@aoi", aoiId);
            cmd.Parameters.AddWithValue("@name", compName);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            cmd.Parameters.AddWithValue("@results", result);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            competition.CompetitionId = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return competition.CompetitionId;
        }

        public int AddAreaInterest(string aoiName)
        {
            AreaInterest aoi = new AreaInterest();

            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"SELECT * FROM AreaInterest WHERE NAME = @name";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", aoiName);

            conn.Open();

            aoi.AreaInterestId = (int)cmd.ExecuteScalar();

            conn.Close();

            return aoi.AreaInterestId;
        }
    }
}