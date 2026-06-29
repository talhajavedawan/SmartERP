using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ErpChatService.Models
{
    public class ChatDatabaseSettings : IChatDatabaseSettings
    {
        public string PersonalChatCollectionName { get; set; }
        public string GroupsCollectionName { get; set; }
        public string GroupChatCollectionName { get; set; }
        public string ChatUsersCollectionName { get; set; }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IChatDatabaseSettings
    {
        public string PersonalChatCollectionName { get; set; }
        public string GroupsCollectionName { get; set; }
        public string GroupChatCollectionName { get; set; }
        public string ChatUsersCollectionName { get; set; }

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
