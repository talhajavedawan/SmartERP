using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Databases;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERP_BL.ChatManager
{
    //interface IChatManager
    //{
    //    async void SyncContacts();
    //    //List<ChatContact> getAllContact();
    //    //List<ChatContact> getActiveContact();

    //}
    public class ChatManagerLocal
    {
        const string APILanPath = "https://203.135.42.34:5001/api/";
        const string APIWANPath = "https://203.135.42.34:5001/api/";
        bool isLAN = false;

        const string DEPARTMENTAL_GROUP_PATH = "ChatGroups/";
        const string CUSTOM_GROUP_PATH = "ChatGroups/";

        const string CHAT_USER_PATH = "ChatUsers/";

        const string DEPARTMENTAL_CHAT_PATH = "DepartmentalChat/";
        const string CUSTOM_CHAT_PATH = "CustomChat/";


        readonly IFirebaseConfig config;
        readonly IFirebaseClient client;
        private ChatNotifs ChatNotifs;

        public ChatManagerLocal()
        {
            //check user on LAN or not, than keep the state 
            
        }


        /// <summary>
        /// this method will push new Ticker type notification.
        /// </summary>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public async Task<bool> pushTickerAsync(Ticker ticker)
        {
            return await ChatNotifs.pushTickerAsync(ticker, client);
        }

        /// <summary>
        /// This function will delete a ticker based on its ID
        /// </summary>
        /// <param name="tickerId">unique number of ticker</param>
        /// <returns></returns>
        public bool DeleteTicker(int tickerId)
        {
            return ChatNotifs.DeleteTicker(tickerId, client);
        }

        /// <summary>
        /// this method will push new splash screen or replace exisitng.
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        public async Task<bool> pushSplashAsyc(SplashScreen screen)
        {
            return await ChatNotifs.pushSplashScreenAsync(screen, client);
        }

        /// <summary>
        /// Detele splash screen
        /// </summary>
        /// <returns></returns>
        public bool deleteSplash()
        {
            return ChatNotifs.DeleteSplashScreen(client);
        }
        /// <summary>
        /// Get all tickers (expired will get deleted auto)
        /// </summary>
        /// <returns></returns>
        public List<Ticker> GetTickers()
        {
            return ChatNotifs.GetTickers(client);
        }

        /// <summary>
        /// Get active splashscreen
        /// </summary>
        /// <returns></returns>
        public SplashScreen GetSplashScreen()
        {
            return ChatNotifs.GetSplashScreen(client);
        }

        /// <summary>
        /// this method will sync local database with online chat database (admin only)
        /// </summary>
        public async void syncContact()
        {
            var usersRepo = new UsersRepo();
            var users = usersRepo.getAllActiveUsers();

            List<ChatContact> contacts = new List<ChatContact>();
            foreach (var user in users)
            {

                var data = new ChatContact
                {
                    id = user.id,
                    deviceType = DeviceType.Desktop,
                    FirstName = user.employee.person.FName,
                    LastName = user.employee.person.LName,
                    userName = user.userName,
                    password = user.password
                };

                SetResponse response = await client.SetAsync("Contacts/" + user.employeeId, data);
                contacts.Add(response.ResultAs<ChatContact>());
            }
            Console.Write("successfully updated contacts. ");
        }

        /// <summary>
        /// this function helps to update any online contact detals. like, online-status, username, password etc
        /// </summary>
        /// <param name="username">userName of the contact to be updated</param>
        /// <param name="contact">ChatContact object with all information needed to be updated. you need to fill complete object with existing and changed info</param>
        public async void updateContact(string username, ChatContact contact)
        {
            var usersRepo = new UsersRepo();
            var user = usersRepo.getAllActiveUsers().ToList().First(x => x.userName == username);

            if (string.IsNullOrEmpty(username))
                throw new Exception("Username empty");
            if (user == null)
                throw new Exception("Username not found");

            try
            {
                var response = await client.UpdateAsync<ChatContact>("Contacts/" + user.employeeId, contact);
                //contacts.Add(response.ResultAs<ChatContact>());

                Console.WriteLine("successfully updated contact. ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// this function helps to update any chat user status. like, online, idle and away etc.
        /// </summary>
        /// <param name="username">userName of the contact to be updated</param>
        /// <param name="status">UserStatus type value to set status of any chat user</param>
        public async void updateStatus(string username, UserStatus status)
        {
            var usersRepo = new UsersRepo();
            var user = usersRepo.getAllActiveUsers().ToList().First(x => x.userName == username);



            if (string.IsNullOrEmpty(username))
                throw new Exception("Username empty");
            if (user == null)
                throw new Exception("Username not found");
            var contact = GetContact(user.employeeId);
            contact.status = status;
            try
            {
                var response = await client.UpdateAsync<ChatContact>("Contacts/" + user.employeeId, contact);
                //contacts.Add(response.ResultAs<ChatContact>());

                Console.WriteLine("successfully updated contacts. ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// User can create new group online by passing ChatGroup object
        /// </summary>
        /// <param name="group">ChatGroup object to push into chat database</param>
        public async void CreateNewGroup(ChatGroup group, bool isDepartmental)
        {
            try
            {
                string groupPath = isDepartmental ? DEPARTMENTAL_GROUP_PATH : CUSTOM_GROUP_PATH;
                var response = await client.SetAsync(groupPath + group.id, group);
                //contacts.Add(response.ResultAs<ChatContact>());
                Console.Write("successfully updated contacts. ");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Unable to greate group " + group.groupName);
            }
        }

        /// <summary>
        /// Get individual ChatContact object of userId
        /// </summary>
        /// <param name="userId">Employee Id as userId to get contact details against</param>
        /// <returns>ChatContact Object</returns>
        public ChatContact GetContact(int userId)
        {
            try
            {
                ChatContact contact = new ChatContact();
                var response = client.Get("Contacts/" + userId);
                contact = (response.ResultAs<ChatContact>());
                Console.Write("successfully got contact for user. ");
                return contact;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Unable to greate group ");
            }
        }

        /// <summary>
        /// This function will return all contact avavilable in chat database.
        /// </summary>
        /// <returns>List of ChatContact</returns>
        public List<ChatContact> GetAllContacts()
        {
            try
            {
                List<ChatContact> contacts = new List<ChatContact>();
                var response = client.Get("Contacts");
                var children = JArray.Parse(response.Body);

                if (children[0] == null)
                    children.RemoveAt(0);
                var json = JArray.Parse(children.ToString());
                contacts.AddRange(json.ToObject<List<ChatContact>>());
                //contacts.Add(response.ResultAs<ChatContact>());
                Console.Write("successfully got all contacts. ");
                return contacts;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Unable to get contacts");
            }
        }

        /// <summary>
        /// This function returns all chat groups available in chat database
        /// </summary>
        /// <returns>List of ChatGroup objects</returns>
        public List<ChatGroup> GetAllGroups(bool isDepartmental)
        {
            List<ChatGroup> groups = new List<ChatGroup>();
            try
            {
                string groupPath = isDepartmental ? DEPARTMENTAL_GROUP_PATH : CUSTOM_GROUP_PATH;
                var response = client.Get(groupPath);

                var children = JArray.Parse(response.Body);
                var json = JArray.Parse(children.ToString());
                groups.AddRange(json.ToObject<List<ChatGroup>>());

                Console.Write("successfully updated contacts. ");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //throw new Exception("Unable to retrieve groups ");
            }
            return groups;
        }

        public ChatGroup GetGroup(int id, bool isDepartmental)
        {
            ChatGroup groups = new ChatGroup();
            try
            {

                string groupPath = isDepartmental ? DEPARTMENTAL_GROUP_PATH : CUSTOM_GROUP_PATH;
                var response = client.Get(groupPath + "/" + id);
                groups = response.ResultAs<ChatGroup>();
                Console.Write("Group fetched successfully ");

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //throw new Exception("Unable fetch group " + id);
            }
            return groups;
        }

        /// <summary>
        /// this function will push a one2oneMessage object to chat database live.
        /// </summary>
        /// <param name="message">Object of One2OneMessage</param>
        public void pushOne2OneMsg(One2OneMessage message)
        {
            //var usersRepo = new UsersRepo();
            //var sender = usersRepo.getAllActiveUsers().ToList().First(x => x.employeeId == message.sender);
            //var user = usersRepo.getAllActiveUsers();
            //var receiver = usersRepo.getAllActiveUsers().ToList().First(x => x.employeeId == message.receiver);


            //if (sender == null)
            //    throw new Exception("sender account suspended or not exist anymore");
            //if (receiver == null)
            //    throw new Exception("receiver account suspended or not exist anymore");

            if (message.msgId == null || message.msgId < 0)
                message.msgId = 0;
            try
            {
                var response = client.Update<One2OneMessage>("One2One/" + message.sender.ToString() + "/" + message.receiver.ToString() + "/" + message.msgId, message);
                var response2 = client.Update<One2OneMessage>("One2One/" + message.receiver.ToString() + "/" + message.sender.ToString() + "/" + message.msgId, message);
                //contacts.Add(response.ResultAs<ChatContact>());

                Console.WriteLine("Message pushed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public void pushGroupMsg(GroupMessage message, ChatGroup group, bool isDepartmental)
        {
            var usersRepo = new UsersRepo();
            var sender = usersRepo.getAllActiveUsersForChat().ToList().First(x => x.employeeId == message.sender);


            if (sender == null)
                throw new Exception("sender account suspended or not exist anymore");


            if (message.msgId == null || message.msgId < 0)
                message.msgId = 0;
            try
            {
                string groupPath = isDepartmental ? DEPARTMENTAL_CHAT_PATH : CUSTOM_CHAT_PATH;

                var response = client.Update<GroupMessage>(groupPath + group.id + "/" + message.msgId, message);
                //var response2 = client.Update<One2OneMessage>("One2One/" + receiver.employeeId.ToString() + "/" + sender.employeeId.ToString() + "/" + message.msgId, message);
                //contacts.Add(response.ResultAs<ChatContact>());

                Console.WriteLine("Message pushed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// this function will return all personal chats for any user
        /// </summary>
        /// <param name="chatUserId">UserId or EmployeeId to get persoal chat agaist</param>
        /// <returns>Jobject of all personal messages</returns>
        public List<One2OneMessage> getOne2OneMsg(int chatUserId, int receiverId)
        {
            var usersRepo = new UsersRepo();
            //var sender = usersRepo.getAllActiveUsers().ToList().First(x => x.employeeId == chatUserId);

            //if (sender == null)
            //    throw new Exception("sender account suspended or not exist any more");

            try
            {
                List<One2OneMessage> msgs = new List<One2OneMessage>();
                var response = client.Get("One2One/" + chatUserId + "/" + receiverId + "");
                string responseString = response.Body.ToString().Trim();
                //JArray chat = null;
                if (responseString.Trim().StartsWith("["))
                {
                    var temp = JArray.Parse(responseString);
                    foreach (var chatmsg in temp.Children())
                    {
                        Console.Write(chatmsg);
                        if (string.IsNullOrEmpty(chatmsg.ToString()))
                            continue;

                        One2OneMessage msg = null;
                        try
                        {

                            msg = JsonConvert.DeserializeObject<One2OneMessage>(chatmsg.ToString());
                        }
                        catch
                        {
                            var temp1 = chatmsg.Children().First().ToString();
                            msg = JsonConvert.DeserializeObject<One2OneMessage>(temp1.ToString());
                        }
                        msgs.Add(msg);
                    }
                }
                else
                {
                    var temp = JObject.Parse(response.Body.ToString().Trim());
                    foreach (var chatmsg in temp.Children())
                    {
                        Console.Write(chatmsg);
                        if (string.IsNullOrEmpty(chatmsg.ToString()))
                            continue;

                        One2OneMessage msg = null;
                        try
                        {

                            msg = JsonConvert.DeserializeObject<One2OneMessage>(chatmsg.ToString());
                        }
                        catch
                        {
                            var temp1 = chatmsg.Children().First().ToString();
                            msg = JsonConvert.DeserializeObject<One2OneMessage>(temp1.ToString());
                        }
                        msgs.Add(msg);
                    }
                }

                //var chat = JObject.Parse(response.Body.ToString().Trim());



                //if (response.ToString().Trim().StartsWith("["))
                //    chat = JArray.Parse(responseString );
                //else
                //    chat = JArray.Parse("[" + responseString +"]");
                //string jsonstring = JsonConvert.SerializeObject(response.Body);
                //jsonstring = jsonstring ;
                return msgs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public bool isMessageExist(Int64 msgId, int chatUserId, int receiverId)
        {
            bool isExist = false;

            try
            {
                var response = client.Get("One2One/" + chatUserId + "/" + receiverId + "/" + msgId);

                if (response.Body == "null")
                    isExist = false;
                else
                {
                    isExist = true;
                }
            }
            catch
            { }
            return isExist;
        }

        public bool isMessageExist(Int64 msgId, int groupId, bool isDepartmental)
        {
            bool isExist = false;


            try
            {
                string groupPath = isDepartmental ? DEPARTMENTAL_CHAT_PATH : CUSTOM_CHAT_PATH;
                var response = client.Get(groupPath + "/" + groupId + "/" + msgId);

                if (response.Body == "null")
                    isExist = false;
                else
                {
                    isExist = true;
                }
            }
            catch
            { }
            return isExist;
        }

        public JArray getOne2OneMsg(int chatUserId)
        {
            //var usersRepo = new UsersRepo();
            //var sender = usersRepo.getAllActiveUsers().ToList().First(x => x.employeeId == chatUserId);

            //if (sender == null)
            //    throw new Exception("sender account suspended or not exist any more");

            try
            {
                var response = client.Get("One2One/" + chatUserId + "");
                //contacts.Add(response.ResultAs<ChatContact>());
                //var result = response.ResultAs< List<One2OneMessage>>();
                //return response;
                string responseString = response.Body.ToString().Trim();
                //responseString = responseString.Substring(1);
                //responseString = responseString.Substring(0, responseString.Length - 1);
                //var jsObject = JObject.Parse(responseString);
                //var mList = JsonConvert.DeserializeObject<IDictionary<string, List<One2OneMessage>>>(responseString);
                JArray chat = null;
                if (response.Body.ToString().Trim().StartsWith("["))
                    chat = JArray.Parse(response.Body);
                else
                    chat = JArray.Parse("[" + response.Body + "]");
                string jsonstring = JsonConvert.SerializeObject(response.Body);
                jsonstring = jsonstring;

                var t = chat.Children().Children().Where(j => j.Type != JTokenType.Null);

                //var arr = JArray.Parse(jsonstring);


                //var datalist =  JsonConvert.DeserializeObject<List<One2OneMessage>>(chat.Children().ToString());
                return chat;

                //return new JArray(t); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public One2OneMessage getOne2OneMsg(int chatUserId, int receiverId, Int64 msgId)
        {
            var usersRepo = new UsersRepo();
            var sender = usersRepo.getAllActiveUsers().ToList().First(x => x.employeeId == chatUserId);

            if (sender == null)
                throw new Exception("sender account suspended or not exist any more");

            try
            {
                var response = client.Get("One2One/" + chatUserId + "/" + receiverId + "/" + msgId);

                if (response.Body == "null")
                    throw new Exception("Message doesn't exist");

                var data = JsonConvert.DeserializeObject<One2OneMessage>(response.Body);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public List<GroupMessage> getGroupMsg(int groupId, bool isDepartmental)
        {
            List<GroupMessage> msgs = new List<GroupMessage>();
            try
            {
                string groupPath = isDepartmental ? DEPARTMENTAL_CHAT_PATH : CUSTOM_CHAT_PATH;
                var response = client.Get(groupPath + "/" + groupId);
                var chat = JArray.Parse(response.Body.ToString().Trim());

                foreach (var chatmsg in chat.Children())
                {
                    Console.Write(chatmsg);
                    var msg = JsonConvert.DeserializeObject<GroupMessage>(chatmsg.ToString());
                    msgs.Add(msg);
                }
                return msgs;
            }
            catch (Exception ex)
            {
                return msgs;
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public GroupMessage getGroupMsg(int groupId, int msgNo, bool isDepartmental)
        {
            try
            {
                string groupPath = isDepartmental ? DEPARTMENTAL_CHAT_PATH : CUSTOM_CHAT_PATH;
                var response = client.Get(groupPath + "/" + groupId + "/" + msgNo);
                if (response.Body == "null")
                    throw new Exception("Message doesn't exist");
                GroupMessage msg = new GroupMessage();
                msg = JsonConvert.DeserializeObject<GroupMessage>(response.Body.ToString());
                return msg;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
