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
using ERP_BL.Reports;
using DevExpress.Xpf.Core;
using DevExpress.Data.Filtering.Helpers;

namespace ZAS_ERP.Reportss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmReportGroupList : DXWindow
    {

        List<GridReportGroup> reportGroups = new List<GridReportGroup>();
        GridReportRepo repo = new GridReportRepo();
        List<GridReport> products = new List<GridReport>();
        GridReport product = new GridReport();
        
        public frmReportGroupList()
        {
            InitializeComponent();

            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to View Memorized Groups") != null) ? false : true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to View Standard Groups") != null) ? false : true)
            {
                tabBtnStandardReport.Visibility = Visibility.Collapsed;
                tabBtnMemorized.Visibility = Visibility.Collapsed;
                grdStandardGroups.Visibility = Visibility.Collapsed;
                grdMemorized.Visibility = Visibility.Collapsed;
                DXMessageBox.Show("You don't have permission to view report Groups!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to View Standard Groups") != null) ? false : true)
            {
                //DXMessageBox.Show("You don't have permission to view Standard Groups!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Stop);

                tabBtnStandardReport.Visibility = Visibility.Collapsed;
                grdStandardGroups.Visibility = Visibility.Collapsed;
            }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to View Memorized Groups") != null) ? false : true)
            {
                //DXMessageBox.Show("You don't have permission to view Memorized Groups!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Stop);
                tabBtnMemorized.Visibility = Visibility.Collapsed;
                grdMemorized.Visibility = Visibility.Collapsed;
            }
            
        }
        public void loadGroups()
        {

            loadMemorizedGroups();
            loadStandardGroups();

        }

        private void winGroupsList_Loaded(object sender, RoutedEventArgs e)
        {
            loadGroups();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdGroups);
        }

        private void WinGroupsList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdGroups);

        }

        public void newReportGroup()
        {
            Reportss.frmReportGroup productReportGroupAdd = new Reportss.frmReportGroup();
            productReportGroupAdd.ShowDialog(); 



            if (productReportGroupAdd.gridReportGroup != null && productReportGroupAdd.gridReportGroup.groupName!=null && productReportGroupAdd.gridReportGroup.gridReportType!=null&& productReportGroupAdd.gridReportGroup.parent!=null&& productReportGroupAdd.gridReportGroup.parentId!=null)
            {
                GridReportRepo repo = new GridReportRepo();
                repo.AddGridReportGroup(productReportGroupAdd.gridReportGroup);
                DXMessageBox.Show("New Report Group (" + productReportGroupAdd.gridReportGroup.groupName + ") Added!","Congratulations!",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            loadGroups();
        }
        private void mbtnNewReportGroup_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Add new Standard Group") != null) ? true : false)
            {
                newReportGroup();
            }
            else
                DXMessageBox.Show("You don't have permission to add new Group!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Stop);
            
        }
        public void EditStndardGroup ()
        {
            var group = (GridReportGroup)grdGroups.SelectedItem;
            if (group != null)
            {
                if (group.parent == null || group.parentId == null)
                {
                    DXMessageBox.Show("Please select child Group to edit", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
                else
                {
                    Reportss.frmReportGroup productReportGroupAdd = new Reportss.frmReportGroup(group);
                    productReportGroupAdd.ShowDialog();
                    loadGroups();
                }
            }
        }
        public void EditMemorizedGroups()
        {
            var group = (GridReportGroup)grdMemorizedGroups.SelectedItem;
            if (group != null)
            {
                if (group.parent == null || group.parentId == null)
                {
                    DXMessageBox.Show("Please select child Group to edit", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
                else
                {
                    Reportss.frmReportGroup productReportGroupAdd = new Reportss.frmReportGroup(group);
                    productReportGroupAdd.ShowDialog();
                    loadGroups();
                }
            }
        }
        private void mbtnEditReportGroup_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Edit Standard Group") != null) ? true : false)
            {
                EditStndardGroup();
            }
            else
                DXMessageBox.Show("You don't have permission to edit Standard Groups!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Stop);


        }

        public void editReportGroup( GridReportGroup group)
        {
            Reportss.frmReportGroup productReportGroupAdd = new Reportss.frmReportGroup();
            //productReportGroupAdd.loadGroups();
            //productReportGroupAdd.LoadGridReportTypes();
            //productReportGroupAdd.gridReportGroup = group;
            //productReportGroupAdd.txtreportGroup.Text = group.groupName;
            //productReportGroupAdd.gridReportTypes.EditValue = group.gridReportType;
            //productReportGroupAdd.lookupGroup.EditValue = group.parent.groupName;
            productReportGroupAdd.ShowDialog();

            if (productReportGroupAdd.gridReportGroup != null)
            {
                GridReportRepo repo = new GridReportRepo();
                repo.UpdateReportGroup(productReportGroupAdd.gridReportGroup);
                DXMessageBox.Show(" Group Renamed with"+"("+group.groupName+ ")"+" successfully", "New Group Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
  
        private void loadStandardGroups()
        {
            GridReportRepo gridReportRepo = new GridReportRepo();
            List<GridReportGroup> standardGroups = new List<GridReportGroup>();
            var groupType = GridReportType.StandardReport;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Inquiry, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Order, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Offer, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Bill, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Order, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Invoice, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Receipt, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterBank_Transfer, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Admin_Bill, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterCompanyBank_Transfer, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Payments, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Invoice, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Chart_of_Account, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Targets, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.TargetReward, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Trial_Balance, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.STL, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Tasks, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Advances, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Company Loans") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Loans, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Assets, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.CashFlow, groupType));
            }
            this.grdGroups.ItemsSource = standardGroups;
            grdGroups.Columns["Id"].Visible = false;
            grdGroups.Columns["parentId"].Visible = false;
            grdGroups.Columns["isActive"].Visible = false;
            grdGroups.Columns["parent"].Visible = false;
            grdGroups.Columns["user"].Visible = false;
            grdGroups.Columns["userId"].Visible = false;
            grdGroups.Columns["gridReportType"].Visible = false;
            grdGroups.Columns["groupName"].MinWidth = 400;
            this.grdGroups.AutoExpandAllGroups = false;

        }

        private void loadMemorizedGroups()
        {
            List<GridReportGroup> memorizedGroups = new List<GridReportGroup>();
            GridReportRepo gridReportRepo = new GridReportRepo();
            var groupType = GridReportType.MemorizedReport;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Inquiry, groupType,SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Order, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Offer, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Bill, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Order, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Invoice, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Receipt, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterBank_Transfer, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Admin_Bill, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterCompanyBank_Transfer, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Payments, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Invoice, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Chart_of_Account, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Targets, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.TargetReward, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Trial_Balance, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.STL, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Tasks, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Advances, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Company Loans") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Loans, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Assets, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.CashFlow, groupType, SYSTEM_STATIC.currentUser.id));
            }
            this.grdMemorizedGroups.ItemsSource = memorizedGroups;
            grdMemorizedGroups.Columns["Id"].Visible = false;
            grdMemorizedGroups.Columns["parentId"].Visible = false;
            grdMemorizedGroups.Columns["isActive"].Visible = false;
            grdMemorizedGroups.Columns["parent"].Visible = false;
            grdMemorizedGroups.Columns["user"].Visible = false;
            grdMemorizedGroups.Columns["userId"].Visible = false;
            grdMemorizedGroups.Columns["gridReportType"].Visible = false;
            grdMemorizedGroups.Columns["groupName"].MinWidth = 400;
        }

        private void BtnEditMemorizedGroups_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Edit Memorized Groups") != null) ? true : false)
            {
                EditMemorizedGroups();
            }
            else
                DXMessageBox.Show("You don't have permission to edit Memorizd Groups!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Stop);

        }

        private void MbtnNewMemorizedReportGroup_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Add new Memorized Group") != null) ? true : false)
            {
                newReportGroup();
            }
            else
                DXMessageBox.Show("You don't have permission to add new Group!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        private void BtnStandardDeleteGroups_Click(object sender, RoutedEventArgs e)
        {
            var group = (GridReportGroup)grdGroups.SelectedItem;
            var reports = repo.GetALLReportsbyGroupId(group.Id);

            if (group != null)
            {
                if (group.parent == null || group.parentId == null)
                {
                    DXMessageBox.Show("Please select child Group to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
                else
                {
                    if (reports.Count == 0)
                    {
                        repo.DeleteGroup(group);
                        loadGroups();
                        return;
                    }
                    else
                        DXMessageBox.Show("Please Delete Reports containing group " + "(" + group.groupName + ")", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    this.Close();

                }
            }
        }
        private void BtnMemorizedDeleteGroups_Click(object sender, RoutedEventArgs e)
        {
            var group = (GridReportGroup)grdMemorizedGroups.SelectedItem;
            var reports = repo.GetALLReportsbyGroupId(group.Id);

            if (group != null)
            {
                if (group.parent == null || group.parentId == null)
                {
                    DXMessageBox.Show("Please select child Group to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
                else
                {
                    if (reports.Count == 0)
                    {
                        repo.DeleteGroup(group);
                        loadGroups();
                        return;
                    }
                    else
                        DXMessageBox.Show("Please Delete Reports containing group " + "(" + group.groupName + ")", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    this.Close();

                }
            }
        }
    }
}
