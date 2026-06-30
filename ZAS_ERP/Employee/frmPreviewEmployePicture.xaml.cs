using DevExpress.Xpf.Editors;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.Windows.Shapes;


namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for frmPreviewEmployePicture.xaml
    /// </summary>
    public partial class frmPreviewEmployePicture : Window
    {
        EmployeeRepo emprepo = new EmployeeRepo();
        ERP_BL.Databases.Employee emp = new ERP_BL.Databases.Employee();
        public frmPreviewEmployePicture()
        {
            InitializeComponent();
        }
        public frmPreviewEmployePicture(int editEmpId, int value)
        {
            InitializeComponent();
            emp = emprepo.GetEmployee(editEmpId);
            //Load User Photo
            if(value == 0)
            {
               
                //if (emp.person.EmployeePhoto != null)
                //{
                //    var byteImg = emp.person.EmployeePhoto;
                //    if (byteImg != null)
                //    {
                //        var image = GetBitmapImageFromByteArray(byteImg);
                //        empImage.Source = image;
                //    }

                //}
            }
            if(value == 1)
            {
                //empImage.Source = null;
                //if (emp.person.EmployeePhotoLeft != null)
                //{
                //    var byteImg = emp.person.EmployeePhotoLeft;
                //    if (byteImg != null)
                //    {
                //        var image = GetBitmapImageFromByteArray(byteImg);
                //        empImage.Source = image;
                //    }

                //}
            }
            if (value == 2)
            {
                //empImage.Source = null;
                //if (emp.person.EmployeePhotoRight != null)
                //{
                //    var byteImg = emp.person.EmployeePhotoRight;
                //    if (byteImg != null)
                //    {
                //        var image = GetBitmapImageFromByteArray(byteImg);
                //        empImage.Source = image;
                //    }

                //}
            }
            if (value == 3)
            {
                //empImage.Source = null;
                //if (emp.person.EmployeePhotoFront != null)
                //{
                //    var byteImg = emp.person.EmployeePhotoFront;
                //    if (byteImg != null)
                //    {
                //        var image = GetBitmapImageFromByteArray(byteImg);
                //        empImage.Source = image;
                //    }

                //}
            }
            if (value == 4)
            {
                //empImage.Source = null;
                //if (emp.person.EmployeePhotoBack != null)
                //{
                //    var byteImg = emp.person.EmployeePhotoBack;
                //    if (byteImg != null)
                //    {
                //        var image = GetBitmapImageFromByteArray(byteImg);
                //        empImage.Source = image;
                //    }

                //}
            }
            if (value == 5)
            {
                //empImage.Source = null;
                //if (emp.person.EmployeePhotoUp != null)
                //{
                //    var byteImg = emp.person.EmployeePhotoUp;
                //    if (byteImg != null)
                //    {
                //        var image = GetBitmapImageFromByteArray(byteImg);
                //        empImage.Source = image;
                //    }

                //}
            }


        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
       
        }
        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        
    }
}
