using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Agent.DAO
{
    public class DAO
    {
        private static string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=C:\Komp_mag\Komp_mag\App_Data\aspnet-Agent-20190103081620.mdf;Initial Catalog=aspnet-Agent-20190103081620;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        public SqlConnection Connection { get; set; }
        public void Connect()
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public void Disconnect()
        {
            Connection.Close();
        }
    }
}