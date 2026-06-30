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

namespace ZAS_ERP.BackgroundUpload
{
    /// <summary>
    /// Interaction logic for ucBackgroundImageUploadedDetails.xaml
    /// </summary>
    public partial class ucBackgroundImageUploadedDetails : UserControl
    {
        public Window addCategoryWindow = new Window();
        public List<BackgroundImages> images = new List<BackgroundImages>();
        // public BackgroundImages images = new BackgroundImages();
        public static int imageUploadTime;
        BitmapImage bmImg = new BitmapImage();
        OpenFileDialog fileDialog = new OpenFileDialog();
        public bool scale;

        public ucBackgroundImageUploadedDetails()
        {
            InitializeComponent();
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(imageUploadTime == 1)
            {
                btnViewAll.Visibility = Visibility.Collapsed;
                btnViewSpecific.Visibility = Visibility.Collapsed;
                imageUploadTime = 0;
                lblHeading.HorizontalAlignment = HorizontalAlignment.Center;
                lblHeading.VerticalAlignment = VerticalAlignment.Center;
            }
            grdCntrlUploadedDetail.ItemsSource = images; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            BackgroundImagesRepo repo = new BackgroundImagesRepo();
            grdCntrlUploadedDetail.ItemsSource = null;
            images = repo.getAllUploadedDetailsSpecific();
            grdCntrlUploadedDetailAll.ItemsSource = images;
            grdUploadShareAll.Visibility = Visibility.Visible;
            lblHeading.Text = "Specific History";


            //addCategoryWindow.Close();


            //BackgroundImagesRepo repo = new BackgroundImagesRepo();
            //ucBackgroundImageUploadedDetails ucLoginUser = new ucBackgroundImageUploadedDetails();
            //ucLoginUser.images = repo.getAllUploadedDetailsSpecific();

            //if (ucLoginUser.images.Count != 0)
            //{
            //    ucLoginUser.addCategoryWindow.ResizeMode = ResizeMode.NoResize;
            //    ucLoginUser.addCategoryWindow.Width = 500;
            //    ucLoginUser.addCategoryWindow.Height = 500;
            //    btnViewSpecific.Visibility = Visibility.Collapsed;
            //    btnViewAll.Visibility = Visibility.Visible;
            //    ucLoginUser.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //    ucLoginUser.addCategoryWindow.Content = ucLoginUser;
            //    ucLoginUser.addCategoryWindow.ShowDialog();

            //   // btnViewAll.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    MessageBox.Show("Please upload First");
            //}
        }

        private void btnViewSpecific_Click(object sender, RoutedEventArgs e)
        {


            BackgroundImagesRepo repo = new BackgroundImagesRepo();
            grdCntrlUploadedDetail.ItemsSource = null;
            images = repo.getAllUploadedDetails();
            grdCntrlUploadedDetail.ItemsSource = images;
            grdUploadShareAll.Visibility = Visibility.Collapsed;
            lblHeading.Text = "ShareAll History";
            //addCategoryWindow.Close();


            //BackgroundImagesRepo repo = new BackgroundImagesRepo();
            //ucBackgroundImageUploadedDetails ucLoginUser = new ucBackgroundImageUploadedDetails();
            //ucLoginUser.images = repo.getAllUploadedDetails();

            //if (ucLoginUser.images.Count != 0)
            //{
            //    ucLoginUser.addCategoryWindow.ResizeMode = ResizeMode.NoResize;
            //    ucLoginUser.addCategoryWindow.Width = 500;
            //    ucLoginUser.addCategoryWindow.Height = 500;
            //    btnViewSpecific.Visibility = Visibility.Visible;
            //    btnViewAll.Visibility = Visibility.Collapsed;
            //    ucLoginUser.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //    ucLoginUser.addCategoryWindow.Content = ucLoginUser;
            //    ucLoginUser.addCategoryWindow.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("Please upload First");
            //}
        }
        private void LoadGroupData()
        {
            try
            {
                string path = @"";
                var group = grdCntrlUploadedDetail.SelectedItem as BackgroundImages;
                
                if (group != null)
                {
                   BackgroundImagesRepo taskRepo = new BackgroundImagesRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var groups = taskRepo.GetImageForPreview(group.Id); 
                    if (groups != null)
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

                                    
                                    grdPreview.Children.Clear();
                                    Image img = new Image();
                                    
                                    img.Stretch = Stretch.Fill;
                                    img.Name = "imgDynamic1";
                                    RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                                    grdPreview.Children.Add(img);

                                    Grid grid = (Grid)grdPreview; //get grid and grid name is favouriteitem
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
                        grdPreview.Children.Clear();
                    }



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void grdCntrlUploadedDetail_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            LoadGroupData();
        }

        private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

         
        }
        private void UpdateViewBox(int newValue)
        {
            if ((ZoomViewbox.Width > 0) && ZoomViewbox.Height > 0)
            {
                ZoomViewbox.Width += newValue;
                ZoomViewbox.Height += newValue;
                //if (ZoomViewbox.Height != 0)
                //{
                //    ZoomViewbox.Height += newValue;
                //}
                //else
                //{
                //    if (newValue == 5)
                //    {
                //        ZoomViewbox.Width += newValue;
                //        ZoomViewbox.Height += newValue;
                //    }
                //    else
                //    {

                //    }
                //}


                return;
            }
           if(ZoomViewbox.Height == 0)
            {
                if(newValue == 5)
                {
                    ZoomViewbox.Height += newValue; //comment added
                    
                }
            }
           if(ZoomViewbox.Width == 0)
            {
                if (newValue == 5)
                {
                    ZoomViewbox.Width += newValue;
                
                }
            }

        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if (Keyboard.Modifiers != ModifierKeys.Control)
            //    return;
            UpdateViewBox((e.Delta > 0) ? 5 : -5); 
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
         {

            if (scale == true)
            {
                ZoomViewbox.Width = 875;
                ZoomViewbox.Height = 715;
                scale = false;
                //Grid grid = new Grid();
                //Viewbox viewBox = new Viewbox();
                //viewBox.Stretch = Stretch.Uniform;
                //viewBox.Width = 875;
                //viewBox.Height = 715;
                //viewBox.Child = grid;

                //grdPreview.Children.Add(viewBox);
            }
            else
            {
                ZoomViewbox.Width = 420;
                ZoomViewbox.Height = 340;
                scale = true;
                //Grid grid = new Grid();
                //Viewbox viewBox = new Viewbox();
                //viewBox.Stretch = Stretch.Uniform;
                //viewBox.Width = 420;
                //viewBox.Height = 340;
                //viewBox.Child = grid;
                //grdPreview.Children.Add(viewBox);
            }
       

        }
    }
}
