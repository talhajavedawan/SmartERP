using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class UsersRepo
    {
        DBContextERP context = new DBContextERP();

        public UsersRepo()
        {
            SystemLog.LogInfo(this.GetType(), "Instance Created");

        }
        /// <summary>
        /// Add new Role 
        /// </summary>
        /// <param name="roleFiled">Role  Object</param>
        public void AddRoleField(RoleField roleField) 
        {
            var fieldToDelete =  context.roleFields.FirstOrDefault(x => x.FieldName == roleField.FieldName);
            if(fieldToDelete!=null)
             context.roleFields.Remove(fieldToDelete);
            context.roleFields.Add(roleField);
            context.SaveChanges();
        }


        /// <summary>
        /// Add new Role 
        /// </summary>
        /// <param name="role">Role  Object</param>
        public void Add(Role role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added Role with Name= " + role.Name + " Id= " + role.Id);

        }


        /// <summary>
        /// return all Role Company list.
        /// </summary>
        /// <returns></returns>
        public List<Role> getAll()
        {
            List<Role> roles = new List<Role>();
            try
            { 
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Roles");

                var _roles = context.Roles.Where(x => x.isActive == true).ToList();
            if (_roles != null && _roles.Count()>0)
                roles = _roles.ToList();
            }
            catch (Exception ex)
            { }
            return roles;
        }
        public List<Role> getAllinActive()
        {
            List<Role> roles = new List<Role>();
            try
            {
                SystemLog.LogInfo(this.GetType(), "Retrived List of All Roles");

                var _roles = context.Roles.Where(x => x.isActive == false).ToList();
                if (_roles != null && _roles.Count() > 0)
                    roles = _roles.ToList();
            }
            catch (Exception ex)
            { }
            return roles;
        }

        /// <summary>
        /// get Role matching to ID
        /// </summary>
        /// <param name="roleID">Role ID</param>
        /// <returns></returns>
        public Role get(int roleID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrive Role with  Id= " + roleID);
            return context.Roles
                .Include("Users")
                .Include("Permissions")
                .FirstOrDefault(x => x.Id == roleID);
        }



        /// <summary>
        /// update Role  object details
        /// </summary>
        /// <param name="role">Role Object</param>
        public void Update(Role role)
        {
            Role roleToUpdate = context.Roles.FirstOrDefault(x => x.Id == role.Id);

            //roleToUpdate.Name = role.Name;
            //roleToUpdate.Description = role.Description;
            //roleToUpdate.isActive = role.isActive;
            //roleToUpdate.LastModified = DateTime.Now;

            //var newPermissions = role.Permissions.Except(roleToUpdate.Permissions);
            //var removedPermissions = roleToUpdate.Permissions.Except(role.Permissions);

            //if (newPermissions != null)
            //    roleToUpdate.Permissions.AddRange(newPermissions);

            //foreach (var item in removedPermissions)
            //{
            //    roleToUpdate.Permissions.Remove(item);
            //}

            roleToUpdate = role;
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Role with Name= " + role.Name + " Id= " + role.Id);
        }
        /// <summary>
        /// update Role  object details
        /// </summary>
        /// <param name="role">Role Object</param>
        public void Update(int roleId, Permission permission)
        {
            Role roleToUpdate = context.Roles
                .Include("Users")
                .Include("Permissions")
                .FirstOrDefault(x => x.Id == roleId);
            roleToUpdate.Permissions.Add(permission);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Role with Name= " + roleToUpdate.Name + " Id= " + roleToUpdate.Id);
        }
        /// <summary>
        /// Get List of All Permissions
        /// </summary>
        public List<RoleField> getAllRoleField()
        {       
            List<RoleField> roleFields = new List<RoleField>();
            return roleFields = context.roleFields.ToList();
        }


        /// <summary>
        /// Get List of All Permissions
        /// </summary>
        public List<Permission> getAllPermissions()
        {
            List<Permission> permissions = new List<Permission>();
            //using (DBContextERP contextERP = new DBContextERP ()) {

         
            //    permissions = contextERP.Permissions.Include("Roles").ToList();
            //}
           
            permissions = context.Permissions.Include("Roles").ToList();
            return permissions;
        }
        /// <summary>
        /// Get List of All Permissions
        /// </summary>
        /// 
        public Permission getPermission(int permissionId)
        {
            return context.Permissions.Include("Roles").FirstOrDefault(x => x.Id == permissionId);

        }
        /// <summary>
        /// Add new viewInfo 
        /// </summary>
        /// <param name="viewInfo">viewInfo  Object</param>
        public void Add(ViewInfo viewInfo)
        {
            context.viewInfos.Add(viewInfo);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added viewInfo for userName= " + viewInfo.User.userName + " Id= " + viewInfo.Id);

        }
        /// <summary>
        /// Add new viewInfo for transaction
        /// </summary>
        /// <param name="viewInfo">viewInfo  Object for Inquiry</param>
        public void Add(Enums.TransactionInfo Info, int Transactionid, int Transactiontype, string comment)
        {
            ViewInfo viewInfo = new ViewInfo();
            if (SystemLog.CurrentUserId != 0)
            {
                viewInfo.UserId = SystemLog.CurrentUserId;
                viewInfo.TransactionId = Transactionid;
                viewInfo.TransactionType = Transactiontype;
                viewInfo.Timestamp = System.DateTime.Now;
                viewInfo.Info = Info.ToString();
                viewInfo.Comment = comment;
                context.viewInfos.Add(viewInfo);

                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Added viewInfo  Id= " + viewInfo.Id);
            }
        }
        /// <summary>
        /// Get List of ViewersInfo for currenttransaction
        /// </summary>
        /// 
        public List<ViewInfo> getViwerInfo(int TransactionId, int transactiontype)
        {
            return context.viewInfos.Include("User").Include("User.employee.contact").Include("User.employee.person").Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();

        }
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        public List<User> getAllusers()
        {
            return context.Users.Include("employee.address")
                //.Include("employee.contact")
                .Include("employee.Desig")
                //.Include("employee.empFunction")

                //.Include("employee.Companies")
                //.Include("employee.departments.customers")
                //.Include("employee.Companies.departments")
                //.Include("employee.Companies.departments.parentDepartment")
                .Include("employee.departments")
                //.Include("employee.departments.parentDepartment")
                //.Include("userSettings")
                .Include("Roles").ToList();

        }

        /// <summary>
        /// Get all active users. 
        /// </summary>
        /// <returns>list of User which are active</returns>
        public List<User> getAllActiveUsers()
        {
            return context.Users.Include("employee.address")
                .Include("employee.contact")
                .Include("employee.Desig")
                //.Include("employee.empFunction")

                .Include("employee.Companies")
                .Include("employee.departments.customers")
                .Include("employee.Companies.departments")
                .Include("employee.Companies.departments.parentDepartment")
                .Include("employee.departments")
                .Include("employee.departments.parentDepartment")
                .Include("userSettings").Include("Roles")
                .Where(x => x.isActive == true).ToList();

        }

        /// <summary>
        /// Get all active users. 
        /// </summary>
        /// <returns>list of User which are active</returns>
        public List<User> getAllActiveUsersForMemo()
        {
            return context.Users
                .Where(x => x.isActive == true).ToList();

        }
        public List<User> getAllActiveUsersForSharedReports()
        {
            return context.Users
                .Where(x => x.isActive == true).ToList();

        }
        public List<User> getAllActiveUsersForChat()
        {
            return context.Users
                //.Include("employee.departments")
                //.Include("employee.departments.parentDepartment")
                //.Include("userSettings").Include("Roles")
                .Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public List<User> getusersByDepartment(int departmentID)
        {
            return context.Users
                .Where(x => x.employee.departments.FirstOrDefault(y => y.Id == departmentID) != null && x.isActive != false).ToList();

        }

        //public getusersByDepartment(int departmentID)
        //{

        //    return context.Users
        //        .Where(x => x.employee.departments.FirstOrDefault(y => y.Id == departmentID) != null && x.isActive != false).ToList();

        //}

        /// <summary>
        /// Get All Users by Department and Company Id
        /// </summary>
        /// <param name="departmentID">Department Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns></returns>
        public List<User> getusersByCompanyDepartment(int departmentID, int companyId)
        {

            var u = context.Users
                .Where(x => x.employee.departments.FirstOrDefault(y => y.Id == departmentID) != null && x.employee.Companies.FirstOrDefault(z => companyId == z.Id) != null && x.isActive != false).ToList();
            return u;

        }
        /// <summary>
        /// Get All Users from list of departments
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public List<User> getusersByDepartmentIdsList(List<int> departmentIDs, List<int> companyIDs)
        {

            return context.Users
                .Where(x => x.employee.departments.FirstOrDefault(y => departmentIDs.Contains(y.Id)) != null && x.employee.Companies.FirstOrDefault(y => companyIDs.Contains(y.Id)) != null && x.isActive != false).ToList();

        }
        /// <summary>
        /// get user based on employeeid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public User getuser(int userid)
        {
            return context.Users.Include("employee.address")
                .Include("employee.contact")
                .Include("employee.Desig")
                .Include("employee.Companies")

                // .Include("employee.empFunction")

                .Include("employee.departments.customers.company")
                .Include("employee.departments.customers")
                .Include("employee.Companies.departments")
                .Include("employee.Companies.departments.parentDepartment")
                .Include("employee.departments")
                .Include("employee.departments.parentDepartment")
                .Include("userSettings").Include("Roles").Include("Roles.Permissions").FirstOrDefault(x => x.id == userid);

        }

        /// <summary>
        /// get user based on employeeid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public User GetUserforPerformanceReview(int userid)
        {
            return context.Users.FirstOrDefault(x => x.id == userid);

        }

        public User getOnlineUser(int userId)
        {
            return context.Users.Include("employee.person").FirstOrDefault(x =>x.id == userId);
        }
        public List<User> getOnlineUserList()
        {
            return context.Users
               .Include("employee.person")
               .Where(x => x.isLoggedIn == true && x.isActive == true)
               .ToList();
        }
        /// <summary>
        /// get user based on employeeid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public User getuserForTenant(int userid)
        {
            return context.Users
                .Include("employee.Companies.departments")
                .FirstOrDefault(x => x.id == userid);

        }
        /// <summary>
        /// get user based on employeeid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public User getuserByEmpId(int empId)
        {
            return context.Users.Include("employee.address")
                .Include("employee.contact")
                .Include("employee.Desig")
                .Include("employee.Companies")
                // .Include("employee.empFunction")

                .Include("employee.departments.customers")
                .Include("employee.Companies.departments")
                .Include("employee.Companies.departments.parentDepartment")
                .Include("employee.departments")
                .Include("employee.departments.parentDepartment")
                .Include("userSettings").Include("Roles").Include("Roles.Permissions").FirstOrDefault(x => x.employeeId == empId);

        }

        public List<User> getuserListByEmpId(int empId)
        {
            return context.Users.Include("employee.address")
                .Include("employee.contact")
                .Include("employee.Desig")
                .Include("employee.Companies")
                // .Include("employee.empFunction")

                .Include("employee.departments.customers")
                .Include("employee.Companies.departments")
                .Include("employee.Companies.departments.parentDepartment")
                .Include("employee.departments")
                .Include("employee.departments.parentDepartment")
                .Include("userSettings")
                .Include("Roles")
                .Include("Roles.Permissions")
                .Where(x => x.employeeId == empId)
                .ToList()
                //.FirstOrDefault(x => x.employeeId == empId)
                ;

        }
        /// <summary>
        /// get user based by username and Password
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User getuser(string username, string Password)
        {
            var user = context.Users
               //.Include("employee.PassportNo")
               .Include("employee.address")
               .Include("employee.contact")
               .Include("employee.Desig")
               .Include("employee.person")
               // .Include("employee.empFunction")
               //.Include("employee.address2")
               //.Include("employee.Qualifications")
               //.Include("employee.Companies")
               ////.Include("employee.departments.customers")
               //.Include("employee.Companies.departments")
               //.Include("employee.Companies.departments.parentDepartment")
               //.Include("employee.departments")
               //.Include("employee.departments.parentDepartment")
               //.Include("userSettings")
               //.Include("Roles")
               //.Include("Roles.Permissions")

               .FirstOrDefault(x => x.userName.Equals(username) && x.password == Password);



            //if (user == null)
            //    System.Threading.Thread.Sleep(1000);

            //List<UserSettings> userSettings;
            //    System.Threading.Thread th = new System.Threading.Thread(() =>
            //    {

            //        var _userSettings = context.userSettings.Where(x => x.userId == user.id);
            //        if (_userSettings != null)
            //            user.userSettings = _userSettings.ToList();
            //    });
            //    th.Start();

            //    System.Threading.Thread thRole = new System.Threading.Thread(() =>
            //    {
            //        var u = context.Users.Include("Roles").Include("Roles.Permissions").FirstOrDefault(x => x.id == user.id);
            //        //user.Roles = u.Roles;
            //    });
            //    thRole.Start();

            //    thRole.Join();
            //user.userSettings = userSettings;

            return user;
        }
        /// <summary>
        /// Get user based on user name password and machine key
        /// </summary>
        /// <param name="username"></param>
        /// <param name="Password"></param>
        /// <param name="machineKey"></param>
        /// <returns></returns>
        /// 
        public List<UserSettings> getUserSettings(int uId)
        {
            List<UserSettings> _userSettings = new List<UserSettings>();
            _userSettings = context.userSettings.Where(x => x.userId == uId).ToList();
            return _userSettings;
        }
        public List<Role> getUserRoles(int uId)
        {
         
            List<Role> roles = new List<Role>();
            using (DBContextERP _context = new DBContextERP())
            {
                var u = _context.Users.Include("Roles").Include("Roles.Permissions").FirstOrDefault(x => x.id == uId);
                if (u != null)
                    roles = u.Roles;
            }
            return roles;
        }
        public User getuser(string username, string Password, string machineKey)
        {
            return context.Users.Include("employee.address")
               .Include("employee.contact")
               .Include("employee.Desig")
                              .Include("employee.address2")
                                              .Include("employee.empFunction")

              //.Include("employee.Companies")
              ////.Include("employee.departments.customers")
              //.Include("employee.Companies.departments")
              //.Include("employee.Companies.departments.parentDepartment")
              .Include("employee.Qualifications")

               .Include("employee.departments")
               .Include("employee.departments.parentDepartment")
               .Include("userSettings")
               .Include("Roles")
               .Include("Roles.Permissions")
               .FirstOrDefault(x => x.userName == username && x.password == Password && x.machineKey == machineKey);

        }
        /// <summary>
        /// get Power user based on username and Password
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public pUser getPowerUser(string username, string Password)
        {
            foreach (pUser user in context.pUsers.ToList())

                if (user.userName == username && user.Password == Password)
                    return user;
            return null;
        }

        /// <summary>
        /// get Power user based on username and Password
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public pUser getPowerUserByUsername(string username)
        {
            foreach (pUser user in context.pUsers.ToList())
                if (user.userName == username)
                    return user;
            return null;
        }


        /// <summary>
        /// get user based by username and Password
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User getuserbyUsername(string username)
        {
            var user = context.Users
               .FirstOrDefault(x => x.userName == username);




            //if (user == null)
            //    System.Threading.Thread.Sleep(1000);



            //List<UserSettings> userSettings;
            //    System.Threading.Thread th = new System.Threading.Thread(() =>
            //    {



            //        var _userSettings = context.userSettings.Where(x => x.userId == user.id);
            //        if (_userSettings != null)
            //            user.userSettings = _userSettings.ToList();
            //    });
            //    th.Start();



            //    System.Threading.Thread thRole = new System.Threading.Thread(() =>
            //    {
            //        var u = context.Users.Include("Roles").Include("Roles.Permissions").FirstOrDefault(x => x.id == user.id);
            //        //user.Roles = u.Roles;
            //    });
            //    thRole.Start();



            //    thRole.Join();
            //user.userSettings = userSettings;



            return user;
        }

        /// <summary>
        /// Update Power user based on username and Password
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool UpdatePowerUserPassword(string username, string Password, string newPassword)
        {
            foreach (pUser user in context.pUsers.ToList())

                if (user.userName == username && user.Password == Password)
                {
                    user.Password = newPassword;
                    context.SaveChanges();
                    return true;
                }
            return false;
        }
        /// <summary>
        /// add user to db
        /// </summary>
        /// <param name="user"></param>
        public void Adduser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
        /// <summary>
        /// update user to db
        /// </summary>
        /// <param name="user"></param>
        public void updateuser(User user)
        {
          
            var _user = context.Users.FirstOrDefault(x => x.id == user.id);
            _user = user;
            //if (_user != null)
            //{
            //    //_user.employeeId = user.employeeId;
            //    //_user.userName = user.userName;
            //    _user.password = user.password;
            //    _user.isActive = user.isActive;
            //    //_user.isLoggedIn = user.isLoggedIn;
            //    _user.isKeyApproved = user.isKeyApproved;
            //    _user.machineKey = user.machineKey;

            //    context.SaveChanges();
            //}
            context.SaveChanges();
        }
        public User GetbyEmpId(int EMPId)
        {

            return context.Users.FirstOrDefault(x => x.employeeId == EMPId);
        }
        public void updateTaskGroupUser(User user)
        {
            var _user = context.Users.FirstOrDefault(x => x.id == user.id);
            if(_user != null)
            {
                _user.isBlink = false;
                context.SaveChanges();
            }
            
        }
        public void updateTaskGroupUserId(int userId)
        {
            var _user = context.Users.FirstOrDefault(x => x.id == userId);
            _user.isBlink = false;
            context.SaveChanges();
        }
        public void updateLoginUser(int user, LoginUserDetails loginUserDetails)
        {
            User Userss = context.Users.FirstOrDefault(x => x.id == user);
            //Userss.LoginTime = DateTime.Now;
            Userss.isLoggedIn = true;
            if (Userss.loginUserDetails == null)
            {
                if(loginUserDetails != null)
                {
                    Userss.loginUserDetails = new List<LoginUserDetails>();
                    Userss.loginUserDetails.Add(loginUserDetails);
                }
            }
            else
            {
                Userss.loginUserDetails.Add(loginUserDetails);
            }          

                  
            context.Entry(Userss).Property("isLoggedIn").IsModified = true;
            context.SaveChanges();
        }
        public void updateLogoutUser(int user) 
        {
            var _user = context.Users.Include("loginUserDetails").FirstOrDefault(x => x.id == user);
           // _user.LoginTime = null;
            _user.isLoggedIn = false;
            if(_user.loginUserDetails != null && _user.loginUserDetails.Count > 0)
            {
                _user.loginUserDetails.Last().LogoutTime = DateTime.Now;
                _user.loginUserDetails.Last().crashingDetail = "Normal Logout";
            } 
                context.SaveChanges();
        }

        public void updateUserKey(User user, string key)
        {
            using (var _context = new DBContextERP())
            {
                var _user = _context.Users.FirstOrDefault(x => x.id == user.id);
                if (_user != null)
                {
                    _user.machineKey = key;
                    _context.SaveChanges();
                }
            }
               
        }
        public void updateRDCUserKey(User user, string key)
        {
            using (var _context = new DBContextERP())
            {
                var _user = _context.Users.FirstOrDefault(x => x.id == user.id);
                if (_user != null)
                {
                    _user.rdcMachineKey = key;
                    _context.SaveChanges();
                }
            }

        }
        public void seeddb()
        {
            IList<Permission> permissions = new List<Permission>();
            //Comapny Related
            permissions.Add(new Permission() { Id = 1, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Company Center", Description = "Allow User To Open Company Center" });
            permissions.Add(new Permission() { Id = 2, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Add New Comapny", Description = "Allow User To Open New Company" });
            permissions.Add(new Permission() { Id = 3, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Edit Company", Description = "Allow User To Edit Company Information" });
            permissions.Add(new Permission() { Id = 4, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Add New Department", Description = "Allow User To Open New Department" });
            permissions.Add(new Permission() { Id = 5, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Edit Department", Description = "Allow User To Edit Department Information" });
            permissions.Add(new Permission() { Id = 6, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "View InActive Companies", Description = "Allow User To View InActive Companies" });
            permissions.Add(new Permission() { Id = 7, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "View InActive Departments", Description = "Allow User To View InActive Departments" });
            permissions.Add(new Permission() { Id = 8, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Mark Company as  InActive", Description = "Allow User To Mark Company as InActive." });
            permissions.Add(new Permission() { Id = 9, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Mark Department as InActive", Description = "Allow User To Mark Department as InActive." });


            //Employee 
            permissions.Add(new Permission() { Id = 10, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Employee Center", Description = "Allow User To Open Employee Center" });
            permissions.Add(new Permission() { Id = 11, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 10, Name = "Add New Employee", Description = "Allow User To Add New Employee " });
            permissions.Add(new Permission() { Id = 12, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 10, Name = "Edit Employee", Description = "Allow User To Edit Employee Information" });
            permissions.Add(new Permission() { Id = 13, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 10, Name = "View InActive Employees", Description = "Allow User To View InActive Employees" });
            permissions.Add(new Permission() { Id = 14, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 10, Name = "Mark Employee as InActive", Description = "Allow User To Mark Employee as InActive." });

            //Customer
            permissions.Add(new Permission() { Id = 20, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Customer Center", Description = "Allow User To Open Customer Center" });
            permissions.Add(new Permission() { Id = 21, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Add New Customer", Description = "Allow User To Add New Customer" });
            permissions.Add(new Permission() { Id = 22, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Edit Customer", Description = "Allow User To Edit Customer" });
            permissions.Add(new Permission() { Id = 23, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "See Transactions", Description = "Allow User To View Transaction done From his Account" });
            permissions.Add(new Permission() { Id = 24, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "View InActive Customers", Description = "Allow User To View InActive Customers" });
            permissions.Add(new Permission() { Id = 25, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Mark Customer as InActive", Description = "Allow User To Mark Customer as InActive." });


            //Vendor
            permissions.Add(new Permission() { Id = 30, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Vendor Center", Description = "Allow User To Open Vendor Center" });
            permissions.Add(new Permission() { Id = 31, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Add New Vendor", Description = "Allow User To Add Vendor" });
            permissions.Add(new Permission() { Id = 32, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Edit Vendor", Description = "Allow User To Edit Vendor Information" });
            permissions.Add(new Permission() { Id = 33, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "View InActive Vendor", Description = "Allow User To View InActive Vendors" });
            permissions.Add(new Permission() { Id = 34, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Mark Vendor as InActive", Description = "Allow User To Mark Vendor as InActive." });
            permissions.Add(new Permission() { Id = 35, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "View InActive Vendor Payment Statuses", Description = "Allow User To View InActive Vendors" });
            permissions.Add(new Permission() { Id = 36, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Add Vendor Payment Status", Description = "Allow User To Add New Vendor Payment Status" });
            permissions.Add(new Permission() { Id = 37, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Edit Vendor Payment Status", Description = "Allow User To edit Vendor Payment Status" });


            //Principal
            permissions.Add(new Permission() { Id = 40, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Principal Center", Description = "Allow User To Open Principal Center" });
            permissions.Add(new Permission() { Id = 41, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "Add New Principal", Description = "Allow User To Add New Principal" });
            permissions.Add(new Permission() { Id = 42, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "Edit Principal", Description = "Allow User To Edit Principal" });
            permissions.Add(new Permission() { Id = 43, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "View InActive Principal", Description = "Allow User To View InActive Principals" });
            permissions.Add(new Permission() { Id = 44, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "Mark Prinicpal as InActive", Description = "Allow User To Mark Principal as InActive." });

            //procurment
            permissions.Add(new Permission() { Id = 50, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Procurment Panel", Description = "Allow User To Open Procurmant Panel" });
            permissions.Add(new Permission() { Id = 51, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Inquiries", Description = "Inquiries" });

            permissions.Add(new Permission() { Id = 52, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Add Inquiry", Description = "Allow User To Add New Inquiry" });
            permissions.Add(new Permission() { Id = 53, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Edit Inquiry", Description = "Allow User To Edit Inquiry" });
            permissions.Add(new Permission() { Id = 54, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View Inquiry Details", Description = "Allow User To View Inquiry Details" });
            permissions.Add(new Permission() { Id = 55, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "List Of Inquiries", Description = "Allow User To View List Of Inquiries" });
            permissions.Add(new Permission() { Id = 56, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Add Inquiry Status", Description = "Allow User to Add New Status for Inquiry" });
            permissions.Add(new Permission() { Id = 57, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Create Offer from Inquiry", Description = "Allow User To Create Offer from an Existing Inquiry" });
            permissions.Add(new Permission() { Id = 58, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Edit Inquiry Status", Description = "Allow User To Edit Inquiry Status" });
            permissions.Add(new Permission() { Id = 59, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View InActive Inquiry Statuses", Description = "Allow User To View InActive Inquiry Statuses" });
            permissions.Add(new Permission() { Id = 60, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Add Inquiry without Approval", Description = "Allow User To Add New Inquiry without Approval" });
            permissions.Add(new Permission() { Id = 61, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View InActive Inquiries", Description = "Allow User To View InActive Inquiries" });
            permissions.Add(new Permission() { Id = 62, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Close Inquiry without Approval", Description = "Allow User To Close Inquiry without Approval" });

            permissions.Add(new Permission() { Id = 301, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Offers", Description = "Offers" });

            permissions.Add(new Permission() { Id = 302, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add New Offer ", Description = "Allow User To Add New Offer" });
            permissions.Add(new Permission() { Id = 303, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit Offer", Description = "Allow User To Edit Offer Details" });
            permissions.Add(new Permission() { Id = 304, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Offer Details", Description = "Allow User To View Offer Details" });
            permissions.Add(new Permission() { Id = 305, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "List Of Offers", Description = "Allow User To view List of Offers" });
            permissions.Add(new Permission() { Id = 306, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add Offer Status", Description = "Allow User To Add new Status for Offer" });
            permissions.Add(new Permission() { Id = 307, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View InActive Offers", Description = "Allow User To View InActive Offers" });
            permissions.Add(new Permission() { Id = 308, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add Offer without Approval", Description = "Allow User To Add New Offer without Approval" });
            permissions.Add(new Permission() { Id = 309, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit Offer Status", Description = "Allow User To Edit Offer Status" });
            permissions.Add(new Permission() { Id = 310, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View InActive Offer Statuses", Description = "Allow User To View InActive Offer Statuses" });
            permissions.Add(new Permission() { Id = 311, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Create Sale Order from Offer", Description = "Allow User To Create Purchase Order from an existing Offer" });
            permissions.Add(new Permission() { Id = 312, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Close Offer without Approval", Description = "Allow User To Close Offer without Approval" });

            permissions.Add(new Permission() { Id = 331, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Sale Orders", Description = "Sale Orders" });
            permissions.Add(new Permission() { Id = 332, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order", Description = "Allow User To Add Sale Order" });
            permissions.Add(new Permission() { Id = 333, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order", Description = "Allow User To Edit Sale Order" });
            permissions.Add(new Permission() { Id = 334, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View Sale Order", Description = "Allow User To View Sale Order" });
            permissions.Add(new Permission() { Id = 335, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "List of Sale Orders", Description = "Allow User To View List of Sale Order" });
            permissions.Add(new Permission() { Id = 336, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order Status", Description = "Allow User To Add new status for Sale Order" });
            permissions.Add(new Permission() { Id = 337, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order without Approval", Description = "Allow User To Add New Sale Order without Approval" });
            permissions.Add(new Permission() { Id = 338, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View InActive Sale Orders", Description = "Allow User To View InActive Sale Orders" });
            permissions.Add(new Permission() { Id = 339, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Close Sale Order", Description = "Allow User To Close Sale Order" });
            permissions.Add(new Permission() { Id = 340, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order Status", Description = "Allow User To Edit Sale Order Status" });
            permissions.Add(new Permission() { Id = 341, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View InActive Sale Order Statuses", Description = "Allow User To View InActive Sale Order Statuses" });
            permissions.Add(new Permission() { Id = 342, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Create Po from Sale Order", Description = "Allow User To Create Purchase Order from an existing SaleOrder" });
            permissions.Add(new Permission() { Id = 343, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Close Sale Order without Approval", Description = "Allow User To Close Sale Order without Approval" });
            permissions.Add(new Permission() { Id = 344, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Unapproved Sale Order", Description = "Allow User To Edit Unapproved Sale Order without Approval" });
            permissions.Add(new Permission() { Id = 344, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View Market Exchange Rate in Sale Order", Description = "Allow User To View Market Exchange Rate in Sale Order" });


            permissions.Add(new Permission() { Id = 361, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Purchase Orders", Description = "Purchase Orders" });

            permissions.Add(new Permission() { Id = 362, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Add Purchase Order", Description = "Allow User To Add Purchase Order" });
            permissions.Add(new Permission() { Id = 363, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Edit Purchase Order", Description = "Allow User To Edit Purchase Order" });
            permissions.Add(new Permission() { Id = 364, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "View Purchase Order", Description = "Allow User To View Purchase Order" });
            permissions.Add(new Permission() { Id = 365, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "List of Purchase Orders", Description = "Allow User To View List of Purchase Order" });
            permissions.Add(new Permission() { Id = 366, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Add Purchase Order Status", Description = "Allow User To Add new status for Purchase Order" });
            permissions.Add(new Permission() { Id = 367, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Close Inquiry", Description = "Allow User To Close Inquiry" });
            permissions.Add(new Permission() { Id = 368, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Close Offer", Description = "Allow User To Close Offer" });
            permissions.Add(new Permission() { Id = 369, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Close Purchase Order", Description = "Allow User To Close Purchase Order" });
            permissions.Add(new Permission() { Id = 370, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "View InActive Purchase Orders", Description = "Allow User To View InActive Purchase Orders" });
            permissions.Add(new Permission() { Id = 370, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Add Purchase Order without Approval", Description = "Allow User To Add New Purchase Order without Approval" });
            permissions.Add(new Permission() { Id = 371, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Edit Purchase Order Status", Description = "Allow User To Edit Purchase Order Status" });
            permissions.Add(new Permission() { Id = 372, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "View InActive Purchase Order Statuses", Description = "Allow User To View InActive Purchase Order Statuses" });
            permissions.Add(new Permission() { Id = 373, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Close Purchase Order without Approval", Description = "Allow User To Close Purchase Order without Approval" });




            //Reports
            permissions.Add(new Permission() { Id = 80, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Reports", Description = "Allow User To View Reports Menu" });
            permissions.Add(new Permission() { Id = 81, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "View Memorized Reports", Description = "Allow User To View Memorized Reports" });
            permissions.Add(new Permission() { Id = 82, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Report Center", Description = "Allow User To create new reports in Reports Center " });
            permissions.Add(new Permission() { Id = 83, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "User Procurment Reports", Description = "Allow User To Edit template for User Procurment reports" });
            //Lists
            permissions.Add(new Permission() { Id = 90, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Lists", Description = "Allow User To View Lists" });
            permissions.Add(new Permission() { Id = 91, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Currencies", Description = "Allow User To View List of Currencies" });
            permissions.Add(new Permission() { Id = 92, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Items", Description = "Allow User To View List of Items" });
            permissions.Add(new Permission() { Id = 93, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Industry Types", Description = "Allow User To View List of Industry Types" });
            permissions.Add(new Permission() { Id = 94, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Payment Terms", Description = "Allow User To View List of Payment Terms" });
            permissions.Add(new Permission() { Id = 95, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Incoterms", Description = "Allow User To View List of Incoterms" });
            permissions.Add(new Permission() { Id = 96, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Procurment Statuses ", Description = "Allow User To View List of Procurment Statuses" });
            permissions.Add(new Permission() { Id = 97, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Inquiry Statuses", Description = "Allow User To View List of Inquiry Statuses" });
            permissions.Add(new Permission() { Id = 98, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Offer Statuses", Description = "Allow User To View List of Offer Statuses" });
            permissions.Add(new Permission() { Id = 99, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Purchase Order Statuses", Description = "Allow User To View List of Purchase Order Statuses" });

            //users
            permissions.Add(new Permission() { Id = 110, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Users", Description = "Allow User To view Users List" });

            permissions.Add(new Permission() { Id = 111, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Manage Users And Roles", Description = "Allow User To Manage Users And Roles" });
            permissions.Add(new Permission() { Id = 112, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Add New User", Description = "Allow User To Add New User" });
            permissions.Add(new Permission() { Id = 113, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Add New Role", Description = "Allow User To Add New Role" });
            permissions.Add(new Permission() { Id = 114, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Edit Role", Description = "Allow User To Edit Role" });
            permissions.Add(new Permission() { Id = 115, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Edit User", Description = "Allow User To Edit User" });
            //permissions.Add(new Permission() { Id = 116, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Mark Role as InActive", Description = "Allow User To Mark Role as InActive." });

            //Currencies
            permissions.Add(new Permission() { Id = 400, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Currencies", Description = "Allow User To Access Currencies" });

            permissions.Add(new Permission() { Id = 401, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Add Currency", Description = "Allow User To add Currency" });
            permissions.Add(new Permission() { Id = 402, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Edit Currency", Description = "Allow User To add Currency" });
            permissions.Add(new Permission() { Id = 403, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Add Exchange Rates", Description = "Allow User To add Exchange Rates" });
            permissions.Add(new Permission() { Id = 404, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Edit Exchange Rates", Description = "Allow User To Edit Exchange Rates" });
            permissions.Add(new Permission() { Id = 405, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Add Sales Exchange Rates", Description = "Allow User To Add Sales Exchange Rates" });
            permissions.Add(new Permission() { Id = 406, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Edit Sales Exchange Rates", Description = "Allow User To Edit Sales Exchange Rates" });

            if (context.Permissions.Count() != permissions.Count() + 1)
                if (context.Permissions.Count() == 0)
                {
                    context.Permissions.AddRange(permissions);
                }
                else
                {
                    foreach (Permission permission in permissions)
                    {
                        Permission permis = context.Permissions.FirstOrDefault(x => x.Name == permission.Name);
                        if (permission.Name == "Create Po from Offer")
                        {

                        }
                        if (permission.ParentId != null)
                        {
                            Permission permissionaa = permissions.FirstOrDefault(x => x.Id == permission.ParentId);
                            if (permissionaa != null && permissionaa.ParentId != null)
                            {
                                Permission parentpermission = context.Permissions.FirstOrDefault(x => x.Name == permissionaa.Name);
                                if (parentpermission != null)
                                    permission.ParentId = parentpermission.Id;
                            }
                            else
                            {
                                permission.ParentId = null;
                            }
                        }
                        if (permis == null)
                        {
                            context.Permissions.Add(permission);
                            context.SaveChanges();
                        }

                        else if (permis.Name != permission.Name || permis.ParentId != permission.ParentId)
                        {
                            permis.Name = permission.Name;
                            permis.ParentId = permission.ParentId;
                            //permis.ParentId = permission.ParentId;
                            context.SaveChanges();
                        }

                    }
                }
        }

        public List<User> GetAllLoggedinUsers()
        {
            var loggedInUsers = context.Users.Where(x => x.isLoggedIn == true).ToList();

            return loggedInUsers;
        }
        public User getSender(int userid)
        {
            return context.Users.FirstOrDefault(x => x.id == userid);

        }
        public List<RoleField> getField(int userid)
        {
            return context.roleFields.Where(x => x.Id == userid).ToList();

        }
    
        //public LoginUserDetails getLoginUser(int userIds)
        //{
        //    return context.loginUserDetails
        //        .Include("user.employee.CoreCompany")
        //         .Include("user.employee.CoreDepartment")
        //         .Contains(x => x.id == userIds);

        //   // var listOfRoleId = context.loginUserDetails.Select(r => r.userId == userIds);
        //}
        public LoginUserDetails getLoginUserDetails(int usId)
        {
            var user = context.Users.Include("Employee.CoreCompany").Include("Employee.CoreDepartment").FirstOrDefault(x => x.id == usId);
            var userDetail = context.loginUserDetails
                               .Where(t =>t.user == user);
            return (LoginUserDetails)userDetail;   
        }
        public List<User> getUserDetails(int uId)
        { 
            return context.Users.Include("employee.CoreDepartment")
                 .Include("employee.CoreCompany").Include("employee.person")
                 .Where(x => x.id == uId)
                 .ToList();
        }
        public List<LoginUserDetails> getlogUserDetails(int UserId)
        {
            var baselineDate = DateTime.Now.AddDays(-35);
            return context.loginUserDetails.Include("user.employee.CoreDepartment")
                 .Include("user.employee.CoreCompany").Include("user.employee.person").Where(x => x.user.id == UserId && x.LoginTime >= baselineDate).ToList();
            
           
        }
        public void updateCrashingLogoutUser(int user,string error)
        {
            var _user = context.Users.Include("loginUserDetails").FirstOrDefault(x => x.id == user);
            // _user.LoginTime = null;
            _user.isLoggedIn = false;
            if (_user.loginUserDetails != null && _user.loginUserDetails.Count > 0)
            {
                _user.loginUserDetails.Last().LogoutTime = DateTime.Now;
                _user.loginUserDetails.Last().crashingDetail = error;
            }
            context.SaveChanges();
        }
        public List<User> getAllActiveUsersForComparativeStatements()
        {
            return context.Users
                .Where(x => x.isActive == true).ToList();

        }

    }
}
