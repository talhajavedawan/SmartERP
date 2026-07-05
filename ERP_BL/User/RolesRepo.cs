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
