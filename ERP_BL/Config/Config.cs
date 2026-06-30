using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Databases;
using ERP_BL.Enums;

namespace ERP_BL.Config
{

    public class Config : IConfig
    {
        string connectedDB=null;
        string connectedServer= null;

        private Server server = new Server();


        /// <summary>
        /// this will keep Database name.
        /// </summary>
        public string groupName
        {
            get { return this.connectedDB; }
            set { this.connectedDB = value; }
        }

        /// <summary>
        /// this will keep Server name connected.
        /// </summary>
        public string serverName
        {
            get { return this.connectedServer; }
            set { this.connectedServer = value; }
        }

        /// <summary>
        /// this will return all industry types supported
        /// </summary>
        /// <returns></returns>
        public List<string> getIndustryType()
        {
            List<string> list= new List<string>();
            //IndustryTypes indt = new IndustryTypes();
            list.Add(IndustryTypes.ServiceProvider.ToString());

            list.Add(IndustryTypes.InformationTechnology.ToString());
            list.Add(IndustryTypes.Construction.ToString());
            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "list of Industry Types is Retrived");

            return list;
        }
        /// <summary>
        /// this will return all Inquiry types supported
        /// </summary>
        /// <returns></returns>
        public List<string> getInquiryType()
        {
            List<string> list = new List<string>();
            list.Add(InquiryType.Tender.ToString());
            list.Add(InquiryType.Supply.ToString());
            list.Add(InquiryType.Principal.ToString());
            list.Add(InquiryType.Bill.ToString());
            list.Add(InquiryType.Standard.ToString());
            list.Add(InquiryType.Inventory.ToString());
            list.Add(InquiryType.DistributionBiz.ToString());
            list.Add(InquiryType.SupplyCCC.ToString());
            list.Add(InquiryType.DistributionBiz_CustomerCredit.ToString());
            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "list of Inquiry Types is Retrived");

            return list;
        }
        /// <summary>
        /// this will return all Inquiry types supported
        /// </summary>
        /// <returns></returns>
        public List<string> getPurchaseOrderTypes()
        {
            List<string> list = new List<string>();
            list.Add(InquiryType.Tender.ToString());
            list.Add(InquiryType.Supply.ToString());
            list.Add(InquiryType.Principal.ToString());
            list.Add(InquiryType.Bill.ToString());
            list.Add(InquiryType.Standard.ToString());
            list.Add(InquiryType.Inventory.ToString());
            list.Add(InquiryType.DistributionBiz.ToString());

            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "list of Inquiry Types is Retrived");

            return list;
        }
        /// <summary>
        /// this will return all Business types supported
        /// </summary>
        /// <returns></returns>
        public List<string> getBizType()
        {
            List<string> list = new List<string>();
            list.Add(BizTypes.Corporation.ToString());
            
            list.Add(BizTypes.Partnership.ToString());
            list.Add(BizTypes.SoleProprietor.ToString());
            list.Add(BizTypes.NGO.ToString());
            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "list of BizTypes is Retrived");

            return list;
        }
        public List<string> getGenderType()
        {
            List<string> list = new List<string>();
            list.Add(Gender.Male.ToString());
            list.Add(Gender.Female.ToString());
            list.Add(Gender.Shemale.ToString());
            list.Add(Gender.Not_Disclosed.ToString());
            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "list of gender Types is Retrived");

            return list;
        }
        public List<string> getItemNature()
        {
            List<string> list = new List<string>();
            list.Add(ItemNature.Inventory.ToString());
            list.Add(ItemNature.Services.ToString());
            
            return list;
        }
        /// <summary>
        /// Get all active SQL server instances over LAN
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public List<string> getAllSQLServer(string machineName )
        {
            return server.localServers(machineName);
                
        }

        /// <summary>
        /// Get all associated Databases names in selected server
        /// </summary>
        /// <param name="serverName">Name of the server</param>
        /// <param name="winAuth">windows authentication for server login or not</param>
        /// <returns></returns>
        public List<string> getDBListAssociated(string serverName, bool winAuth)
        {
            List<string> list = new List<string>();

            list = server.connectLANServer(serverName, winAuth, "Zas367@");
            if(list==null)
                list = server.connectLANServer(serverName, winAuth, "sa");
            if(list==null)
                ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Occured connecting to server " + serverName );
            else
            {
                this.serverName = serverName;
                ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Retrived list of Databases associated with Server name= " + serverName + "");

            }

            return list;
        }


        public List<pUser> connectDB(string DBName)
        {
            List<pUser> list = null;
            bool connSuccess = server.connectDB(this.serverName, DBName);
            if (connSuccess)
            {
                connectedDB = DBName;
                // load user names and passwords into datatabale
                list = server.getAllLogins(connectedServer, connectedDB);

            }

            return list;

        }
    
    public List<pUser> connectDB(string DBName, string systemVersion)
        {
            List<pUser> list = null;
            try
            {
           
            bool connSuccess= server.connectDB(this.serverName,DBName, systemVersion);
            if (connSuccess)
            {
                connectedDB = DBName;
                // load user names and passwords into datatabale
                list = server.getAllLogins(connectedServer, connectedDB);

            }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;


        }
    }
   

}
