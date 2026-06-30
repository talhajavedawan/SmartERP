using DevExpress.Xpf.Core;
using ERP_BL.BackgroundImages;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.ToDoTasks.UserControls;

namespace ZAS_ERP.BackgroundUpload
{
    /// <summary>
    /// Interaction logic for ucFrmBackgroundGroupList.xaml
    /// </summary>
    public partial class ucFrmBackgroundGroupList : UserControl
    {
        BackgroundImagesRepo taskRepo = new BackgroundImagesRepo();
        List<TaskGroups> TaskGroups = new List<TaskGroups>();
        BitmapImage bmImg = new BitmapImage();
        OpenFileDialog fileDialog = new OpenFileDialog();
        public static int isSpecific;

        public ucFrmBackgroundGroupList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadGroups(); //comment
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlGroupList);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void LoadGroupData()
        {
            try
            {
                string path = @"";
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                txtGrpName.Text = group.GroupName;
                lblData.Text = group.GroupName;
                if (group != null)
                {
                    //taskRepo = new BackgroundImagesRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var groups = taskRepo.GetImageByGroupId(group.Id); 
                    if(groups != null)
                    {
           
                        if (!string.IsNullOrEmpty(groups.Path))
                        {
                            path = groups.Path;
                            BackgroundImagesRepo bgImages = new BackgroundImagesRepo(path);
                            var result = bgImages.startDownloadBackgroundImage(path, TransactionItemType.BackgroundImages);

                        }

                        else
                            return;

                        var _str = path;

                        int pos = path.LastIndexOf("/") + 1;
                        int posId = path.IndexOf(",");
                        _str = path.Substring(pos, path.Length - pos);

                        string root = @"ErpBackground\\BackgroundImages\\Downloaded\\";
                        string[] fileEntries = Directory.GetFiles(root);
                        path = path.Substring(pos, path.Length - pos);
                        foreach (string fileName in fileEntries)
                        {
                            int position = fileName.LastIndexOf("\\") + 1;
                            var _filename = fileName.Substring(position, fileName.Length - position);
                            if (_filename == path)
                            {


                                BitmapImage bitmap = new BitmapImage();
                                using (FileStream fs = new FileStream(root + path, FileMode.Open))
                                {
                                    bitmap.BeginInit();
                                    bitmap.StreamSource = fs;
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.EndInit();

                                    grdImagePreview.Children.Clear();
                                    Image img = new Image();
                                    img.Stretch = Stretch.Fill;
                                    img.Name = "imgDynamic1";
                                    RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                                    grdImagePreview.Children.Add(img);

                                    Grid grid = (Grid)grdImagePreview; //get grid and grid name is favouriteitem
                                    var image = bitmap;
                                    var elements = grid.Children;

                                    foreach (UIElement _element in elements)
                                    {
                                        if (_element is Image)
                                        {
                                            var imge = (Image)_element;
                                            imge.Source = image;
                                            break;
                                        }

                                    }
                                }
                                break;






                            }
                        }
                    }


                    else
                    {
                        grdImagePreview.Children.Clear();
                    }



                    //lblTaskHeading.Text = "Open Tasks";
                    //  grdCntrlToDoTaskList.ItemsSource = tasks;


                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void loadSelectedUser()
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                grdCntrlSelectedUser.ItemsSource = group.users;
                if(group.Companies.Count > 0)
                {
                    grdCntrlSelectedComp.ItemsSource = group.Companies;
                    grdCntrlSelectedComp.Visibility = Visibility.Visible;
                    grdCntrlSelectedUser.SetValue(Grid.RowProperty, 2);
                    grdCntrlSelectedUser.SetValue(Grid.RowSpanProperty, 1);
                }
                else
                {
                    grdCntrlSelectedComp.Visibility = Visibility.Collapsed;
                    grdCntrlSelectedUser.SetValue(Grid.RowProperty, 0);
                    grdCntrlSelectedUser.SetValue(Grid.RowSpanProperty, 3);

                    //groupDataGrid.Children.Add(grdCntrlSelectedUser);
                    
                }
                if (group.Departments.Count > 0)
                {
                    grdCntrlSelectedDept.ItemsSource = group.Departments;
                    grdCntrlSelectedDept.Visibility = Visibility.Visible;
                }
                else
                {
                    grdCntrlSelectedDept.Visibility = Visibility.Collapsed;
                    grdCntrlSelectedUser.SetValue(Grid.RowSpanProperty, 3);
                } 
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
                
            }
        }
        private void GrdCntrlGroupList_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            LoadGroupData();
            loadSelectedUser();
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddGroup();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MenuGroupUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateGroup();
        }

