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
    public class CommentsDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        // Constructor
        public CommentsDAL()
        {
            // Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJP_DBConnectionString");

            // Instantiate a SqlConnection object with the connection string read.
            conn = new SqlConnection(strConn);
        }

        public List<Comments> GetCompetitionComments(int competitionId)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM Comment WHERE CompetitionID = @selectedCompetitionId ORDER BY DateTimePosted DESC";
            cmd.Parameters.AddWithValue("@selectedCompetitionId", competitionId);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Comments> commentsList = new List<Comments>();
            while (reader.Read())
            {
                commentsList.Add(
                    new Comments
                    {
                        CommentId = reader.GetInt32(0),
                        CompetitionID = reader.GetInt32(1),
                        CommentDesc = reader.GetString(2),
                        DateTimePosted = reader.GetDateTime(3)
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return commentsList;
        }

        public int AddComments(Comments cmmts)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify an INSERT SQL Statement which will return the auto-generated CommentID after insertion
            cmd.CommandText = @"INSERT INTO Comment (CompetitionID, Description, DateTimePosted) OUTPUT INSERTED.CommentID VALUES(@competitionId, @commentDesc, @datetimePosted)";
            cmd.Parameters.AddWithValue("@competitionId", cmmts.CompetitionID);
            cmd.Parameters.AddWithValue("@commentDesc", cmmts.CommentDesc);
            cmd.Parameters.AddWithValue("@datetimePosted", cmmts.DateTimePosted);

            // A connection to database must be opened before any operations made.
            conn.Open();

            // ExecuteScalar is used to retrieve the auto-generated CommentID after executing the INSERT SQL Statement
            cmmts.CommentId = (int) cmd.ExecuteScalar();

            // Close the connection to the database after operations
            conn.Close();

            // Return the id when no error occurs.
            return (int) cmmts.CommentId;
        }
    }
}
