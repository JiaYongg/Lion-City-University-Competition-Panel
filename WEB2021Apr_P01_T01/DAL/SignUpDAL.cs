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
    public class SignUpDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        private SignUp signUp = new SignUp();
        //Constructor
        public SignUpDAL()
        {
            // Read ConnectionString from appsetting.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJP_DBConnectionString");

            // Instantiate a SqlConnection object with the Connection String read.
            conn = new SqlConnection(strConn);
        }

        public int AddCompetitor(Competitor competitor)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO Competitor (CompetitorName, Salutation, EmailAddr, Password) 
            OUTPUT INSERTED.CompetitorId VALUES(@name, @salutation, @email, @password)";

            cmd.Parameters.AddWithValue("@name", competitor.Name);
            cmd.Parameters.AddWithValue("@salutation", competitor.Salutation);
            cmd.Parameters.AddWithValue("@email", competitor.EmailAddr);
            cmd.Parameters.AddWithValue("@password", competitor.Password);

            conn.Open();

            competitor.CompetitorId = (int)cmd.ExecuteScalar();

            conn.Close();

            return competitor.CompetitorId;
        }

        public int AddJudge(Judge judge)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Judge (JudgeName, Salutation, EmailAddr, Password)
            OUTPUT INSERTED.JudgeId VALUES(@name, @salutation, @email, @password)";

            cmd.Parameters.AddWithValue("@name", judge.Name);
            cmd.Parameters.AddWithValue("@salutation", judge.Salutation);
            cmd.Parameters.AddWithValue("@email", judge.EmailAddr);
            cmd.Parameters.AddWithValue("@password", judge.Password);

            conn.Open();

            judge.JudgeId = (int)cmd.ExecuteScalar();

            conn.Close();

            return judge.JudgeId;
        }

        public bool IsCompetitorEmailExist(string email, int competitorId)
        {
            bool emailFound = false;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitorID FROM Competitor WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != competitorId)
                        //The email address is used by another staff
                        emailFound = true;
                }
            }
            else
            { //No record
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }

        public bool IsJudgeEmailExist(string email, int judgeId)
        {
            bool emailFound = false;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT JudgeID FROM Judge WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != judgeId)
                        //The email address is used by another staff
                        emailFound = true;
                }
            }
            else
            { //No record
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }
    }
}
