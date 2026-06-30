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
        public static ERP_BL.Databases.DBContextERP context { get; } // = new Databases.DBContextERP();
       public static string connStr { get; } //= ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public static SqlConnection connection { get; }

        static Connections()
        {
            connection = new SqlConnection(@"Server = DESKTOP-5KPOCCP\SQLEXPRESS; database=MK_ERP; user=sa;pwd=sa;"); // "Server=MICROKOSM; database=MK_ERP; user=sa;pwd=sa;");
            context = new Databases.DBContextERP();
            connStr = "test"; // ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        }
    }
}
