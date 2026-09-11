using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.DBContext
{
    public static class Connections
    {
        public static ERP_BL.Databases.DBContextERP context { get; }
        public static string connStr { get; }
        public static SqlConnection connection { get; }

        static Connections()
        {
            var configuredConnectionString = ConfigurationManager.ConnectionStrings["DBContextERP"]?.ConnectionString;
            connection = new SqlConnection(configuredConnectionString ?? string.Empty);
            context = new Databases.DBContextERP();
            connStr = configuredConnectionString ?? string.Empty;
        }
    }
}
