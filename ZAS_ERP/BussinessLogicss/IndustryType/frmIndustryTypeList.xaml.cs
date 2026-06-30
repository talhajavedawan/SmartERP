using DevExpress.Xpf.Grid;
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
using ERP_BL;

namespace ZAS_ERP
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmIndustryTypeList : Window
    {


        public bool addnew;

        CompanyRepo repo = new CompanyRepo();
        List<IndustryType> industryTypess = new List<IndustryType>();
        IndustryType industryType = new IndustryType();
        public frmIndustryTypeList()
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
            industryTypess = repo.GetIndustryTypes();
            this.grdIndustryType.ItemsSource = industryTypess;
            grdIndustryType.Columns.GetColumnByFieldName("isActive").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("isApproved").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user").Visible = false;
            addnew = false;


        }


        public void newIndustryType()
        {
            frmIndustryTypeAdd frmIndustryTypeadd = new frmIndustryTypeAdd();
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
                frmIndustryTypeAdd.industryTypeId = (grdIndustryType.SelectedItem as IndustryType).Id;
                newIndustryType();
            }
            else
            {
                MessageBox.Show("Please select a IndustryType to Edit");
            }
        }

        private void grdIndustryType_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmIndustryTypeAdd.industryTypeId = (grdIndustryType.SelectedItem as Product).Id;
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
            industryTypess = repo.GetVoidIndustryTypes();
            this.grdIndustryType.ItemsSource = industryTypess;
            lblIndustryType.Text = "Void Industry Types";
            grdIndustryType.Columns.GetColumnByFieldName("isActive").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("isApproved").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user").Visible = false;
            addnew = false;
        }

        private void mbtnIndustryType_Click(object sender, RoutedEventArgs e)
        {
            repo = new CompanyRepo();
            industryTypess = repo.GetIndustryTypes();
            this.grdIndustryType.ItemsSource = industryTypess;
            lblIndustryType.Text = "Industry Types";
            grdIndustryType.Columns.GetColumnByFieldName("isActive").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("isApproved").Visible = false;
            grdIndustryType.Columns.GetColumnByFieldName("user").Visible = false;
            addnew = false;
        }
    }
}
