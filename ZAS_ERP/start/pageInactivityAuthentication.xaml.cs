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

namespace ZAS_ERP.start
{
    /// <summary>
    /// Interaction logic for pageInactivityAuthentication.xaml
    /// </summary>
    public partial class pageInactivityAuthentication : Page
    {
        public pageInactivityAuthentication()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public void ConvertByteToBmp(byte[] bytesArr)
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

                UserImage.Source = returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
