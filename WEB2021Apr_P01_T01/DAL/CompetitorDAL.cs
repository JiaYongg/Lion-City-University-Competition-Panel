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
    public class CompetitorDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        //Constructor
        public CompetitorDAL()
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

        public List<Competitor> GetAllCompetitor()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Competitor ORDER BY ComepetitorID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Competitor> competitorList = new List<Competitor>();
            while (reader.Read())
            {
                competitorList.Add(
                new Competitor
                {
                    CompetitorId = reader.GetInt32(0), 
                    Name = reader.GetString(1),
                    Salutation = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                    EmailAddr = reader.GetString(3), 
                    Password = reader.GetString(4), 
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return competitorList;
        }

        public bool IsEmailExist(string email, int competitorId)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
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
    }
}
