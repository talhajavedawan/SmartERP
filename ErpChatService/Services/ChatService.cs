using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Text.RegularExpressions;
using ERP_BL.ChatManager;
namespace ErpChatService.Services
{
    public class ChatService
    {
        private readonly IMongoCollection<ERP_BL.ChatManager.ChatGroup> _chatGroups;
        private readonly IMongoCollection<ERP_BL.ChatManager.One2OneMessage> _onetoOneChat;
        private readonly IMongoCollection<ERP_BL.ChatManager.GroupMessage> _groupChat;
        private readonly IMongoCollection<ERP_BL.ChatManager.ChatUser> _ChatUser;

        public ChatService(Models.IChatDatabaseSettings settings)
        {
            Console.WriteLine(settings.ConnectionString);
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _chatGroups = database.GetCollection<ERP_BL.ChatManager.ChatGroup>(settings.GroupsCollectionName);
            _onetoOneChat = database.GetCollection<ERP_BL.ChatManager.One2OneMessage>(settings.PersonalChatCollectionName);
            _groupChat = database.GetCollection<ERP_BL.ChatManager.GroupMessage>(settings.GroupChatCollectionName);
            _ChatUser = database.GetCollection<ERP_BL.ChatManager.ChatUser>(settings.ChatUsersCollectionName);
        }

        //================= Chat server test ==========================
        public bool IsServerAvailable() =>
            true;

        //================= chat groups ==========================
        public List<ChatGroup> GetChatGroups() =>
            _chatGroups.Find(x => true).ToList();

        public ChatGroup GetChatGroup(int id) =>
            _chatGroups.Find<ChatGroup>(x => x.id == id).FirstOrDefault();

        public ChatGroup CreateChatGroup(ChatGroup chatGroup)
        {
            _chatGroups.InsertOne(chatGroup);
            return chatGroup;
        }

        public void UpdateChatGroup(int id, ChatGroup groupIn) =>
            _chatGroups.ReplaceOne(x => x.id == id, groupIn);

        public void RemoveChatGroup(ChatGroup groupIn) =>
            _chatGroups.DeleteOne(x => x.id == groupIn.id);

        public void RemoveChatGroup(int id) =>
            _chatGroups.DeleteOne(x => x.id == id);

        //================= chat Contacts ==========================
        public List<ChatUser> GetChatUsers() =>
            _ChatUser.Find(x => true).ToList();

        public ChatUser GetChatUser(int id) =>
            _ChatUser.Find<ChatUser>(x => x.employeeId == id).FirstOrDefault();

        public ChatUser CreateChatUser(ChatUser ChatUser)
        {
            _ChatUser.InsertOne(ChatUser);
            return ChatUser;
        }

        public void UpdateChatUser(int id, ChatUser contactIn) =>
            _ChatUser.ReplaceOne(x => x.employeeId == id, contactIn);

        public void RemoveChatUser(ChatUser ChatUserIn) =>
            _ChatUser.DeleteOne(x => x.employeeId == ChatUserIn.employeeId);

        public void RemoveChatUser(long id) =>
            _ChatUser.DeleteOne(x => x.employeeId == id);

        //================= One2One Chat ==========================
        //public List<ChatGroup> GetChatUser() =>
        //    _chatGroups.Find(x => true).ToList();

        //public ChatGroup GetChatUser(int id) =>
        //    _chatGroups.Find<ChatGroup>(x => x.id == id).FirstOrDefault();

        //public ChatGroup CreateChatUser(ChatGroup chatGroup)
        //{
        //    _chatGroups.InsertOne(chatGroup);
        //    return chatGroup;
        //}

        //public void UpdateChatUser(int id, ChatGroup groupIn) =>
        //    _chatGroups.ReplaceOne(x => x.id == id, groupIn);

        //public void RemoveChatUser(ChatGroup groupIn) =>
        //    _chatGroups.DeleteOne(x => x.id == groupIn.id);

        //public void RemoveChatUser(int id) =>
        //    _chatGroups.DeleteOne(x => x.id == id);
    }
}
