using ERP_BL.Databases;
using System;
using System.Collections.Generic;
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

namespace ZAS_ERP.Userss
{
    /// <summary>
    /// Interaction logic for ucLoginUserDetail.xaml
    /// </summary>
    public partial class ucLoginUserDetail : UserControl
    {
        public Window addCategoryWindow = new Window();
        public List<LoginUserDetails> login = new List<LoginUserDetails>();
        public ucLoginUserDetail()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {    
            //var LastFiveRowsRecords = Enumerable.Reverse(login.Take(3).Reverse().ToList());
            grdCntrlUserDetail.ItemsSource = login;
        } 
    }
}
