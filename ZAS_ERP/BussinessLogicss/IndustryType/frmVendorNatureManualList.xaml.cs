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

namespace ZAS_ERP.BussinessLogicss.IndustryType
{
    /// <summary>
    /// Interaction logic for frmVendorNatureManualList.xaml
    /// </summary>
    public partial class frmVendorNatureManualList : DXWindow
    {
        public bool addnew;

        CompanyRepo repo = new CompanyRepo();
        List<VendorNatureManual> industryTypess = new List<VendorNatureManual>();
        VendorNatureManual industryType = new VendorNatureManual();
        public frmVendorNatureManualList()
        {
            InitializeComponent();
        }
        private void winIndustryType_Loaded(object sender, RoutedEventArgs e)
        {
            loadIndustryTypegrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdIndustryType);
        }

        private void loadIndustryTypegrid()
        {
            repo = new CompanyRepo();
            industryTypess = repo.GetVendorNaturesManuals();
            this.grdIndustryType.ItemsSource = industryTypess;
            grdIndustryType.Columns.GetColumnByFieldName("isActive").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("isApproved").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user").Visible = false;
            addnew = false;
        }
        public void newIndustryType()
        {
            frmVendorNatureManual frmIndustryTypeadd = new frmVendorNatureManual();
            frmIndustryTypeadd.ShowDialog();
            loadIndustryTypegrid();
        }


        private void mbtnNewIndustryType_Click(object sender, RoutedEventArgs e)
        {
            newIndustryType();

        }

        private void mbtnEditIndustryType_Click(object sender, RoutedEventArgs e)
        {
            if (grdIndustryType.SelectedItem != null)
            {
                frmVendorNatureManual.industryTypeId = (grdIndustryType.SelectedItem as VendorNatureManual).Id;
                newIndustryType();
            }
            else
            {
                MessageBox.Show("Please select a VendorNature to Edit");
            }
        }

        private void grdIndustryType_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmVendorNatureManual.industryTypeId = (grdIndustryType.SelectedItem as VendorNatureManual).Id;
            newIndustryType();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WinIndustryType_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdIndustryType);
        }

        private void mbtnVoidList_Click(object sender, RoutedEventArgs e)
        {
            repo = new CompanyRepo();
            industryTypess = repo.GetVoidVendorNatureManual();
            this.grdIndustryType.ItemsSource = industryTypess;
            lblIndustryType.Text = "Void Vendor Nature Types";
            grdIndustryType.Columns.GetColumnByFieldName("isActive").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("isApproved").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user").Visible = false;
            addnew = false;
        }

        private void mbtnIndustryType_Click(object sender, RoutedEventArgs e)
        {
            repo = new CompanyRepo();
            industryTypess = repo.GetIndustryTypesManuals();
            this.grdIndustryType.ItemsSource = industryTypess;
            lblIndustryType.Text = "Vendor Nature Types";
            grdIndustryType.Columns.GetColumnByFieldName("isActive").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("isApproved").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user").Visible = false;
            addnew = false;
        }
    }
}
