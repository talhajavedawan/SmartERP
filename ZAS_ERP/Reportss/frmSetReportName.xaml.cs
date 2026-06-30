using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Migrations;
using ERP_BL.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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


namespace ZAS_ERP.Reportss
{
    /// <summary>
    /// Interaction logic for frmSetReportName.xaml
    /// </summary>
    public partial class frmSetReportName : DXWindow
    {
        public string onlychar { get; set; }
        public static string ReportName;

        public static GridReportType ReportType;

        public string gridControlName;
        public ERP_BL.CashFlow.CashFlow cashFlow = new ERP_BL.CashFlow.CashFlow();

        public static GridReportGroup gridReportGroup;

        public GridReport report = new GridReport();

        public GridReportGroup editableGroup;

        GridReportRepo repo = new GridReportRepo();

        public frmSetReportName()
        {
            InitializeComponent();
            //GetEnum();
            //ReportType= (ERP_BL.Enums.GridReportType)reportTypes.EditValue;
            //viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            //grdDepartments.SelectionChanged += OnGridSelectionChanged;
        }

        public frmSetReportName(string name)
        {
            InitializeComponent();
            gridControlName = name;
            //GetEnum();
        }
        public bool CheckEmptyFields()
        {
            if(txtName.Text=="")
            {
                DXMessageBox.Show("Please enter Report name", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                txtName.Focus();
                return false;
            }
            if (reportTypes.Text == "")
            {
                DXMessageBox.Show("Please Select Report type", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            else if (lookupGroup.Text == "")
            {
                MessageBox.Show("Please Select Report Group", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            else
                if (gridReportGroup.parent == null)
            {
                MessageBox.Show("Please Select different level Group", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                return false;
            }
            else
                return true;
        }

        public void InitialCreation()
        {
            GetEnum();


            report.reportName = txtName.Text;
            report.gridReportType = ReportType;
            report.gridReportGroup = gridReportGroup;
            report.userId = SystemLog.CurrentUserId;
            if(lookupReportTitle.SelectedIndex>-1)
            {
                report.titleId = (lookupReportTitle.SelectedItem as ERP_BL.Reports.ReportTitle).Id;
            }
        }

        public void LoadReport()
        {  
            if(editableGroup!=null)
            { gridReportGroup = editableGroup; }

            if(cashFlow.Id!=0)
            {
                cashFlow.reportName = txtName.Text;
                GetEnum();
                cashFlow.gridReportType = ReportType;
                cashFlow.gridReportGroup = gridReportGroup;
                cashFlow.userId = SystemLog.CurrentUserId;
                if (lookupReportTitle.SelectedIndex > -1)
                {
                    report.titleId = (lookupReportTitle.SelectedItem as ERP_BL.Reports.ReportTitle).Id;
                }
            }
            else
            {
                report.reportName = txtName.Text;
                GetEnum();
                report.gridReportType = ReportType;
                report.gridReportGroup = gridReportGroup;
                report.lastModified = DateTime.Now;
                report.userId = SystemLog.CurrentUserId;
                if (lookupReportTitle.SelectedIndex > -1)
                {
                    report.titleId = (lookupReportTitle.SelectedItem as ERP_BL.Reports.ReportTitle).Id;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            
            if (report.Id == 0)
            {
                var key=CheckEmptyFields();
                if (key == false)
                    return;
                InitialCreation();
            }
            else
            {
                LoadReport();     
            }
            this.Close();
        }

        public void GetEnum()
        {
            if (reportTypes.Text == "StandardReport")
            {
                ReportType = GridReportType.StandardReport;
                LoadStandardTitles();
            }
            else
            if (reportTypes.Text == "MemorizedReport")
            {
                ReportType = GridReportType.MemorizedReport;
                LoadMemorizedTitles();
            }
            else
                return;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReportTypes();
        }
        public void LoadCashflowReport()
        {
            if(cashFlow.Id>0)
            {
                txtName.Text = cashFlow.reportName;
            }
        }
        public void LoadStandardTitles()
        {
            lookupReportTitle.ItemsSource = repo.GetReportStandardTitles();
        }
        public void LoadMemorizedTitles()
        {
            lookupReportTitle.ItemsSource = repo.GetReportMemorizedTitles(SYSTEM_STATIC.currentUser.id);
        }

        public void LoadGroups()
        
        {
           
            if (reportTypes.Text == "StandardReport")
            {
               var reportGroups = repo.GetAllStandardGroups();

                List<GridReportGroup> standardGroups = new List<GridReportGroup>();

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null  && gridControlName == "Inquiries")
                {
                    standardGroups.AddRange(reportGroups.Where(x=>x.transactionType== ReportTransactionType.Inquiry).ToList());
                }
                else
                 if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null  && gridControlName == "Sale Orders" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null  && gridControlName == "Sale Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Sale_Order).ToList());
                }
                else
                 if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null && gridControlName == "Sale Orders" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null && gridControlName == "PRIT Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Sale_Order).ToList());
                }
                else
                 if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null  && gridControlName == "Offers" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null  && gridControlName == "Offer Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Offer).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null  && gridControlName == "Bills" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null  && gridControlName == "Bill Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Bill).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null  && gridControlName == "Purchase Orders" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null  && gridControlName == "Purchase Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Purchase_Order).ToList());
                }
                else
                 if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null && gridControlName == "Sale Invoices" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null && gridControlName == "Sales Invoice Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Sale_Invoice).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null  && gridControlName == "Sale Receipts" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null  && gridControlName == "Sale Receipt Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Sale_Receipt).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null  && gridControlName == "Inter-Bank Transfer" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null  && gridControlName == "Inter-Bank Transfer Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.InterBank_Transfer).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null  && gridControlName == "Admin Bills" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null  && gridControlName == "Admin Bill Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Admin_Bill).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null  && gridControlName == "Payments" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null  && gridControlName == "Payment Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Payments).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null  && gridControlName == "Purchase Invoices" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null  && gridControlName == "Purchase Invoice Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Purchase_Invoice).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null  && gridControlName == "Chart of Accounts" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null && gridControlName == "Chart of Accounts Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Chart_of_Account).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null  && gridControlName == "Step Register" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null  && gridControlName == "Step Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Targets).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null  && gridControlName == "Reward Register" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null  && gridControlName == "Reward Register")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.TargetReward).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null  && gridControlName == "Trial Balance as of:" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null  && gridControlName == "Trial Balance as of:")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Trial_Balance).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null && gridControlName == "STL" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null && gridControlName == "STL")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.STL).ToList());
                }  
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null && gridControlName == "Tasks" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null && gridControlName == "Tasks")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Tasks).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null && gridControlName == "Advances" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null && gridControlName == "Advances")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Tasks).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Company Loans") != null && gridControlName == "Loans" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Company Loans") != null && gridControlName == "Loans")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Loans).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null && gridControlName == "Assets" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null && gridControlName == "Assets")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Assets).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null && gridControlName == "CashFlow" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null && gridControlName == "CashFlow")
                {
                    standardGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.CashFlow).ToList());
                }
                lookupGroup.ItemsSource = standardGroups;
            }
            else
            {
               var reportGroups = repo.GetAllMemorizedGroups(SYSTEM_STATIC.currentUser.id);
               List<GridReportGroup> memorizedGroups = new List<GridReportGroup>();

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null && gridControlName == "Inquiries")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Inquiry).ToList());
                }
                else
                   if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null && gridControlName == "Sale Orders" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null && gridControlName == "Sale Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Sale_Order).ToList());
                }  
                else
                   if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null && gridControlName == "PRIT Register" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null && gridControlName == "PRIT Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Sale_Order).ToList());
                }
                else
                   if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null && gridControlName == "Offers" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null && gridControlName == "Offer Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Offer).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null && gridControlName == "Bills" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null && gridControlName == "Bill Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Bill).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null && gridControlName == "Purchase Orders" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null && gridControlName == "Purchase Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Purchase_Order).ToList());
                }
                else
                   if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null && gridControlName == "Sale Invoices" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null && gridControlName == "Sales Invoice Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Sale_Invoice).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null && gridControlName == "Sale Receipts" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null && gridControlName == "Sale Receipt Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Sale_Receipt).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null && gridControlName == "Inter-Bank Transfer" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null && gridControlName == "Inter-Bank Transfer Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.InterBank_Transfer).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null && gridControlName == "Admin Bills" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null && gridControlName == "Admin Bill Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Admin_Bill).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null && gridControlName == "Payments" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null && gridControlName == "Payment Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Payments).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null && gridControlName == "Purchase Invoices" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null && gridControlName == "Purchase Invoice Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Purchase_Invoice).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null && gridControlName == "Chart of Accounts" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null && gridControlName == "Chart of Accounts Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Chart_of_Account).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null && gridControlName == "Step Register" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null && gridControlName == "Step Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Targets).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null && gridControlName == "Reward Register" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null && gridControlName == "Reward Register")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.TargetReward).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null && gridControlName == "STL" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null && gridControlName == "STL")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.STL).ToList());
                }
                else
                  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null && gridControlName == "Tasks" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null && gridControlName == "Tasks")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Tasks).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null && gridControlName == "Tasks" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null && gridControlName == "Advances")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Tasks).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Company Loans") != null && gridControlName == "Loans" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Company Loans") != null && gridControlName == "Loans")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Loans).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null && gridControlName == "Assets" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null && gridControlName == "Assets")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.Assets).ToList());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null && gridControlName == "CashFlow" || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null && gridControlName == "CashFlow")
                {
                    memorizedGroups.AddRange(reportGroups.Where(x => x.transactionType == ReportTransactionType.CashFlow).ToList());
                }
                lookupGroup.ItemsSource = memorizedGroups;
            }
        }
        public void LoadReportTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.GridReportType.MemorizedReport; i++)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to View Standard Groups") != null)
                {
                    if (((ERP_BL.Enums.GridReportType)i).ToString() == GridReportType.StandardReport.ToString())
                    {
                        reportTypes.Items.Add(((ERP_BL.Enums.GridReportType)i).ToString());
                    }
                }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to View Memorized Groups") != null)
                {
                    if (((ERP_BL.Enums.GridReportType)i).ToString() == GridReportType.MemorizedReport.ToString())
                    {
                        reportTypes.Items.Add(((ERP_BL.Enums.GridReportType)i).ToString());
                    }

                }
                else
                    return;


            }
            ReportName = "";
        }

        private void lookupGroup_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            gridReportGroup = lookupGroup.SelectedItem as GridReportGroup;
            
            if (gridReportGroup != null)
            {
                string selectedcust = gridReportGroup.groupName;
                lookupGroup.EditValue = selectedcust;
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            
        }
        private void ReportTypes_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            GetEnum();
            if (reportTypes.Text == "StandardReport" || reportTypes.Text == "MemorizedReport")
            {
                LoadGroups();
            }
        }
    }
}
