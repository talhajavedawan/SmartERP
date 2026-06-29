using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ERP_BL;
using ERP_BL.Config;
using ERP_BL.Databases;
using ERP_BL.ChatManager;
using Newtonsoft.Json.Linq;
using System.Threading;
using ERP_BL.FixAssets;
using RestSharp;
using HtmlAgilityPack;
using System.Net;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Security.Cryptography;

namespace testConsol
{
    class Program
    {
        static void Main(string[] args)
        {
            string plainText = "kashif is here";
            Console.WriteLine("plaint text -> " + plainText);
            var encrypted = Encryptxx(plainText);

            Console.WriteLine("encrypted string -> " + encrypted);
            Console.WriteLine("decrypted string -> " + Decryptxx(encrypted));

            //getFileNBP();
            //machineInfo();

            //var list1 = new List<int> { 1, 2, 3, 4, 5 };
            //var list2 = new List<int> { 2, 3, 4, 5, 6 };

            //Console.WriteLine(list1);
            
            //list1.Except(list2); //1 - items removed
            ////list2.Except(list1); //6 - items added
            //Console.WriteLine("====== L1 ===============");
            //foreach(var item in list1.Except(list2))
            //Console.WriteLine(item);
            //Console.WriteLine("====== L2 ===============");
            //foreach(var item in list2.Except(list1))
            //Console.WriteLine(item);
            
            //updatePermission();
            //Building();

            //addAssetNature();

            //uploadChatFile();
            //pushTicker();
            //getTicker();

            //DeleScreen();
            //sendSplash();
            //temp2();
            // Attach attachment = new Attach(@"D:\Notify1.txt");
            //Console.Write( attachment.startUploading(ERP_BL.Enums.TransactionItemType.Offer));

            //getGroupChat();
            //pushGroupMsg();
            //createGroup("MicroKosm");
            //pushContacts();
            //pushOne2One();
            //getAllOne2One();
            //loadAll(69);
            //getaAllContacts();            //updateContact("kashif422");


            //var depts = new DepartmentRepo().GetDepartments();
            //var userDept = new UsersRepo().getAllActiveUsers().First().employee.departments;

            //Console.WriteLine(depts.Count());
            //foreach (var dept in userDept)
            //{
            //    Console.WriteLine(dept.Id + "\t" + dept.ParentID + "\t " + dept.isActive + "\t" + dept.DeptName );
            //}
            //UsersRepo repo = new UsersRepo();
            //var r = repo.getAllActiveUsers();
            //foreach (var _r in r)
            //{
            //    var depts = _r.employee.departments.ToList();
            //    Console.Write(_r.employee.person.FName + ">>\n");

            //    foreach (var _Dept in depts)
            //    {
            //        Console.WriteLine("\t" + _Dept.DeptName);

            //    }
            //    Console.Write("--------------------------------");
            //}
            Console.ReadKey();
        }

        public static string Encryptxx(string text)
        {

            byte[] src = Encoding.UTF8.GetBytes(text);
            byte[] key = Encoding.ASCII.GetBytes("Yflh7n8wTSftl911Cwivt1EM7PvyQA52Dod1Lw1PHELpVatxM7q/N5gAx7to7dkl");
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 256;

            using (ICryptoTransform encrypt = aes.CreateEncryptor(key, null))
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                encrypt.Dispose();
                return Convert.ToBase64String(dest);
            }
        }

