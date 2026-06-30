using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.BaseClasses
{
    public static class UpdateChecker
    {
        static string updateFileFtpPathLAN=  "ftp://192.168.10.95/Updates/updates.txt";
        static string updateFileFtpPathWAN = "ftp://203.135.42.34/Updates/updates.txt";

        static string ftpUserName = "zaserpupdates";
        static string ftpPassword = "";


        public static bool isSystemUpToDate(string Version)
        {
            string accuteVersion = Version.Split('.')[0] +'.'+ Version.Split('.')[1];

            // download ftp version config file.
            FtpWebRequest request;
            FtpWebResponse response=null ;
            try
            {
                request = (FtpWebRequest)WebRequest.Create(updateFileFtpPathLAN);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                request.Timeout = 3000;
                response = (FtpWebResponse)request.GetResponse();

            }
            catch (Exception ex)
            {
                try
                {
                    request = (FtpWebRequest)WebRequest.Create(updateFileFtpPathWAN);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Timeout = 3000;
                    // This example assumes the FTP site uses anonymous logon.
                    request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                    response = (FtpWebResponse)request.GetResponse();
                }
                catch (Exception exp)
                { }
            }
            if (response==null)
            {
                throw new Exception("Please check your connection");
            }
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string curerntLine = "";
            while (!reader.EndOfStream)
            {
                curerntLine = reader.ReadLine();
                // check if desired line is arrived.

                if (curerntLine.StartsWith("ProductVersion"))
                {
                    // get our of loop
                    break;
                }
            }

            // dispose connections.
            reader.Close();
            reader.Dispose();
            response.Close();


            // get accute version number from file.
            curerntLine = curerntLine.Split('=')[1].Trim();
            string onlineAccuteVersion= curerntLine.Split('.')[0] + '.' + curerntLine.Split('.')[1];

            if ( Convert.ToDouble( onlineAccuteVersion) == Convert.ToDouble( accuteVersion))
                return true;
            else
                return false;

        }
    }
}