        private void MenuItemDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedGroup = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (selectedGroup != null)
                {
                    taskRepo.DeleteGroupImage(selectedGroup.Id, selectedGroup.users);
                }
                else { DXMessageBox.Show("Please select group first"); }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
         
        }
        private void AddGroup()
        {
            DXWindow win = new DXWindow();
            ucFrmSelectTemplate frmTemplate = new ucFrmSelectTemplate();
            frmTemplate.isbg = true;
            win.Title = "Templates";

            win.Content = frmTemplate;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Height = 250;
            win.Width = 400;
            win.ShowDialog();

            //LoadGroups();
        }
        private void LoadGroups()
        {
         

                TaskGroups = taskRepo.GetAllTaskGroups();
                grdCntrlGroupList.ItemsSource = TaskGroups;
            
        }
        private void UpdateGroup()
        {
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Group") != null)
            //{
                var _item = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (_item != null && _item.Id > 0)
                {
                    var taskGroup = taskRepo.GetTaskGroup(_item.Id);
                if(taskGroup.Companies.Count > 0 && taskGroup.Departments.Count > 0)
                {
                    ucFrmGroupListItem itemTitle = new ucFrmGroupListItem();
                    itemTitle.template = TaskGroupTemplate.Standard;
                    itemTitle.editFlag = true;
                    itemTitle.groupId = taskGroup.Id;
                    //itemTitle.isBackground = true;
                    DXWindow win = new DXWindow();
                    win.Title = "Update Group";
                    win.Content = itemTitle;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //win.Height = 450;
                    //win.Width = 250;
                    win.ShowDialog();
                }
                else
                {
                    ucFrmGroupListItem itemTitles = new ucFrmGroupListItem();
                    itemTitles.editFlag = true;
                    itemTitles.template = TaskGroupTemplate.Optional;

                    itemTitles.groupId = taskGroup.Id;
                    //itemTitle.isBackground = true;
                    DXWindow wins = new DXWindow();
                    wins.Title = "Update Group";
                    wins.Content = itemTitles;
                    wins.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //win.Height = 450;
                    //win.Width = 250;
                    wins.ShowDialog();
                }
            
            }
            //}
            //else
            //{
            //    DXMessageBox.Show("Permission required to View Group details!");
            //}
        }

        private void MbtnAddNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                AddGroup();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MbtnUpdate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                UpdateGroup();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MbtnSaveLayout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlGroupList);
         
        }

        private void MbtnDelete_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void MbtnRefresh_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                taskRepo = new BackgroundImagesRepo();
                LoadGroups();
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        private void MbtnExpand_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdCntrlGroupList.ShowLoadingPanel = true;
            tblGroupListView.ExpandAllNodes();
            grdCntrlGroupList.ShowLoadingPanel = false;

            //grdCntrlGroupList.SetValue(Grid.ColumnSpanProperty, 3);
            //grdCntrlGroupList.SetZIndex(5);
        }

        private void MbtnCollapse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdCntrlGroupList.ShowLoadingPanel = true;
            tblGroupListView.CollapseAllNodes();
            grdCntrlGroupList.ShowLoadingPanel = false;

            //grdCntrlGroupList.SetValue(Grid.ColumnSpanProperty, 1);
        }

        private void tblGroupListView_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            //try
            //{
            //    if (e.Column.FieldName == "dept" && e.IsGetData)
            //    {
            //        var dept = grdCntrlGroupList.GetRow(e.Node.RowHandle) as TaskGroups;
            //        string deptNames = "";

            //        if (dept.Departments != null && dept.Departments.Count > 0)
            //        {
            //            deptNames = String.Join(" | ", dept.Departments.Select(x => x.DeptName));
            //        }
            //        e.Value = deptNames;
            //    }

            //    if (e.Column.FieldName == "comp" && e.IsGetData)
            //    {
            //        var cmpy = grdCntrlGroupList.GetRow(e.Node.RowHandle) as TaskGroups;
            //        string cmpyNames = "";

            //        if (cmpy.Companies != null && cmpy.Companies.Count > 0)
            //        {
            //            cmpyNames = String.Join(" | ", cmpy.Companies.Select(x => x.CompanyName));
            //        }
            //        e.Value = cmpyNames;
            //    }
            //    if (e.Column.FieldName == "emp" && e.IsGetData)
            //    {
            //        var user = grdCntrlGroupList.GetRow(e.Node.RowHandle) as TaskGroups;
            //        string userNames = "";

            //        if (user.users != null && user.users.Count > 0)
            //        {
            //            userNames = String.Join(" | ", user.users.Select(x => x.userName));
            //        }
            //        e.Value = userNames;
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Upload Background Image") != null)
            {
                try
                {

                    if (MainWindow.currentUserid != 0)
                    {
                      
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; .png;";
                        fileDialog.Multiselect = false;

                        if (fileDialog.ShowDialog() == true)
                        {
                            var fileName = System.IO.Path.GetFileName(fileDialog.FileName);
                            var filePath = System.IO.Path.GetFullPath(fileDialog.FileName);
                            bmImg = new BitmapImage();
                            using (FileStream fs = new FileStream(filePath, FileMode.Open))
                            {
                                bmImg.BeginInit();
                                bmImg.StreamSource = fs;
                                bmImg.CacheOption = BitmapCacheOption.OnLoad;
                                bmImg.EndInit();
                                MessageBox.Show("Background Image is successfully Uploaded Press ok!");
                            }
                        }
                    }

                }
                catch
                {

                }
            }
            else
            {
                DXMessageBox.Show("You are not allowed to Upload Image!");
            }
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bmImg.StreamSource != null)
                {
                    if (bmImg.PixelWidth > 700 && bmImg.PixelHeight > 500)

                    {

                        grdImagePreview.Children.Clear(); //Clear previous children
                        Image img = new Image();
                        img.Stretch = Stretch.Fill;
                        RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                        grdImagePreview.Children.Add(img); //add new properties 
                        Grid grid = (Grid)grdImagePreview; //get grid and grid name is favouriteitem
                        var image = bmImg;
                        var elements = grid.Children;
                        foreach (UIElement _element in elements)
                        {
                            if (_element is Image) //check element in an image
                            {
                                var imge = (Image)_element;
                                imge.Source = image;
                                break;
                            }

                        }

                    }
                    else
                    {
                        MessageBox.Show("Please Upload image with higher dimension");
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload First to Preview the images");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void loadBackground()
        {
            selectEmployeeWindow selectEmployee = new selectEmployeeWindow();

            try
            {
                Window win = new Window();
                ucSelectOption selectoption = new ucSelectOption();
                win.Content = selectoption;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Width = 400;
                win.Height = 250;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.ShowDialog();
                if (MainWindow.isSpecific == 1)
                {

                    string sourceFile = @"";
                    string exePath = System.Environment.GetCommandLineArgs()[0];
                    string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                    destination += "ErpBackground\\BackgroundImages\\ToUpload\\";
                    System.IO.Directory.CreateDirectory(destination);
                    sourceFile = fileDialog.FileName;
                    if (!string.IsNullOrEmpty(sourceFile))
                    {
                        destination += MainWindow.currentUserid + "_" + TransactionItemType.BackgroundImages.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                        if (File.Exists(destination))
                        {
                            File.Delete(destination);
                        }
                        System.IO.File.Copy(sourceFile, destination);
                        BackgroundImagesRepo bgImages = new BackgroundImagesRepo(destination);
                        BackgroundImages shareAllBgImages = new BackgroundImages();
                        var result = bgImages.startUploadingBackgroundImage(TransactionItemType.BackgroundImages);
                        if (result.Item1)
                        {
                            shareAllBgImages.Path = result.Item2;
                            shareAllBgImages.userId = SYSTEM_STATIC.currentUser.id;
                            shareAllBgImages.UploadedTime = DateTime.Now;
                            bgImages.DeleteBackground(shareAllBgImages);
                            //bgImages.AddEmployee(result.Item2, selectEmployee.empId);
                        }

                    }
                }
                if (MainWindow.isSpecific == 2)
                {
                    selectEmployee = new selectEmployeeWindow();
                    selectEmployee.ShowDialog();
                    if(selectEmployee.isCancle == true)
                    {
                        MainWindow.isSpecific = 0;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "ErpBackground\\BackgroundImages\\ToUpload\\";
                        System.IO.Directory.CreateDirectory(destination);
                        sourceFile = fileDialog.FileName;
                        if (!string.IsNullOrEmpty(sourceFile))
                        {
                            destination += MainWindow.currentUserid + "_" + TransactionItemType.BackgroundImages.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (File.Exists(destination))
                            {
                                File.Delete(destination);
                            }
                            System.IO.File.Copy(sourceFile, destination);

                            BackgroundImagesRepo bgImages = new BackgroundImagesRepo(destination);
                            var result = bgImages.startUploadingBackgroundImage(TransactionItemType.BackgroundImages);
                            if (result.Item1)
                            {
                                bgImages.AddEmployee(result.Item2, selectEmployee.empId, SYSTEM_STATIC.currentUser.id);
                            }
                           
                        }
                    }
                   
                }
                if (MainWindow.isSpecific == 3)
                {
                    frmSelectGroups selectGroups = new frmSelectGroups();
                    selectGroups.ShowDialog();
                    if (selectGroups.isCancle == true)
                    {
                        MainWindow.isSpecific = 0;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "ErpBackground\\BackgroundImages\\ToUpload\\";
                        System.IO.Directory.CreateDirectory(destination);
                        sourceFile = fileDialog.FileName;
                        if (!string.IsNullOrEmpty(sourceFile))
                        {
                            destination += MainWindow.currentUserid + "_" + TransactionItemType.BackgroundImages.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (File.Exists(destination))
                            {
                                File.Delete(destination);
                            }
                            System.IO.File.Copy(sourceFile, destination);

                            BackgroundImagesRepo bgImages = new BackgroundImagesRepo(destination);
                            var result = bgImages.startUploadingBackgroundImage(TransactionItemType.BackgroundImages);
                            if (result.Item1)
                            {
                                selectGroups.taskRepoGroup.AddGroup(result.Item2, selectGroups.groupId, SYSTEM_STATIC.currentUser.id, selectGroups.groupUser);
                                //bgImages.AddGroup(result.Item2, selectGroups.groupId, SYSTEM_STATIC.currentUser.id, selectGroups.groupUser);
                            }

                        }
                    }




                }



            }
            catch (Exception ex)
            {
                DXMessageBox.Show(this, ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Stop);
            }


        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            loadBackground();
        }

        private  void btnInstantChange_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnUploadHistory_Click(object sender, RoutedEventArgs e)
        {
            BackgroundImagesRepo repo = new BackgroundImagesRepo();


            ucBackgroundImageUploadedDetails ucLoginUser = new ucBackgroundImageUploadedDetails();

            ucLoginUser.images = repo.getAllUploadedDetails();

            if (ucLoginUser.images.Count != 0)
            {
                //ucLoginUser.addCategoryWindow.ResizeMode = ResizeMode.NoResize;
                ucLoginUser.addCategoryWindow.Width = 1000;
                ucLoginUser.addCategoryWindow.Height = 500;
                ucLoginUser.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ucLoginUser.addCategoryWindow.Content = ucLoginUser;
                ucLoginUser.addCategoryWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please upload First");
            }
        }

        private void mbtnViewGroupImage_Click(object sender, RoutedEventArgs e)
        {
            ucBackgroundImageUploadedDetails ucLoginUser = new ucBackgroundImageUploadedDetails();
            //taskRepo = new BackgroundImagesRepo();
            var group = grdCntrlGroupList.SelectedItem as TaskGroups;
        

            string fullName = "";
            var node = group;
            while (node != null)
            {
                if (fullName.Length != 0)
                    fullName = " | " + fullName;
                fullName = node.GroupName + fullName;

                node = node.parentGroup;
            }
            ucLoginUser.lblHeading.Text = fullName;


            if (group != null)
            {
                var groupImage = taskRepo.GetUserTaskGroupsUploadTime(group.Id);
                
                if (groupImage.Count != 0)
                {
                    ucLoginUser.images = groupImage;
                    ucBackgroundImageUploadedDetails.imageUploadTime = 1;
                    //ucLoginUser.addCategoryWindow.ResizeMode = ResizeMode.NoResize;
                    ucLoginUser.addCategoryWindow.Width = 1000;
                    ucLoginUser.addCategoryWindow.Height = 500;
                    ucLoginUser.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucLoginUser.addCategoryWindow.Content = ucLoginUser;
                    ucLoginUser.addCategoryWindow.ShowDialog();

                }
            }
           
        }

        
    }
}
