using DevExpress.Xpf.Core;
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
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.Offerss
{
    /// <summary>
    /// Interaction logic for winUniqueNumberList.xaml
    /// </summary>
    public partial class winUniqueNumberList : Window
    {
        OfferRepo offerRepo = new OfferRepo();
        public winUniqueNumberList()
        {
            InitializeComponent();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unique Number") != null)
            {
                frmUniqueNumber frmFocSampling = new frmUniqueNumber((grdUniqueNumberList.SelectedItem as UniqueNumber).Id);
                frmFocSampling.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission Edit Unique Number", "Information");
            }

        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Unique Number") != null)
            {
                frmUniqueNumber frmUniqueNumber = new frmUniqueNumber();
                frmUniqueNumber.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission Add Unique Number", "Information");
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            offerRepo = new OfferRepo();
            grdUniqueNumberList.ItemsSource = offerRepo.GetAllUniqueNumbers();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grdUniqueNumberList.ItemsSource = offerRepo.GetAllUniqueNumbers();
        }
    }
}
