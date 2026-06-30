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

using ERP_BL.Config;
using ERP_BL.Enums;
using DevExpress.Xpf.Core;

namespace ZAS_ERP.Procurementss.Inquiriess
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmInquiryStatusList : Window
    {


        List<InquiryStatus> InquiryStatuss= new List<InquiryStatus>();
        InquiryRepo repo = new InquiryRepo();

        //InquiryStatus InquiryStatus = new InquiryStatus();
        public frmInquiryStatusList()
        {
            InitializeComponent();
            
            
        }

        private void winInquiryStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadInquiryStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdInquiryStatus);


        }



        private void loadInquiryStatus()
        {
           
            if (MainWindow.currentUserid == 0)
                InquiryStatuss = repo.getAllInquiryStatus();
            
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inquiry Statuses") != null)
            {
                InquiryStatuss = repo.getAllInquiryStatus();
            }
            else
            {
                InquiryStatuss = repo.getAllActiveStatus();
            }
            //InquiryStatuss = repo.getAllActiveInquiryStatus();

            
            this.grdInquiryStatus.ItemsSource = InquiryStatuss;
            grdInquiryStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdInquiryStatus.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdInquiryStatus.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newInquiryStatus()
        {
            frmInquiryStatussAdd frmInquiryStatussadd = new frmInquiryStatussAdd();
            frmInquiryStatussadd.ShowDialog();
            
            loadInquiryStatus();
        }
        

        private void mbtnNewInquiryStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry Status") != null)
            {
                newInquiryStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to add new Status!");
            }
            

        }

        private void mbtnEditInquiryStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inquiry Status") != null)
            {
                if (grdInquiryStatus.SelectedItem != null)
                {

                    Inquiriess.frmInquiryStatussAdd.StatusId = (grdInquiryStatus.SelectedItem as InquiryStatus).Id;

                    newInquiryStatus();
                }
                else
                {
                    MessageBox.Show("Please select a Inquiry Payment Status to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to update existing Status!");
            }
            
        }



        private void grdInquiryStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inquiry Status") != null)
            {
                Inquiriess.frmInquiryStatussAdd.StatusId = (grdInquiryStatus.SelectedItem as InquiryStatus).Id;
                newInquiryStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to update existing Status!");
            }
            
        }

        private void WinInquiryStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdInquiryStatus);
        }
        //int empid;



    }
}