        public static string Decryptxx(string text)
        {

            byte[] src = Convert.FromBase64String(text);
            RijndaelManaged aes = new RijndaelManaged();
            byte[] key = Encoding.ASCII.GetBytes("Yflh7n8wTSftl911Cwivt1EM7PvyQA52Dod1Lw1PHELpVatxM7q/N5gAx7to7dkl");
            aes.KeySize = 256;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.ECB;
            using (ICryptoTransform decrypt = aes.CreateDecryptor(key, null))
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                decrypt.Dispose();
                return Encoding.UTF8.GetString(dest);
            }
        }


        private static void getFileNBP()
        {
            string filePath = "";
            var client = new RestClient("https://www.nbp.com.pk/RATESHEET/index.aspx");
            var request = new RestRequest("Get");
            request.AddHeader("Cookie", "ASP.NET_SessionId=zscolwuegwwjs345k1e33vas");
            RestResponse response = (RestResponse)client.ExecuteGetAsync(request).Result;
           Console.WriteLine("response Got");

            HtmlDocument htmlDoc = new HtmlDocument();
            HttpStatusCode statusCode = response.StatusCode;
            Console.WriteLine("response status code: " + statusCode);
            if (statusCode == HttpStatusCode.OK)
                if (!string.IsNullOrEmpty(response.Content))
                {
                    Console.WriteLine("Loading content | \n" + response.Content);
                    htmlDoc.LoadHtml(response.Content);
                    // check if response 
                    Console.WriteLine("Finding nodes");
                    var docNodes = htmlDoc.DocumentNode.SelectNodes("//a[contains(@id,'ctl00_ContentPlaceHolder1_GridView1_')]");
                    Console.WriteLine("Got total nodes as : " + docNodes.Count);
                    if (docNodes.Count > 0)
                    {

                        filePath = docNodes.First().GetAttributeValue("href", "");
                        filePath = !string.IsNullOrEmpty(filePath) ? "https://www.nbp.com.pk" + filePath.Replace("..", "") : filePath;
                        Console.WriteLine( filePath);

                        string localPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Today.ToString("dd-mm-yyyy"));
                        WebClient Client = new WebClient();
                        Client.DownloadFile(filePath, localPath+".pdf");

                        Console.WriteLine( localPath);
                        PdfReader reader = new PdfReader(filePath);
                        Console.WriteLine("reading in pdf reader");
                        string fileText = string.Empty;
                        for (int page = 1; page <= reader.NumberOfPages; page++)
                        {
                            Console.WriteLine("reading page " + page);
                            fileText += PdfTextExtractor.GetTextFromPage(reader, page);
                        }
                        reader.Close();

                        Console.Write(fileText);
                    }
                }
        }

        //private static void updatePermission()
        //{
        //    UsersRepo repo = new UsersRepo();
        //    DBContextERP context = new DBContextERP();
        //    var rol = context.Roles.FirstOrDefault(x => x.Id == 1052);
        //    var users = repo.getAllusers();
        //    int i = 0;
        //    foreach (var user in users)
        //    {

        //        var userRol = user.Roles.FirstOrDefault(x => x.Id == 1052);
        //        if (userRol == null)
        //        {
        //            var _user = context.Users.Include("Roles").FirstOrDefault(x => x.id == user.id);
        //            _user.Roles.Add(context.Roles.FirstOrDefault(x => x.Id == 1052));
        //            context.SaveChanges();
        //            Console.WriteLine("[" + (i++).ToString() + "/" + users.Count + " ]" + user.userName + " is updated");
        //        }
        //        else
        //            Console.WriteLine("[" + (i++).ToString() + "/" + users.Count + " ]" + user.userName + " >>> is already updated");
        //    }

        //    Console.WriteLine("Verifying.......");
        //    users.Clear();
        //    users = repo.getAllusers();
        //    i = 0;
        //    foreach (var user in users)
        //    {
        //        var userRol = user.Roles.FirstOrDefault(x => x.Id == 1052);
        //        if (userRol != null)
        //        { Console.WriteLine("[" + (i++).ToString() + "/" + users.Count + " ]" + user.userName + " is " + "+++ varified ++++"); }
        //        else
        //        { Console.WriteLine("[" + (i++).ToString() + "/" + users.Count + " ]" + user.userName + " is " + "-------- NOT VERIFIED ------"); }

        //    }
        //}

        private static void machineInfo()
        {
            //var machine = new MachineAPI.MachineAPI();
            //Console.WriteLine("CPU ID: " + machine.getCPU_ID());
            //Console.WriteLine("Mac Address: " + machine.getMacAddress());
            //Console.WriteLine("Board Serial # " + machine.GetBoardSerialNo());

            //Console.WriteLine(machine.GenKey("ZASERP"));

        }

        private static void uploadChatFile()
        {
            Attach attach = new Attach();
            attach.uploadFile(ERP_BL.Enums.ChatFileType.One2One, @"C:\Users\kashif\Pictures\hiring.PNG", 8, 1);

            var t = attach.downloadFile(ERP_BL.Enums.ChatFileType.One2One, @"C:\Users\kashif\Pictures\hiring.PNG", 8, 1);
        }
        private static void addOfficialAuth()
        {
            OfficialAuth officialAuth = new OfficialAuth();
            AssetsRepo repo = new AssetsRepo();
            officialAuth.AuthName = "CDA";

        }

        private static void Building()
        {
            AssetsRepo assetsRepo = new AssetsRepo();
            //CompanyRepo companyRepo = new CompanyRepo();

            //var company = companyRepo.GetCompany(8);
            //DepartmentRepo departmentRepo = new DepartmentRepo();

            //var employeeRepo = new EmployeeRepo();

            //Building building = new Building();

            //var asset = new Asset();
            //asset.AssetName = "test Asset with managing dept";
            //asset.AssetNature = assetsRepo.GetAssetNature(4);
            //asset.isInsured = false;
            //asset.isRentable = true;
            //asset.isSubsidary = false;
            //asset.mustInsured = true;
            //asset.OwnerCompany = company;
            //asset.purchaseInfo = new PurchaseInfo() { acquireAt = DateTime.Today, currecncy = company.currency, FA_Amount = 10000, FA_Amount_PER = 12000, PER = 150 };
            //asset.address = new Address() { addressType = ERP_BL.Enums.AddressTypes.assetAddress, City = "Islamabad", Country = "Pakistan", Line1 = "House No =123", Line2 = "Sector E", region = "NA", State = "Islamabad", Zip = 41000 };
            //asset.handler = departmentRepo.GetActiveDepartments()[0].user;
            //asset.owner = employeeRepo.GetAllEmployees().First();
            //asset.managingDept = company.departments[0];

            //building.asset = asset;
            //building.isMortgaged = true;
            //building.measureUnit = new Area() { Length = 150, measureUnitType = ERP_BL.Enums.MeasureUnitType.Meter, width = 150 };

            //assetsRepo.AddBuilding(building);

            //var b = assetsRepo.GetAllBuildingAsset();
            //foreach (var _b in b)
            //{
            //    Console.Write(_b.asset.AssetName);
            //}

            var ads = assetsRepo.GetAllAddress();

            // show countries
            foreach (var ad in ads)
            {
                Console.WriteLine(ad.Country);
            }

            Console.WriteLine("=========================================");
            // show cities
            var cities = ads.Where(x => x.Country == "Pakistan");

            foreach (var city in cities)
            {
                Console.WriteLine(city.City);
            }



        }

        private static void addAssetNature()
        {
            // adding top lavel.

            //AssetsRepo assetsRepo = new AssetsRepo();
            //assetsRepo.AddAssetNature(new AssetNature()
            //{
            //    isActive = true,
            //    isSubsdary=false,
            //    NatureName="Building",
            //});

            // adding sub level. 
            AssetsRepo assetsRepo = new AssetsRepo();
            var parentNature = assetsRepo.GetAssetNature(2);
            assetsRepo.AddAssetNature(new AssetNature()
            {
                parentNature = parentNature,
                isActive = true,
                isSubsdary = false,
                NatureName = "Commercial",
            });
        }

        private static void getAllOne2One()
        {
            ChatManager cm = new ChatManager();
            var cc = cm.getOne2OneMsg(8);

            foreach (var c in cc)
            {
                Console.WriteLine(c.ToString());
            }
        }
        private static void getTicker()
        {
            ChatManager manager = new ChatManager();
            var tickers = manager.GetTickers();
            foreach (var ticker in tickers)
            {
                Console.WriteLine(ticker.newsString);
            }
        }

        private static void pushTicker()
        {
            Ticker ticker = new Ticker()
            {
                id = 1,
                initUserId = 9,
                newsString = "this is my ticker test 2",
                tickerColor = ConsoleColor.Black.ToString(),
                validTill = DateTime.Today,

            };
            ChatManager manager = new ChatManager();
            Console.WriteLine(manager.pushTickerAsync(ticker));
        }

        public static void DeleScreen()
        {
            ChatManager manager = new ChatManager();
            Console.WriteLine(manager.deleteSplash());
        }

        public static void sendticker()
        {
            Ticker ticker = new Ticker()
            {
                id = 0,
                initUserId = 9,
                newsString = "This is important ticker",
                tickerColor = ConsoleColor.Black.ToString(),
                validTill = DateTime.Today
            };
            ChatManager manager = new ChatManager();
            Console.WriteLine(manager.pushTickerAsync(ticker));
        }
        public static void sendSplash()
        {
            SplashScreen screen = new SplashScreen()
            {
                ImagePath = "tes",
                timeoutSecond = 6,
            };

            ChatManager manager = new ChatManager();
            Console.Write(manager.pushSplashAsyc(screen));

        }
        public static void temp2()
        {
            //var client = new RestClient("https://touch.facebook.com/page_content_list_view/more/?page_id=1493704227531628&start_cursor={\"recommendations_post_cursor\":\"{\\\"in_progresscursor_type\\\":\\\"open_graph_and_page_rec\\\",\\\"local_rec_pattern_cursor\\\":\\\"\\\",\\\"rating_cursor\\\":\\\"\\\",\\\"offline_offset\\\":-1,\\\"og_post_cursor\\\":\\\"start_of_og_post_query\\\"}\"}&num_to_fetch=1000&surface_type=page_recommendations_tab");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.GET);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content.Length);
            //Console.WriteLine(response.Content);

        }
        public static void temp()
        {

            ChatContact userId = new ChatContact();
            ChatManager cm = new ChatManager();
            var loggedInid1 = 69;
            var userRepo = new UsersRepo();

            List<User> users = new List<User>();
            Thread th1 = new Thread(() => users = userRepo.getAllActiveUsers());
            th1.Priority = ThreadPriority.Highest;
            th1.Start();

            JArray chat = null;
            Thread th2 = new Thread(() => chat = cm.getOne2OneMsg(loggedInid1));
            th2.Priority = ThreadPriority.Highest;
            th2.Start();

            th1.Join();
            th2.Join();



            ////Kashif Bhai optimized Solution end start

            foreach (var _msg in chat.Children<JObject>())
            {

                foreach (var prop in _msg.Properties())
                {


                    // pro.name is user id.
                    var userDetail = users.FirstOrDefault(x => x.employeeId == Convert.ToInt16(prop.Name));

                    var msgs = _msg.SelectToken(prop.Name).Children();
                    //foreach (var msg3 in msgs)
                    //{
                    var propMsgID = prop.Children();


                    var arr = prop.ToArray().Last();


                    if (string.IsNullOrEmpty(arr.ToString()))
                        return;
                    else
                    {
                        var msg6 = arr.Children();
                        One2OneMessage msgObj = null;
                        try
                        {
                            var temp = msg6.Last();
                            msgObj = Newtonsoft.Json.JsonConvert.DeserializeObject<One2OneMessage>(temp.ToString());

                        }
                        catch
                        {
                            var temp = msg6.Last().Children().First();
                            msgObj = Newtonsoft.Json.JsonConvert.DeserializeObject<One2OneMessage>(temp.ToString());
                        }

                        //var msgsArray = JArray.Parse(msgsString.ToString());

                        //recentData._reciever = m.receiver;
                        //if (msgojb==Convert.ToInt16(prop.Name))

                    }

                    //}
                }

            }
        }
        public static void getGroupChat()
        {
            ChatManager cm = new ChatManager();
            var chat = cm.getGroupMsg(1, true);
        }
        public static void pushGroupMsg()
        {
            ChatManager cm = new ChatManager();
            var group = cm.GetGroup(1, true);

            GroupMessage message = new GroupMessage()
            {
                attachPath = "NA",
                MessageBody = "this group message test from kashif",
                msgId = 0,
                sender = 69,
                //readers = group.users.ConvertAll(x=> new reader { userName = x.name, userId=x.employeeId, receipt=readReceipt.Sent, timestamp= DateTime.Now })
            };

            cm.pushGroupMsg(message, group, true);
        }
        public static void createGroup(string groupName)
        {
            ChatManager cm = new ChatManager();

            var repo = new UsersRepo().getAllActiveUsers();
            List<ChatUser> users = new List<ChatUser>();
            foreach (var user in repo)
            {
                ChatUser _user = new ChatUser()
                {
                    employeeId = user.employeeId,
                    //name=user.employee.person.FName,
                };
                users.Add(_user);
            }
            ChatGroup group = new ChatGroup()
            {
                groupName = groupName,
                id = 1,
                users = users
            };

            cm.CreateNewGroup(group, true);
            Console.Write("Group Createed successfully");
        }
        public static void getaAllContacts()
        {
            ChatManager cm = new ChatManager();
            var contacts = cm.GetAllContacts();
            foreach (var contact in contacts)
            {
                Console.Write(contact.ToString() + "\n");
            }
            Console.Write(contacts.Count);

        }


        public static void loadAll(int userId)
        {
            ChatManager cm = new ChatManager();
            var res = cm.getOne2OneMsg(8);

            foreach (var msg in res)
            {
                Console.Write(msg);
            }
            //Console.Write(res.ToString());
            //foreach (var msg in res.Children<JObject>())
            //{
            //    foreach (JProperty prop in msg.Properties())
            //    {
            //        Console.WriteLine(prop.Name);
            //    }
            //}
        }
        public static void pushOne2One()
        {
            ChatManager cm = new ChatManager();
            One2OneMessage message = new One2OneMessage()
            {
                attachPath = "-",
                messageBody = "hello 3 how are you",
                readReceipt = readReceipt.Delivered,
                receiver = 5,
                sender = 69,
                msgId = 0,
                seenStamp = DateTime.Now
            };

            cm.pushOne2OneMsg(message);
        }
        public static void pushContacts()
        {
            ChatManager cm = new ChatManager();
            cm.syncContact();
            Console.Write("done");
        }
        public static void updateContact(string username)
        {
            ChatManager cm = new ChatManager();
            ChatContact contact = new ChatContact()
            {
                deviceType = DeviceType.Android,
                lastSeen = DateTime.Now,
                FirstName = "Kashif",
                LastName = "Ayub",
                password = "password",
                status = UserStatus.Active,
                userName = username
            };

            cm.updateContact(username, contact);

        }



        //            Config config = new Config();

        //            {
        //                Console.Write("\nNo DB found, creating new");
        //                var context = new DBContextERP();
        //                Console.Write("\ncreated new DB");
        //                context.Dispose();
        //            }

        //            CompanyRepo repo = new CompanyRepo();


        //            // --------------------- Direct Functions testing --------------------------------
        //            addUser();
        //            showUsers();

        //            Console.Write("\nPress any key to exit");
        //            Console.ReadKey();
        //        }

        //        private static void addUser()
        //        {
        //            DBContextERP cont = new DBContextERP();
        //            User user = new User();
        //            user.employee = cont.Employees.FirstOrDefault(x => x.EmpId == 3);
        //            user.userName = "UserName";
        //            user.password = "123";
        //            cont.Users.Add(user);
        //            cont.SaveChanges();
        //            Console.Write("\nUserAccount Generate");

        //        }
        private static void showUsers()
        {
            DBContextERP cont = new DBContextERP();
            List<User> users = new List<User>();
            users = cont.Users.ToList();

            foreach (User user in users)
            {
                Console.Write("\n UserName:" + user.userName);

                //Console.Write("\n Employee ID:" + user.employeeId);
            }

            Console.Write("\nUserAccounts completed");

        }
        //        private static void updateEmp()
        //        {
        //            EmployeeRepo employeeRepo = new EmployeeRepo();
        //            Employee emp = employeeRepo.GetEmployee(3);
        //            emp.person.FName = "Updated Forsst Name";

        //            emp.person.LName="Updated Last Name";
        //            employeeRepo.updateEmployee(emp);


        //        }

        //        private static void showEmp()
        //        {
        //            EmployeeRepo employeeRepo = new EmployeeRepo();
        //            Employee emp = employeeRepo.GetEmployee(3);
        //            Console.Write("\n" + emp.person.FName + " "+ emp.person.LName);
        //            Console.Write("\n" + emp.person.FatherName);
        //            Console.Write("\n" + emp.person.DOB);
        //            Console.Write("\n" + emp.person.CNIC);
        //            Console.Write("\n" + emp.BasicPay);
        //            Console.Write("\n" + emp.Desig);
        //            Console.Write("\n" + emp.Disability);
        //            Console.Write("\n" + emp.EmpId);
        //            Console.Write("\n" + emp.JoinDate);
        //            foreach (Company add in emp.Companies)
        //            {
        //                Console.Write("\n" + add.CompanyName);
        //                Console.Write("\n" + add.contact);
        //                Console.Write("\n" + add.BizType);
        //            }

        //        }
        //        private static void addEmp()
        //        {
        //            DBContextERP cont = new DBContextERP();
        //            EmployeeRepo repo = new EmployeeRepo();
        //            CompanyRepo compRepo = new CompanyRepo();
        //            DepartmentRepo deptRepo = new DepartmentRepo();
        //            Employee emp = new Employee();
        //            emp.person = new Person()
        //            {
        //                FName = "kashif",
        //                LName = "Ayub",
        //                CNIC = "38201-2828282-2",
        //                DOB = Convert.ToDateTime("03-03-1990"),
        //                FatherName = "Father Name",
        //                NextKin = "NA"
        //            };
        //            emp.address = new Address()
        //            {
        //                Country = "PK",
        //                Line1 = "Line 1",
        //                Line2 = "Line 2",
        //                State = "State",
        //                Zip = 123
        //            };
        //            emp.BasicPay = 1500;
        //            List<Company> comp = new List<Company>();
        //            comp.Add(cont.Companies.FirstOrDefault());

        //            emp.Companies = comp;
        //            new List<Company>()
        //                {
        //                    compRepo.GetCompany(1)
        //            };
        //            emp.contact = new Contact()
        //            {
        //                ContactNo = "030303030303",
        //                Email = "email@gmail.com",
        //                Fax = "0303030303",
        //                SMLink1 = "NA",
        //                SMLink2 = "NA",
        //                SMLink3 = "NA",
        //                Website = "www.web.com"
        //            };

        //            emp.Disability = false;
        //            emp.JoinDate = DateTime.Today;
        //            emp.MaritalStatus = "Single";
        //            emp.Status = ERP_BL.Enums.EmployeeStatus.Active;
        //            repo.addEmployee(emp);
        //            Console.Write("\nEmployee Added\n");
        //        }
        //        private static void addDepartment()
        //        {
        //            DepartmentRepo cont = new DepartmentRepo();
        //            cont.addDepartment(new Department()
        //            {
        //                Abbrivation = "FNC",
        //                Code = "FBC001",
        //                DeptName = "Finance ISB",
        //                Timestamp = DateTime.Today,
        //                //sections = new List<Section>()
        //                //{
        //                //    new Section()
        //                //    {
        //                //        SectionName="UPPER", Timestamp=DateTime.Today
        //                //    },
        //                //    new Section()
        //                //    {
        //                //        SectionName="LOWER", Timestamp=DateTime.Today
        //                //    }
        //                //}
        //            });
        //        }

        //        private static void AssignDept()
        //        {
        //            CompanyRepo cont = new CompanyRepo();
        //            Company compToUpdate = new Company();
        //            compToUpdate = cont.GetCompany(8);

        //            DepartmentRepo deptCont = new DepartmentRepo();

        //            List<Department> depts = new List<Department>();
        //            depts = deptCont.GetDepartments(); // get all departmetnts

        //            foreach (Department dept in depts)
        //                if (!compToUpdate.departments.Contains(dept))
        //                {
        //                    compToUpdate.departments.Add(dept);
        //                }
        //            cont.updateCompany(compToUpdate);

        //            Console.Write("\n Departmet updated \n");
        //        }



        //        //private static void addComp()
        //        //{

        //        //    Company company = new Company()
        //        //    {
        //        //        CompanyName = "SFE",
        //        //        BizType = ERP_BL.Enums.BizTypes.NGO,
        //        //        EmployeerNo = "Employees No",
        //        //        IndustryType = ERP_BL.Enums.IndustryTypes.Construction,
        //        //        address = new Address()
        //        //        {
        //        //            Line1 = "Line 1",
        //        //            Line2 = "Line 2",
        //        //            State = "Punjab",
        //        //            Zip = 12211,
        //        //            Country = "Pakistan"
        //        //        }
        //        //        ,
        //        //        contact= new Contact()
        //        //        {
        //        //            ContactNo="033242424212",
        //        //            Fax="FAX",
        //        //            Email="email@doamin.com",
        //        //            SMLink1="Facebook.com",
        //        //            SMLink2="twitter.com",SMLink3="LinkedIn.com"
        //        //        }
        //        //    };


        //            CompanyRepo repo = new CompanyRepo();
        //            repo.addCompany(company);

        //            Console.Write("\n  new company added");
        //        }

        //        private static void showCompanies()
        //        {
        //            CompanyRepo cont = new CompanyRepo();
        //            List<Company> comps = cont.GetCompanies();
        //            foreach (Company comp in comps)
        //            {
        //                Console.Write(comp.Id + "\n");
        //                Console.Write(comp.CompanyName + "\n");
        //                Console.Write(comp.address.Line1 + "\n");
        //                Console.Write(comp.address.Line2 + "\n");
        //                Console.Write(comp.address.State + "\n");
        //                Console.Write(comp.address.Zip + "\n");
        //                Console.Write(comp.address.Country + "\n");

        //                Console.Write(comp.BizType + "\n");
        //                Console.Write(comp.contact + "\n");
        //                Console.Write(comp.EmployeerNo + "\n");
        //                Console.Write(comp.contact.ContactNo + "\n");
        //                Console.Write(comp.contact.Email + "\n");
        //                Console.Write(comp.contact.Fax + "\n");
        //                Console.Write(comp.contact.SMLink1 + "\n");

        //                foreach(Department dept in comp.departments)
        //                {
        //                    Console.Write("\n DeptName= " + dept.DeptName + "\n");
        //                }
        //                Console.Write("\n----------------------------------\n");
        //            }

        //        }

        //        private static void updateComp(int CompID)
        //        {
        //            CompanyRepo repo = new CompanyRepo();
        //            Company comp= repo.GetCompany(CompID);
        //            comp.CompanyName = "Updated Company Name";
        //            repo.updateCompany(comp);

        //        }

        //        private static void updateCompAdd(int AddressID)
        //        {
        //            CompanyRepo repo = new CompanyRepo();
        //            Address add = repo.GetAddresses(AddressID);
        //            add.Line1= "Updated Address Line 1";
        //            repo.updateCompany(add);
        //        }

        //        private static void updateCompContact(int contactID)
        //        {
        //            CompanyRepo repo = new CompanyRepo();
        //            Contact Cont = repo.GetContacts(contactID);
        //            Cont.ContactNo = "Cntact number updated";
        //            repo.updateCompany(Cont);
        //        }
        //        private static void addEmployee()
        //        {
        //            // establish new conneciton
        //            Employee emp = new Employee();

        //            emp.address.Line1 = "Address";
        //            emp.address.Country = "Pakistan";
        //            emp.address.State = "Punjab";
        //            emp.address.Zip = 44000;

        //            emp.person.FName = "FIrst Name";
        //            emp.person.LName = "New";
        //            emp.person.CNIC = "0000-0000000-0";
        //            emp.person.DOB = new DateTime(2018, 06, 24);
        //            emp.JoinDate = new DateTime(2018, 09, 01);
        //            emp.MaritalStatus = "Married";
        //            emp.Disability = false;
        //            var cont = new DBContextERP();
        //            cont.Employees.Add(emp);
        //            cont.SaveChanges();

        //        }

        //        private static void showDepartments()
        //        {
        //            DepartmentRepo cont = new DepartmentRepo();
        //            List<Department> departments= cont.GetDepartments();
        //            foreach (Department dept in departments)
        //            {
        //                Console.Write(dept.Id + "\n");
        //                Console.Write(dept.DeptName + "\n");

        //               Console.Write(dept.Abbrivation + "\n");
        //                Console.Write(dept.Code + "\n");
        //                //Console.Write(dept.sections.Count() + "\n");
        //                //foreach (var sec in dept.sections)
        //                //{

        //                //    Console.Write(sec.SectionName +"\n");
        //                //    Console.Write(sec.Timestamp + "\n");
        //                //    Console.Write(sec.Id + "\n");

        //                //}
        //                Console.Write("\n----------------------------------\n");
        //            }

        //        }

        //        private static void addpUser(List<string> DBList, Config config)
        //        {
        //            if (config.connectDB(DBList[0]) != null && config.connectDB(DBList[0]).Count() > 0)
        //                foreach (pUser user in config.connectDB(DBList[0]))
        //                    Console.Write("\nPower User: " + user.userName + "\n Password: " + user.Password);
        //            else
        //            {
        //                var context = new DBContextERP();
        //                var user = new pUser
        //                {
        //                    Password = "123456"
        //                };

        //                context.pUsers.Add(user);
        //                context.SaveChanges();
        //                Console.Write("\nNew power user added");
        //            }


    }
}

