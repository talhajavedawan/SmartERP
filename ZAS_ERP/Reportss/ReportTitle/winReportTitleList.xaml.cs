using DevExpress.Xpf.Core;
using ERP_BL.Reports;
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

namespace ZAS_ERP.Reportss.ReportTitle
{
    /// <summary>
    /// Interaction logic for winReportTitleList.xaml
    /// </summary>
    public partial class winReportTitleList : DXWindow
    {
        GridReportRepo repo = new GridReportRepo();

        public winReportTitleList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
        
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Standard Title List") == null)
            {
                tabBtnStandardTitle.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Memorized Title List") == null)
            {
                tabBtnMemorizedTitles.Visibility = Visibility.Collapsed;
            }
            grdStandardTitles.ItemsSource= repo.GetReportStandardTitles();
            grdMemorizedTitles.ItemsSource= repo.GetReportMemorizedTitles(SYSTEM_STATIC.currentUser.id);
        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //repo = new GridReportRepo();
            //grdCntrlTitleNameList.ItemsSource = repo.GetReportTitles();
        }

        private void mbtnStandardUpdate_Click(object sender, RoutedEventArgs e)
        {
            //if (grdCntrlTitleNameList.SelectedItem != null)
            //{

            //    var selectedTitle = grdCntrlTitleNameList.SelectedItem as ERP_BL.Reports.ReportTitle;
            //    frmGridReportTitle title = new frmGridReportTitle(selectedTitle.Id);
            //    title.Show();
            //}
        }

        private void MbtnStandardAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Standard Titles") != null)
            {
                frmGridReportTitle title = new frmGridReportTitle();
                title.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("You are not allowed to Add Standard Title ");
            }
        }

        private void mbtnNewStandardTitle_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Standard Titles") != null)
            {
                frmGridReportTitle title = new frmGridReportTitle();
                title.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("You are not allowed to Add Standard Title ");
            }
        }

        private void btnNewStandardTitle_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Standard Titles") != null)
            {
                frmGridReportTitle title = new frmGridReportTitle();
                title.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("You are not allowed to Add Standard Title ");
            }
        }
    

        private void btnEditStandardTitle_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Standard Titles") != null)
            {
                if (grdStandardTitles.SelectedItem != null)
                {
                    var selectedTitle = grdStandardTitles.SelectedItem as ERP_BL.Reports.ReportTitle;
                    frmGridReportTitle title = new frmGridReportTitle(selectedTitle.Id);
                    title.Show();
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("You are not allowed to Edit Standard Title ");
            }
        }

        private void mbtnNewMemorizedTitle_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorized Titles") != null)
            {
                frmGridReportTitle title = new frmGridReportTitle();
                title.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("You are not allowed to Add Memorized Title ");
            }
        }

        private void btnNewMemorizedTitle_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorized Titles") != null)
            {
                frmGridReportTitle title = new frmGridReportTitle();
                title.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("You are not allowed to Add Memorized Title ");
            }
        }

        private void btnEditMemorizedTitle_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Memorized Titles") != null)
            {
                if (grdMemorizedTitles.SelectedItem != null)
                {
                    var selectedTitle = grdMemorizedTitles.SelectedItem as ERP_BL.Reports.ReportTitle;
                    frmGridReportTitle title = new frmGridReportTitle(selectedTitle.Id);
                    title.Show();
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("You are not allowed to Edit Memorized Titles ");
            }
        }
    }
}
