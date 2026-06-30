using ERP_BL.BaseClasses;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Databases;
namespace ERP_BL.BackgroundImages
{
   public class BackgroundImagesRepo
    {
        DBContextERP context = new DBContextERP();

        public BackgroundImagesRepo()
        {

        }
       
        ftpAddress adressDemo = new ftpAddress()
        {
            IPWan = "ftp://119.156.232.242",
            IPLan = "ftp://192.168.10.95",
            userName = "Demo@ZASErp",
            password = "d5lbHt4D&ns4dekS"
        };

        ftpAddress addressProd = new ftpAddress()
        {
            IPWan = "ftp://119.156.232.242",
            IPLan = "ftp://192.168.10.91",

            userName = "ZAS@ZASErpProd",
            password = "0zLjmxumEdR9*ls8"
        };


        fileType _fileType;
        string fileName = "";
        Bitmap _image;// = new Bitmap();

        public BackgroundImagesRepo(string filePath)
        {
            fileName = filePath;
            _fileType = GetFileType(filePath);

        }
        public fileType GetFileType(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            fileType type = new fileType();
            for (int i = 0; i < (int)fileType.all; i++)
            {
                if (((fileType)i).ToString() == extension.Replace(".", ""))
                {
                    type = (fileType)i;
                    break;
                }
            }
            return type;
        }
      
        public List<BackgroundImages> getAllUploadedDetails()
        {
            return context.BackgroundImages.Where(x => x.isSharedAll == false && x.EmployeeIds == null && x.isGroup == false).OrderByDescending(x=>x.Id).Take(1).ToList();
        }
        public List<BackgroundImages> getAllUploadedDetailsSpecific()
        {
            return context.BackgroundImages.Where(x => x.isSharedAll == false && x.EmployeeIds != null).OrderByDescending(x => x.Id).Take(10).ToList();
        }
        public ERP_BL.BackgroundImages.BackgroundImages getShareAllImage()
        {
            var lastid = context.BackgroundImages.Where(x => x.EmployeeIds == null && x.isSharedAll == false && x.isGroup == false).ToList().LastOrDefault();
            if (lastid == null)
            {
                return null;
            }
            else
            {
               
                lastid.Id.ToString();
                return lastid;
            }
        }
        public ERP_BL.BackgroundImages.BackgroundImages getPopupImage()
        {
            var lastid = context.BackgroundImages.Where(x => x.isSharedAll == true).ToList().LastOrDefault();
            if (lastid == null)
            {
                return null;
            }
            else
            {

                lastid.Id.ToString();
                return lastid;
            }
        }

        public List< ERP_BL.BackgroundImages.BackgroundImages> getSpecificImage()
        {
            /*var lastid =*/return  context.BackgroundImages.Where(x => x.EmployeeIds != null && x.isSharedAll == false && x.isGroup == false).ToList();
            //if (lastid == null)
            //{
            //    return null;
            //}
            //else
            //{
            //    lastid.Id.ToString();
            //    return lastid;
            //}
        }
        public List<ERP_BL.BackgroundImages.BackgroundImages> getGroupBackgroundImages()  
        {
            /*var lastid =*/
            return context.BackgroundImages.Where(x => x.isGroup == true).ToList();
            //if (lastid == null)
            //{
            //    return null;
            //}
            //else
            //{
            //    lastid.Id.ToString();
            //    return lastid;
            //}
        }




