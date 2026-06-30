using DevExpress.Xpf.Core;
using ERP_BL.BackgroundImages;
using ERP_BL.Enums;
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

namespace ZAS_ERP.PopupNotificatios
{
    /// <summary>
    /// Interaction logic for frmCreatePopupImage.xaml
    /// </summary>
    public partial class frmCreatePopupImage : DXWindow
    {
        public static bool isSpecific = false;
        OpenFileDialog fileDialog = new OpenFileDialog();
        BitmapImage bmImg = new BitmapImage();
        public frmCreatePopupImage()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Upload Background Image") != null)
            {
                try
                {
                    if (MainWindow.currentUserid != 0)
                    {
                        var abc = isSpecific;
                       
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
                               // MessageBox.Show("Background Image is successfully Uploaded Press ok!");
                                //preview

                                 if (bmImg.StreamSource != null)
                                    {
                                        //if (bmImg.PixelWidth > 700 && bmImg.PixelHeight > 500)

                                        //{

                                            grdImage.Children.Clear(); //Clear previous children
                                            Image img = new Image();
                                            img.Stretch = Stretch.Fill;
                                            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                                            grdImage.Children.Add(img); //add new properties 
                                            Grid grid = (Grid)grdImage; //get grid and grid name is favouriteitem
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

                                        //}
                                        //else
                                        //{
                                        //    MessageBox.Show("Please Upload image with higher dimension");
                                        //}
                                    }
                                


                                //End
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
              
                    string sourceFile = @"";
                    string exePath = System.Environment.GetCommandLineArgs()[0];
                    string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                    destination += "ErpBackground\\BackgroundImages\\ToUpload\\";
                    System.IO.Directory.CreateDirectory(destination);
                    sourceFile = fileDialog.FileName;
                    if (!string.IsNullOrEmpty(sourceFile))
                    {
                    var response = MessageBox.Show("Do you really want to add version notification?", "Notifications...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (response == MessageBoxResult.No)
                    {

                    }
                    else
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
                            shareAllBgImages.isSharedAll = true;
                            bgImages.DeletePopupImage(shareAllBgImages);
                            //shareAllBgImages.isSharedAll = true;
                            //bgImages.AddEmployee(result.Item2, selectEmployee.empId);
                            this.Close();
                        }

                    }

                }
                

              
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        //btnshow is not in use will use later
        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = @"";
                BackgroundImagesRepo imagesRepo = new BackgroundImagesRepo();
                ERP_BL.BackgroundImages.BackgroundImages storeShareAll = imagesRepo.getPopupImage();
                if (storeShareAll != null)
                {
                    if (!string.IsNullOrEmpty(storeShareAll.Path))
                    {
                        path = storeShareAll.Path;
                        BackgroundImagesRepo bgImages = new BackgroundImagesRepo(path);
                        bgImages.startDownloadBackgroundImage(path, TransactionItemType.BackgroundImages);
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

                                grdImage.Children.Clear();
                                Image img = new Image();
                                img.Stretch = Stretch.Fill;
                                img.Name = "imgDynamic1";
                                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                                grdImage.Children.Add(img);

                                Grid grid = (Grid)grdImage; //get grid and grid name is favouriteitem
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
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSaves_Click(object sender, RoutedEventArgs e)
        {
            try
            {

              
                    string sourceFile = @"";
                    string exePath = System.Environment.GetCommandLineArgs()[0];
                    string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                    destination += "ErpBackground\\BackgroundImages\\ToUpload\\";
                    System.IO.Directory.CreateDirectory(destination);
                    sourceFile = fileDialog.FileName;
                    if (!string.IsNullOrEmpty(sourceFile))
                    {
                        var response = MessageBox.Show("Do you really want to add notification image?", "Notifications...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        if (response == MessageBoxResult.No)
                        {

                        }
                        else
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
                            shareAllBgImages.isSharedAll = true;
                            bgImages.DeletePopupImages(shareAllBgImages);
                            this.Close();
                        }
                        }
                   

                    }
                
          

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void BtnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (bmImg.StreamSource != null)
            {
                frmShowPopupImage popup = new frmShowPopupImage();

                popup.txtUpdateAvailable.Visibility = Visibility.Visible;
                //popup.txtCurrentVersion.Visibility = Visibility.Visible;
                //popup.runningVersion.Visibility = Visibility.Visible;

                popup.txtInformation.Visibility = Visibility.Visible;
                popup.txtReleasedVersion.Visibility = Visibility.Visible;
                popup.releaseVersion.Visibility = Visibility.Visible;

                popup.btnUpdate.Visibility = Visibility.Visible;

                popup.btncloseVersion.Visibility = Visibility.Visible;

                var currentVersion = popup.version;
                var ReleasedVersion = popup.sub;
                if (!string.IsNullOrEmpty(currentVersion) && !string.IsNullOrEmpty(ReleasedVersion))
                {
                    if (currentVersion == ReleasedVersion)
                    {
                        popup.btnUpdate.IsEnabled = false;
                    }
                    else
                    {
                        popup.btnUpdate.IsEnabled = true;
                    }
                }


                popup.grdMain.Children.Clear();
               // grdImage.Children.Clear(); //Clear previous children
                Image img = new Image();
                img.Stretch = Stretch.Fill;
                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                popup.grdMain.Children.Add(img); //add new properties 
                Grid grid = (Grid)popup.grdMain; //get grid and grid name is favouriteitem
                var image = bmImg;
                var elements = grid.Children;
                foreach (UIElement _element in elements)
                {
                    if (_element is Image) //check element in an image
                    {
                        var imge = (Image)_element;
                        imge.Source = image;
                        popup.ShowDialog();
                        break;
                    }

                } 
            }
        }

        private void BtnPreviewImageNoti_Click(object sender, RoutedEventArgs e)
        {
            if (bmImg.StreamSource != null)
            {
                frmShowPopupImage popup = new frmShowPopupImage();
                popup.btncloseImageNoti.Visibility = Visibility.Visible;

                popup.grdMain.Children.Clear();
                Image img = new Image();
                img.Stretch = Stretch.Fill;
                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);

                popup.grdMain.Children.Add(img);
                Grid grid = (Grid)popup.grdMain;
                var image = bmImg;
                var elements = grid.Children;
                foreach (UIElement _element in elements)
                {
                    if (_element is Image)
                    {
                        var imge = (Image)_element;
                        imge.Source = image;

                        popup.ShowDialog();
                        break;
                    }
                }
            }
        }
    }
}
