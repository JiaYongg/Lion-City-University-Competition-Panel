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

        public int AddCompetitor(string name, string? salutation, string email, string password)
        {
            Competitor competitor = new Competitor();
            if (salutation == "")
            {
                salutation = null;
            }
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Competitor (CompetitorName, Salutation, EmailAddr, Password) 
            OUTPUT INSERTED.CompetitorId VALUES(@name, @salutation, @email, @password)";

            cmd.Parameters.AddWithValue("@name", name);      
            if (salutation != null)
            {
                cmd.Parameters.AddWithValue("@salutation", salutation);
            }
            else
            {
                cmd.Parameters.AddWithValue("@salutation", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

            conn.Open();

            competitor.CompetitorId = (int)cmd.ExecuteScalar();

            conn.Close();

            return competitor.CompetitorId;
        }

        public int AddJudge(string name, string? salutation, int aoi, string email, string password)
        {
            Judge judge = new Judge();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Judge (JudgeName, Salutation, AreaInterestID, EmailAddr, Password)
            OUTPUT INSERTED.JudgeId VALUES(@name, @salutation, @aoi, @email, @password)";

            cmd.Parameters.AddWithValue("@name", name);
            if (salutation != null)
            {
                cmd.Parameters.AddWithValue("@salutation", salutation);
            }
            else
            {
                cmd.Parameters.AddWithValue("@salutation", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@aoi", aoi);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

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