        /// <summary>
        /// Add new attachment for transaction
        /// </summary>
        /// <param name="path">attachment  Object for Inquiry</param>
        public void AddImage(string path)
        {
            ERP_BL.BackgroundImages.BackgroundImages images = new ERP_BL.BackgroundImages.BackgroundImages();          
        
                images.Path = path;          
                context.BackgroundImages.Add(images);              
                context.SaveChanges();     
           
        }
        /// <summary>
        /// Add new attachment for transaction
        /// </summary>
        /// <param name="employId">attachment  Object for Inquiry</param>
        public void AddEmployee(string path,string employId, int uId) 
        {
            ERP_BL.BackgroundImages.BackgroundImages images = new ERP_BL.BackgroundImages.BackgroundImages();
            images.Path = path;
            images.EmployeeIds = employId;
            images.userId = uId;
            images.UploadedTime = DateTime.Now;
            context.BackgroundImages.Add(images);
            context.SaveChanges();

        }
        public void AddGroup(string path, int employId, int uId, List<ERP_BL.Databases.User> user)
        {
            ERP_BL.BackgroundImages.BackgroundImages images = new ERP_BL.BackgroundImages.BackgroundImages();
            images.Path = path;
            images.taskGroupId = employId;
            images.userId = uId;
            images.UploadedTime = DateTime.Now;
            images.isGroup = true;

            user.Select(c => { c.isBlink = true; return c; }).ToList();
            context.BackgroundImages.Add(images);
            context.SaveChanges();

        }

    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="background"></param>
        /// <returns></returns>
        public void DeleteBackground(BackgroundImages background /*int userId*/)  
        {
          
           var backgrounds = context.BackgroundImages.Where(x=>x.isSharedAll == false && x.isGroup == false && x.isUpdate == false).ToList();
            context.BackgroundImages.RemoveRange(backgrounds);

            //ERP_BL.BackgroundImages.BackgroundImages images = new ERP_BL.BackgroundImages.BackgroundImages();
            //images.Id = background.Id;
            //images.Path = background.Path;
            //images.EmployeeIds = background.EmployeeIds;
            // images.userId = userId;
            //background.userId = userId;
            context.BackgroundImages.Add(background);

            context.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="background"></param>
        /// <returns></returns>
        public void DeletePopupImage(BackgroundImages background)
        {

            var backgrounds = context.BackgroundImages.Where(x=>x.isSharedAll == true).ToList();
            context.BackgroundImages.RemoveRange(backgrounds);
            if (background.isSharedAll == true)
                background.isSharedAll = true;
            else
                background.isSharedAll = false;

            context.BackgroundImages.Add(background);

            context.SaveChanges();
        }
        public void DeleteImage(BackgroundImages background) 
        {
            var backgrounds = context.BackgroundImages.Where(x => x.isSharedAll == true).ToList();
            context.BackgroundImages.RemoveRange(backgrounds);
            context.SaveChanges();
        }
        public void DeleteGroupImage(int id, List<ERP_BL.Databases.User> user) 
        {
            var backgrounds = context.BackgroundImages.Where(x => x.taskGroupId == id).ToList();
            if (backgrounds.Count > 0) 
            {
                user.Select(c => { c.isBlink = false; return c; }).ToList();
                context.BackgroundImages.RemoveRange(backgrounds); 
                context.SaveChanges(); 
            } 
        }

        /// <summary>
        /// Add new attachment for transaction
        /// </summary>
        /// <param name="attachment">attachment  Object for Inquiry</param>
        public void GetImage(string name)
        {
            ERP_BL.BackgroundImages.BackgroundImages images = new ERP_BL.BackgroundImages.BackgroundImages();

            if (SystemLog.CurrentUserId != 0)
            {
                images.Path = name;


                context.BackgroundImages.Add(images);


                context.SaveChanges();

            }
        }



        // *****************************************************ErpBackground**************************************************************************************************************************************

        public Tuple<bool, string, string> startUploadingBackgroundImage(TransactionItemType type)
        {
            bool isUploaded = false;

            string filepath = "";
            //#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                filepath = ftpUploadBgImages(fileName, adressDemo, type);
                //  downloadFile("ftp://Demo%2540ZASErp@182.180.85.82/Attachments/Inquiry/11_Inquiry_FROM.html", adressDemo, type);
            }

            else
            {
                filepath = ftpUploadBgImages(fileName, addressProd, type);
                //downloadFile("", adressDemo, type);

            }


            if (string.IsNullOrEmpty(filepath))
            {
                return Tuple.Create(isUploaded = false, filepath, "File Uploading Error");

            }
            else
                return Tuple.Create(isUploaded = true, filepath, "Uploaded");
        }


        // **************************************************************Ftpupload****************************************************************************

