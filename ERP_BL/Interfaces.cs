using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Config
{
    interface IServer
    {
        List<String> localServers(string _machineName);
        List<String> connectLANServer(string _server, bool winAuth,string password);
        List<String> connectCloudServer(string _server);
        bool connectDB(string _server, string _dbName, string softwareVersion);

    }

    interface IConfig
    {
        List<string> getIndustryType();
        List<string> getBizType();
    }

    interface ICompany
    {

    }

}
