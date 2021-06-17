using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;
namespace WEB2021Apr_P01_T01.DAL
{
    public class LoginDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

    }
}
