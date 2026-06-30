using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.FixAssets;
using ERP_BL.HR;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.TransactionCounter;
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
using ZAS_ERP.Bankings.InterCompanyBankTransfer;
using ZAS_ERP.Bankings.InterCompanyWindows;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.Bankings.Windows;
using ZAS_ERP.ChartofAccounts.UserControls;
using ZAS_ERP.ChartofAccounts.Windows;
using ZAS_ERP.Employee;
using ZAS_ERP.Employeess;
using ZAS_ERP.HR;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.AdminBillss.Windows;
using ZAS_ERP.Procurementss.Billss;
using ZAS_ERP.Procurementss.Inquiriess;
using ZAS_ERP.Procurementss.MemorandumSaless;
using ZAS_ERP.Procurementss.Offerss;
using ZAS_ERP.Procurementss.PurchaseOrderss;
using ZAS_ERP.Procurementss.SaleInvoicess;
using ZAS_ERP.Procurementss.SaleOrderss;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.UserTasks
{
    /// <summary>
    /// Interaction logic for userTasks.xaml
    /// </summary>
    public partial class ucUserTasks : UserControl
    {

        //for company start
        User loginuser = new User();
        UsersRepo usersRepo = new UsersRepo();
        public InterBankTransfer bankTransfer = new InterBankTransfer();
        //For company end
       
        List<userTask> tasks = new List<userTask>();
        JournalVoucherRepo JVrepo = new JournalVoucherRepo();
        AssetsRepo assetsRepo  = new AssetsRepo();
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        InterCompanyBankTransferRepo transferRepo = new InterCompanyBankTransferRepo();
        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
        HrRepo hrRepo = new HrRepo();
        SalesReceiptRepo repo = new SalesReceiptRepo();
        BillRepo billrepo = new BillRepo();
        PurchaseOrderRepo purchaseOrderrepo = new PurchaseOrderRepo();
        SaleInvoiceRepo saleInvoicerepo = new SaleInvoiceRepo();
        MemorandumSaleRepo memorandumSalerepo = new MemorandumSaleRepo();
        OfferRepo offerrepo = new OfferRepo(); 
        SaleOrderRepo saleOrderrepo = new SaleOrderRepo(); 
        InquiryRepo InquiryRepo = new InquiryRepo();
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        List<Company> companies = new List<Company>();
        List<Department> departments = new List<Department>();
       
        List<Company> compLst = new List<Company>();
        List<Department> DeptList = new List<Department>();
        TransactionCounterRepo transactionCounterRepo = new TransactionCounterRepo();
        PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();

        public ucUserTasks() 
        {
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();
            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
            InitializeComponent();
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loginuser = usersRepo.getuserForTenant(SYSTEM_STATIC.currentUser.id);
                LoadCompanies();
                loadUserTask();

               // LoadYears();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "Loaded");
            }
           

        }
        public void LoadCompanies()
        {
            var Company = loginuser.employee.Companies;
            lookUpCompanies.ItemsSource = Company;

        }

        //public void LoadYears() 
        //{
        //    //this is only for loading years in combobox(waqas)
        //    var currentYear = DateTime.Now.Year;
        //    for (int i = 1980; i <= currentYear; i++)
        //    {
        //        var comboboxInsertion = new ComboBoxItem();
            
        //        comboboxInsertion.Content = i;
        //        cmbYears.Items.Add(comboboxInsertion);
        //        cmbYears.SelectedItem = comboboxInsertion;
        //    }
        //}


        public void loadUserTask()
        {
         

            for (TransactionItemType i = TransactionItemType.Inquiry; i <= TransactionItemType.InterCompanyBank_Transfer; i++)
            {
               
                //user.TransactionType = i;
                switch (i)
                {
                    case TransactionItemType.Inquiry:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inquiry List") != null)
                                {
                                    user.PendingForApproval = InquiryRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    user.Open = InquiryRepo.getAllActiveCount(SYSTEM_STATIC.currentUser.id);
                                    user.Close = InquiryRepo.getAllInActiveCount(SYSTEM_STATIC.currentUser.id);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                                    {
                                        user.PendingForApproval = InquiryRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForApproval = InquiryRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);

                                    }

                                }
                            }

                            else
                            {
                                user.PendingForApproval = InquiryRepo.getAllPendingInquiriesForApprovalAdministratorCount();
                                
                            }
                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inquiry List") != null)
                                {
                                    user.PendingForClosing = InquiryRepo.getAllPendingForClosingCount(MainWindow.currentUserid);

                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                                    {
                                        user.PendingForClosing = InquiryRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = InquiryRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                                    }

                                }
                            }

                            else
                            {
                                user.PendingForClosing = InquiryRepo.getAllPendingInquiriesForClosingAdministratorCount();
                            }

                            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "View list of Void Inquiries") != null)
                            //{
                            //    mbtnVoid.Visibility = Visibility.Visible;

                            //    if (MainWindow.currentUserid != 0)
                            //    {
                            //        VoidCount = inquiryrepo.getVoidRegisterCount(MainWindow.currentUserid);

                            //    }
                            //    else
                            //    {
                            //        VoidCount = inquiryrepo.getVoidRegisterAdministratorCount();

                            //    }
                            //}
                            //else
                            //{
                            //    mbtnVoid.Visibility = Visibility.Collapsed;
                            //}
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);

                            break;
                        }
                    case TransactionItemType.Offer:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Offer List") != null)
                                {

                                    user.PendingForApproval = offerrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    user.Open = offerrepo.getAllActiveCount(SYSTEM_STATIC.currentUser.id);
                                    user.Close = offerrepo.getAllInActiveCount(SYSTEM_STATIC.currentUser.id);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                                    {
                                        user.PendingForApproval = offerrepo.getAllPendingforApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForApproval = offerrepo.getAllPendingforApprovalCountOwn(MainWindow.currentUserid);

                                    }

                                }

                            }

                            else
                            {
                                user.PendingForApproval = offerrepo.getAllPendingsforApprovalAdministratorCount();
                            }
                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Offer List") != null)
                                {

                                    user.PendingForClosing = offerrepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                                    {
                                        user.PendingForClosing = offerrepo.getAllPendingforClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = offerrepo.getAllPendingforClosingCountown(MainWindow.currentUserid);

                                    }

                                }

                            }

                            else
                            {
                                user.PendingForClosing = offerrepo.getAllPendingforClosingAdministratorCount();
                            }
                            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "View list of Void Inquiries") != null)
                            //{
                            //    mbtnVoid.Visibility = Visibility.Visible;

                            //    if (MainWindow.currentUserid != 0)
                            //    {
                            //        VoidCount = inquiryrepo.getVoidRegisterCount(MainWindow.currentUserid);

                            //    }
                            //    else
                            //    {
                            //        VoidCount = inquiryrepo.getVoidRegisterAdministratorCount();

                            //    }
                            //}
                            //else
                            //{
                            //    mbtnVoid.Visibility = Visibility.Collapsed;
                            //}
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.Sale_Order:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleOrder List") != null)
                            {
                                user.PendingForApproval = saleOrderrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                user.Open = saleOrderrepo.getAllActiveCount(SYSTEM_STATIC.currentUser.id);
                                user.Close = saleOrderrepo.getAllInActiveCount(SYSTEM_STATIC.currentUser.id);
                               

                            }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleOrder without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                {
                                    user.PendingForApproval = saleOrderrepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    user.PendingForApproval = saleOrderrepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                                }
                            }
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleOrder List") != null)
                            {
                                user.PendingForReapproval = saleOrderrepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                            }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleOrder without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                {
                                    user.PendingForReapproval = saleOrderrepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    user.PendingForReapproval = saleOrderrepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                                }
                            }

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleOrder List") != null)
                                {
                                    user.PendingForClosing = saleOrderrepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);


                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                    {
                                        user.PendingForClosing = saleOrderrepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = saleOrderrepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                                    }

                                }
                            }

                            else
                            {
                                user.PendingForClosing = saleOrderrepo.getAllPendingForClosingAdministratorCount();
                            }
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.Memorandum_Sale:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;
                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) MemorandumSale List") != null)
                                {
                                    user.PendingForApproval = memorandumSalerepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    user.Open = memorandumSalerepo.getAllActiveCount(SYSTEM_STATIC.currentUser.id);
                                    user.Close = memorandumSalerepo.getAllInActiveCount(SYSTEM_STATIC.currentUser.id);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add MemorandumSale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                                    {
                                        user.PendingForApproval = memorandumSalerepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForApproval = memorandumSalerepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);

                                    }

                                }
                            }

                            else
                            {
                                user.PendingForApproval = memorandumSalerepo.getAllPendingForAdministratorCount();
                            }
                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) MemorandumSale List") != null)
                                {
                                    user.PendingForClosing = memorandumSalerepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);

                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                                    {
                                        user.PendingForClosing = memorandumSalerepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = memorandumSalerepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                                    }

                                }
                            }

                            else
                            {
                                user.PendingForClosing = memorandumSalerepo.getAllPendingForClosingAdministratorCount();
                            }

                           
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.Sale_Invoice:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleInvoice List") != null)
                                {

                                    user.PendingForApproval = saleInvoicerepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    user.Open = saleInvoicerepo.getAllActiveCount(SYSTEM_STATIC.currentUser.id);
                                    user.Close = saleInvoicerepo.getAllInActiveCount(SYSTEM_STATIC.currentUser.id);
                                }

                                else
                                {

                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleInvoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                                    {
                                        user.PendingForApproval = saleInvoicerepo.getAllPendingForApprovalCount(MainWindow.currentUserid);

                                    }
                                    else
                                    {
                                        user.PendingForApproval = saleInvoicerepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);


                                    }

                                }
                            }

                            else
                            {
                                user.PendingForApproval = saleInvoicerepo.getAllPendingForAdministratorCount();
                            }
                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleInvoice List") != null)
                                {
                                    user.PendingForClosing = saleInvoicerepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);

                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                                    {
                                        user.PendingForClosing = saleInvoicerepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = saleInvoicerepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                                    }

                                }
                            }

                            else
                            {
                                user.PendingForClosing = saleInvoicerepo.getAllPendingForClosingAdministratorCount();
                            }

                          
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.Purchase_Order:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (MainWindow.currentUserid != 0)
                            {


                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseOrder List") != null)
                                {
                                    user.PendingForApproval = purchaseOrderrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    user.Open = purchaseOrderrepo.getAllActiveCount(SYSTEM_STATIC.currentUser.id);
                                    user.Close = purchaseOrderrepo.getAllInActiveCount(SYSTEM_STATIC.currentUser.id);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseOrder without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                                    {
                                        user.PendingForApproval = purchaseOrderrepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForApproval = purchaseOrderrepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) PurchaseOrder List") != null)
                                {
                                    user.PendingForReapproval = purchaseOrderrepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseOrder without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                                    {
                                        user.PendingForReapproval = purchaseOrderrepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForReapproval = purchaseOrderrepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }

                              
                               
                            }

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseOrder List") != null)
                                {
                                    user.PendingForClosing = purchaseOrderrepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);


                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                                    {
                                        user.PendingForClosing = purchaseOrderrepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = purchaseOrderrepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                                    }

                                }
                            }

                            else
                            {
                                user.PendingForClosing = purchaseOrderrepo.getAllPendingForClosingAdministratorCount();
                            }

                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.Purchase_Invoice:
                        {
                          
                            break;
                        }
                    case TransactionItemType.CostCenter:
                        {
                           
                            break;
                        }
                    case TransactionItemType.SummarySheet:
                        {
                           
                            break;
                        }
                    case TransactionItemType.Bill:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (MainWindow.currentUserid != 0)
                            {


                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Bill List") != null)
                                {
                                    user.PendingForApproval = billrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    user.Open = billrepo.getAllActiveCount(SYSTEM_STATIC.currentUser.id);
                                    user.Close = billrepo.getAllInActiveCount(SYSTEM_STATIC.currentUser.id);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                                    {
                                        user.PendingForApproval = billrepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForApproval = billrepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Bill List") != null)
                                {
                                    user.PendingForReapproval = billrepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                                    {
                                        user.PendingForReapproval = billrepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForReapproval = billrepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }
                            }

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Bill List") != null)
                                {
                                    user.PendingForClosing = billrepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);


                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                                    {
                                        user.PendingForClosing = billrepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = billrepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                                    }

                                }
                            }


                            else
                            {
                                user.PendingForClosing = billrepo.getAllPendingForClosingAdministratorCount();
                            }
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.Sale_Receipt:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleReceipt List") != null)
                                {
                                    user.PendingForApproval = repo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    user.Open = repo.GetAllSalesReceiptCount(SYSTEM_STATIC.currentUser.id);
                                    user.Close = repo.GetAllInActiveSalesReceiptCount(SYSTEM_STATIC.currentUser.id);

                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                                    {
                                        user.PendingForApproval = repo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForApproval = repo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleReceipts List") != null)
                                {
                                    user.PendingForReapproval = repo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                                    {
                                        user.PendingForReapproval = repo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForReapproval = repo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }
                            }
                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleReceipt List") != null)
                                {
                                    user.PendingForClosing = repo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                                    {
                                        user.PendingForClosing = repo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = repo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                                    }
                                }
                            }

                            else
                            {
                                user.PendingForClosing = repo.getAllPendingForClosingAdministratorCount();
                            }
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.FixedAssets:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;
                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Land and Building List") != null
                                    && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Vehicles List") != null)
                                {
                                    user.PendingForApproval = assetsRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    //user.Open = assetsRepo.GetAllActiveAssetStatusCount();
                                    //user.Close = assetsRepo.GetAllInActiveAssetStatusCount();
                                }
                                else
                                {
                                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Land and Buildings without Approval") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Land and Buildings") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Land and Buildings") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Land and Buildings") != null)
                                        &&
                                        (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vehicles without Approval") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Vehicles") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Vehicles") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Vehicles") != null)
                                        )
                                    {
                                        user.PendingForApproval = assetsRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForApproval = assetsRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }


                               


                            }
                            else
                            {
                                user.PendingForApproval = assetsRepo.getAllPendingForAdministratorCount();
                            }

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Land and Building List") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Vehicles List") != null)
                                {
                                    user.PendingForClosing = assetsRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Land and Buildings without Approval") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Land and Buildings") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Land and Buildings") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Land and Buildings") != null)
                                        &&
                                        (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Vehicles without Approval") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Vehicles") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Vehicles") != null
                                        || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Vehicles") != null)
                                        )
                                    {
                                        user.PendingForClosing = repo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = repo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                                    }
                                }
                            }

                            else
                            {
                                user.PendingForClosing = repo.getAllPendingForClosingAdministratorCount();
                            }
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.InterBank_Transfer:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
                            {
                                user.PendingForApproval = bankTransRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                user.Open = bankTransRepo.getAllActiveandUnapprovedTransactionsCount(SYSTEM_STATIC.currentUser.id);
                                user.Close = bankTransRepo.getAllInActiveandUnapprovedReceiptsCount(SYSTEM_STATIC.currentUser.id);

                            }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                {
                                    user.PendingForApproval = bankTransRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    user.PendingForApproval = bankTransRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                                }
                            }
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                            {
                                user.PendingForReapproval = bankTransRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                            }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                {
                                    user.PendingForReapproval = bankTransRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    user.PendingForReapproval = bankTransRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                                }
                            }

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inter-Bank Transfer List") != null)
                                {
                                    user.PendingForClosing = bankTransRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                    {
                                        user.PendingForClosing = bankTransRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = bankTransRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                                    }

                                }
                            }


                            else
                            {
                                user.PendingForClosing = bankTransRepo.getAllPendingForClosingAdministratorCount();
                            }
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    //case TransactionItemType.Employee:
                    //    {
                    //        userTask user = new userTask();
                    //        user.TransactionType = i;

                    //        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Employee List") != null)
                    //        {
                    //            user.PendingForApproval = employeeRepo.getAllPendingForApprovalAdminCount();
                         
                             
                    //        }
                    //        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Employee List") != null)
                    //        {
                    //            user.PendingForClosing = employeeRepo.getAllPendingForClosingAdministratorCount();
                    //        }

                    //        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Employee List") != null)
                    //        {
                    //            user.PendingForReapproval = employeeRepo.getAllPendingForReApprovalAdminCount();
                    //        }
                    //        user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                    //        tasks.Add(user);
                    //        break;
                    //    }
                    case TransactionItemType.Leave:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;
                           

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Leave List") != null)
                                {
                                    user.PendingForApproval = hrRepo.getAllPendingForApprovalAdminCount();
                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Leave List") != null)
                                {
                                    user.PendingForClosing = hrRepo.getAllPendingForClosingAdministratorCount();
                                }

                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Leave List") != null)
                                {
                                    user.PendingForReapproval = hrRepo.getAllPendingForReApprovalAdminCount();
                                }
                            }
                            else
                            {
                                user.PendingForClosing = hrRepo.getAllPendingForClosingAdministratorCount();
                            }
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                                break;
                        }
                    case TransactionItemType.JV:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (MainWindow.currentUserid != 0)
                            {


                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) JV List") != null)
                                {
                                    user.PendingForApproval = JVrepo.getAllPendingForApprovalCount(MainWindow.currentUserid,companyIds,deptIds);
                                    user.Open = JVrepo.getAllActiveCount(SYSTEM_STATIC.currentUser.id,companyIds,deptIds);
                                    user.Close = JVrepo.getAllInActiveCount(SYSTEM_STATIC.currentUser.id,companyIds,deptIds);

                                }
                                else
                                {

                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) JV List") != null)
                                {
                                    user.PendingForReapproval = JVrepo.getAllPendingForReApprovalCount(MainWindow.currentUserid, companyIds, deptIds);

                                }
                                else
                                {

                                }

                              
                            }

                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) JV List") != null)
                                {
                                    user.PendingForClosing = JVrepo.getAllPendingForClosingCount(MainWindow.currentUserid, companyIds, deptIds);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                                    {
                                        user.PendingForClosing = JVrepo.getAllPendingForClosingCount(MainWindow.currentUserid, companyIds, deptIds);
                                    }

                                }
                            }
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.Admin_Bill:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (MainWindow.currentUserid != 0)
                            {


                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Admin Bill List") != null)
                                {
                                    user.PendingForApproval = billsRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    user.Open = billsRepo.getAllActiveandUnapprovedTransactionsCount(SYSTEM_STATIC.currentUser.id);
                                    user.Close = billsRepo.getAllInActiveandUnapprovedReceiptsCount(SYSTEM_STATIC.currentUser.id);
                                       
                                   
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Admin Bill") != null)
                                    {
                                        user.PendingForApproval = billsRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForApproval = billsRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Admin Bills List") != null)
                                {
                                    user.PendingForReapproval = billsRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Admin Bill") != null)
                                    {
                                        user.PendingForReapproval = billsRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForReapproval = billsRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }
                            }


                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Admin Bill List") != null)
                                {
                                    user.PendingForClosing = billsRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Admin Bill") != null)
                                    {
                                        user.PendingForClosing = billsRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = billsRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                                    }

                                }
                            }


                            else
                            {
                                user.PendingForClosing = billsRepo.getAllPendingForClosingAdministratorCount();
                            }
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }
                    case TransactionItemType.InterCompanyBank_Transfer:
                        {
                            userTask user = new userTask();
                            user.TransactionType = i;

                            if (MainWindow.currentUserid != 0)
                            {


                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
                                {
                                    user.PendingForApproval = transferRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                                    user.Open = transferRepo.getAllActiveandTransactionsCount(SYSTEM_STATIC.currentUser.id);
                                    user.Close = transferRepo.getAllInActiveTransactionsCount(SYSTEM_STATIC.currentUser.id);

                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                    {
                                        user.PendingForApproval = transferRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForApproval = transferRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                                {
                                    user.PendingForReapproval = transferRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                    {
                                        user.PendingForReapproval = transferRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForReapproval = transferRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                                    }
                                }
                            }


                            if (MainWindow.currentUserid != 0)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inter-Bank Transfer List") != null)
                                {
                                    user.PendingForClosing = transferRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                                }
                                else
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                    {
                                        user.PendingForClosing = transferRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                                    }
                                    else
                                    {
                                        user.PendingForClosing = transferRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                                    }

                                }
                            }


                            else
                            {
                                user.PendingForClosing = transferRepo.getAllPendingForClosingAdministratorCount();
                            }
                            user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                            tasks.Add(user);
                            break;
                        }

                }
                
            }
            
            grdUserTask.ItemsSource = tasks;
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdUserTask);
          //  SystemLogic.SaveUserSettingForCurrentWindow(grdUserTasks); 
        }

      

      
        public void RefreshData()
        {
            // var ucTasks = new ucUserTasks();
            var userTasks = new ucUserTasks();
            //ucUserTasks userTasks = new ucUserTasks();
            this.Content = userTasks;
           // userTask user = new userTask();
            //grdUserTasks.ItemsSource = user.TransactionType;
           // SetColumnsVisibility();
        }


        private void GrdUserTask_MouseDoubleClick(object sender, MouseButtonEventArgs e) 
        {
            loadDoubleClickData();
            //loadDoubleClickData();
        }
        public void loadDoubleClickData() 
        {
            TransactionItemType transactionItemType = TransactionItemType.UnDefined;
            DataType dataType = DataType.All;
            try
            {
                var selectedrow = grdUserTask.SelectedItem as userTask;
                if (selectedrow.TransactionType == TransactionItemType.InterCompanyBank_Transfer)
                {
                    //var interCompBankTransRegister = new ucInterCompBankTransRegister();
                    //Window win = new Window();
                    //win.Content = interCompBankTransRegister;
                    //win.ShowDialog();

                    interCompanyBanktransfer win = new interCompanyBanktransfer();
                    //Window win = new Window();
                    //win.Content = obj;
                    win.Show();
                }
                else if (selectedrow.TransactionType == TransactionItemType.InterBank_Transfer)
                {
                    try
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
                        {
                            //Window win = new Window();
                            InterBankTransferRegister interBankTransferWin = new InterBankTransferRegister();
                            interBankTransferWin.Show();
                            //ucBankTransferRegister bankTransferRegister = new ucBankTransferRegister();
                            //win.Content = bankTransferRegister;
                            //win.WindowState = WindowState.Maximized;
                            //win.Show();
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view list of Inter-Bank Transfer!");
                            return;

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

                else if (selectedrow.TransactionType == TransactionItemType.Inquiry)
                {
                    transactionItemType = TransactionItemType.Inquiry;
                    dataType = DataType.All;
                    Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(1, transactionItemType, dataType);
                    customerCenter.Show();

                }
                else if (selectedrow.TransactionType == TransactionItemType.Offer)
                {
                    transactionItemType = TransactionItemType.Offer;
                    dataType = DataType.All;
                    Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(1, transactionItemType, dataType);
                    customerCenter.Show();

                    
                }
                else if (selectedrow.TransactionType == TransactionItemType.Memorandum_Sale)
                {
                    transactionItemType = TransactionItemType.Memorandum_Sale;
                    dataType = DataType.All;
                    Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(1, transactionItemType, dataType);
                    customerCenter.Show();
                }
                else if (selectedrow.TransactionType == TransactionItemType.Sale_Invoice)
                {
                    transactionItemType = TransactionItemType.Sale_Invoice;
                    dataType = DataType.All;
                    Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(1, transactionItemType, dataType);
                    customerCenter.Show();

                   
                }
                else if (selectedrow.TransactionType == TransactionItemType.Bill)
                {
                    transactionItemType = TransactionItemType.Bill;
                    dataType = DataType.All;
                    Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(1, transactionItemType, dataType);
                    customerCenter.Show();
                  
                }
                else if (selectedrow.TransactionType == TransactionItemType.Sale_Receipt)
                {
                    try
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Sale Receipts") != null)
                        {
                            SalesReceiptRegister salesReceiptRegister = new SalesReceiptRegister();

                            salesReceiptRegister.Show();
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view List of Sale Receipt!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else if (selectedrow.TransactionType == TransactionItemType.Leave)
                {
                    //winLeavesRegister win = new winLeavesRegister();
                    //win.Show();
                }
                else if (selectedrow.TransactionType == TransactionItemType.JV)
                {
                    winTransactionRegister register = new winTransactionRegister();
                    ucTransactions transactionList = new ucTransactions();
                    register.faRightGrid.Children.Add(transactionList);
                    register.Show();
                }

                else if (selectedrow.TransactionType == TransactionItemType.Sale_Order)
                {

                    transactionItemType = TransactionItemType.Sale_Order;
                    dataType = DataType.All;
                    Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(1, transactionItemType, dataType);
                    customerCenter.Show();

                }
                else if (selectedrow.TransactionType == TransactionItemType.Admin_Bill)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name != "List of Admin Bills") != null)
                    {
                        AdminBillsRegister win = new AdminBillsRegister();
                        win.Show();

                    //    ucBillList billList = new ucBillList();
                    //var win = new Window();
                    //win.Content = billList;
                    //win.ShowDialog();

                        //ucSaleOrderGrid saleOrderGrid = new ucSaleOrderGrid();
                        //var win = new Window();
                        //win.Content = saleOrderGrid;
                        //win.ShowDialog();
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to View Bills Register!");
                    }

                }
                else if (selectedrow.TransactionType == TransactionItemType.Purchase_Order)
                {
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.All;
                    Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(1, transactionItemType, dataType);
                    customerCenter.Show();
                }
                else if (selectedrow.TransactionType == TransactionItemType.FixedAssets)
                {

                    bool isEnable = false;
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w.Name == "faWindow")
                        {
                            isEnable = true;
                            w.Activate();
                        }
                    }


                    if (isEnable == false)
                    {
                       
                    }


                    //ucGridControlFixedAssets fixedAssets = new ucGridControlFixedAssets();
                    //var win = new Window();
                    //win.Content = fixedAssets;
                    //win.ShowDialog();
                }
                else if (selectedrow.TransactionType == TransactionItemType.Employee)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name != "View Employee Register") != null)
                    {
                        //Employeess.frmEmployeeCenter employee = new Employeess.frmEmployeeCenter();
                        // employee.Owner = this;
                        //employee.empTabControl.SelectedTabItem = SelectedTabItem.registersTab;
                        //employee.empTabControl.SelectedTabItem = SelectedTabItem.registersTab;
                        //employee.empTabControl.SelectedTabItem = SelectedTabItem.registersTab;

                        // employee.registersTab.IsSelected = true;
                        // employee.registersGrid.Visibility = Visibility.Visible;

                        //employee.Show();

                        //ucEmployeeInfo employeeInfo = new ucEmployeeInfo();

                        //var win = new Window();
                        //win.Content = employeeInfo;
                        //win.ShowDialog();
                        frmEmployeeCenter employeeCenter = new frmEmployeeCenter();
                        // employeeCenter.Owner = this;
                        employeeCenter.registersTab.IsSelected = true;
                        employeeCenter.registersGrid.Visibility = Visibility.Visible;
                        employeeCenter.Show();
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to View Employee Register!");
                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        //{
        //    SystemLogic.SaveUserSettingForCurrentWindow(grdUserTask);
        //    DXMessageBox.Show("Layout is Saved successfully for this Register", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
        //}

        //private void BtnExpand_Click(object sender, RoutedEventArgs e)
        //{
          

        //}

        //private void BtnClose_Click(object sender, RoutedEventArgs e)
        //{
            
        //    MainWindow m = new MainWindow();
        //    ucUserTasks u = new ucUserTasks();
        //    var v = m;
        //    v.FavouriteItems.Visibility = Visibility.Collapsed;
        //    if(v.FavouriteItems.Content != null)
        //    {
        //        v.FavouriteItems.Content = null;
               
        //    }
        //    else
        //    {
        //        m.FavouriteItems.Content = null;
        //    }
           
        //}

        private void mbtnRefesh_click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            RefreshData();
        }

        private void mbtnSaveLayout_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdUserTask);
            DXMessageBox.Show("Layout is Saved successfully for this Register", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void TblViewLandTypeLst_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
        {

            var columnHeader = e.Column;
            TransactionItemType transactionItemType = TransactionItemType.UnDefined;
            DataType dataType = DataType.All;
            try
            {
                // userTask selectedrow = new userTask();
                var selectedrow = grdUserTask.SelectedItem as userTask;
                switch (selectedrow.TransactionType)
                {
                    case TransactionItemType.InterCompanyBank_Transfer:
                        {
                            switch (columnHeader.FieldName)
                            {
                                case "PendingForApproval":
                                    {
                                        interCompanyBanktransfer win = new interCompanyBanktransfer();
                                        win.transctions = columnHeader.FieldName;
                                        win.Show();
                                        break;
                                    }
                                case "PendingForReapproval":
                                    {

                                        interCompanyBanktransfer win = new interCompanyBanktransfer();
                                        win.transctions = columnHeader.FieldName;
                                        win.Show();

                                        break;
                                    }
                                case "PendingForClosing":
                                    {

                                        interCompanyBanktransfer win = new interCompanyBanktransfer();
                                        win.transctions = columnHeader.FieldName;
                                        win.Show();
                                        break;
                                    }
                                case "Open":
                                    {

                                        interCompanyBanktransfer win = new interCompanyBanktransfer();
                                        win.transctions = columnHeader.FieldName;
                                        win.Show();

                                        break;
                                    }
                                case "Close":
                                    {

                                        interCompanyBanktransfer win = new interCompanyBanktransfer();
                                        win.transctions = columnHeader.FieldName;
                                        win.Show();

                                        break;
                                    }

                                default: break;
                            }









                            break;
                        }
                    case TransactionItemType.InterBank_Transfer:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                InterBankTransferRegister win = new InterBankTransferRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {
                                InterBankTransferRegister interBankTransferWin = new InterBankTransferRegister();
                                interBankTransferWin.transctions = columnHeader.FieldName;
                                interBankTransferWin.Show();
                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                InterBankTransferRegister interBankTransferWin = new InterBankTransferRegister();
                                interBankTransferWin.transctions = columnHeader.FieldName;
                                interBankTransferWin.Show();
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                InterBankTransferRegister interBankTransferWin = new InterBankTransferRegister();
                                interBankTransferWin.transctions = columnHeader.FieldName;
                                interBankTransferWin.Show();

                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                InterBankTransferRegister interBankTransferWin = new InterBankTransferRegister();
                                interBankTransferWin.transctions = columnHeader.FieldName;
                                interBankTransferWin.Show();
                            }

                            break;
                        }
                    case TransactionItemType.Admin_Bill:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                AdminBillsRegister win = new AdminBillsRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {
                                AdminBillsRegister win = new AdminBillsRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                AdminBillsRegister win = new AdminBillsRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                AdminBillsRegister win = new AdminBillsRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                                //if (SystemLogic.AllowedPermissions.Find(x => x.Name != "List of Admin Bills") != null)
                                //{
                                //    AdminBillsRegister win = new AdminBillsRegister();
                                //    win.Show();
                                //}
                                //else
                                //{
                                //    DXMessageBox.Show("Permission required to View Bills Register!");
                                //}

                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                AdminBillsRegister win = new AdminBillsRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                            }

                            break;
                        }
                    case TransactionItemType.Bill:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                LoadTransactionGrid("Bill(PendingForApproval)");
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                LoadTransactionGrid("Bill(PendingForClosing)");
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                LoadTransactionGrid("Bill(Open)");
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                LoadTransactionGrid("Bill(Close)");
                            }

                            break;
                        }
                    case TransactionItemType.CostCenter:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {

                            }
                            if (columnHeader.FieldName == "Open")
                            {

                            }
                            if (columnHeader.FieldName == "Close")
                            {

                            }

                            break;
                        }
                    case TransactionItemType.Employee:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                frmEmployeeCenter win = new frmEmployeeCenter();
                                win.transctions = columnHeader.FieldName;
                                // employeeCenter.Owner = this;
                                win.registersTab.IsSelected = true;
                                win.registersGrid.Visibility = Visibility.Visible;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {
                                frmEmployeeCenter win = new frmEmployeeCenter();
                                win.transctions = columnHeader.FieldName;
                                // employeeCenter.Owner = this;
                                win.registersTab.IsSelected = true;
                                win.registersGrid.Visibility = Visibility.Visible;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                frmEmployeeCenter win = new frmEmployeeCenter();
                                win.transctions = columnHeader.FieldName;
                                // employeeCenter.Owner = this;
                                win.registersTab.IsSelected = true;
                                win.registersGrid.Visibility = Visibility.Visible;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                frmEmployeeCenter win = new frmEmployeeCenter();
                                win.transctions = columnHeader.FieldName;
                                // employeeCenter.Owner = this;
                                win.registersTab.IsSelected = true;
                                win.registersGrid.Visibility = Visibility.Visible;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                frmEmployeeCenter win = new frmEmployeeCenter();
                                win.transctions = columnHeader.FieldName;
                                // employeeCenter.Owner = this;
                                win.registersTab.IsSelected = true;
                                win.registersGrid.Visibility = Visibility.Visible;
                                win.Show();
                                break;
                            }

                            break;
                        }
                    case TransactionItemType.FixedAssets:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {

                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                bool isEnable = false;
                                foreach (Window w in Application.Current.Windows)
                                {
                                    if (w.Name == "faWindow")
                                    {
                                        isEnable = true;
                                        w.Activate();
                                    }
                                }


                                if (isEnable == false)
                                {
                                   
                                }

                            }
                            if (columnHeader.FieldName == "Close")
                            {

                            }

                            break;
                        }
                    case TransactionItemType.JV:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                LoadTransactionGrid("jv(PendingForApproval)");

                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                LoadTransactionGrid("jv(PendingForClosing)");
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                LoadTransactionGrid("jv(Open)");

                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                LoadTransactionGrid("jv(Close)");
                            }

                            break;
                        }
                    case TransactionItemType.Leave:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                //winLeavesRegister win = new winLeavesRegister();
                                //win.transctions = columnHeader.FieldName;
                                //win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {
                                //winLeavesRegister win = new winLeavesRegister();
                                //win.transctions = columnHeader.FieldName;
                                //win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {

                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                //winLeavesRegister win = new winLeavesRegister();
                                //win.transctions = columnHeader.FieldName;
                                //win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                //winLeavesRegister win = new winLeavesRegister();
                                //win.transctions = columnHeader.FieldName;
                                //win.Show();
                                break;
                            }

                            break;
                        }
                    case TransactionItemType.Memorandum_Sale:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                LoadTransactionGrid("MemorandumSale(PendingForApproval)");
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                LoadTransactionGrid("MemorandumSale(PendingForClosing)");
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                LoadTransactionGrid("MemorandumSale(Open)");
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                LoadTransactionGrid("MemorandumSale(Close)");
                            }

                            break;
                        }
                    case TransactionItemType.Offer:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                LoadTransactionGrid("Offer(PendingForApproval)");
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                LoadTransactionGrid("Offer(PendingForClosing)");
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                LoadTransactionGrid("Offer(Open)");
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                LoadTransactionGrid("Offer(Close)");
                            }

                            break;
                        }
                    case TransactionItemType.Purchase_Invoice:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {

                            }
                            if (columnHeader.FieldName == "Open")
                            {

                            }
                            if (columnHeader.FieldName == "Close")
                            {

                            }

                            break;
                        }
                    case TransactionItemType.Purchase_Order:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                LoadTransactionGrid("PurchaseOrder(PendingForApproval)");
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {
                                LoadTransactionGrid("PurchaseOrder(PendingForReapproval)");
                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                LoadTransactionGrid("PurchaseOrder(PendingForClosing)");
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                LoadTransactionGrid("PurchaseOrder(Open)");
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                LoadTransactionGrid("PurchaseOrder(Close)");
                            }

                            break;
                        }
                    case TransactionItemType.Sale_Invoice:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                LoadTransactionGrid("SaleInvoice(PendingForApproval)");
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                LoadTransactionGrid("SaleInvoice(PendingForClosing)");
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                LoadTransactionGrid("SaleInvoice(Open)");
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                LoadTransactionGrid("SaleInvoice(Close)");
                            }

                            break;
                        }
                    case TransactionItemType.Sale_Order:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                LoadTransactionGrid("SaleOrder(PendingForApproval)");
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                LoadTransactionGrid("SaleOrder(PendingForClosing)");
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                LoadTransactionGrid("SaleOrder(Open)");
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                LoadTransactionGrid("SaleOrder(Close)");
                            }

                            break;
                        }
                    case TransactionItemType.Sale_Receipt:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {

                                SalesReceiptRegister win = new SalesReceiptRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {
                                SalesReceiptRegister win = new SalesReceiptRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                SalesReceiptRegister win = new SalesReceiptRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                SalesReceiptRegister win = new SalesReceiptRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                SalesReceiptRegister win = new SalesReceiptRegister();
                                win.transctions = columnHeader.FieldName;
                                win.Show();
                                break;
                            }

                            break;
                        }
                    case TransactionItemType.SummarySheet:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {

                            }
                            if (columnHeader.FieldName == "Open")
                            {

                            }
                            if (columnHeader.FieldName == "Close")
                            {

                            }

                            break;
                        }
                    case TransactionItemType.Inquiry:
                        {

                            if (columnHeader.FieldName == "PendingForApproval")
                            {
                                LoadTransactionGrid("Inquiry(PendingForApproval)");
                            }
                            if (columnHeader.FieldName == "PendingForReapproval")
                            {

                            }
                            if (columnHeader.FieldName == "PendingForClosing")
                            {
                                LoadTransactionGrid("Inquiry(PendingForClosing)");
                            }
                            if (columnHeader.FieldName == "Open")
                            {
                                LoadTransactionGrid("Inquiries(Open)");
                            }
                            if (columnHeader.FieldName == "Close")
                            {
                                LoadTransactionGrid("Inquiries(Closed)");
                            }


                            break;
                        }

                    default: break;
                }
            }
            catch
            {
                MessageBox.Show("Please Ungroup first to apply this filter");
            }
        }



        private void LoadTransactionGrid(string TransactionName)
        {
            TransactionItemType transactionItemType = TransactionItemType.UnDefined;
            DataType dataType = DataType.All;

            switch (TransactionName)
            {

                case "Inquiry(PendingForApproval)":
                    {
                        transactionItemType = TransactionItemType.Inquiry;
                        dataType = DataType.PendingForApproval;

                        break;
                    }
                case "Inquiry(PendingForClosing)":
                    {
                        transactionItemType = TransactionItemType.Inquiry;
                        dataType = DataType.PendingForClosing;

                        break;
                    }
                case "Inquiries(Open)":
                    {
                        transactionItemType = TransactionItemType.Inquiry;
                        dataType = DataType.Open;
                        break;
                    }
                case "Inquiries(Closed)":
                    {
                        transactionItemType = TransactionItemType.Inquiry;
                        dataType = DataType.Closed;
                        break;
                    }


                case "Offer(Open)":
                    transactionItemType = TransactionItemType.Offer;
                    dataType = DataType.Open;

                    break;
                case "Offer(Close)":

                    transactionItemType = TransactionItemType.Offer;
                    dataType = DataType.Closed;

                    break;
                case "Offer(PendingForApproval)":
                    transactionItemType = TransactionItemType.Offer;
                    dataType = DataType.PendingForApproval;
                    break;
                case "Offer(PendingForClosing)":
                    transactionItemType = TransactionItemType.Offer;
                    dataType = DataType.PendingForClosing;
                    break;


                case "Sale Orders":
                    transactionItemType = TransactionItemType.Sale_Order;
                    dataType = DataType.All;
                    break;
                case "SaleOrder(Open)":
                    transactionItemType = TransactionItemType.Sale_Order;
                    dataType = DataType.Open;
                    break;
                case "SaleOrder(Close)":
                    transactionItemType = TransactionItemType.Sale_Order;
                    dataType = DataType.Closed;
                    break;
                case "SaleOrder(PendingForApproval)":
                    transactionItemType = TransactionItemType.Sale_Order;
                    dataType = DataType.PendingForApproval;
                    break;
                case "SaleOrder(PendingForClosing)":
                    transactionItemType = TransactionItemType.Sale_Order;
                    dataType = DataType.PendingForClosing;
                    break;

                //sale_Invoices start********************************************************************************************//
                case "Sale Invoices":
                    transactionItemType = TransactionItemType.Sale_Invoice;
                    dataType = DataType.All;
                    break;
                case "SaleInvoice(Open)":
                    transactionItemType = TransactionItemType.Sale_Invoice;
                    dataType = DataType.Open;
                    break;

                case "SaleInvoice(Close)":
                    transactionItemType = TransactionItemType.Sale_Invoice;
                    dataType = DataType.Closed;
                    break;
                case "SaleInvoice(PendingForApproval)":
                    transactionItemType = TransactionItemType.Sale_Invoice;
                    dataType = DataType.PendingForApproval;
                    break;
                case "SaleInvoice(PendingForClosing)":
                    transactionItemType = TransactionItemType.Sale_Invoice;
                    dataType = DataType.PendingForClosing;
                    break;


                //Memorandum_Sales start********************************************************************************************//          
                case "Memorandum Sales":
                    transactionItemType = TransactionItemType.Memorandum_Sale;
                    dataType = DataType.All;

                    break;
                case "MemorandumSale(Open)":
                    transactionItemType = TransactionItemType.Memorandum_Sale;
                    dataType = DataType.Open;

                    break;
                case "MemorandumSale(Close)":
                    transactionItemType = TransactionItemType.Memorandum_Sale;
                    dataType = DataType.Closed;
                    break;
                case "MemorandumSale(PendingForApproval)":
                    transactionItemType = TransactionItemType.Memorandum_Sale;
                    dataType = DataType.PendingForApproval;
                    break;
                case "MemorandumSale(PendingForClosing)":
                    transactionItemType = TransactionItemType.Memorandum_Sale;
                    dataType = DataType.PendingForClosing;
                    break;

                //Purchase Orders start********************************************************************************************//  
                case "Purchase Orders":
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.All;
                    break;
                case "PurchaseOrder(Open)":
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.Open;
                    break;
                case "PurchaseOrder(Close)":
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.Closed;
                    break;

                case "PurchaseOrder(PendingForApproval)":
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.PendingForApproval;
                    break;
                case "PurchaseOrder(PendingForClosing)":
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.PendingForClosing;
                    break;
                case "PurchaseOrder(PendingForReapproval)":
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.PendingForReApproval;
                    break;
                //Bills  start********************************************************************************************//  
                case "Bills":
                    transactionItemType = TransactionItemType.Bill;
                    dataType = DataType.All;
                    break;
                case "Bill(Open)":
                    transactionItemType = TransactionItemType.Bill;
                    dataType = DataType.Open;
                    break;
                case "Bill(Close)":
                    transactionItemType = TransactionItemType.Bill;
                    dataType = DataType.Closed;
                    break;
                case "Bill(PendingForApproval)":
                    transactionItemType = TransactionItemType.Bill;
                    dataType = DataType.PendingForApproval;
                    break;
                case "Bill(PendingForClosing)":
                    transactionItemType = TransactionItemType.Bill;
                    dataType = DataType.PendingForClosing;
                    break;

                //jv  start********************************************************************************************//  
                case "jv":
                    transactionItemType = TransactionItemType.JV;
                    dataType = DataType.All;
                    break;
                case "jv(Open)":
                    transactionItemType = TransactionItemType.JV;
                    dataType = DataType.Open;
                    break;
                case "jv(Close)":
                    transactionItemType = TransactionItemType.JV;
                    dataType = DataType.Closed;
                    break;
                case "jv(PendingForApproval)":
                    transactionItemType = TransactionItemType.JV;
                    dataType = DataType.PendingForApproval;
                    break;
                case "jv(PendingForClosing)":
                    transactionItemType = TransactionItemType.JV;
                    dataType = DataType.PendingForClosing;
                    break;


                    //jv  start********************************************************************************************//  
                    //case "employee":
                    //    transactionItemType = TransactionItemType.Employee;
                    //    dataType = DataType.All;
                    //    break;
                    //case "employee(Open)":
                    //    transactionItemType = TransactionItemType.Employee;
                    //    dataType = DataType.Open;
                    //    break;
                    //case "employee(Close)":
                    //    transactionItemType = TransactionItemType.Employee;
                    //    dataType = DataType.Closed;
                    //    break;
                    //case "employee(PendingForApproval)":
                    //    transactionItemType = TransactionItemType.Employee;
                    //    dataType = DataType.PendingForApproval;
                    //    break;
                    //case "employee(PendingForClosing)":
                    //    transactionItemType = TransactionItemType.Employee;
                    //    dataType = DataType.PendingForClosing;
                    //    break;


            }

            Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(1, transactionItemType, dataType);
            customerCenter.Show();
        }

        private void LookUpCompanies_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if (lookUpCompanies.SelectedIndex > -1)
            //{
            //    //var selectedCompany = lookUpCompanies.SelectedItem as Company;
            //    lookUpCompanies.SelectedItem = null;
            //    var selectedCompanies = lookUpCompanies.SelectedItems;
            //    lookUpDepartment.SelectedIndex = -1;               
            //    lookUpDepartment.SelectedItem = null;
            //    compLst = new List<Company>();
            //    DeptList = new List<Department>();
            //    foreach (Company _item in selectedCompanies)
            //    {
            //        compLst.Add(_item);
            //        foreach (var _dept in _item.departments)
            //        {
            //            if (_dept.isActive == true)
            //            {
            //                List<int> empyoyeeIds = new List<int>();
            //                foreach (var emp in _dept.employees)
            //                {
            //                    empyoyeeIds.Add(emp.EmpId);
            //                }
            //                if (empyoyeeIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
            //                    DeptList.Add(_dept);
            //            }
            //        }
            //    } 

            //    //var departments = selectedCompany.departments;
            //    //List<Department> departmentList = new List<Department>();
            //    //foreach (var dept in departments)
            //    //{
            //    //    if (dept.isActive == true)
            //    //    {
            //    //        List<int> empyoyeeIds = new List<int>();
            //    //        foreach (var emp in dept.employees)
            //    //        {
            //    //            empyoyeeIds.Add(emp.EmpId);
            //    //        }
            //    //        if (empyoyeeIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
            //    //            departmentList.Add(dept);
            //    //    }
            //    //}

            //    lookUpDepartment.ItemsSource = DeptList;

            //}
        }

       
        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(lookUpCompanies.DisplayText !="" && !string.IsNullOrEmpty(lookUpCompanies.DisplayText))
                {
                    tasks = new List<userTask>();
                    for (TransactionItemType i = TransactionItemType.Inquiry; i <= TransactionItemType.InterCompanyBank_Transfer; i++)
                    {
                        switch (i)
                        {
                            case TransactionItemType.Inquiry:
                                {
                                   
                                    userTask user = new userTask();
                                    user.TransactionType = i;

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inquiry List") != null)
                                        {
                                            if(!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForApprovalDepartmental);
                                                user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.Open);
                                                user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.Close);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForApprovalDepartmental);
                                                user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.Open);
                                                user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.Close);
                                               
                                            }
                                           
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForApproval);
                                                   
                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForApproval);
                                                    
                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForApprovalOwn);
                                                 
                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst,departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForApprovalOwn);
                                                   
                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForApproval = InquiryRepo.getAllPendingInquiriesForApprovalAdministratorCount();
                                    }
                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inquiry List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForClosingDepartmental);
                                 
                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForClosingDepartmental);
                        
                                            }
                                          

                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForClosing);
                                                    
                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForClosing);
                                                 
                                                }
                                                
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForClosingOwn);
                                                   
                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Inquiry, TransactionCounterType.PendingForClosingOwn);
                                                  
                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = InquiryRepo.getAllPendingInquiriesForClosingAdministratorCount();
                                    }

                                
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);

                                    break;
                                }
                            case TransactionItemType.Offer:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Offer List") != null)
                                        {

                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForApprovalDepartmental);
                                                user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.Open);
                                                user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.Close);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForApprovalDepartmental);
                                                user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.Open);
                                                user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.Close);

                                            }


                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForApproval);

                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForApproval);

                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForApprovalOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForApprovalOwn);

                                                }

                                            }

                                        }

                                    }

                                    else
                                    {
                                        user.PendingForApproval = offerrepo.getAllPendingsforApprovalAdministratorCount();
                                    }
                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Offer List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForClosingDepartmental);

                                            }

                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Offer, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }

                                    }

                                    else
                                    {
                                        user.PendingForClosing = offerrepo.getAllPendingforClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                            case TransactionItemType.Sale_Order:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleOrder List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.Close);

                                        }
                                        else
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.Close);

                                        }

                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleOrder without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForApproval);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForApproval);

                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForApprovalOwn);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForApprovalOwn);

                                            }

                                        }
                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleOrder List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        { 
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst,  SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForReapprovalDepartmental);
                                     
                                        }
                                        else
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForReapprovalDepartmental);
                                          
                                        }
                                        

                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleOrder without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForReapproval);
                                               
                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForReapproval);
                           
                                            }
                                           
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForReapprovalOwn);
                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForReapprovalOwn);
                                            }

                                            
                                        }
                                    }

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleOrder List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Order, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = saleOrderrepo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                                
                            case TransactionItemType.Memorandum_Sale:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) MemorandumSale List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForApprovalDepartmental);
                                                user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.Open);
                                                user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.Close);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForApprovalDepartmental);
                                                user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.Open);
                                                user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.Close);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add MemorandumSale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForApproval);

                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForApproval);

                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForApprovalOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForApprovalOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForApproval = memorandumSalerepo.getAllPendingForAdministratorCount();
                                    }
                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) MemorandumSale List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Memorandum_Sale, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = memorandumSalerepo.getAllPendingForClosingAdministratorCount();
                                    }


                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                            case TransactionItemType.Sale_Invoice:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleInvoice List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForApprovalDepartmental);
                                                user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.Open);
                                                user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.Close);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForApprovalDepartmental);
                                                user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.Open);
                                                user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.Close);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleInvoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForApproval);

                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForApproval);

                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForApprovalOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForApprovalOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForApproval = memorandumSalerepo.getAllPendingForAdministratorCount();
                                    }
                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleInvoice List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = saleInvoicerepo.getAllPendingForClosingAdministratorCount();
                                    }


                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                            case TransactionItemType.Purchase_Order:
                                  {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseOrder List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.Close);

                                        }
                                        else
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.Close);

                                        }

                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseOrder without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForApproval);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForApproval);

                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForApprovalOwn);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForApprovalOwn);

                                            }

                                        }
                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) PurchaseOrder List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }
                                        else
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }


                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseOrder without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForReapproval);

                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForReapproval);

                                            }

                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForReapprovalOwn);
                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForReapprovalOwn);
                                            }


                                        }
                                    }

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseOrder List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Purchase_Order, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = purchaseOrderrepo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                            case TransactionItemType.Purchase_Invoice:
                                {
                                    break;
                                }

                            case TransactionItemType.CostCenter:
                                {

                                    break;
                                }
                            case TransactionItemType.SummarySheet:
                                {

                                    break;
                                }
                            case TransactionItemType.Bill:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Bill List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.Close);

                                        }
                                        else
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.Close);

                                        }

                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForApproval);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForApproval);

                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForApprovalOwn);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForApprovalOwn);

                                            }

                                        }
                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Bill List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }
                                        else
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }


                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForReapproval);

                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForReapproval);

                                            }

                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForReapprovalOwn);
                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForReapprovalOwn);
                                            }


                                        }
                                    }

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Bill List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = billrepo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                            case TransactionItemType.Sale_Receipt:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleReceipt List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.Close);

                                        }
                                        else
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.Close);

                                        }

                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForApproval);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForApproval);

                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForApprovalOwn);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForApprovalOwn);

                                            }

                                        }
                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleReceipt List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }
                                        else
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }


                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForReapproval);

                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForReapproval);

                                            }

                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForReapprovalOwn);
                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForReapprovalOwn);
                                            }


                                        }
                                    }

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleReceipt List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = repo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                            case TransactionItemType.FixedAssets:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Land and Building List") != null
                                            && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Vehicles List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForApprovalDepartmental);
                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst,departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForApprovalDepartmental);
                                            }
                                               
                                           
                                        }
                                        else
                                        {
                                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Land and Buildings without Approval") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Land and Buildings") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Land and Buildings") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Land and Buildings") != null)
                                                &&
                                                (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vehicles without Approval") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Vehicles") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Vehicles") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Vehicles") != null)
                                                )
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForApproval);
                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForApproval);
                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForApprovalOwn);
                                                }
                                                else
                                                {
                                                    user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForApprovalOwn);
                                                }
                                            }
                                        }





                                    }
                                    else
                                    {
                                        user.PendingForApproval = assetsRepo.getAllPendingForAdministratorCount();
                                    }

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Land and Building List") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Vehicles List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForClosingDepartmental);
                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForClosingDepartmental);
                                            }
                                        }
                                        else
                                        {
                                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Land and Buildings without Approval") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Land and Buildings") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Land and Buildings") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Land and Buildings") != null)
                                                &&
                                                (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Vehicles without Approval") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Vehicles") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Vehicles") != null
                                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Vehicles") != null)
                                                )
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForClosing);
                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForClosing);
                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForClosingOwn);
                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.FixedAssets, TransactionCounterType.PendingForClosingOwn);
                                                } 
                                            }
                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = assetsRepo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                                
                            case TransactionItemType.InterBank_Transfer:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.Close);

                                        }
                                        else
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.Close);

                                        }

                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForApproval);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForApproval);

                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForApprovalOwn);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForApprovalOwn);

                                            }

                                        }
                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }
                                        else
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }


                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForReapproval);

                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForReapproval);

                                            }

                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForReapprovalOwn);
                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForReapprovalOwn);
                                            }


                                        }
                                    }

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inter-Bank Transfer List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = bankTransRepo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                            case TransactionItemType.Employee:
                                {
                                    break;
                                }
                            case TransactionItemType.Leave:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;


                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Leave List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Leave, TransactionCounterType.PendingForApprovalDepartmental);
                                             
                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Leave, TransactionCounterType.PendingForApprovalDepartmental);   

                                            }
                                          
                                        }
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Leave List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Leave, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Leave, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            //user.PendingForClosing = hrRepo.getAllPendingForClosingAdministratorCount();
                                        }

                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Leave List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Leave, TransactionCounterType.PendingForReapprovalDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Leave, TransactionCounterType.PendingForReapprovalDepartmental);

                                            }
                                            user.PendingForReapproval = hrRepo.getAllPendingForReApprovalAdminCount();
                                        }
                                    }
                                    else
                                    {
                                        user.PendingForClosing = hrRepo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                            case TransactionItemType.JV:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) JV List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.Close);

                                        }
                                        else
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.Close);

                                        }

                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForApproval);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForApproval);

                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForApprovalOwn);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForApprovalOwn);

                                            }

                                        }
                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) JV List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }
                                        else
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }


                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForReapproval);

                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForReapproval);

                                            }

                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForReapprovalOwn);
                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForReapprovalOwn);
                                            }


                                        }
                                    }

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) JV List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.JV, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = JVrepo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                            case TransactionItemType.Admin_Bill:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Admin Bill List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.Close);

                                        }
                                        else
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.Close);

                                        }

                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Admin Bill") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForApproval);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForApproval);

                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForApprovalOwn);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForApprovalOwn);

                                            }

                                        }
                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Admin Bills List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }
                                        else
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }


                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Admin Bill") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForReapproval);

                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForReapproval);

                                            }

                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForReapprovalOwn);
                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForReapprovalOwn);
                                            }


                                        }
                                    }

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Admin Bill List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Admin Bill") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = billrepo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }
                                
                            case TransactionItemType.InterCompanyBank_Transfer:
                                {
                                    userTask user = new userTask();
                                    user.TransactionType = i;
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.Close);

                                        }
                                        else
                                        {
                                            user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForApprovalDepartmental);
                                            user.Open = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.Open);
                                            user.Close = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.Close);

                                        }

                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForApproval);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForApproval);

                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForApprovalOwn);

                                            }
                                            else
                                            {
                                                user.PendingForApproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForApprovalOwn);

                                            }

                                        }
                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                                    {
                                        if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }
                                        else
                                        {
                                            user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForReapprovalDepartmental);

                                        }


                                    }
                                    else
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForReapproval);

                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForReapproval);

                                            }

                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForReapprovalOwn);
                                            }
                                            else
                                            {
                                                user.PendingForReapproval = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForReapprovalOwn);
                                            }


                                        }
                                    }

                                    if (MainWindow.currentUserid != 0)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inter-Bank Transfer List") != null)
                                        {
                                            if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                            else
                                            {
                                                user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForClosingDepartmental);

                                            }
                                        }
                                        else
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForClosing);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForClosing);

                                                }

                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(lookUpCompanies.DisplayText) && string.IsNullOrEmpty(lookUpDepartment.DisplayText) && lookUpDepartment.DisplayText == "")
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompany(compLst, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForClosingOwn);

                                                }
                                                else
                                                {
                                                    user.PendingForClosing = transactionCounterRepo.GetAllCounterForCompanyDepartment(compLst, departments, SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer, TransactionCounterType.PendingForClosingOwn);

                                                }

                                            }

                                        }
                                    }

                                    else
                                    {
                                        user.PendingForClosing = transferRepo.getAllPendingForClosingAdministratorCount();
                                    }
                                    user.Total = user.PendingForApproval + user.PendingForClosing + user.PendingForReapproval + user.Open + user.Close;
                                    tasks.Add(user);
                                    break;
                                }

                        }

                    }

                    grdUserTask.ItemsSource = tasks;

                }
               
            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message + "Filter button");
            }
           
        }

        private void LookupIndustry_PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            try
            {
                companies = new List<Company>();
                var grd = lookUpCompanies.GetGridControl();
                string industryName = "";
                if (grd.SelectedItems.Count != 0)
                {
                    foreach (Company _item in grd.SelectedItems)
                    {
                        industryName = industryName + " | " + _item.CompanyName;
                        companies.Add(_item);
                    }
                }

                lookUpCompanies.EditValue = industryName;
                lookUpCompanies.DisplayMember = "";
                if(grd.SelectedItems.Count == 0)
                {
                    lookUpDepartment.ItemsSource = null;
                }
                else
                {
                    var selectedCompanies = grd.SelectedItems;
                    lookUpDepartment.SelectedIndex = -1;
                    lookUpDepartment.SelectedItem = null;
                    compLst = new List<Company>();
                    DeptList = new List<Department>();
                    foreach (Company _item in selectedCompanies)
                    {
                        compLst.Add(_item);
                        foreach (var _dept in _item.departments)
                        {
                            if (_dept.isActive == true)
                            {
                                List<int> empyoyeeIds = new List<int>();
                                foreach (var emp in _dept.employees)
                                {
                                    empyoyeeIds.Add(emp.EmpId);
                                }
                                if (empyoyeeIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                    DeptList.Add(_dept);
                            }
                        }
                    }
                    lookUpDepartment.ItemsSource = DeptList;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PART_GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var grd = lookUpCompanies.GetGridControl();
                if (companies.Count != 0)
                    grd.SelectedItems = companies;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LookupDepartment_PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            try
            {
                departments = new List<Department>();
                var grd = lookUpDepartment.GetGridControl();
                string industryName = "";
                if (grd.SelectedItems.Count != 0)
                {
                    foreach (Department _item in grd.SelectedItems)
                    {
                        industryName = industryName + " | " + _item.DeptName;
                        departments.Add(_item);
                    }
                }
                lookUpDepartment.EditValue = industryName;
                lookUpDepartment.DisplayMember = "";
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "Department Popup Close");
            }
        }
        private void PART_GridControl_Department_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var grd = lookUpDepartment.GetGridControl();
                if (departments.Count != 0)
                    grd.SelectedItems = departments;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        //private void CmbYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //    var cmb = (ComboBoxItem)cmbYears.SelectedItem;
        //    var item = cmb.Content;
        //    var year = item.ToString();
        //    // lblHeading.Text = year + " Counter ";
        //    cmbYear = Convert.ToInt32(item);

        //    var op = TransactionItemType.FixedAssets;
        //    userTask user = new userTask();
        //    List<userTask> l = new List<userTask>();
        //    user.TransactionType = op;
        //  //  user.Open = assetsRepo.GetAllActiveAssetStatus();
        //    l.Add(user);
        //    grdUserTasks.ItemsSource = l;

        //}

        //Sum start by waqas
        //public void summary()
        //{
        //    grdUserTask.TotalSummary.Add(new GridSummaryItem()
        //    {
        //        FieldName = "TransactionType",
        //        SummaryType = SummaryItemType.Count,
        //        DisplayFormat = "Total Module Count is: {0}"

        //    });
        //    grdUserTask.TotalSummary.Add(new GridSummaryItem()
        //    {
        //        FieldName = "PFA",
        //        SummaryType = SummaryItemType.Sum,
        //        DisplayFormat = "Sum of pfA is: {0}"

        //    });
        //}
    }

}