        private string ftpUploadBgImages(string localFile, ftpAddress address, TransactionItemType type)
        {
            string filePath = "";

            try
            {


                using (var client = new WebClient())
                {
                    Console.Write(address.password);
                    client.Credentials = new NetworkCredential(address.userName, address.password);
                    string str = Path.GetFileName(fileName);
                    try
                    {
                        filePath = "/ErpBackground/" + type.ToString() + "/" + str;

                        client.UploadFile(address.IPWan + filePath, WebRequestMethods.Ftp.UploadFile, localFile);

                    }
                    catch (Exception ex)
                    {
                        filePath = "/ErpBackground/" + type.ToString() + "/" + str;
                        client.UploadFile(address.IPLan + filePath, WebRequestMethods.Ftp.UploadFile, localFile);


                    }
                }
            }
            catch (Exception exception)
            {
                SystemLog.LogError(this.GetType(), exception.ToString());
                filePath = "";
            }

            return filePath;

        }
        //**********************************************************************************StartDownload**********************************************************************

        public Tuple<bool, string> startDownloadBackgroundImage(string filePath, TransactionItemType type)
        {
            bool isDownLoaded = false;

            string downloadpath = "";

            downloadpath = downloadFileBgImages(filePath, addressProd, type);


            return Tuple.Create(isDownLoaded = true, downloadpath);
        }
        public string downloadFileBgImages(string fileName, ftpAddress address, TransactionItemType type)
        {

            string destination = "";
            try
            {


                try
                {
                    using (var client = new WebClient())
                    {
                        Console.Write(address.password);
                        client.Credentials = new NetworkCredential(address.userName, address.password);
                        try
                        {
                            string str = Path.GetFileName(fileName);
                            string exePath = System.Environment.GetCommandLineArgs()[0];
                            string path = address.IPLan + fileName;
                            destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                            destination += "ErpBackground\\" + type.ToString() + "\\Downloaded\\";
                            //string destination = @"D:\MovedFiles\new\";
                            string _str = "";
                            int pos = fileName.LastIndexOf("/") + 1;
                            int posId = fileName.IndexOf(",");
                            _str = fileName.Substring(pos, fileName.Length - pos);

                            if (!System.IO.Directory.Exists(destination))
                            {
                                System.IO.Directory.CreateDirectory(destination);
                                if(!System.IO.File.Exists(destination+_str))
                                    client.DownloadFile(path, destination);
                            }
                            else
                            {
                                if (!System.IO.File.Exists(destination + _str))
                                {
                                    destination += System.IO.Path.GetFileName(fileName);
                                    client.DownloadFile(path, destination);
                                } 
                            }
                                
                            
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                string str = Path.GetFileName(fileName);
                                string exePath = System.Environment.GetCommandLineArgs()[0];
                                string path = address.IPWan + fileName;
                                destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                                destination += @"ErpBackground\\" + type.ToString() + "\\Downloaded\\";
                                //string destination = @"D:\MovedFiles\new\";
                                if (!System.IO.Directory.Exists(destination))
                                    System.IO.Directory.CreateDirectory(destination);
                                destination += System.IO.Path.GetFileName(fileName);

                                client.DownloadFile(path, destination);
                            }
                            catch (Exception ex2)
                            {
                                Console.Write(ex2.Message);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    using (var client = new WebClient())
                    {
                        Console.Write(address.password);
                        client.Credentials = new NetworkCredential(address.userName, address.password);
                        string str = Path.GetFileName(fileName);
                        string exePath = System.Environment.GetCommandLineArgs()[0];

                        destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += @"ErpBackground\\" + type.ToString() + "\\Downloaded\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        destination += System.IO.Path.GetFileName(fileName);
                        client.DownloadFile(fileName, destination);
                    }
                }

            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                destination = "";

            }
            return destination;
        }


        //  ******************************************************ErpEnd*************************************************************************************************************************************

        /// <summary>
        /// 
        /// </summary>
        /// <param name="background"></param>
        /// <returns></returns>
        public void DeletePopupImages(BackgroundImages background)
        {

            var backgrounds = context.BackgroundImages.Where(x => x.isSharedAll == true).ToList();
            context.BackgroundImages.RemoveRange(backgrounds);
            if (background.isSharedAll == true)
                background.isSharedAll = true;
            else
                background.isSharedAll = false;

            background.isUpdate = true;

            context.BackgroundImages.Add(background);

            context.SaveChanges();
        }


        //start grouping function
        public ERP_BL.Databases.Employee GetEmployeeForBackground(int empID)
        {
            return context.Employees
                //.Include("person")


                //.Include("Companies")
                //.Include("departments.customers")
                //.Include("Companies.departments")
                //.Include("Companies.departments.parentDepartment")
                //.Include("departments")
                //.Include("departments.Vendors")                
                //.Include("departments.parentDepartment")
                .FirstOrDefault(x => x.EmpId == empID);
        }
        public List<ERP_BL.Databases.User> getAllusersByEmpIds(List<int> empIds)
        {
            return context.Users
                
                .Where(x => empIds.Contains(x.employeeId) && x.isActive == true)
                .ToList();
        }
        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        public List<TaskGroups> GetAllTaskGroups()
        {
            return context.taskGroups
                .Where(x => x.isBackground == true)
                .ToList();
        }
        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        public List<TaskGroups> GetAllOptionalTaskGroups()
        {
            return context.taskGroups
                .Where(x => x.isBackground == true && x.Companies.Count == 0 && x.Departments.Count == 0)
                .ToList();
        }

        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        public List<TaskGroups> GetAllParentTaskGroups()
        {
            return context.taskGroups
                .Where(x => x.isBackground == true && x.Companies.Count > 0 && x.Departments.Count>0)
                .ToList();
        }
        public List<BackgroundImages> GetUserTaskGroup(int uId) 
        {
            var groups = context.taskGroups
               .Where(x => x.users.Any(y => y.id == uId) && x.isBackground == true)
               .ToList();
            if (groups.Count != 0)
            {
                List<int> listOfGroupId = new List<int>();
                foreach (var _gId in groups)
                {
                    listOfGroupId.Add(_gId.Id);
                }
                return context.BackgroundImages.Where(x => listOfGroupId.Contains((int)x.taskGroupId)).ToList();
            }
            else return null;
         
        }

        /// <summary>
        /// Get Group by Id
        /// </summary>
        /// <returns></returns>
        public TaskGroups GetTaskGroup(int groupId)
        {
            return context.taskGroups
                .FirstOrDefault(x => x.Id == groupId && x.isBackground == true);
        }
        public List<ERP_BL.Databases.User> getAllusers()
        {
            return context.Users.Where(x=>x.isActive == true).ToList();
        }
        public void UpdateTaskGroup(TaskGroups TaskGroup)
        {
            TaskGroups _taskGroup = context.taskGroups.FirstOrDefault(x => x.Id == TaskGroup.Id);
            _taskGroup = TaskGroup;
            context.SaveChanges();
        }
        public void DeleteTaskGroup(TaskGroups TaskGroup)
        {
            TaskGroups _taskGroup = context.taskGroups.Remove(TaskGroup);
            context.SaveChanges();
        }
        public void AddTaskGroup(TaskGroups taskGroup)
        {
            context.taskGroups.Add(taskGroup);
            context.SaveChanges();
        }

        public BackgroundImages GetImageByGroupId(int taskGroupId)
        {
            var data = context.BackgroundImages.Where(x => x.taskGroupId == taskGroupId && x.isGroup != false).OrderByDescending(a => a.Id).FirstOrDefault();

            return data;

        }
        public BackgroundImages GetImageForPreview(int taskGroupId)
        {
            var data = context.BackgroundImages.Where(x => x.Id == taskGroupId ).OrderByDescending(a => a.Id).FirstOrDefault();

            return data;

        }
        public List<BackgroundImages> GetUserTaskGroupsUploadTime(int taskGroupId) 
        {
            var data = context.taskGroups.Where(x => x.Id == taskGroupId).ToList();
        
            if (data.Count != 0)
            {
                List<int> listOfGroupId = new List<int>();
                foreach (var _gId in data)
                {
                    listOfGroupId.Add(_gId.Id);
                }
                return context.BackgroundImages.Where(x => listOfGroupId.Contains((int)x.taskGroupId)).OrderByDescending(x => x.Id).Take(10).ToList();
            }
            else return null;

        }

    }
}
