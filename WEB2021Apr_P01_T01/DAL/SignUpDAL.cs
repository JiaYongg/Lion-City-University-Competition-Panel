﻿using Microsoft.Extensions.Configuration;
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

    }
}
