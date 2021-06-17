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

        public int AddJudge()
        {

        }
    }
}
