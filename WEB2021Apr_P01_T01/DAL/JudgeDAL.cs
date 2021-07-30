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
    public class JudgeDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        //Constructor
        public JudgeDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJP_DBConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Judge> GetAllJudge()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Judge";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Judge> judgeList = new List<Judge>();
            while (reader.Read())
            {
                judgeList.Add(
                new Judge
                {
                    JudgeId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Salutation = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                    AreaInterestId = reader.GetInt32(3),
                    EmailAddr = reader.GetString(4),
                    Password = reader.GetString(5),
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return judgeList;
        }

        public List<Competition> GetJudgesCompetition(int judgeId)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM Competition INNER JOIN CompetitionJudge ON CompetitionJudge.CompetitionID = Competition.CompetitionID WHERE JudgeID = @selectedJudgeId;";
            cmd.Parameters.AddWithValue("@selectedJudgeId", judgeId);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Competition> judgeCompList = new List<Competition>();
            while (reader.Read())
            {
                judgeCompList.Add(
                    new Competition
                    {
                        CompetitionId = reader.GetInt32(0),
                        CompetitionName = reader.GetString(2),
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return judgeCompList;
        }
        
        public List<Competition> GetJudgesFutureCompetition(int judgeId)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM Competition INNER JOIN CompetitionJudge ON CompetitionJudge.CompetitionID = Competition.CompetitionID WHERE JudgeID = @selectedJudgeId AND StartDate >= LEFT(CONVERT(DATETIME,GetDate(),103),12);";
            cmd.Parameters.AddWithValue("@selectedJudgeId", judgeId);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Competition> judgeCompList = new List<Competition>();
            while (reader.Read())
            {
                judgeCompList.Add(
                    new Competition
                    {
                        CompetitionId = reader.GetInt32(0),
                        CompetitionName = reader.GetString(2),
                    });
            }

            reader.Close();
            conn.Close();

            return judgeCompList;
        }

        // Following Method Added by Jordan for populating in Competition Details View
        public List<Judge> GetCompetitionJudges(int competitionId)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM Judge INNER JOIN CompetitionJudge ON CompetitionJudge.JudgeID = Judge.JudgeID WHERE CompetitionID = @selectedCompetitionId;";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);

            // Opens a Database Connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<Judge> compyJudgeList = new List<Judge>();
            while (reader.Read())
            {
                compyJudgeList.Add(
                    new Judge
                    {
                        JudgeId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Salutation = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                        AreaInterestId = reader.GetInt32(3),
                        // not required, so set to null for security purposes. unsure if its acceptable
                        EmailAddr = null,
                        Password = null
                    });
            }

            // Close the DataReader & DB connection
            reader.Close();
            conn.Close();

            return compyJudgeList;
        }

        public bool CheckJudgeComp(int jid, int cid)
        {
            bool exist = false;

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM CompetitionJudge WHERE CompetitionID = @cid AND JudgeID = @jid;";
            cmd.Parameters.AddWithValue("@cid", cid);
            cmd.Parameters.AddWithValue("@jid", jid);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                exist = true;
            }
            reader.Close();

            conn.Close();

            return exist;
        }

        public bool AssignJudgeToComp(int jid, int cid)
        {
            if(!CheckJudgeComp(jid,cid))
            {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"INSERT INTO CompetitionJudge (CompetitionID, JudgeID)
                VALUES(@cid, @jid)";

                cmd.Parameters.AddWithValue("@cid", cid);
                cmd.Parameters.AddWithValue("@jid", jid);

                conn.Open();
                cmd.ExecuteScalar();
                conn.Close();
                return true;
            }
            return false;
        }

        public bool UnassignJudgeFromComp(int jid, int cid)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"DELETE FROM CompetitionJudge WHERE CompetitionID = @cid AND JudgeID = @jid";

            cmd.Parameters.AddWithValue("@cid", cid);
            cmd.Parameters.AddWithValue("@jid", jid);

            conn.Open();
            cmd.ExecuteScalar();
            conn.Close();
            return true;
        }
    }
}
