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
using DevExpress.Xpf.Core;
using ERP_BL;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Reports;

namespace ZAS_ERP.Reportss
{
    /// <summary>
    /// Interaction logic for frmReportGroup.xaml
    /// </summary>
    public partial class frmReportGroup : DXWindow
    {
        public GridReportType groupType;
        GridReportRepo repo = new GridReportRepo();
        public GridReportGroup gridReportGroup = new GridReportGroup();
        GridReportGroup parentGroup = new GridReportGroup();
        public frmReportGroup()
        {
            InitializeComponent();
        }
        public frmReportGroup(GridReportGroup group)
        {
            InitializeComponent();
            gridReportGroup = group;
        }
        private void btnreportGroupSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    if (gridTransactionType.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please select Grid Group type");
                        return;
                    }
                    else
                    if(gridTransactionType.SelectedIndex==-1)
                    {
                        DXMessageBox.Show("Please select Transaction type");
                        return;
                    }
                    else
                    if(lookupParentGroup.SelectedIndex==-1)
                    {
                        DXMessageBox.Show("Please select Parent Group");
                        return;
                    }
                    else
                    if (string.IsNullOrEmpty(txtGroupName.Text))
                    {
                        DXMessageBox.Show("Please input group name");
                        return;
                    }
                    else
                    {
                       
                     var transactionType = gridTransactionType.SelectedItem.ToString();
                    switch (transactionType)
                    {
                        case "Inquiry":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Inquiry;
                                break;
                            }
                        case "Offer":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Offer;
                                break;
                            }
                        case "Sale_Order":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Sale_Order;
                                break;
                            }
                        case "Sale_Invoice":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Sale_Invoice;
                                break;
                            }
                        case "Purchase_Order":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Purchase_Order;
                                break;
                            }
                        case "Purchase_Invoice":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Purchase_Invoice;
                                break;
                            }
                        case "Bill":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Bill;
                                break;
                            }
                        case "Sale_Receipt":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Sale_Receipt;
                                break;
                            }
                        case "InterBank_Transfer":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.InterBank_Transfer;
                                break;
                            }
                        case "Admin_Bill":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Admin_Bill;
                                break;
                            }
                        case "InterCompanyBank_Transfer":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.InterCompanyBank_Transfer;
                                break;
                            }
                        case "Payments":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Payments;
                                break;
                            }
                        case "Targets":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Targets;
                                break;
                            }
                        case "Trial_Balance":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Trial_Balance;
                                break;
                            }
                        case "TargetReward":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.TargetReward;
                                break;
                            }
                        case "Chart_of_Account":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Chart_of_Account;
                                break;
                            }
                        case "STL":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.STL;
                                break;
                            }
                        case "Tasks":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Tasks;
                                break;
                            }
                        case "Loans":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Loans;
                                break;
                            }
                        case "Advances":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Advances;
                                break;
                            }
                        case "Assets":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.Assets;
                                break;
                            }
                        case "CashFlow":
                            {
                                gridReportGroup.transactionType = ReportTransactionType.CashFlow;
                                break;
                            }
                    }
                    gridReportGroup.groupName = txtGroupName.Text;
                    gridReportGroup.isActive = true;
                    if (gridReportTypes.SelectedIndex == 0)
                    {
                        gridReportGroup.gridReportType = GridReportType.StandardReport;
                    }
                    else
                    if (gridReportTypes.SelectedIndex == 1)
                    {
                        gridReportGroup.gridReportType = GridReportType.MemorizedReport;
                        gridReportGroup.userId = SYSTEM_STATIC.currentUser.id;
                    }
                    if(lookupParentGroup.SelectedIndex>-1)
                    gridReportGroup.parentId=(lookupParentGroup.SelectedItem as GridReportGroup).Id;
                    }
                    
                if (gridReportGroup.Id == 0)
                {
                    repo.SaveGroup(gridReportGroup);
                    DXMessageBox.Show("Group has been Added Successfully");

                }
                else
                {
                    repo.UpdateGroup(gridReportGroup);
                    DXMessageBox.Show("Group has been Updated Successfully");

                }
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public  void LoadGridReportTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.GridReportType.MemorizedReport; i++)
            {
                gridReportTypes.Items.Add(((ERP_BL.Enums.GridReportType)i).ToString());
            }
           
        }
        private void lookupGroup_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
           
        }
        private void GridReportTypes_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            loadGroups();
        }
        private void winReportGroupAdd_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGridReportTypes();
            LoadTransactionTypes();
            if(gridReportGroup.Id!=0)
            {
                gridReportTypes.SelectedItem = gridReportGroup.gridReportType.ToString();
                gridTransactionType.SelectedItem = gridReportGroup.transactionType.ToString();
                txtGroupName.Text = gridReportGroup.groupName;
                if(gridReportGroup.parent!=null)
                lookupParentGroup.Text = gridReportGroup.parent.groupName;
            }
        }

        private void gridTransactionType_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            loadGroups();
        }
        public void LoadTransactionTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.ReportTransactionType.CashFlow; i++)
            {
                if (((ERP_BL.Enums.ReportTransactionType)i).ToString()== "Inquiry" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null )
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Sale_Order")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null &&  ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Offer")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Bill")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Purchase_Order")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Sale_Invoice")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Sale_Receipt")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "InterBank_Transfer")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Admin_Bill")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "InterCompanyBank_Transfer")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Payments")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Purchase_Invoice")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Chart_of_Account")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Targets")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "TargetReward")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Trial_Balance")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "STL")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Tasks")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Advances")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Loans") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Loans")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Assets")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "CashFlow")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
            }
        }
        public void loadGroups()
        {
            if (gridTransactionType.SelectedIndex != -1 && gridReportTypes.SelectedIndex!=-1)
            {
                GridReportRepo gridReportRepo = new GridReportRepo();
                var groups = new List<GridReportGroup>();
                var transactionType = gridTransactionType.SelectedItem.ToString();
                if (gridReportTypes.SelectedIndex == 0)
                {
                    groupType = GridReportType.StandardReport;

                }
                else
                if (gridReportTypes.SelectedIndex == 1)
                {
                    groupType = GridReportType.MemorizedReport;
                }
                switch (transactionType)
                {
                    case "Inquiry":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Inquiry, groupType,SYSTEM_STATIC.currentUser.id);
                             
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Inquiry, groupType);
                                
                            }
                            break;
                        }
                    case "Offer":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Offer, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Offer, groupType);

                            }
                            break;
                        }
                    case "Sale_Order":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Order, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Order, groupType);

                            }
                            break;
                        }
                    case "Sale_Invoice":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Invoice, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Invoice, groupType);
                            }
                            break;
                        }
                    case "Purchase_Order":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Order, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Order, groupType);

                            }
                            break;
                        }
                    case "Purchase_Invoice":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Invoice, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Invoice, groupType);
                            }
                            break;
                        }
                    case "Bill":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Bill, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Bill, groupType);

                            }
                            break;
                        }
                    case "Sale_Receipt":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Receipt, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Bill, groupType);

                            }
                            break;
                        }
                    case "InterBank_Transfer":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterBank_Transfer, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterBank_Transfer, groupType);

                            }
                            break;
                        }
                    case "Admin_Bill":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Admin_Bill, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Admin_Bill, groupType);

                            }
                            break;
                        }
                    case "InterCompanyBank_Transfer":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterCompanyBank_Transfer, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterCompanyBank_Transfer, groupType);
                            }
                            break;
                        }
                    case "Payments":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Payments, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Payments, groupType);

                            }
                            break;
                        }
                    case "Targets":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Targets, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Targets, groupType);

                            }
                            break;
                        }
                    case "Trial_Balance":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Trial_Balance, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Trial_Balance, groupType);

                            }
                            break;
                        }
                    case "TargetReward":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.TargetReward, groupType,SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.TargetReward, groupType);

                            }
                            break;
                        }
                    case "Chart_of_Account":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Chart_of_Account, groupType,SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Chart_of_Account, groupType);
                            }
                            break;
                        }
                    case "STL":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.STL, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.STL, groupType);
                            }
                            break;
                        }
                    case "Tasks":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Tasks, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Tasks, groupType);
                            }
                            break;
                        }
                    case "Advances":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Advances, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Advances, groupType);
                            }
                            break;
                        }
                    case "Loans":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Loans, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Loans, groupType);
                            }
                            break;
                        }
                    case "Assets":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Assets, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Assets, groupType);
                            }
                            break;
                        }
                    case "CashFlow":
                        {
                            if (groupType == GridReportType.MemorizedReport)
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.CashFlow, groupType, SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                lookupParentGroup.ItemsSource = gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.CashFlow, groupType);
                            }
                            break;
                        }
                }
                btnreportGroupSave.IsEnabled = true;
                lookupParentGroup.IsEnabled = true;
            }
        }
    }
}
