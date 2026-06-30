using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Threading;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using ERP_BL.Databases;

namespace ERP_BL.Config
{

    public class Server : IServer
    {

        private string serverName = null;
        private string dbName = null;
        public static string password ;
        /// <summary>
        /// this function will retuen a list of SQL Servers in LAN
        /// </summary>
        /// <returns></returns>
        public List<string> localServers(string machineName)
        {
            StartSqlBrowserService(machineName);
            List<string> servers = GetSQLInfo();
            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "list of SQL Servers in LAN Retrived for " + machineName);

            return servers;
        }

        private void StartSqlBrowserService(String _activeMacihine)
        {
            ServiceController myService = new ServiceController();
            myService.ServiceName = "SQLBrowser";

            //foreach (var machine in activeMachines)
            //{
            try
            {
                myService.MachineName = _activeMacihine;
                string svcStatus = myService.Status.ToString();
                switch (svcStatus)
                {
                    case "ContinuePending":
                        Console.WriteLine("Service is attempting to continue.");
                        break;

                    case "Paused":
                        Console.WriteLine("Service is paused.");
                        Console.WriteLine("Attempting to continue the service.");
                        myService.Continue();
                        break;

                    case "PausePending":
                        Console.WriteLine("Service is pausing.");
                        Thread.Sleep(1000);
                        try
                        {
                            Console.WriteLine("Attempting to continue the service.");
                            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Attempting to continue the service for Sql Browser Service. ");

                            myService.Start();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Occured while Attempting to Continue Sql Browser Service. " + e.ToString());

                        }
                        break;

                    case "Running":
                        Console.WriteLine("Service is already running.");
                        break;

                    case "StartPending":
                        Console.WriteLine("Service is starting.");
                        ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Service is starting, start was pending for Sql Browser Service. ");
                        break;

                    case "Stopped":
                        Console.WriteLine("Service is stopped.");
                        Console.WriteLine("Attempting to start service.");
                        ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), " Attempting to restart Sql Browser Service. ");
                        myService.Start();
                        break;

                    case "StopPending":
                        Console.WriteLine("Service is stopping.");
                        Thread.Sleep(1000);
                        try
                        {
                            Console.WriteLine("Attempting to restart service.");
                            myService.Start();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Occured while Attempting to restart Sql Browser Service. " + e.ToString());
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Occured while Starting Sql Browser Service. " + e.ToString());

            }
        }



