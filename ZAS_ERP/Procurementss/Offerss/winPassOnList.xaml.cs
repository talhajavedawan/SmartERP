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
    /// Interaction logic for winPassOnList.xaml
    /// </summary>
    public partial class winPassOnList : DXWindow
    {
        OfferRepo offerRepo = new OfferRepo();
        public winPassOnList()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlPassOnList.ItemsSource = offerRepo.GetAllPassOns();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit PassOn") != null)
            {
                frmPassOn frmPassOn = new frmPassOn((grdCntrlPassOnList.SelectedItem as PassOn).Id);
                frmPassOn.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Edit Edit PassOn", "Information");
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            offerRepo = new OfferRepo();
            grdCntrlPassOnList.ItemsSource = offerRepo.GetAllPassOns();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PassOn") != null)
            {
                frmPassOn frmPassOn = new frmPassOn();
                frmPassOn.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission Add Add PassOn", "Information");
            }
        }
    }
}
