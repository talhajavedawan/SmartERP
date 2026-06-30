#define MYTEST
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ERP_BL.BaseClasses;
using ERP_BL.Databases;
using ERP_BL.Enums;
 
namespace ERP_BL
{
    public static class LoaderClass
    {
        public static double loaderValue;
    }


    public interface IAttachment
    {
        fileType getFileType(string filePath);
        Bitmap cleanHeaders(Bitmap img);
        Bitmap compressImage(Bitmap img, double compressionLevel);
        bool uploadFile(string filePath);
        bool uploadFile(Bitmap img);
        //upload doc files
    }
    public class Attach
    {
        
        ftpAddress adressDemo = new ftpAddress()
        {
            IPWan = "ftp://203.135.42.34",
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

        public Attach(string filePath)
        {
            fileName = filePath;
            _fileType = GetFileType(filePath);

        }
        public Attach()
        {


        }
        public Tuple<bool, string, string> startUploading(TransactionItemType type)
        {
            bool isUploaded = false;
            //            if ((_fileType == fileType.bmp) || _fileType == fileType.gif || _fileType == fileType.jpeg || _fileType == fileType.jpg || _fileType == fileType.png)
            //            {
            //                _image = cleanHeaders(fileName);
            //                _image.Save(Path.GetDirectoryName(Environment.CurrentDirectory) + "\temp\temp.png");
            //#if DEBUG
            //                ftpUpload(Path.GetDirectoryName(Environment.CurrentDirectory) + "\temp\temp.png", adressDemo,type);
            //#else
            //                ftpUpload(Path.GetDirectoryName(Environment.CurrentDirectory) + "\temp\temp.png", addressProd);
            //#endif
            //                return true;
            //            }
            //            else
            //            {
            string filepath = "";
            //#if DEBUG
           



                if (System.Diagnostics.Debugger.IsAttached)
                {
                    filepath = ftpUpload(fileName, adressDemo, type);
                    //  downloadFile("ftp://Demo%2540ZASErp@182.180.85.82/Attachments/Inquiry/11_Inquiry_FROM.html", adressDemo, type);
                }
                //#else     
                else
                {
                    filepath = ftpUpload(fileName, addressProd, type);
                    //downloadFile("", adressDemo, type);

                }

                //#endif
                //return false;
                //}
                if (string.IsNullOrEmpty(filepath))
                {
                    return Tuple.Create(isUploaded = false, filepath, "File Uploading Error");

                }
                else
                    return Tuple.Create(isUploaded = true, filepath, "Uploaded");
          
        }
        public Tuple<bool, string> startDownload(string filePath, TransactionItemType type)
        {
            bool isDownLoaded = false;

            string downloadpath = "";
            //#if DEBUG
            //if (System.Diagnostics.Debugger.IsAttached)

            //{
            //    //filepath = ftpUpload(fileName, adressDemo, type);
            //    downloadpath = downloadFile(filePath, adressDemo, type);
            //}
            ////#else
            //else
            //{
                //filepath=ftpUpload(fileName, addressProd,type);
                downloadpath = downloadFile(filePath, addressProd, type);


            //}

//#endif
            //return false;
            //}

            return Tuple.Create(isDownLoaded = true, downloadpath);
        }
        private string ftpUpload(string localFile, ftpAddress address, TransactionItemType type)
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

                        var attachementFolder = address.IPLan + "/" + DateTime.Now.Year.ToString("0000") + "Attachments/";
                        var typeFolder = address.IPLan + "/" + DateTime.Now.Year.ToString("0000") + "Attachments/" + type.ToString();
                        var toUploadFolder = address.IPLan + "/" + DateTime.Now.Year.ToString("0000") + "Attachments/" + type.ToString() + "/ToUpload";
                        bool attachementFolderResult = FtpDirectoryExists(attachementFolder, address.userName, address.password);
                        bool typeFolderResult = FtpDirectoryExists(typeFolder, address.userName, address.password);
                        bool toUploadFolderResult = FtpDirectoryExists(toUploadFolder, address.userName, address.password);
                        filePath = "/" + DateTime.Now.Year.ToString("0000") + "Attachments/" + type.ToString() + "/ToUpload/" + str;
                        client.UploadFile(address.IPLan + filePath, WebRequestMethods.Ftp.UploadFile, localFile);
                    }
                    catch (Exception ex)
                    {
                        var attachementFolder = address.IPWan + "/" + DateTime.Now.Year.ToString("0000") + "Attachments/";
                        var typeFolder = address.IPWan + "/" + DateTime.Now.Year.ToString("0000") + "Attachments/" + type.ToString();
                        var toUploadFolder = address.IPWan + "/" + DateTime.Now.Year.ToString("0000") + "Attachments/" + type.ToString()+"/ToUpload";
                        bool attachementFolderResult = FtpDirectoryExists(attachementFolder, address.userName, address.password);
                        bool typeFolderResult = FtpDirectoryExists(typeFolder, address.userName, address.password);
                        bool toUploadFolderResult = FtpDirectoryExists(toUploadFolder, address.userName, address.password);
                        filePath = "/" + DateTime.Now.Year.ToString("0000") + "Attachments/" + type.ToString()+"/ToUpload/"+ str;
                        client.UploadFile(address.IPWan + filePath, WebRequestMethods.Ftp.UploadFile, localFile);
                    }
                }
            }
            catch (Exception exception)
            {

                SystemLog.LogError(this.GetType(), exception.ToString());
                filePath = "";
            }

            return filePath;
            // Delete all files in a directory    
            //System.Threading.Thread th = new System.Threading.Thread(() =>
            //{
            //    string[] files = Directory.GetFiles(Path.GetDirectoryName(localFile));
            //    foreach (string file in files)
            //    {
            //        try
            //        {
            //            File.Delete(file);
            //        }
            //        catch { }
            //    }
            //});
            //th.Start();
        }
      
        public bool FtpDirectoryExists(string directoryPath, string ftpUser, string ftpPassword)
        {
            try
            {
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(directoryPath));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
        }
        public string uploadFile( ChatFileType type, string localFile, int sender, int receiver=-1)
        {
            string filePath = "";

            try
            {
                using (var client = new WebClient())
                {

                    //if (Debugger.IsAttached)
                    //{
                    //   client.Credentials = new NetworkCredential( this.adressDemo.userName, this.adressDemo.password);
                    //}
                    //else
                    {
                        client.Credentials = new NetworkCredential( this.addressProd.userName, this.addressProd.password);
                    }
                    
                    
                    string str = Path.GetFileName(localFile);
                    // Directory Path logic. 
                    if (type == ChatFileType.One2One && receiver>-1)
                    {
                        str = (sender>receiver? sender.ToString()+"_"+receiver.ToString() : receiver.ToString()+"_"+ sender.ToString()) +str;
                    }

                    try
                    {
                        filePath = "/Chat/" + type.ToString() + "/" + str;
                        //if (Debugger.IsAttached)
                        //{
                        //    client.UploadFile(this.adressDemo.IPLan + filePath, WebRequestMethods.Ftp.UploadFile, localFile);
                        //}
                        //else
                        {
                            client.UploadFile(this.addressProd.IPLan + filePath, WebRequestMethods.Ftp.UploadFile, localFile);
                        }

                       
                    }
                    catch (Exception ex)
                    {
                        filePath = "/Chat/" + type.ToString() + "/" + str;
                        //if (Debugger.IsAttached)
                        //{
                        //    client.UploadFile(this.adressDemo.IPWan + filePath, WebRequestMethods.Ftp.UploadFile, localFile);
                        //}
                        //else
                        {
                            client.UploadFile(this.addressProd.IPWan + filePath, WebRequestMethods.Ftp.UploadFile, localFile);
                        }

                    }
                }
            }
            catch (Exception exception)
            {
                SystemLog.LogError(this.GetType(), exception.ToString());
                filePath = "";
            }

            return filePath;
            // Delete all files in a directory    
            //System.Threading.Thread th = new System.Threading.Thread(() =>
            //{
            //    string[] files = Directory.GetFiles(Path.GetDirectoryName(localFile));
            //    foreach (string file in files)
            //    {
            //        try
            //        {
            //            File.Delete(file);
            //        }
            //        catch { }
            //    }
            //});
            //th.Start();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            LoaderClass.loaderValue = bytesIn / totalBytes * 100;  
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }

        public List<UserSettings> GetChangeLogs()
        {
            List<UserSettings> userSettings = new List<UserSettings>();
            try
            {
                
                {
                    var parentDirectory = "ftp://zaserpupdates@203.135.42.34/Updates/";
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(parentDirectory);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;

                    //request.Credentials = new NetworkCredential("ZAS@ZASErpProd", "0zLjmxumEdR9*ls8");
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string names = reader.ReadToEnd();
                    string[] result = Regex.Split(names, @"\r\n");
                    string logs = "";

                    foreach (var _str in result)
                    {
                        if (!String.IsNullOrEmpty(_str))
                        {
                            if (_str.Equals("updates.txt"))
                            {
                                WebClient client = new WebClient();
                                Uri Uri = new Uri("ftp://zaserpupdates@203.135.42.34/Updates/" + _str);
                                byte[] contents = client.DownloadData(Uri);

                                // Get the object used to communicate with the server.

                                FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create(Uri);
                                request1.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                                FtpWebResponse response1 = (FtpWebResponse)request1.GetResponse();
                                DateTime lastModifiedDate = response1.LastModified;

                                string fileString = System.Text.Encoding.UTF8.GetString(contents);

                                string[] fileContent = Regex.Split(fileString, @"\r\n");

                                UserSettings userSetting = new UserSettings();

                                for (int i = 0; i <= fileContent.Length - 1; i++)
                                {
                                    if (fileContent[i].Contains("ProductVersion "))
                                    {
                                        logs = logs + "----------" + fileContent[i].Substring(7, fileContent[i].Length - 7) + "----------" + System.Environment.NewLine + "Date: " + response1.LastModified + System.Environment.NewLine;

                                        userSetting.lastModified = response1.LastModified;
                                        userSetting.settingkey = fileContent[i].Substring(7, fileContent[i].Length - 7);
                                    }
                                    else if (fileContent[i].Contains("Enhancement"))
                                    {
                                        var strr = fileContent[i].Split('=');
                                        logs = logs + strr[1] + System.Environment.NewLine;

                                        userSetting.settingValue = userSetting.settingValue + strr[1] + System.Environment.NewLine;
                                    }
                                }

                                userSettings.Add(userSetting);
                            }

                            logs = logs + System.Environment.NewLine;
                            //logs = logs + System.Environment.NewLine;
                        }
                    }
                    reader.Close();
                    response.Close();
                }
                {
                    var serverUri = "ftp://zaserpupdates@203.135.42.34/Updates/Old%20ERP%20Versions/";
                    
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverUri);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;

                    //request.Credentials = new NetworkCredential("ZAS@ZASErpProd", "0zLjmxumEdR9*ls8");
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string names = reader.ReadToEnd();
                    string[] result = Regex.Split(names, @"\r\n");
                    string logs = "";

                    foreach (var _str in result)
                    {
                        if (!String.IsNullOrEmpty(_str))
                        {
                            if (_str.Contains(".txt"))
                            {
                                WebClient client = new WebClient();
                                Uri Uri = new Uri("ftp://zaserpupdates@203.135.42.34/Updates/Old%20ERP%20Versions/" + _str);
                                byte[] contents = client.DownloadData(Uri);

                                // Get the object used to communicate with the server.

                                FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create(Uri);
                                request1.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                                FtpWebResponse response1 = (FtpWebResponse)request1.GetResponse();
                                DateTime lastModifiedDate = response1.LastModified;

                                string fileString = System.Text.Encoding.UTF8.GetString(contents);

                                string[] fileContent = Regex.Split(fileString, @"\r\n");

                                UserSettings userSetting = new UserSettings();

                                for (int i = 0; i <= fileContent.Length - 1; i++)
                                {
                                    if (fileContent[i].Contains("ProductVersion "))
                                    {
                                        logs = logs + "----------" + fileContent[i].Substring(7, fileContent[i].Length - 7) + "----------" + System.Environment.NewLine + "Date: " + response1.LastModified + System.Environment.NewLine;

                                        userSetting.lastModified = response1.LastModified;
                                        userSetting.settingkey = fileContent[i].Substring(7, fileContent[i].Length - 7);
                                    }
                                    else if (fileContent[i].Contains("Enhancement"))
                                    {
                                        var strr = fileContent[i].Split('=');
                                        logs = logs + strr[1] + System.Environment.NewLine;

                                        userSetting.settingValue = userSetting.settingValue + strr[1] + System.Environment.NewLine;
                                    }
                                }

                                userSettings.Add(userSetting);
                            }
                            else
                            {
                                WebClient client = new WebClient();
                                if (CheckIfFileExistsOnServer(_str))
                                {

                                    Uri Uri = new Uri("ftp://zaserpupdates@203.135.42.34/Updates/Old%20ERP%20Versions/" + _str + "/Updates.txt");
                                    //client.Credentials = new NetworkCredential("username", "password");
                                    byte[] contents = client.DownloadData(Uri);
                                    string fileString = System.Text.Encoding.UTF8.GetString(contents);

                                    // Get the object used to communicate with the server.

                                    FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create(Uri);
                                    request1.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                                    FtpWebResponse response1 = (FtpWebResponse)request1.GetResponse();
                                    DateTime lastModifiedDate = response1.LastModified;

                                    string[] fileContent = Regex.Split(fileString, @"\r\n");

                                    UserSettings userSetting = new UserSettings();
                                    for (int i = 0; i <= fileContent.Length - 1; i++)
                                    {
                                        if (fileContent[i].Contains("ProductVersion "))
                                        {
                                            logs = logs + "----------" + fileContent[i].Substring(7, fileContent[i].Length - 7) + "----------" + System.Environment.NewLine + "Date: " + lastModifiedDate + System.Environment.NewLine;

                                            userSetting.lastModified = lastModifiedDate;
                                            userSetting.settingkey = fileContent[i].Substring(7, fileContent[i].Length - 7);
                                        }
                                        else if (fileContent[i].Contains("Enhancement"))
                                        {
                                            var strr = fileContent[i].Split('=');
                                            logs = logs + strr[1] + System.Environment.NewLine;

                                            userSetting.settingValue = userSetting.settingValue + strr[1] + System.Environment.NewLine;
                                        }

                                    }

                                    userSettings.Add(userSetting);
                                }
                            }

                            logs = logs + System.Environment.NewLine;
                            //logs = logs + System.Environment.NewLine;
                        }
                    }
                    reader.Close();
                    response.Close();
                }
                return userSettings;

                //return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            //String str = "";
            //var directories = Directory.GetDirectories("ftp://zaserpupdates@203.135.42.34/Updates/Old%20ERP%20Versions");

            //foreach(var d in directories)
            //{
            //    DirectoryInfo info = new DirectoryInfo(d);

            //    FileInfo[] Files = info.GetFiles("Updates.txt");
            //    foreach(var file in Files)
            //    {
            //        //string[] lines = File.ReadAllLines(file.);
            //    }
            //}
        }

        private bool CheckIfFileExistsOnServer(string str)
        {
            var request = (FtpWebRequest)WebRequest.Create("ftp://zaserpupdates@203.135.42.34/Updates/Old%20ERP%20Versions/"+str + "/Updates.txt");
            //request.Credentials = new NetworkCredential("username", "password");
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    return false;
            }
            return false;
        }

        public string downloadFile(string fileName, ftpAddress address, TransactionItemType type)
        {
            string destination = "";
            try
            {


                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadProgressChanged += client_DownloadProgressChanged;
                        client.DownloadFileCompleted += client_DownloadFileCompleted;
                        Console.Write(address.password);
                        client.Credentials = new NetworkCredential(address.userName, address.password);
                        try
                        {
                            string str = Path.GetFileName(fileName);
                            string exePath = System.Environment.GetCommandLineArgs()[0];
                            string path = address.IPLan + fileName;
                            destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                            destination += "Attachments\\"+type.ToString()+"\\Downloaded\\";
                            //string destination = @"D:\MovedFiles\new\";
                            if (!System.IO.Directory.Exists(destination))
                                System.IO.Directory.CreateDirectory(destination);

                            destination += System.IO.Path.GetFileName(fileName);
                            //if (System.IO.Directory.Exists(path))
                                client.DownloadFile(path, destination);
                            //else
                            //    throw new Exception();

                            //path = "ftp://119.156.232.242/2021Attachments/Sale_Order/ToUpload/17634_Sale_Order_2.2.pdf";

                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                string str = Path.GetFileName(fileName);
                                string exePath = System.Environment.GetCommandLineArgs()[0];
                                string path = address.IPWan + fileName;
                                destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                                destination += @"Attachments\\" + type.ToString() + "\\Downloaded\\";
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
                        destination += @"Attachments\\" + type.ToString() + "\\Downloaded\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        destination += System.IO.Path.GetFileName(fileName);
                        client.DownloadFileAsync(new System.Uri(fileName), destination);
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
        public string downloadFile(ChatFileType type, string localFile, int sender, int receiver = -1)
        {
            string destination = "";
            try
            {


                try
                {
                    using (var client = new WebClient())
                    {
                       
                        if (Debugger.IsAttached)
                        {
                            client.Credentials = new NetworkCredential(this.adressDemo.userName, this.adressDemo.password);
                        }
                        else
                        {
                            client.Credentials = new NetworkCredential(this.addressProd.userName, this.addressProd.password);
                        }

                        try
                        {
                            string str = Path.GetFileName(fileName);
                            if (type == ChatFileType.One2One && receiver > -1)
                            {
                                str = (sender > receiver ? sender.ToString() + "-" + receiver.ToString() : receiver.ToString() + "-" + sender.ToString()) + str;
                            }
                            string exePath = System.Environment.GetCommandLineArgs()[0];
                            string path = Debugger.IsAttached? this.adressDemo.IPLan + fileName : this.addressProd.IPLan + fileName ;
                            destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                            destination += "Downloads\\Chat\\";
                            //string destination = @"D:\MovedFiles\new\";
                            if (!System.IO.Directory.Exists(destination))
                                System.IO.Directory.CreateDirectory(destination);
                            destination += System.IO.Path.GetFileName(fileName);

                            client.DownloadFile(path, destination);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                string str = Path.GetFileName(fileName);
                                if (type == ChatFileType.One2One && receiver > -1)
                                {
                                    str = (sender > receiver ? sender.ToString() + "-" + receiver.ToString() : receiver.ToString() + "-" + sender.ToString()) + str;
                                }

                                string exePath = System.Environment.GetCommandLineArgs()[0];
                                string path = Debugger.IsAttached ? this.adressDemo.IPWan + fileName : this.addressProd.IPWan+ fileName;
                                destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                                destination += "Downloads\\Chat\\";
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
                       
                        client.Credentials = new NetworkCredential(this.addressProd.userName, this.addressProd.password);
                        string str = Path.GetFileName(fileName);
                        if (type == ChatFileType.One2One && receiver > -1)
                        {
                            str = (sender > receiver ? sender.ToString() + "-" + receiver.ToString() : receiver.ToString() + "-" + sender.ToString()) + str;
                        }

                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        //string path = Debugger.IsAttached ? this.adressDemo.IPWan + fileName : this.addressProd.IPWan + fileName;
                        destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Downloads\\Chat\\";
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
        public Bitmap cleanHeaders(string filePath)
        {
            ((System.Drawing.Bitmap)System.Drawing.Image.FromFile(filePath).Clone()).Save(Path.GetDirectoryName(Environment.CurrentDirectory) + @"\temp\file-nometa.png");
            return ((System.Drawing.Bitmap)System.Drawing.Image.FromFile(Path.GetDirectoryName(Environment.CurrentDirectory) + @"\temp\file-nometa.png"));
        }
    }
}