        private static List<string> GetSQLInfo()
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable table = instance.GetDataSources();
            //DisplayData(table);
            //return table;
            List<string> serverList = new List<string>();
            foreach (DataRow r in table.Rows)
            {
                serverList.Add(r["ServerName"].ToString() + (String.IsNullOrWhiteSpace(r["InstanceName"].ToString()) == true ? "" : "\\" + r["InstanceName"].ToString()));
            }
            return serverList;

        }

        private static void DisplayData(DataTable _table)
        {
            foreach (DataRow row in _table.Rows)
            {
                foreach (DataColumn dataColumn in _table.Columns)
                {
                    Console.WriteLine("{0} = {1}", dataColumn.ColumnName, row[dataColumn]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// this function will return DB names/Company Group from server 
        /// </summary>
        /// <param name="_server">SQL Server over LAN from which DB Names are being requested</param>
        /// <returns></returns>
        public List<string> connectLANServer                                                                                                                                                                                                                                                                                                                                                                                                                   (string _server, bool winAuth, string Serverpassword)
        {
            //bool connected = false;
            List<string> list = new List<string>();
            // Open connection to the database
            string conString;
            if (!winAuth)
                conString = "Server="+ _server +"; database=MK_ERP; user=sa;pwd="+ Serverpassword +";"; // "Server=" + _server + ";user=sa; pwd=" + Serverpassword + "; Database=MK_ERP";
            else
                conString = "Server =" + _server + "; Database =MK_ERP; Integrated Security =True;";


            using (SqlConnection con = new SqlConnection(conString) )
            {
                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Occured while opening Connection to db. " + ex.ToString());

                    return null;
                }
                password = Serverpassword;
                // Set up a command with the given query and associate
                // this with the current connection.
                using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr[0].ToString().ToLower().StartsWith("mk_"))
                                list.Add(dr[0].ToString().Replace("MK_",""));
                        }
                    }
                }
                //connected = true;
            }
            return list;
        }


        public List<string> connectCloudServer(string _server)
        {
            List<string> str = null;
            str.Add(" ");
            str.Add(" "); // { "0", "1" };
            return str;
        }
        /// </summary>
        /// <param name="_dbName">selected DBName of Compnay Group Name</param>
        /// <returns></returns>
        public bool connectDB(string _server, string _dbName)
        {
            try
            {
                SqlConnection openCon = new SqlConnection();
                openCon.ConnectionString = "server=" + _server + ";uid=sa;pwd=" + password + "; database=mk_" + _dbName + "";
                try
                {
                    openCon.Open();
                }
                catch (Exception ex)
                {
                    ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Occured while Oppening connection to Server name= " + _server + " and Database name= " + _dbName + ex.ToString());

                    if (openCon.State != ConnectionState.Open)
                        openCon.ConnectionString = "server=" + _server + ";uid=sa;pwd=" + password + "; database=mk_" + _dbName + "";
                }
                if (openCon.State != ConnectionState.Open)
                    openCon.Open();
                serverName = _server;
                dbName = _dbName;
                openCon.Close();
                ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Retrived list of Databases associated with Server name= " + serverName + "and connected to database= " + _dbName);

                return true;
            }
            catch (DbException ex)
            {
                Console.Write(ex.Message);
                ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Occured while connecting to Server name= " + _server + " and Database name= " + _dbName + ex.ToString());

                return false;
            }
        }

        /// <summary>
        /// this function will check connection to DB.
        /// </summary>
        /// <param name="_dbName">selected DBName of Compnay Group Name</param>
        /// <returns></returns>
        public bool connectDB(string _server, string _dbName, string softwareVersion)
        {
            try
            {
                SqlConnection openCon = new SqlConnection();
                openCon.ConnectionString = "server=" + _server + ";uid=sa;pwd=" + password + "; database=mk_" + _dbName + "";
                try
                {
                    openCon.Open();
                }
                catch (Exception ex)
                {
                    ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Occured while Oppening connection to Server name= " + _server + " and Database name= " + _dbName + ex.ToString());

                    if (openCon.State != ConnectionState.Open)
                        openCon.ConnectionString = "server=" + _server + ";uid=sa;pwd="+password+"; database=mk_" + _dbName + "";
                }
                if (openCon.State != ConnectionState.Open)
                    openCon.Open();
                if (!ERP_BL.BaseClasses.UpdateChecker.isSystemUpToDate(softwareVersion))
                    throw new Exception("New version is released, please update");

                serverName = _server;
                dbName = _dbName;
                openCon.Close();
                ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Retrived list of Databases associated with Server name= " + serverName + "and connected to database= " + _dbName);

                return true;
            }
            catch (DbException ex)
            {
                Console.Write(ex.Message);
                ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Occured while connecting to Server name= " + _server + " and Database name= " + _dbName + ex.ToString());

                return false;
            }
        }

        public List<pUser> getAllLogins(string _server, string _dbName)
        {

            List<pUser> pUser = null;
            try
            {
                DBContextERP ct = new DBContextERP("server=" + _server + ";uid=sa;pwd=" + password + "; database=mk_" + _dbName + "");

                //DataTable table = null;

                try
                {
                    pUser = ct.pUsers.ToList();
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), ex.ToString());
                    //throw ex;
                }
            }
            catch (Exception exception)
            {
               return null;
            }
            return pUser;

            //return table =(from r in ct.pUsers select r);
        }
    }


}
