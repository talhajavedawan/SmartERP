using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Databases;
namespace ERP_BL.ChatManager
{
    public enum DeviceType
    {
        Desktop,
        Android
    }

    public enum UserStatus
    {
        Active,
        Idle,
        Offline
    }

    public class ChatContact
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserStatus status { get; set; }
        public DeviceType deviceType {get; set;}
        public DateTime lastSeen { get; set; }

    }


}
