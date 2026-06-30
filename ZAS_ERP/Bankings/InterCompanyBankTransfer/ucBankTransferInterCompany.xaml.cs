using DevExpress.Xpf.Core;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Tax;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ZAS_ERP.Bankings.InterCompanyBankTransfer.Windows;
using ZAS_ERP.Procurementss;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.Bankings.UserControls
{

    /// <summary>
    /// Interaction logic for ucBankTransferIntraCompany.xaml
    /// </summary>
    public partial class ucBankTransferInterCompany : UserControl
    {


        InterBankTransferStatus status = new InterBankTransferStatus();
        InterBankTransferStatus oldStatus = new InterBankTransferStatus();
        static InterBankTransferStatus statusChanged = new InterBankTransferStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        Department department = new Department();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        UsersRepo UsersRepo = new UsersRepo();

        public virtual List<ProcurementProduct> procurementProducts { get; set; }
        public virtual List<Product> products { get; set; }
        List<ViewInfo> views = new List<ViewInfo>();

        //start
        
        //DepartmentRepo departmentRepo = new DepartmentRepo();
        //User loginuser = new User();
        public UcListWindow frmBankTranfer = new UcListWindow();

        //List<Bank> allowedBanks = new List<Bank>();

        ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer bankTransfer = new ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer();
        public int bankTransferId = 0;
        //Currency currency = new Currency();
        InterBankTransRepo transferRepo = new InterBankTransRepo();
        InterCompanyBankTransferRepo compTransferRepo = new InterCompanyBankTransferRepo();
        static string systemRefIntitials = "IBT-";

        UsersRepo usersRepo = new UsersRepo();

        public bool editFlag = false;

        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        //end
        public int paymentGroupId = 0;
        public int receiptGroupId = 0;
        public int stlId = 0;
        List<cmbitem> interBankTransStatusLst = new List<cmbitem>();
        List<InterBankTransferStatus> allInterBankTransStatus = new List<InterBankTransferStatus>();
        public ucBankTransferInterCompany()
        {
            InitializeComponent();
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();

            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
        }
        public ucBankTransferInterCompany(int _receiptGroupId, bool _isReceipt)
        {
            InitializeComponent();
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();

            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
            if (_isReceipt == true)
            {
                receiptGroupId = _receiptGroupId;
            }
            else
                if(_isReceipt==false)
            {
                paymentGroupId = _receiptGroupId;
            }

        }
        public ucBankTransferInterCompany(TransactionItemType type, int _stlId)
        {
            InitializeComponent();
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();

            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
            if (type == TransactionItemType.STL)
            {
                stlId = _stlId;
            }
        }

        public void loadcomments()
        {
            try
            {
                if (bankTransfer.Id != 0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void loadIBTReferenceNoFrom()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            List<BillRefNumber> references = new List<BillRefNumber>();
            references = BillsRepo.GetAllActiveBillReferenceNo((lookupCompanyFrom.SelectedItem as Company).Id);

            if (editFlag == true && bankTransfer != null)
            {
                if (bankTransfer.PettyCashRefFrom != null && references.FirstOrDefault(x => x.Id == bankTransfer.PettyCashRefFromId) == null)
                    references.Add(bankTransfer.PettyCashRefFrom);
            }

            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (BillRefNumber _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.BillReferenceNo, id = _ref.Id });
            }
            cmbxPettyCashRefFrom.ItemsSource = cmbitems;
        }

        private void loadIBTReferenceNoTo()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            List<BillRefNumber> references = new List<BillRefNumber>();
            references = BillsRepo.GetAllActiveBillReferenceNo((lookupCompanyTo.SelectedItem as Company).Id);

            if (editFlag == true && bankTransfer != null)
            {
                if (bankTransfer.PettyCashRefTo != null && references.FirstOrDefault(x => x.Id == bankTransfer.PettyCashRefToId) == null)
                    references.Add(bankTransfer.PettyCashRefTo);
            }

            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (BillRefNumber _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.BillReferenceNo, id = _ref.Id });
            }
            cmbxPettyCashRefTo.ItemsSource = cmbitems;
        }
        public void LoadStatuses()
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer Statuses") != null)
                allInterBankTransStatus = transferRepo.GetAllInterBankTransStatus();
            else
                allInterBankTransStatus = transferRepo.GetAllOpenBankTransferStatus();
            allInterBankTransStatus = allInterBankTransStatus.Where(x => x.isDisable != true).ToList();

            if (allInterBankTransStatus != null)
            {
                Parallel.ForEach(allInterBankTransStatus, delegate (InterBankTransferStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                {
                    interBankTransStatusLst.Add
                    (new cmbitem()
                    {
                        name = status.Status,
                        id = status.Id,
                        bcolor = status.backcolor,
                        fcolor = "#FF000000"
                    });
                });
                cmbxInterBankTransStatus.ItemsSource = interBankTransStatusLst;
            }
        }
        public void LoadStatuses(InterBankTransferStatus _status)
        {

            allInterBankTransStatus.Add(_status);
            if (allInterBankTransStatus != null)
            {
                Parallel.ForEach(allInterBankTransStatus, delegate (InterBankTransferStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                {
                    interBankTransStatusLst.Add
                    (new cmbitem()
                    {
                        name = status.Status,
                        id = status.Id,
                        bcolor = status.backcolor,
                        fcolor = "#FF000000"
                    });
                });
                cmbxInterBankTransStatus.ItemsSource = interBankTransStatusLst;
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
            //loginuser = usersRepo.getuser(SYSTEM_STATIC.currentUser.id);

            grdCntrlPOItems.ItemsSource = procurementProducts;
            products = SYSTEM_STATIC.GetItemsForCurrentUser();
            lookupProductsinGrid.ItemsSource = products;
            LoadTransferTypes();
           // LoadStatuses();
            LoadCompanies();
            LoadDepartments();
            LoadCUrrencies();

            LoadTransferMethod();

            datCreationDate.DateTime = DateTime.Now;
            cmbxTransferType.SelectedIndex = 2;
            LoadStatuses();



            
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit GL Posting Date of Inter-Bank Transfer") != null)
            {
                datglPostingdate.IsEnabled = true;
            }
            else
            {
                datglPostingdate.IsEnabled = false;

            }
            if (editFlag == true && bankTransferId > 0)
            {
                bankTransfer = compTransferRepo.GetInterCompanyBankTransfer(bankTransferId);
                //if (bankTransfer.journalTransactions != null && bankTransfer.journalTransactions.Count > 0)
                //{
                //    btnPushtoGL.IsChecked = true;
                //}
                var creditJournalTransactions = bankTransfer.journalTransactions.Where(x => x.credit != 0).ToList();
                var debitJournalTransactions = bankTransfer.journalTransactions.Where(x => x.debit != 0).ToList();
                if (creditJournalTransactions.Count > 0)
                {
                    btnPushCredits.IsChecked = true;
                }
                if (debitJournalTransactions.Count > 0)
                {
                    btnPushDebits.IsChecked = true;
                }

                if (bankTransfer.isApproved == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Accounting Fields After Approval") == null)
                    {
                        layoutCurrency.IsEnabled = false;
                        layoutGrpTransferFrom.IsEnabled = false;
                        layoutGrpTransferTo.IsEnabled = false;
                    }
                }

                if (bankTransfer.isDepositFrom == true)
                {
                    btnDepositFrom.IsChecked = true;
                    if (bankTransfer.isAmountOCfrom == true)
                    {
                        chkAmountOCfrom.IsChecked = true;
                    }
                    else if (bankTransfer.isAmountOCfrom == false)
                    {
                        chkAmountMERfrom.IsChecked = true;
                    }
                }
                else if (bankTransfer.isDepositFrom == false)
                {
                    btnPaymentFrom.IsChecked = true;
                    if (bankTransfer.isAmountOCfrom == true)
                    {
                        chkAmountOCfrom.IsChecked = true;
                    }
                    else if (bankTransfer.isAmountOCfrom == false)
                    {
                        chkAmountMERfrom.IsChecked = true;
                    }
                }

                if (bankTransfer.isDepositTo == true)
                {
                    btnDepositTo.IsChecked = true;
                    if (bankTransfer.isAmountOCto == true)
                    {
                        chkAmountOCto.IsChecked = true;
                    }
                    else if (bankTransfer.isAmountOCto == false)
                    {
                        chkAmountMERto.IsChecked = true;
                    }
                }
                else if (bankTransfer.isDepositTo == false)
                {
                    btnPaymentTo.IsChecked = true;
                    if (bankTransfer.isAmountOCto == true)
                    {
                        chkAmountOCto.IsChecked = true;
                    }
                    else if (bankTransfer.isAmountOCto == false)
                    {
                        chkAmountMERto.IsChecked = true;
                    }
                }
                if(bankTransfer.GLPostingDate!=null)
                {
                    datglPostingdate.EditValue = bankTransfer.GLPostingDate;
                }
                else
                {
                    datglPostingdate.EditValue = bankTransfer.CreationDate;
                }




        
            }
            else
            {
                btnPushCredits.IsChecked = true;
                btnPushDebits.IsChecked = true;
                datglPostingdate.EditValue = DateTime.Now;
            }
            


            if (editFlag == true && bankTransfer.Id != 0)
            {
                if (bankTransfer.interBankTransStatus.isActive == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed Inter-Bank Transfer") == null)
                    {
                        btnSave.IsEnabled = false;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer Statuses") == null)
                    {
                        cmbxInterBankTransStatus.IsEnabled = false;
                    }
                }
                else if (bankTransfer.interBankTransStatus.isActive == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inter-Bank Transfer") == null)
                    {
                        btnSave.IsEnabled = false;
                    }
                }
                if (bankTransfer.FinanceRefNo != null)
                {
                    lblTransferRefNo.Text = " (" + bankTransfer.FinanceRefNo + ")";
                }

                if (bankTransfer.isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                else if (bankTransfer.isReApproved == false)
                {
                    //lblStage.Text = "Under Re-Approval";
                }
                else if (bankTransfer.isApproved == true && bankTransfer.stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (bankTransfer.isApproved == true && bankTransfer.interBankTransStatus.isActive == false && bankTransfer.PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (bankTransfer.isApproved == true && bankTransfer.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (bankTransfer.isApproved == true)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Amount in Inter-Bank Transfer After Approval") != null)
                        grdCntrlPOItems.Columns["value2"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                    else
                        grdCntrlPOItems.Columns["value2"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;

                }
                else if (bankTransfer.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Amount in Inter-Bank Transfer Under Approval") != null)
                        grdCntrlPOItems.Columns["value2"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                    else
                        grdCntrlPOItems.Columns["value2"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;

                }
                else if (bankTransfer.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }

                
                if (bankTransfer.isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                }
                views = UsersRepo.getViwerInfo(bankTransfer.Id, 18);
                grdUsers.ItemsSource = views;
                loadcomments();
                loadAttachments();

                datCreationDate.DateTime = (DateTime)bankTransfer.CreationDate;
                datTransactionDate.EditValue = bankTransfer.TransactionDate;
                datInstrumentDate.EditValue = bankTransfer.InstrumentDate;
                txtFinanceRefNo.Text = bankTransfer.FinanceRefNo;
                txtSystemRefNo.Text = bankTransfer.SystemRefNo;
                txtInstrument.Text = bankTransfer.InstrumentNo;

                //Select Transfer Type
                if (bankTransfer.transferType == TransferType.Inter_Company_Multiple_Currency )
                {
                    cmbxTransferType.SelectedIndex = 1;
                    txtMERto.Text = bankTransfer.MERto.ToString();
                    txtAmountMERto.Text = bankTransfer.AmountMERto.ToString();
                    txtAmountOCto.Text = bankTransfer.AmountTo.ToString();

                    txtER.Text = bankTransfer.AmountER.ToString();

                    txtMERfrom.Text = bankTransfer.MERfrom.ToString();
                    txtAmountMERfrom.Text = bankTransfer.AmountMERfrom.ToString();
                    txtAmountOCfrom.Text = bankTransfer.AmountFrom.ToString();
                }
                else if (bankTransfer.transferType == TransferType.Inter_Company_Multiple_Currency_Account_Convertor)
                {
                    cmbxTransferType.SelectedIndex = 2;
                    txtMERto.Text = bankTransfer.MERto.ToString();
                    txtAmountMERto.Text = bankTransfer.AmountMERto.ToString();
                    txtAmountOCto.Text = bankTransfer.AmountTo.ToString();

                    txtER.Text = bankTransfer.AmountER.ToString();

                    txtMERfrom.Text = bankTransfer.MERfrom.ToString();
                    txtAmountMERfrom.Text = bankTransfer.AmountMERfrom.ToString();
                    txtAmountOCfrom.Text = bankTransfer.AmountFrom.ToString();
                }
                else if (bankTransfer.transferType == TransferType.Inter_Company)
                {
                    cmbxTransferType.SelectedIndex = 0;
                }

                txtDescription.Text = bankTransfer.Description;
                int index = 0;
                if (bankTransfer.interBankTransStatus != null)
                {
                    var disAbleStatus = allInterBankTransStatus.FirstOrDefault(x => x.Id == bankTransfer.interBankTransStatus.Id);
                    if (disAbleStatus == null)
                    {
                        LoadStatuses(bankTransfer.interBankTransStatus);
                    }
                }
                if (bankTransfer.interBankTransStatus != null)
                {
                    var disAbleStatus = allInterBankTransStatus.FirstOrDefault(x => x.Id == bankTransfer.interBankTransStatus.Id);
                    if (disAbleStatus == null)
                    {
                        LoadStatuses(bankTransfer.interBankTransStatus);
                    }
                }
                //Select Status 
                if (bankTransfer.StatusId != null)
                {
                    oldStatus = bankTransfer.interBankTransStatus;
                    foreach (var _status in interBankTransStatusLst)
                    {
                        if (_status.id == bankTransfer.StatusId)
                        {
                            cmbxInterBankTransStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                //Select Company from
                var companyList = (lookupCompanyFrom.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompanyFrom.ItemsSource as List<Company>;
                if (bankTransfer.companyFrom_Id != null)
                {
                    index = 0;
                    foreach (var _company in companyList)
                    {
                        if (_company.Id == bankTransfer.companyFrom_Id)
                        {
                            lookupCompanyFrom.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                var IBTrefFromList = (cmbxPettyCashRefFrom.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPettyCashRefFrom.ItemsSource as List<cmbitem>;
                if (bankTransfer.PettyCashRefFrom != null)
                {
                    index = 0;
                    foreach (var _ref in IBTrefFromList)
                    {
                        if (_ref.id == bankTransfer.PettyCashRefFrom.Id)
                        {
                            cmbxPettyCashRefFrom.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Department from
                var deptList = (lookupDepartmentFrom.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartmentFrom.ItemsSource as List<Department>;
                if (bankTransfer.deptFrom_Id != null)
                {
                    foreach (var _dept in deptList)
                    {
                        if (_dept.Id == bankTransfer.deptFrom_Id)
                        {
                            lookupDepartmentFrom.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Bank From
                var bankList = (cmbxBankFrom.ItemsSource as List<Bank>) == null ? new List<Bank>() : cmbxBankFrom.ItemsSource as List<Bank>;
                if (bankTransfer.bankFrom_Id != null)
                {
                    foreach (var _bank in bankList)
                    {
                        if (_bank.Id == bankTransfer.bankFrom_Id)
                        {
                            cmbxBankFrom.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }



                if (bankTransfer.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                    //grdPOItems.ItemsSource = offer.products;
                    foreach (var procurementProduct in bankTransfer.products)
                    {
                        procurementProducts.Add(new ProcurementProduct()
                        {
                            Id = procurementProduct.Id,

                            inquiryProduct = new InquiryProduct()
                            {
                                Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product = new Product()
                                {
                                    Id = procurementProduct.inquiryProduct.product.Id,
                                    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    item = procurementProduct.inquiryProduct.product.item,
                                    code = procurementProduct.inquiryProduct.product.code,
                                    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    nature = procurementProduct.inquiryProduct.product.nature,
                                    category = procurementProduct.inquiryProduct.product.category,
                                    isActive = procurementProduct.inquiryProduct.product.isActive

                                }
                                    ,
                                product_Id = procurementProduct.inquiryProduct.product.Id

                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,
                            UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity,
                            InvoicedQuantity = (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,
                            UnInvoicedWeight = (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,
                            InvoicedWeight = (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,
                            value1 = procurementProduct.value1,
                            value2 = procurementProduct.value2,
                            //caption1 = cmbcaption1.Text.Trim(),
                            //caption2 = cmbcaption2.Text.Trim(),
                            UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount
                            //UnInvoicedQuantity= procurementProduct.UnInvoicedQuantity,


                        });


                    }
                    //dGitems.ItemsSource = datagriditems;
                    grdCntrlPOItems.ItemsSource = procurementProducts;
                    int x = 0;
                    foreach (var pro in grdCntrlPOItems.ItemsSource as List<ProcurementProduct>)
                    {
                        grdCntrlPOItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                        x++;

                    }
                    lookupProductsinGrid.DisplayMember = "code";

                }
                else
                {
                    grdCntrlPOItems.ItemsSource = procurementProducts;
                }




                //Select account From
                var accountlist  = (cmbxAccountFrom.ItemsSource as List<Account>) == null ? new List<Account>() : cmbxAccountFrom.ItemsSource as List<Account>;
                if (bankTransfer.accountFrom_Id != null)
                {
                    foreach (var _Accnt in accountlist)
                    {
                        if (_Accnt.Id == bankTransfer.accountFrom_Id)
                        {
                            cmbxAccountFrom.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //start
                //Select Company to
                var companytolist = (lookupCompanyTo.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompanyTo.ItemsSource as List<Company>;
                if (bankTransfer.companyTo_Id != null)
                {
                    index = 0;
                    foreach (var _companyto  in companytolist)
                    {
                        if (_companyto.Id == bankTransfer.companyTo_Id)
                        {
                            lookupCompanyTo.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                var IBTrefToList = (cmbxPettyCashRefTo.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPettyCashRefTo.ItemsSource as List<cmbitem>;
                if (bankTransfer.PettyCashRefTo != null)
                {
                    index = 0;
                    foreach (var _ref in IBTrefToList)
                    {
                        if (_ref.id == bankTransfer.PettyCashRefTo.Id)
                        {
                            cmbxPettyCashRefTo.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Department to
                var depttoList = (lookupDepartmentTo.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartmentTo.ItemsSource as List<Department>;
                if (bankTransfer.deptTo_Id != null)
                {
                    foreach (var _deptto in depttoList)
                    {
                        if (_deptto.Id == bankTransfer.deptTo_Id)
                        {
                            lookupDepartmentTo.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Bank to
                var banktoList = (cmbxBankTo.ItemsSource as List<Bank>) == null ? new List<Bank>() : cmbxBankTo.ItemsSource as List<Bank>;
                if (bankTransfer.bankTo_Id != null)
                {
                    foreach (var _bankto in banktoList)
                    {
                        if (_bankto.Id == bankTransfer.bankTo_Id)
                        {
                            cmbxBankTo.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select account to
                var accounttolist = (cmbxAccountTo.ItemsSource as List<Account>) == null ? new List<Account>() : cmbxAccountTo.ItemsSource as List<Account>;
                if (bankTransfer.accountTo_Id != null)
                {
                    foreach (var _Accntto in accounttolist)
                    {
                        if (_Accntto.Id == bankTransfer.accountTo_Id)
                        {
                            cmbxAccountTo.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }


                if (bankTransfer.bankChargesFrom != null)
                    txtBankChargesFrom.Text = bankTransfer.bankChargesFrom.Sum(x => x.Amount).ToString();
                if (bankTransfer.bankChargesTo != null)
                    txtBankChargesTo.Text = bankTransfer.bankChargesTo.Sum(x => x.Amount).ToString();
                if (bankTransfer.InterCompBankTransferVATFrom != null)
                    txtVATFrom.Text = bankTransfer.InterCompBankTransferVATFrom.Sum(x => x.Amount).ToString();
                if (bankTransfer.InterCompBankTransferVATto != null)
                    txtVATTo.Text = bankTransfer.InterCompBankTransferVATto.Sum(x => x.Amount).ToString();


                //Select COA Debit
                var COAdebitList = (lookupCOAdebit.ItemsSource as List<ChartofAccount>) == null ? new List<ChartofAccount>() : lookupCOAdebit.ItemsSource as List<ChartofAccount>;
                if (COAdebitList.FirstOrDefault(x => x.Id == bankTransfer.COAdebit_Id) == null)
                {
                    COAdebitList.Add(bankTransfer.COAdebit);
                    lookupCOAdebit.ItemsSource = null;
                    lookupCOAdebit.ItemsSource = COAdebitList;
                }
                    

                if (bankTransfer.COAdebit != null)
                {
                    lookupCOAdebit.Text = bankTransfer.COAdebit.accountName;
                }

                //Select COA Debit
                var COAcreditList = (lookupCOAcredit.ItemsSource as List<ChartofAccount>) == null ? new List<ChartofAccount>() : lookupCOAcredit.ItemsSource as List<ChartofAccount>;
                if (COAcreditList.FirstOrDefault(x => x.Id == bankTransfer.COAcredit_Id) == null)
                {
                    COAcreditList.Add(bankTransfer.COAcredit);
                    lookupCOAcredit.ItemsSource = null;
                    lookupCOAcredit.ItemsSource = COAcreditList;
                }
                    

                if (bankTransfer.COAcredit_Id != null)
                {
                    foreach (var _coa in COAcreditList)
                    {
                        if (_coa.Id == bankTransfer.COAcredit_Id)
                        {
                            lookupCOAcredit.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Currency
                var currencyList = (cmbxCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : cmbxCurrency.ItemsSource as List<Currency>;
                if (bankTransfer.currency_Id != null)
                {
                    foreach (var _currency in currencyList)
                    {
                        if (_currency.Id == bankTransfer.currency_Id)
                        {
                            cmbxCurrency.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Currency
                var currencyFromList = (cmbxCurrencyFrom.ItemsSource as List<Currency>) == null ? new List<Currency>() : cmbxCurrencyFrom.ItemsSource as List<Currency>;
                if (bankTransfer.currencyFromId != null)
                {
                    foreach (var _currency in currencyList)
                    {
                        if (_currency.Id == bankTransfer.currencyFromId)
                        {
                            cmbxCurrencyFrom.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Currency
                var currencyToList = (cmbxCurrencyTo.ItemsSource as List<Currency>) == null ? new List<Currency>() : cmbxCurrencyTo.ItemsSource as List<Currency>;
                if (bankTransfer.currencyToId != null)
                {
                    foreach (var _currency in currencyList)
                    {
                        if (_currency.Id == bankTransfer.currencyToId)
                        {
                            cmbxCurrencyTo.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }


                txtMER.Text = bankTransfer.MER.ToString();
                txtAmountMer.Text = bankTransfer.AmountMER.ToString();



                //Select TransferMethod
                var transferMethodList = (cmbxTransferMethod.ItemsSource as List<TranferMethod>) == null ? new List<TranferMethod>() : cmbxTransferMethod.ItemsSource as List<TranferMethod>;
                if (bankTransfer.transferMethod_Id != null)
                {
                    foreach (var _method in transferMethodList)
                    {
                        if (_method.Id == bankTransfer.transferMethod_Id)
                        {
                            cmbxTransferMethod.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                var creditJournalTransactions = bankTransfer.journalTransactions.Where(x => x.credit != 0).ToList();
                var debitJournalTransactions = bankTransfer.journalTransactions.Where(x => x.debit != 0).ToList();
                if (creditJournalTransactions.Count > 0)
                {
                    btnPushCredits.IsChecked = true;
                }
                if (debitJournalTransactions.Count > 0)
                {
                    btnPushDebits.IsChecked = true;
                }
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Inter-Bank Transfer") != null)
            {
                datCreationDate.IsEnabled = true;
            }
            if (bankTransfer.VATBookRefId != 0 && bankTransfer.VATBookRefNumber != null)
            {
                btnVATBookPost.IsChecked = true;
                var vatBookSource = cmbxVATBookRef.Items.SourceCollection as List<cmbitem>;
                var term = vatBookSource.Find(x => x.id == bankTransfer.VATBookRefId);

                if (term == null)
                {
                    vatBookSource.Add(new cmbitem() { name = bankTransfer.VATBookRefNumber.VATBookReferenceNo, id = bankTransfer.VATBookRefNumber.Id });
                    cmbxVATBookRef.ItemsSource = null;
                    cmbxVATBookRef.ItemsSource = vatBookSource;
                }
                cmbxVATBookRef.SelectedItem = cmbxVATBookRef.Items[cmbxVATBookRef.Items.IndexOf(vatBookSource.Find(x => x.id == bankTransfer.VATBookRefId))];
            }
        }


        public List<PettyCash> getPettyCashFrom(int userId, Currency pettyCashCurrency, double AmountOC)
        {
            List<PettyCash> pettyCashes = new List<PettyCash>();
            //ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();

            //var debitChartofAccount = coaRepo.get(chartofAccountDebitId);
            //var creditChartofAccount = coaRepo.get(chartofAccountCreditId);
            double Amount = 0;
            if (AmountOC < 0)
            {
                Amount = AmountOC * (-1);
            }
            else
                Amount = AmountOC;
            if (bankTransfer != null && bankTransfer.Id != 0)
            {
                if (btnDepositFrom.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        InterCompanyId = bankTransfer.Id,
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                        debit = Amount,
                        credit = 0,
                        total = Amount - 0,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                        companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                        currencyId = pettyCashCurrency.Id,
                        PettyCashRefId = cmbxPettyCashRefFrom.SelectedIndex > -1 ? (cmbxPettyCashRefFrom.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDepositFrom = true;
                }
                else if (btnPaymentFrom.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        InterCompanyId = bankTransfer.Id,
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                        debit = 0,
                        credit = Amount,
                        total = 0 - Amount,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                        companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                        currencyId = pettyCashCurrency.Id,
                        PettyCashRefId = cmbxPettyCashRefFrom.SelectedIndex > -1 ? (cmbxPettyCashRefFrom.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDepositFrom = false;
                }
                else
                {
                    bankTransfer.isDepositFrom = null;
                    bankTransfer.isAmountOCfrom = null;
                }
            }
            else
            {
                if (btnDepositFrom.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        InterCompanyId = bankTransfer.Id,
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                        debit = Amount,
                        credit = 0,
                        total = Amount - 0,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                        companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                        currencyId = pettyCashCurrency.Id,
                        PettyCashRefId = cmbxPettyCashRefFrom.SelectedIndex > -1 ? (cmbxPettyCashRefFrom.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDepositFrom = true;
                }
                else if (btnPaymentFrom.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        InterCompanyId = bankTransfer.Id,
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                        debit = Amount,
                        credit = 0,
                        total = 0 - Amount,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                        companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                        currencyId = pettyCashCurrency.Id,
                        PettyCashRefId = cmbxPettyCashRefFrom.SelectedIndex > -1 ? (cmbxPettyCashRefFrom.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDepositFrom = false;
                }
                else
                {
                    bankTransfer.isDepositFrom = null;
                    bankTransfer.isAmountOCfrom = null;
                }

            }
            return pettyCashes;
        }

        public List<PettyCash> getPettyCashTo(int userId, Currency pettyCashCurrency, double AmountOC)
        {
            List<PettyCash> pettyCashes = new List<PettyCash>();
            //ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();

            //var debitChartofAccount = coaRepo.get(chartofAccountDebitId);
            //var creditChartofAccount = coaRepo.get(chartofAccountCreditId);
            double Amount = 0;
            if (AmountOC < 0)
            {
                Amount = AmountOC * (-1);
            }
            else
                Amount = AmountOC;
            if (bankTransfer != null && bankTransfer.Id != 0)
            {
                if (btnDepositTo.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        InterCompanyId = bankTransfer.Id,
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                        debit = Amount,
                        credit = 0,
                        total = Amount - 0,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                        companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                        currencyId = pettyCashCurrency.Id,
                        PettyCashRefId = cmbxPettyCashRefTo.SelectedIndex > -1 ? (cmbxPettyCashRefTo.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDepositTo = true;
                }
                else if (btnPaymentTo.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        InterCompanyId = bankTransfer.Id,
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                        debit = 0,
                        credit = Amount,
                        total = 0 - Amount,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                        companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                        currencyId = pettyCashCurrency.Id,
                        PettyCashRefId = cmbxPettyCashRefTo.SelectedIndex > -1 ? (cmbxPettyCashRefTo.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDepositTo = false;
                }
                else
                {
                    bankTransfer.isDepositTo = null;
                    bankTransfer.isAmountOCto = null;
                }
            }
            else
            {
                if (btnDepositFrom.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        InterCompanyId = bankTransfer.Id,
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                        debit = Amount,
                        credit = 0,
                        total = Amount - 0,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                        companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                        currencyId = pettyCashCurrency.Id,
                        PettyCashRefId = cmbxPettyCashRefTo.SelectedIndex > -1 ? (cmbxPettyCashRefTo.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDepositTo = true;
                }
                else if (btnPaymentFrom.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        InterCompanyId = bankTransfer.Id,
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                        debit = Amount,
                        credit = 0,
                        total = 0 - Amount,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                        companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                        currencyId = pettyCashCurrency.Id,
                        PettyCashRefId = cmbxPettyCashRefTo.SelectedIndex > -1 ? (cmbxPettyCashRefTo.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDepositTo = false;
                }
                else
                {
                    bankTransfer.isDepositTo = null;
                    bankTransfer.isAmountOCto = null;
                }
            }
            return pettyCashes;
        }

        public void LoadCUrrencies()
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            var currencies = currencyRepo.getAll();
            currencies = currencies.Where(x => x.isVoid != true).ToList();
            cmbxCurrency.ItemsSource = currencies;
            cmbxCurrencyFrom.ItemsSource = currencies;
            cmbxCurrencyTo.ItemsSource = currencies;
        }
        private void LoadTransferMethod()
        {
            InterBankTransRepo transRepo = new InterBankTransRepo();
            cmbxTransferMethod.ItemsSource = transRepo.GetAllTransferMethods();

        }
        private void LoadDepartments()
        {
            // cmbxDepatmentTransferFromForm.ItemsSource = departmentRepo.GetActiveDepartments();
            // cmbxDepartmentTransferToForm.ItemsSource = departmentRepo.GetActiveDepartments();

        }
        //start
        private void LoadCompanies()
        {
            CompanyRepo companyRepo = new CompanyRepo();
            lookupCompanyFrom.ItemsSource = companyRepo.GetUserCompanies(SYSTEM_STATIC.currentUser.id);
            lookupCompanyTo.ItemsSource = companyRepo.getAll();

        }
        //end


        private void LoadTransferTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.TransferType.Inter_Company_Multiple_Currency_Account_Convertor; i++)
            {
                if(((ERP_BL.Enums.TransferType)i) == TransferType.Inter_Company || ((ERP_BL.Enums.TransferType)i) == TransferType.Inter_Company_Multiple_Currency || ((ERP_BL.Enums.TransferType)i) == TransferType.Inter_Company_Multiple_Currency_Account_Convertor)
                    cmbxTransferType.Items.Add(((ERP_BL.Enums.TransferType)i).ToString());
            }
        }




        private void CmbxTransferType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            GridsVisibility();
        }

        private void GridsVisibility()
        {
            if(cmbxTransferType.SelectedIndex == 0)
            {
                grdCntrlPOItems.Visibility = Visibility.Visible;
                layoutGrpER.Visibility = Visibility.Collapsed;
                imgArrow.Visibility = Visibility.Visible;

                layoutCurrency.Visibility = Visibility.Visible;

                grdCurrencyFrom.Visibility = Visibility.Collapsed;
                grdAmountFrom.Visibility = Visibility.Collapsed;
                grdMERfrom.Visibility = Visibility.Collapsed;
                grdAmountMERfrom.Visibility = Visibility.Collapsed;

                grdCurrencyTo.Visibility = Visibility.Collapsed;
                grdAmountTo.Visibility = Visibility.Collapsed;
                grdMERto.Visibility = Visibility.Collapsed;
                grdAmountMERto.Visibility = Visibility.Collapsed;

            }
            else if (cmbxTransferType.SelectedIndex == 1)
            {
                grdCntrlPOItems.Visibility = Visibility.Collapsed;
                layoutGrpER.Visibility = Visibility.Visible;
                imgArrow.Visibility = Visibility.Collapsed;

                layoutCurrency.Visibility = Visibility.Collapsed;

                grdCurrencyFrom.Visibility = Visibility.Visible;
                grdAmountFrom.Visibility = Visibility.Visible;
                grdMERfrom.Visibility = Visibility.Visible;
                grdAmountMERfrom.Visibility = Visibility.Visible;

                grdCurrencyTo.Visibility = Visibility.Visible;
                grdAmountTo.Visibility = Visibility.Visible;
                grdMERto.Visibility = Visibility.Visible;
                grdAmountMERto.Visibility = Visibility.Visible;


                cmbxCurrencyFrom.IsEnabled = false;
                cmbxCurrencyTo.IsEnabled = false;
            }
            else if (cmbxTransferType.SelectedIndex == 2)
            {
                grdCntrlPOItems.Visibility = Visibility.Collapsed;
                layoutGrpER.Visibility = Visibility.Visible;
                imgArrow.Visibility = Visibility.Collapsed;

                layoutCurrency.Visibility = Visibility.Collapsed;

                grdCurrencyFrom.Visibility = Visibility.Visible;
                grdAmountFrom.Visibility = Visibility.Visible;
                grdMERfrom.Visibility = Visibility.Visible;
                grdAmountMERfrom.Visibility = Visibility.Visible;

                grdCurrencyTo.Visibility = Visibility.Visible;
                grdAmountTo.Visibility = Visibility.Visible;
                grdMERto.Visibility = Visibility.Visible;
                grdAmountMERto.Visibility = Visibility.Visible;

                cmbxCurrencyFrom.IsEnabled = true;
                cmbxCurrencyTo.IsEnabled = true;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (bankTransfer.Id != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, bankTransfer.Id, 18, "Viewed details of Inter-Bank Transfer");
            }
        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (bankTransfer.Id > 0)
                {
                    if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartment(department.Id), comment, TransactionItemType.InterBank_Transfer);
                        inputBox.ShowDialog();

                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (bankTransfer.Id > 0)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();
                        if (frmInputBox.commentAdded == true && bankTransfer.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Company Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Company Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inter-Company Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inter-Company Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }

                        else if (bankTransfer.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save this Transaction first to add a comment!");
                        }
                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }


        private void Resend_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Resend_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            var comment = grdCommentss.SelectedItem as CommentLog;


            if (comment != null)
            {
                comment = procurementRepo.GetComment(comment.Id);
                if (comment.employee.EmployeeUsers.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) == null)
                {
                    DXMessageBox.Show("Only sender of this comment can change Flag!");
                    return;
                }
                if (department != null && department.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartment(department.Id), TransactionItemType.InterCompanyBank_Transfer);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }

                if (frmInputBox.commentAdded == true && bankTransfer.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (bankTransfer.Id == 0)
                {
                    DXMessageBox.Show("Kindly save this Transaction first to add a comment!");
                }
            }
            loadcomments();
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {

            if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartment(department.Id), TransactionItemType.InterCompanyBank_Transfer);
                inputBox.ShowDialog();

            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (bankTransfer.Id != 0)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.commentAdded == true && bankTransfer.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Company Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Company Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inter-Company Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inter-Company Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }
                    }

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (bankTransfer.Id == 0)
                {
                    DXMessageBox.Show("Kindly save this Transaction first to add a comment!");
                }

            }
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            bankTransfer = compTransferRepo.GetInterCompanyBankTransfer(bankTransfer.Id);

            List<ProcurementProduct> products = new List<ProcurementProduct>();
            foreach (var _prod in bankTransfer.products)
            {
                products.Add(_prod);
            }

            if (bankTransfer.isApproved != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null)
                {
                    bankTransfer.isReviewed = true;
                    bankTransfer.isApproved = true;
                    bankTransfer.stage = TransactionStage.Approved.ToString();
                    compTransferRepo.approveInterBankTransfer(bankTransfer, products);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Inter-Company Bank Transfer has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (bankTransfer.departmentFrom != null && bankTransfer.departmentFrom.Id != 0 && bankTransfer.companyFrom != null && bankTransfer.companyFrom?.Id != 0 && bankTransfer.departmentTo != null && bankTransfer.departmentTo.Id != 0 && bankTransfer.companyTo != null && bankTransfer.companyTo?.Id != 0) 
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { bankTransfer.departmentFrom.Id, bankTransfer.departmentTo.Id }, new List<int> { bankTransfer.companyFrom.Id, bankTransfer.companyTo.Id }), bankTransferId, TransactionItemType.InterCompanyBank_Transfer);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }
                    }

                    string symbolCurr = "";
                    if (bankTransfer.currency != null)
                    {
                        symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "InterCompany Bank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "ICBT Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in ICBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in ICBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }
            else if (bankTransfer.isApproved == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null)
                {
                    bankTransfer.isReviewed = true;
                    bankTransfer.isApproved = false;
                    bankTransfer.stage = TransactionStage.AwaitingApproval.ToString();
                    compTransferRepo.approveInterBankTransfer(bankTransfer, products);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Inter-Company Bank Transfer has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (bankTransfer.departmentFrom != null && bankTransfer.departmentFrom.Id != 0 && bankTransfer.companyFrom != null && bankTransfer.companyFrom?.Id != 0 && bankTransfer.departmentTo != null && bankTransfer.departmentTo.Id != 0 && bankTransfer.companyTo != null && bankTransfer.companyTo?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { bankTransfer.departmentFrom.Id, bankTransfer.departmentTo.Id }, new List<int> { bankTransfer.companyFrom.Id, bankTransfer.companyTo.Id }), bankTransferId, TransactionItemType.InterCompanyBank_Transfer);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }
                    }

                    string symbolCurr = "";
                    if (bankTransfer.currency != null)
                    {
                        symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "InterCompany Bank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                        Timestamp = DateTime.Now,
                        Subject = "ICBT UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in ICBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in ICBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }

        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;

        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach.Visibility == Visibility.Visible)
                grdAttach.Visibility = Visibility.Collapsed;
            else
            {
                grdAttach.Visibility = Visibility.Visible;
            }
        }

        public void loadAttachments()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (bankTransfer.Id > 0)
            {
                if (bankTransfer.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Inter-Bank Transfer") != null))
                {
                    if (DXMessageBox.Show("This Transaction is currently in the list of Void Inter-Bank Transfers! Do you want to remove it from Void?", "Remove Void Inter-Bank Transfers", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        bankTransfer.isVoid = false;
                        compTransferRepo.setInterCompanyBankTransfertoVoid(bankTransfer.Id, false);
                        grdVoid.Visibility = Visibility.Collapsed;

                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        ProcurementRepo procurementRepo = new ProcurementRepo();
                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res1 = MessageBox.Show("InterCompany Bank Transfer has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (bankTransfer.departmentFrom != null && bankTransfer.departmentFrom.Id != 0 && bankTransfer.companyFrom != null && bankTransfer.companyFrom?.Id != 0 && bankTransfer.departmentTo != null && bankTransfer.departmentTo.Id != 0 && bankTransfer.companyTo != null && bankTransfer.companyTo?.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { bankTransfer.departmentFrom.Id, bankTransfer.departmentTo.Id }, new List<int> { bankTransfer.companyFrom.Id, bankTransfer.companyTo.Id }), bankTransferId, TransactionItemType.InterCompanyBank_Transfer);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                                //inputBox.ShowDialog();
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }

                        }

                        string symbolCurr = "";
                        if (bankTransfer.currency != null)
                        {
                            symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "InterCompany Bank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                            Timestamp = DateTime.Now,
                            Subject = "ICBT UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in ICBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in ICBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                    }
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Inter-Bank Transfer") != null)
                {
                    if (DXMessageBox.Show("This Transaction is not currently in the list of Void Inter-Bank Transfers! Do you want to move it to Void Inter-Bank Transfers?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        bankTransfer.isVoid = true;
                        compTransferRepo.setInterCompanyBankTransfertoVoid(bankTransfer.Id, true);
                        grdVoid.Visibility = Visibility.Visible;
                        txtVoid.RenderTransform = new RotateTransform(-45);


                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        ProcurementRepo procurementRepo = new ProcurementRepo();
                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res1 = MessageBox.Show("InterCompany Bank Transfer has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (bankTransfer.departmentFrom != null && bankTransfer.departmentFrom.Id != 0 && bankTransfer.companyFrom != null && bankTransfer.companyFrom?.Id != 0 && bankTransfer.departmentTo != null && bankTransfer.departmentTo.Id != 0 && bankTransfer.companyTo != null && bankTransfer.companyTo?.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { bankTransfer.departmentFrom.Id, bankTransfer.departmentTo.Id }, new List<int> { bankTransfer.companyFrom.Id, bankTransfer.companyTo.Id }), bankTransferId, TransactionItemType.InterCompanyBank_Transfer);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                                //inputBox.ShowDialog();
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }

                        }

                        string symbolCurr = "";
                        if (bankTransfer.currency != null)
                        {
                            symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "InterCompany Bank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                            Timestamp = DateTime.Now,
                            Subject = "ICBT Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in ICBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in ICBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Permission Required to Mark or UnMark a Transaction to Void!");
                }
            }

            loadcomments();
        }

        private void CmbxAccountFrom_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompanyFrom.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank first!");
                return;
            }
        }

        private void CmbxAccountFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxAccountFrom.SelectedItem != null)
            {
                var bankAccount = cmbxAccountFrom.SelectedItem as Account;
                
                layoutGrpTransferFrom.Header = "Transfer From" + " [" + (cmbxAccountFrom.SelectedItem as Account).AccountNick + "]";

                if (cmbxTransferType.SelectedIndex == 1)
                {
                    cmbxCurrencyFrom.Text = bankAccount.currency.CurrencyName;

                    if (bankAccount.COAaccount != null && bankAccount.COAaccount.JournalTransactions != null)
                    {
                        var chartofAccount = bankAccount.COAaccount;
                        var accntBalance = chartofAccount.JournalTransactions.Where(
                        x => x.AdminBill?.isVoid != true
                        && x.Bill?.isVoid != true
                        && x.PurchaseInvoice?.isVoid != true
                        && x.SaleInvoice?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.journalVoucher?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.InterBank?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        &&
                        x.deptId != null && deptIds.Contains((int)x.deptId)
                        &&
                        x.companyId != null && companyIds.Contains((int)x.companyId)
                        ).Sum(x => x.total);

                        var amountOC = Convert.ToDouble(txtAmountOCfrom.Text);
                        txtExistingBalanceFrom.Text = accntBalance.ToString();
                        txtAfterTransferFrom.Text = (accntBalance - amountOC).ToString();
                    }
                }
                else
                {
                    if (bankAccount.COAaccount != null && bankAccount.COAaccount.JournalTransactions != null)
                    {
                        var chartofAccount = bankAccount.COAaccount;
                        var accntBalance = chartofAccount.JournalTransactions.Where(
                        x => x.AdminBill?.isVoid != true
                        && x.Bill?.isVoid != true
                        && x.PurchaseInvoice?.isVoid != true
                        && x.SaleInvoice?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.journalVoucher?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.InterBank?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        &&
                        x.deptId != null && deptIds.Contains((int)x.deptId)
                        &&
                        x.companyId != null && companyIds.Contains((int)x.companyId)
                        ).Sum(x => x.total);

                        var amountOC = Convert.ToDouble(txtAmountOC.Text);
                        txtExistingBalanceFrom.Text = accntBalance.ToString();
                        txtAfterTransferFrom.Text = (accntBalance - amountOC).ToString();
                    }
                }
            }
        }

        private void CmbxBankFrom_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompanyFrom.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void CmbxBankFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxBankFrom.SelectedIndex > -1)
            {
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                Department dept = new Department();
                dept = lookupDepartmentFrom.SelectedItem as Department;
                var bank = cmbxBankFrom.SelectedItem as Bank;
                var accntList = receiptRepo.GetAllAccountsByBankId(bank.Id).Where(x => x.isActive == true).ToList();
                List<Account> allowedAccounts = new List<Account>();

                foreach (var _account in accntList)
                {
                    List<int> dept_ids = new List<int>();

                    foreach (var _dept in _account.departments)
                    {
                        dept_ids.Add(_dept.Id);
                    }
                    if (dept_ids.Contains(dept.Id) && _account.bank.Id == bank.Id && _account.company.Id == (lookupCompanyFrom.SelectedItem as Company).Id && (_account.accountsCategory == ERP_BL.Enums.AccountsCategory.Company || _account.accountsCategory == ERP_BL.Enums.AccountsCategory.Personal))
                        allowedAccounts.Add(_account);
                }
                if (editFlag == true && bankTransfer.AccountFrom != null && allowedAccounts.Find(x => x.Id == bankTransfer.accountFrom_Id) == null)
                    allowedAccounts.Add(bankTransfer.AccountFrom);
                cmbxAccountFrom.ItemsSource = allowedAccounts;
            }


        }

        private void CmbxBankTo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompanyTo.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void CmbxBankTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxBankTo.SelectedIndex > -1)
            {
                cmbxAccountTo.ItemsSource = null;
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                Department department = new Department();
                department = lookupDepartmentTo.SelectedItem as Department;
                var bank = cmbxBankTo.SelectedItem as Bank;
                var accntList = receiptRepo.GetAllAccountsByBankId(bank.Id).Where(x => x.isActive == true).ToList();
                List<Account> allowedAccounts = new List<Account>();

                foreach (var _account in accntList)
                {
                    List<int> dept_ids = new List<int>();

                    foreach (var _dept in _account.departments)
                    {
                        dept_ids.Add(_dept.Id);
                    }
                    if (dept_ids.Contains(department.Id) && _account.bank.Id == bank.Id && _account.company.Id == (lookupCompanyTo.SelectedItem as Company).Id && (_account.accountsCategory == ERP_BL.Enums.AccountsCategory.Company || _account.accountsCategory == ERP_BL.Enums.AccountsCategory.Personal))
                        allowedAccounts.Add(_account);
                }
                //if (editFlag == true && bankTransfer.AccountTo != null && allowedAccounts.Find(x => x.Id == bankTransfer.accountTo_Id) == null)
                //    allowedAccounts.Add(bankTransfer.AccountTo);
                cmbxAccountTo.ItemsSource = allowedAccounts;
            }
        }

        private void CmbxAccountTo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompanyTo.SelectedIndex < 0)
            {

                DXMessageBox.Show("Please select Bank first!");
                return;
            }
        }




        private void CmbxAccountTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxAccountTo.SelectedItem != null)
            {
                var bankAccount = cmbxAccountTo.SelectedItem as Account;
                
                layoutGrpTransferTo.Header = "Transfer TO" + " [" + bankAccount.AccountNick + "]";

                if (cmbxTransferType.SelectedIndex == 1)
                {
                    cmbxCurrencyTo.Text = bankAccount.currency.CurrencyName;

                    if (bankAccount.COAaccount != null && bankAccount.COAaccount.JournalTransactions != null)
                    {
                        var chartofAccount = bankAccount.COAaccount;
                        var accntBalance = chartofAccount.JournalTransactions.Where(
                        x => x.AdminBill?.isVoid != true
                        && x.Bill?.isVoid != true
                        && x.PurchaseInvoice?.isVoid != true
                        && x.SaleInvoice?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.journalVoucher?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.InterBank?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        &&
                        x.deptId != null && deptIds.Contains((int)x.deptId)
                        &&
                        x.companyId != null && companyIds.Contains((int)x.companyId)
                        ).Sum(x => x.total);

                        var amountOC = Convert.ToDouble(txtAmountOCfrom.Text);
                        txtExistingBalanceTo.Text = accntBalance.ToString();
                        txtAfterTransferTo.Text = (accntBalance + amountOC).ToString();
                    }
                }
                else
                {
                    if (bankAccount.COAaccount != null && bankAccount.COAaccount.JournalTransactions != null)
                    {
                        var chartofAccount = bankAccount.COAaccount;
                        var accntBalance = chartofAccount.JournalTransactions.Where(
                        x => x.AdminBill?.isVoid != true
                        && x.Bill?.isVoid != true
                        && x.PurchaseInvoice?.isVoid != true
                        && x.SaleInvoice?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.journalVoucher?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.InterBank?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        &&
                        x.deptId != null && deptIds.Contains((int)x.deptId)
                        &&
                        x.companyId != null && companyIds.Contains((int)x.companyId)
                        ).Sum(x => x.total);

                        var amountOC = Convert.ToDouble(txtAmountOC.Text);
                        txtExistingBalanceTo.Text = accntBalance.ToString();
                        txtAfterTransferTo.Text = (accntBalance + amountOC).ToString();
                    }
                }
            }
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            Productss.frmItemadd frmItemadd = new Productss.frmItemadd();
            frmItemadd.ShowDialog();
            //products = productrepo.getAll();
            products = SYSTEM_STATIC.GetItemsForCurrentUser();
            //lookupProductinGrid.ItemsSource = products;
            lookupProductsinGrid.ItemsSource = products;
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (bankTransfer.Id != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\InterBank_Transfer\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            if (sourceFile.Length < 74)
                            {

                                destination += bankTransfer.Id + "_" + TransactionItemType.InterCompanyBank_Transfer.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.InterBank_Transfer);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, bankTransfer.Id, 18, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                });
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;
                            }

                            //;


                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {



                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            {
                try
                {
                    string sSelectedPath = "";

                    Button thisButton = (Button)sender;
                    string str = thisButton.Tag.ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                       


                        System.Threading.Thread thread = new System.Threading.Thread(() =>
                        {
                            //Button thisButton = (Button)sender;

                            ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                            var result = attachment.startDownload(str, TransactionItemType.InterBank_Transfer);


                            // var res =  Tuple.Create( true, sSelectedPath);

                            //result = sSelectedPath;
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                            }
                            else

                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        });
                        thread.Start();
                    }
                    else
                        return;



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    SystemLog.LogError(this.GetType(), ex.ToString());


                }
            }
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        public ucBankTransferInterCompany(InterBankTransferStatus status)
        {
            statusChanged = status;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer") != null)
            {
                var selectedBankTransfer = bankTransfer;//grdInterBankTransfer.SelectedItem as InterBankTransfer;
                UsersRepo usersRepo = new UsersRepo();

                if (selectedBankTransfer != null && selectedBankTransfer.Id != 0)
                {

                    var selectedRow = bankTransfer;//grdInterBankTransfer.SelectedItem as InterBankTransfer;
                    ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer bankTransfer1 = new ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer();
                    bankTransfer1 = compTransferRepo.GetInterCompanyBankTransfer(bankTransfer.Id);

                    List<ProcurementProduct> products = new List<ProcurementProduct>();
                    foreach (var _prod in bankTransfer1.products)
                    {
                        products.Add(_prod);
                    }

                    if (bankTransfer1.isApproved == false)
                    {
                        MessageBox.Show("Transaction should be approved before closing!");
                        return;
                    }
                    var previous_status = bankTransfer1.interBankTransStatus.Status;


                    statusChanged = null;
                    ucFrmInterCompanyDirectClose ucFrmDirectClose = new ucFrmInterCompanyDirectClose();

                    if (bankTransfer1.interBankTransStatus != null)
                    {
                        ucFrmDirectClose.statusName.Text = bankTransfer1.interBankTransStatus.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(bankTransfer1.interBankTransStatus.backcolor);
                    }

                    ucFrmDirectClose.IBTflag = true;
                    
                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null) 
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null)
                        {
                            bankTransfer1.PendingForClosing = false;
                            bankTransfer1.stage = TransactionStage.Approved.ToString();
                            bankTransfer1.interBankTransStatus = statusChanged;
                            bankTransfer1.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer1.ClosingDate = System.DateTime.Now;

                            compTransferRepo.approveInterBankTransfer(bankTransfer1, products);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                        {
                            bankTransfer1.interBankTransStatus = statusChanged;
                            bankTransfer1.stage = TransactionStage.AwaitingApproval.ToString();
                            bankTransfer1.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer1.ClosingDate = System.DateTime.Now;
                            if (bankTransfer1.PendingForClosing == null)
                            {
                                bankTransfer1.PendingForClosing = true;
                            }
                            compTransferRepo.approveInterBankTransfer(bankTransfer1, products);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null)
                        {
                            bankTransfer1.interBankTransStatus = statusChanged;
                            bankTransfer1.stage = TransactionStage.AwaitingApproval.ToString();
                            bankTransfer1.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer1.ClosingDate = System.DateTime.Now;
                            if (bankTransfer1.PendingForClosing != true)
                            {
                                bankTransfer1.PendingForClosing = true;

                            }
                            compTransferRepo.approveInterBankTransfer(bankTransfer1, products);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
                        }
                        else
                        {
                            bankTransfer1.interBankTransStatus = statusChanged;
                            bankTransfer1.stage = TransactionStage.AwaitingFirstReview.ToString();
                            bankTransfer1.PendingForClosing = true;
                            bankTransfer1.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer1.ClosingDate = System.DateTime.Now;
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
                            compTransferRepo.approveInterBankTransfer(bankTransfer1, products);
                        }
                        //Adding signature (comment)

                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Bank Tranfer has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (selectedRow.departmentFrom != null && selectedRow.deptFrom_Id != 0 && selectedRow.companyFrom?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(selectedRow.departmentFrom.Id, selectedRow.companyFrom.Id),bankTransferId, TransactionItemType.InterCompanyBank_Transfer);
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }
                            string oldStat = "";
                            if (oldStatus != null)
                            {
                                oldStat = oldStatus.Status;
                            }
                            string newStat = statusChanged.Status;
                            string symbolCurr = "";
                            if (selectedRow.currency != null)
                            {
                                symbolCurr = selectedRow.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Internal BankTransfer (Amount OC) having value: " + selectedRow.AmountOC.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(selectedRow.Id, TransactionItemType.InterCompanyBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);

                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in InterBank Transfer #" + selectedRow.SystemRefNo, selectedRow.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in InterBank Transfer #" + selectedRow.SystemRefNo, selectedRow.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, 0,user.id, "New Comment ", null);
                                }
                            }

                        }
                        MessageBox.Show("Inter-Bank Transfer status changed to InActive (" + statusChanged.Status + ")");
                    }
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Close Inter-Bank Transfer!");
                return;
            }
        }



        private void CmbIndustry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmbxTaxFlag_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void CmbxTaxName_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void CmbxTaxType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void CmbxVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TxtAmountOC_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void TxtMER_KeyUp(object sender, KeyEventArgs e)
        {
            var amountOC = Convert.ToDouble(txtAmountOC.Text);
            var MER = Convert.ToDouble(txtMER.Text);

            txtAmountMer.Text = (amountOC * MER).ToString();

            //if (((ERP_BL.Enums.TaxFlag)cmbxTaxFlag.SelectedIndex) == ERP_BL.Enums.TaxFlag.Tax)
            //{
            //    // Setting "Tranfer From" and "Transfer To" Layout group fields values
            //    txtAmountPaidOC.Text = (Convert.ToDouble(txtAmountOC.Text) - Convert.ToDouble(txtTaxAmountOC.Text)).ToString();
            //    txtAmountPaidMER.Text = (Convert.ToDouble(txtAmountMer.Text) - Convert.ToDouble(txtTaxAmountMER.Text)).ToString();
            //    txtAmountReceivedMER.Text = txtAmountMer.Text;
            //    txtAmountReceivedOC.Text = txtAmountOC.Text;

            //    //taxCalculations();
            //}
            //else
            //{
            //    txtAmountPaidMER.Text = txtAmountMer.Text;
            //    txtAmountPaidOC.Text = txtAmountOC.Text;
            //}

        }

        private void View_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            calculatetotal();
        }



        private void calculatetotal()
        {
            //double sumfob = 0;
            double sumAmount = 0;
            //decimal? weight = 0;
            //double Quantity = 0;

            if (grdCntrlPOItems.ItemsSource != null)
                foreach (var item in grdCntrlPOItems.ItemsSource as List<ProcurementProduct>)
                {

                    {
                        //if (item.inquiryProduct.quantity != 0)
                        //{
                        //    weight += (item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0) * Convert.ToDecimal(item.inquiryProduct.quantity);
                        //}
                        //else
                        //{
                        //    weight += item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0;
                        //}

                        //Quantity += item.inquiryProduct.quantity;
                        //sumfob += item.value1;
                        sumAmount += item.value2;
                    }
                }
            txtAmountOC.Text = sumAmount.ToString();
            decimal amountOC;
            //fob = Convert.ToDecimal(txtfob.Text);
            amountOC = Convert.ToDecimal(txtAmountOC.Text);

            // Setting "Tranfer From" and "Transfer To" Layout group fields values
            txtAmountPaidOC.Text = (Convert.ToDouble(txtAmountOC.Text) - Convert.ToDouble(txtTaxAmountOC.Text)).ToString();
            txtAmountReceivedOC.Text = txtAmountOC.Text;

            decimal marginExchangeRate = 1;
            if (txtAmountOC.Text != "")
            {
                decimal totalcfr = Convert.ToDecimal(txtAmountOC.Text);
                if (txtMER.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMER.Text);
                    txtAmountMer.Text = (marginExchangeRate * totalcfr).ToString();

                    // Setting "Tranfer From" and "Transfer To" Layout group fields values
                    txtAmountPaidMER.Text = (Convert.ToDouble(txtAmountMer.Text) - Convert.ToDouble(txtTaxAmountMER.Text)).ToString();
                    txtAmountReceivedMER.Text = txtAmountMer.Text;
                }
            }

            //taxCalculations();
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void CmbxCompanyForm_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
            if (lookupCompanyFrom.SelectedIndex > -1)
            {
                var company = lookupCompanyFrom.SelectedItem as Company;
                List<Department> departments = new List<Department>();
                if (company != null)
                    if (company.departments != null)
                    {
                        foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsInterCompTransferType == true))
                        {
                            if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                                departments.Add(_dept);
                        }
                        if (bankTransfer != null && bankTransfer.Id > 0 && editFlag == true)
                            if (bankTransfer.departmentFrom != null && departments.FirstOrDefault(x => x.Id == bankTransfer.deptFrom_Id) == null)
                                departments.Add(bankTransfer.departmentFrom);

                        lookupDepartmentFrom.ItemsSource = departments;

                        if (departments.Count == 0)
                        {
                            MessageBox.Show("This company dosen't contain any department mapped with the current User");
                        }
                    }

                loadIBTReferenceNoFrom();
                //start
                var bankList = receiptRepo.GetAllBanksByCompany(company.Id);
                //allowedBanks = new List<Bank>();

                //foreach (var _bank in bankList)
                //{
                //    List<int> comp_ids = new List<int>();
                //    foreach (var _comp in _bank.companies)
                //    {
                //        comp_ids.Add(_comp.Id);
                //    }
                //    if (comp_ids.Contains(company.Id))
                //        allowedBanks.Add(_bank);
                //}
                //Only Allowed departments to Employee will show in Dropdown
                cmbxBankFrom.ItemsSource = bankList;
                //  cmbxBankTo.ItemsSource = allowedBanks;
            }
            loadVATBookReferenceNo();
        }

        //stop

        private string calculateSystemRefNo()
        {
            string sysRefNo;
            string lastSystemRefNo;

            lastSystemRefNo = compTransferRepo.getLastSystemReferenceNo();

            if (lastSystemRefNo == null)
            {
                sysRefNo = systemRefIntitials + "1";
            }
            else
            {
                int refNo = Convert.ToInt32(lastSystemRefNo.Remove(0, 4));
                refNo = refNo + 1;
                sysRefNo = systemRefIntitials + refNo.ToString();
            }

            return sysRefNo;
        }


        private void CmbxDepatmentTransferFromForm_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(lookupDepartmentFrom.SelectedIndex > -1)
            {
                department = lookupDepartmentFrom.SelectedItem as Department;
                var cmpny = lookupCompanyFrom.SelectedItem as Company;

                ChartofAccountsRepo accntRepo = new ChartofAccountsRepo();
                


                lookupCOAdebit.ItemsSource = accntRepo.GetAllforBills(cmpny, department,SYSTEM_STATIC.currentUser.id);
            }
        }

        private void CmbxCompanyTransferTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
            Company company = lookupCompanyTo.SelectedItem as Company;

            //start
            List<Department> departments = new List<Department>();
            if (company != null)
                if (company.departments != null)
                {
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsInterCompTransferType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (bankTransfer != null && bankTransfer.Id > 0 && editFlag == true)
                        if (bankTransfer.departmentTo != null && departments.FirstOrDefault(x => x.Id == bankTransfer.deptTo_Id) == null)
                            departments.Add(bankTransfer.departmentTo);

                    lookupDepartmentTo.ItemsSource = departments;

                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }

            loadIBTReferenceNoTo();
            //var company = lookupCompanyFrom.SelectedItem as Company;
            var bankList = receiptRepo.GetAllBanksByCompany(company.Id);
            
            //Only Allowed departments to Employee will show in Dropdown

            cmbxBankTo.ItemsSource = bankList;
        }

        private void CmbxDepartmentTransferToForm_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupDepartmentTo.SelectedIndex > -1)
            {
                department = lookupDepartmentTo.SelectedItem as Department;
                var cmpny = lookupCompanyTo.SelectedItem as Company;

                ChartofAccountsRepo accntRepo = new ChartofAccountsRepo();
                
                lookupCOAcredit.ItemsSource = accntRepo.GetAllForInterCompany( cmpny, department, SYSTEM_STATIC.currentUser.id);
            }
        }

        private void lookupCompanyTo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompanyFrom.SelectedIndex < 0)
            {

                DXMessageBox.Show("Please select Company first!");
                return;
            }
        }

        private void CmbxDepatmentTransferFromForm_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompanyFrom.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
        }

        private void CmbxDepartmentTransferToForm_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompanyTo.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(editFlag == false)
                txtSystemRefNo.Text = calculateSystemRefNo();


            //ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer bankTransfer = new ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer();

            //6/3/2021 start
            if (string.IsNullOrEmpty(txtFinanceRefNo.Text))
            {
                DXMessageBox.Show("Please select the finance reference number");
                txtFinanceRefNo.Focus();
                return;
            }
            if (txtSystemRefNo == null)
            {
                DXMessageBox.Show("Please select the System reference number");
            }

            if (editFlag == false && bankTransfer.Id == 0)
            {
                if (cmbxInterBankTransStatus.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Status first!");
                    cmbxInterBankTransStatus.Focus();
                    return;
                }
            }
            if (String.IsNullOrEmpty(txtInstrument.Text))
            {
                DXMessageBox.Show("Please select Instrument number first");
                txtInstrument.Focus();
                return;
            }
            if (cmbxTransferMethod.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Transfer method first");
                cmbxTransferMethod.Focus();
                return;
            }
            
            //Transfer from section
            if (lookupCompanyFrom.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                lookupCompanyFrom.Focus();
                return;
            }
            if (lookupDepartmentFrom.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                lookupDepartmentFrom.Focus();
                return;
            }
            if (cmbxBankFrom.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select bank first!");
                cmbxBankFrom.Focus();
                return;
            }
            if (cmbxAccountFrom.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Account first!");
                cmbxAccountFrom.Focus();
                return;
            }

            if (lookupCOAdebit.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select COA Debit!");
                lookupCOAdebit.Focus();
                return;
            }

            //Transfer to section
            if (lookupCompanyTo.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                lookupCompanyTo.Focus();
                return;
            }
            if (lookupDepartmentTo.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                lookupDepartmentTo.Focus();
                return;
            }
            if (cmbxBankTo.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select bank first!");
                cmbxBankTo.Focus();
                return;
            }
            if (cmbxAccountTo.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Account first!");
                cmbxAccountTo.Focus();
                return;
            }
            if (lookupCOAcredit.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select COA Credit!");
                lookupCOAcredit.Focus();
                return;
            }

            List<ProcurementProduct> products = new List<ProcurementProduct>();
            if (cmbxTransferType.SelectedIndex == 0)
            {
                if ((txtAmountOC.Text == null) || (Convert.ToDouble(txtAmountOC.Text) == 0))
                {
                    DXMessageBox.Show("Please enter Amount(OC) first!");
                    txtAmountOC.Focus();
                    return;
                }
                if (cmbxCurrency.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Currency first!");
                    cmbxCurrency.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtMER.Text) || (Convert.ToDouble(txtMER.Text) == 0))
                {
                    DXMessageBox.Show("Please enter MER first!");
                    txtMER.Focus();
                    return;
                }
                
                
                products = getProductsdata();
                bankTransfer.products = products;
                bankTransfer.transferType = TransferType.Inter_Company;

                if (bankTransfer.products.Count == 0)
                {
                    DXMessageBox.Show("Please Select items against which you want to create a Bank Transfer", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
            }
            else if (cmbxTransferType.SelectedIndex == 1)
            {
                if (cmbxBankTo.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bank first!");
                    return;
                }
                if (cmbxCurrencyFrom.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account having Currency Linked!");
                    cmbxAccountFrom.Focus();
                    return;
                }
                if (cmbxCurrencyTo.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account having Currency Linked!");
                    cmbxAccountTo.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtAmountOCfrom.Text) || Convert.ToDouble(txtAmountOCfrom.Text) == 0)
                {
                    DXMessageBox.Show("Please Enter Amount from the Bank you want to Transfer Amount!");
                    txtAmountOCfrom.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtAmountOCto.Text) || Convert.ToDouble(txtAmountOCto.Text) == 0)
                {
                    DXMessageBox.Show("Please Enter Amount to the Bank you want to Transfer Amount!");
                    txtAmountOCto.Focus();
                    return;
                }

                bankTransfer.transferType = TransferType.Inter_Company_Multiple_Currency;

                bankTransfer.currencyFromId = (cmbxCurrencyFrom.SelectedItem as Currency).Id;
                bankTransfer.AmountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
                bankTransfer.MERfrom = Convert.ToDouble(txtMERfrom.Text);
                bankTransfer.AmountMERfrom = Convert.ToDouble(txtAmountMERfrom.Text);

                bankTransfer.AmountER = Convert.ToDouble(txtER.Text);

                bankTransfer.currencyToId = (cmbxCurrencyTo.SelectedItem as Currency).Id;
                bankTransfer.AmountTo = Convert.ToDouble(txtAmountOCto.Text);
                bankTransfer.MERto = Convert.ToDouble(txtMERto.Text);
                bankTransfer.AmountMERto = Convert.ToDouble(txtAmountMERto.Text);
            }
            else if (cmbxTransferType.SelectedIndex == 2)
            {
                if (cmbxBankTo.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bank first!");
                    return;
                }
                if (cmbxCurrencyFrom.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account having Currency Linked!");
                    cmbxAccountFrom.Focus();
                    return;
                }
                if (cmbxCurrencyTo.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account having Currency Linked!");
                    cmbxAccountTo.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtAmountOCfrom.Text) || Convert.ToDouble(txtAmountOCfrom.Text) == 0)
                {
                    DXMessageBox.Show("Please Enter Amount from the Bank you want to Transfer Amount!");
                    txtAmountOCfrom.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtAmountOCto.Text) || Convert.ToDouble(txtAmountOCto.Text) == 0)
                {
                    DXMessageBox.Show("Please Enter Amount to the Bank you want to Transfer Amount!");
                    txtAmountOCto.Focus();
                    return;
                }

                bankTransfer.transferType = TransferType.Inter_Company_Multiple_Currency_Account_Convertor;

                bankTransfer.currencyFromId = (cmbxCurrencyFrom.SelectedItem as Currency).Id;
                bankTransfer.AmountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
                bankTransfer.MERfrom = Convert.ToDouble(txtMERfrom.Text);
                bankTransfer.AmountMERfrom = Convert.ToDouble(txtAmountMERfrom.Text);

                bankTransfer.AmountER = Convert.ToDouble(txtER.Text);

                bankTransfer.currencyToId = (cmbxCurrencyTo.SelectedItem as Currency).Id;
                bankTransfer.AmountTo = Convert.ToDouble(txtAmountOCto.Text);
                bankTransfer.MERto = Convert.ToDouble(txtMERto.Text);
                bankTransfer.AmountMERto = Convert.ToDouble(txtAmountMERto.Text);
            }


            if (btnDepositFrom.IsChecked == true || btnPaymentFrom.IsChecked == true)
            {
                if (cmbxPettyCashRefFrom.SelectedIndex <= 0)
                {
                    DXMessageBox.Show("Please select Petty Cash Reference #!");
                    cmbxPettyCashRefFrom.Focus();
                    return;
                }
                if(chkAmountOCfrom.IsChecked == false && chkAmountMERfrom.IsChecked == false)
                {
                    DXMessageBox.Show("Please Check mark Amount (OC) or Amount (MER) for Petty Cash Entry!");
                    return;
                }
            }

            if (btnDepositTo.IsChecked == true || btnPaymentTo.IsChecked == true)
            {
                if (cmbxPettyCashRefTo.SelectedIndex <= 0)
                {
                    DXMessageBox.Show("Please select Petty Cash Reference #!");
                    cmbxPettyCashRefTo.Focus();
                    return;
                }
                if (chkAmountOCto.IsChecked == false && chkAmountMERto.IsChecked == false)
                {
                    DXMessageBox.Show("Please Check mark Amount (OC) or Amount (MER) for Petty Cash Entry!");
                    return;
                }
            }
            if (editFlag == false)
            {
                if (receiptGroupId != 0)
                {
                    bankTransfer.receiptGroupId = receiptGroupId;
                }
                if (paymentGroupId != 0)
                {
                    bankTransfer.paymentGroupId = paymentGroupId;
                }
            }
            //transfer to section end
            bankTransfer.Description = txtDescription.Text;
            bankTransfer.CreationDate = datCreationDate.DateTime;
            bankTransfer.SystemRefNo = txtSystemRefNo.Text;
            bankTransfer.FinanceRefNo = txtFinanceRefNo.Text;
            bankTransfer.TransactionDate = datTransactionDate.DateTime;
            bankTransfer.InstrumentNo = txtInstrument.Text;
            bankTransfer.InstrumentDate = datInstrumentDate.DateTime;
            bankTransfer.transferMethod_Id = (cmbxTransferMethod.SelectedItem as TranferMethod).Id;

            if (cmbxTransferType.SelectedIndex == 0)
            {
                bankTransfer.AmountOC = Convert.ToDouble(txtAmountOC.Text);
                bankTransfer.AmountMER = Convert.ToDouble(txtAmountMer.Text);
                bankTransfer.currency_Id = (cmbxCurrency.SelectedItem as Currency).Id;
                bankTransfer.MER = Convert.ToDouble(txtMER.Text);
            }
                
            bankTransfer.companyFrom_Id = (lookupCompanyFrom.SelectedItem as Company).Id;
            bankTransfer.deptFrom_Id = (lookupDepartmentFrom.SelectedItem as Department).Id;
            bankTransfer.bankFrom_Id = (cmbxBankFrom.SelectedItem as Bank).Id;
            bankTransfer.accountFrom_Id = (cmbxAccountFrom.SelectedItem as Account).Id;
            bankTransfer.COAdebit_Id = (lookupCOAdebit.SelectedItem as ChartofAccount).Id;

            bankTransfer.companyTo_Id = (lookupCompanyTo.SelectedItem as Company).Id;
            bankTransfer.deptTo_Id = (lookupDepartmentTo.SelectedItem as Department).Id;
            bankTransfer.bankTo_Id = (cmbxBankTo.SelectedItem as Bank).Id;
            bankTransfer.accountTo_Id = (cmbxAccountTo.SelectedItem as Account).Id;
         

            bankTransfer.COAcredit_Id = (lookupCOAcredit.SelectedItem as ChartofAccount).Id;

            if (cmbxPettyCashRefFrom.SelectedIndex > -1)
                bankTransfer.PettyCashRefFromId = (cmbxPettyCashRefFrom.SelectedItem as cmbitem).id;

            if (cmbxPettyCashRefTo.SelectedIndex > -1)
                bankTransfer.PettyCashRefToId = (cmbxPettyCashRefTo.SelectedItem as cmbitem).id;

            if ((cmbxInterBankTransStatus.SelectedItem as cmbitem) != null)
            {
                var status = cmbxInterBankTransStatus.SelectedItem as cmbitem;
                if (status != null)
                {
                    bankTransfer.StatusId = status.id;
                }
            }
            if (stlId != 0)
            {
                bankTransfer.stlId = stlId;
            }
            bankTransfer.GLPostingDate = (DateTime)datglPostingdate.EditValue;
            if (editFlag == true)
                bankTransfer.journalTransactions = getJournalTransactions(bankTransfer.Id);
            else
            bankTransfer.journalTransactions = getJournalTransactions();
            List<ERP_BL.VATBook.VATBook> vatBooks = new List<ERP_BL.VATBook.VATBook>();
            if (editFlag == true)
            {
                if (cmbxVATBookRef.SelectedIndex > -1)
                {
                    bankTransfer.VATBookRefId = (cmbxVATBookRef.SelectedItem as cmbitem).id;
                    if (btnVATBookPost.IsChecked == true)
                    {
                        int currencyFrom = 0;
                        int currencyTo = 0;
                        if (bankTransfer.transferType == TransferType.Inter_Company)
                        {
                            currencyFrom =(cmbxCurrency.SelectedItem as Currency).Id;
                            currencyTo =(cmbxCurrency.SelectedItem as Currency).Id;

                        }
                        else
                        if (bankTransfer.transferType == TransferType.Inter_Company_Multiple_Currency || bankTransfer.transferType == TransferType.Inter_Company_Multiple_Currency_Account_Convertor)
                        {
                            currencyFrom=(cmbxCurrencyFrom.SelectedItem as Currency).Id;
                            currencyTo=(cmbxCurrencyFrom.SelectedItem as Currency).Id;
                        }

                        foreach (var tax in bankTransfer.InterCompBankTransferVATFrom)
                        {
                            vatBooks.Add(new ERP_BL.VATBook.VATBook()
                            {
                                CreationDate = datCreationDate.DateTime,
                                GLPostingDate = datCreationDate.DateTime,
                                interCompanyId = bankTransfer.Id,
                                TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                                debit = tax.Amount,
                                credit = 0,
                                total = tax.Amount - 0,
                                FinanceRefNo = txtFinanceRefNo.Text,
                                SystemRefNo = txtSystemRefNo.Text,
                                MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                currencyId = currencyFrom,
                                VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                            });
                        }
                        foreach (var tax in bankTransfer.InterCompBankTransferVATto)
                        {
                            vatBooks.Add(new ERP_BL.VATBook.VATBook()
                            {
                                CreationDate = datCreationDate.DateTime,
                                GLPostingDate = datCreationDate.DateTime,
                                paymentId = bankTransfer.Id,
                                TransactionType = TransactionItemType.InterCompanyBank_Transfer,
                                debit = tax.Amount,
                                credit = 0,
                                total = tax.Amount - 0,
                                FinanceRefNo = txtFinanceRefNo.Text,
                                SystemRefNo = txtSystemRefNo.Text,
                                MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                currencyId = currencyTo,
                                VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                            });
                        }
                        bankTransfer.VATBooks = vatBooks;
                    }
                }
            }


            if (editFlag == true)
            {
                if(cmbxTransferType.SelectedIndex == 0)
                {
                    if (chkAmountOCfrom.IsChecked == true)
                    {
                        bankTransfer.pettyCashesFrom = getPettyCashFrom(SYSTEM_STATIC.currentUser.id, cmbxCurrency.SelectedItem as Currency, Convert.ToDouble(txtAmountOC.Text));
                        bankTransfer.isAmountOCfrom = true;
                    }
                    else if (chkAmountMERfrom.IsChecked == true)
                    {
                        bankTransfer.pettyCashesFrom = getPettyCashFrom(SYSTEM_STATIC.currentUser.id, cmbxCurrency.SelectedItem as Currency, Convert.ToDouble(txtAmountMer.Text));
                        bankTransfer.isAmountOCfrom = false;
                    }
                    else
                    {
                        bankTransfer.isAmountOCfrom = null;
                        bankTransfer.isDepositFrom = null;
                    }
                        

                    if (chkAmountOCto.IsChecked == true)
                    {
                        bankTransfer.isAmountOCto = true;
                        bankTransfer.pettyCashesTo = getPettyCashTo(SYSTEM_STATIC.currentUser.id, cmbxCurrency.SelectedItem as Currency, Convert.ToDouble(txtAmountOC.Text));
                    }
                    else if (chkAmountMERto.IsChecked == true)
                    {
                        bankTransfer.pettyCashesTo = getPettyCashTo(SYSTEM_STATIC.currentUser.id, cmbxCurrency.SelectedItem as Currency, Convert.ToDouble(txtAmountMer.Text));
                        bankTransfer.isAmountOCto = false;
                    }
                    else
                    {
                        bankTransfer.isAmountOCto = null;
                        bankTransfer.isDepositTo = null;
                    }
                        
                }
                else
                {
                    
                    if (chkAmountOCfrom.IsChecked == true)
                    {
                        bankTransfer.pettyCashesFrom = getPettyCashFrom(SYSTEM_STATIC.currentUser.id, cmbxCurrencyFrom.SelectedItem as Currency, Convert.ToDouble(txtAmountOCfrom.Text));
                        bankTransfer.isAmountOCfrom = true;
                    }
                    else if (chkAmountMERfrom.IsChecked == true)
                    {
                        bankTransfer.pettyCashesFrom = getPettyCashFrom(SYSTEM_STATIC.currentUser.id, cmbxCurrencyFrom.SelectedItem as Currency, Convert.ToDouble(txtAmountMERfrom.Text));
                        bankTransfer.isAmountOCfrom = false;
                    }
                    else
                        bankTransfer.isAmountOCfrom = null;


                    if (chkAmountOCto.IsChecked == true)
                    {
                        bankTransfer.isAmountOCto = true;

                        bankTransfer.pettyCashesTo= getPettyCashTo(SYSTEM_STATIC.currentUser.id, cmbxCurrencyTo.SelectedItem as Currency, Convert.ToDouble(txtAmountOCto.Text));
                    }
                    else if (chkAmountMERto.IsChecked == true)
                    {
                        bankTransfer.pettyCashesTo = getPettyCashTo(SYSTEM_STATIC.currentUser.id, cmbxCurrencyTo.SelectedItem as Currency, Convert.ToDouble(txtAmountMERto.Text));
                        bankTransfer.isAmountOCto = false;
                    }
                    else
                        bankTransfer.isAmountOCto = null;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null && bankTransfer.isApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Transaction is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        bankTransfer.stage = TransactionStage.Approved.ToString();
                        bankTransfer.isApproved = true;
                        bankTransfer.ApprovedDate = System.DateTime.Now;
                    }
                }
                  compTransferRepo.updateInterBankTransfer(bankTransfer, products);
                if (oldStatus != null)
                {
                    if (oldStatus.Id != bankTransfer.StatusId)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Transaction has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (bankTransfer.departmentFrom != null && bankTransfer.departmentFrom.Id != 0 && bankTransfer.companyFrom?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(bankTransfer.departmentFrom.Id, bankTransfer.companyFrom.Id), bankTransferId, TransactionItemType.InterCompanyBank_Transfer);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }
                        }
                        string oldStat = oldStatus.Status;
                        string newStat = (cmbxInterBankTransStatus.SelectedItem as cmbitem).name;
                        string symbolCurr = "";

                        if (bankTransfer.currency != null)
                        {
                            symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of InterBank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);

                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, 0,user.id, "New Comment ", null);
                            }
                        }
                    }
                }
                DXMessageBox.Show("Updated Successfully");
            }
            else if(editFlag == false)
            {
                if (cmbxTransferType.SelectedIndex == 0)
                {
                    if (chkAmountOCfrom.IsChecked == true)
                    {
                        bankTransfer.pettyCashesFrom = getPettyCashFrom(SYSTEM_STATIC.currentUser.id, cmbxCurrency.SelectedItem as Currency, Convert.ToDouble(txtAmountOC.Text));
                        bankTransfer.isAmountOCfrom = true;
                    }
                    if (chkAmountMERfrom.IsChecked == true)
                    {
                        bankTransfer.pettyCashesFrom = getPettyCashFrom(SYSTEM_STATIC.currentUser.id, cmbxCurrency.SelectedItem as Currency, Convert.ToDouble(txtAmountMer.Text));
                        bankTransfer.isAmountOCfrom = false;
                    }

                    if (chkAmountOCto.IsChecked == true)
                    {
                        bankTransfer.isAmountOCto = true;

                        bankTransfer.pettyCashesTo = getPettyCashTo(SYSTEM_STATIC.currentUser.id, cmbxCurrency.SelectedItem as Currency, Convert.ToDouble(txtAmountOC.Text));
                    }
                    if (chkAmountMERto.IsChecked == true)
                    {
                        bankTransfer.pettyCashesTo = getPettyCashTo(SYSTEM_STATIC.currentUser.id, cmbxCurrency.SelectedItem as Currency, Convert.ToDouble(txtAmountMer.Text));
                        bankTransfer.isAmountOCto = false;
                    }
                }
                else
                {
                    if (chkAmountOCfrom.IsChecked == true)
                    {
                        bankTransfer.pettyCashesFrom = getPettyCashFrom(SYSTEM_STATIC.currentUser.id, cmbxCurrencyFrom.SelectedItem as Currency, Convert.ToDouble(txtAmountOCfrom.Text));
                        bankTransfer.isAmountOCfrom = true;
                    }
                    if (chkAmountMERfrom.IsChecked == true)
                    {
                        bankTransfer.pettyCashesFrom = getPettyCashFrom(SYSTEM_STATIC.currentUser.id, cmbxCurrencyFrom.SelectedItem as Currency, Convert.ToDouble(txtAmountMERfrom.Text));
                        bankTransfer.isAmountOCfrom = false;
                    }

                    if (chkAmountOCto.IsChecked == true)
                    {
                        bankTransfer.isAmountOCto = true;

                        bankTransfer.pettyCashesTo = getPettyCashTo(SYSTEM_STATIC.currentUser.id, cmbxCurrencyTo.SelectedItem as Currency, Convert.ToDouble(txtAmountOCto.Text));
                    }
                    if (chkAmountMERto.IsChecked == true)
                    {
                        bankTransfer.pettyCashesTo = getPettyCashTo(SYSTEM_STATIC.currentUser.id, cmbxCurrencyTo.SelectedItem as Currency, Convert.ToDouble(txtAmountMERto.Text));
                        bankTransfer.isAmountOCto = false;
                    }
                }

                bankTransfer.isApproved = false;
                compTransferRepo.AddTransaction(bankTransfer);
                DXMessageBox.Show("Added Successfully");
            }
            //Comment to show in commit
           
          



            var window = Window.GetWindow(this);
            window.Close();
            

        }
        public List<JournalTransaction> getJournalTransactions()
        {
            List<JournalTransaction> transactionList = new List<JournalTransaction>();
            DateTime postingDate = (DateTime)datglPostingdate.EditValue;
            PaymentRepo paymentRepo = new PaymentRepo();
            TaxRepo taxRepo = new TaxRepo();
            if (bankTransfer.transferType == TransferType.Inter_Company)
            {
                if (btnPushCredits.IsChecked == true)
                {
                    JournalTransaction transactionFromCredit = new JournalTransaction()
                    {
                        coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                        creationDate = postingDate,
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                        credit = Convert.ToDouble(txtAmountOC.Text),
                        debit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        total = 0 - Convert.ToDouble(txtAmountOC.Text),
                        deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                        companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                    };
                    transactionList.Add(transactionFromCredit);

                }
                if (btnPushDebits.IsChecked == true)
                {
                    JournalTransaction transactionFromDebit = new JournalTransaction()
                    {
                        coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                        creationDate = postingDate,
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        accountId = (lookupCOAdebit.SelectedItem as ChartofAccount).Id,
                        credit = 0,
                        debit = Convert.ToDouble(txtAmountOC.Text),
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        total = Convert.ToDouble(txtAmountOC.Text) - 0,
                        deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                        companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                    };
                    transactionList.Add(transactionFromDebit);

                }
                if (btnPushDebits.IsChecked == true)
                {
                    JournalTransaction transactionToDebit = new JournalTransaction()
                    {
                        coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                        creationDate = postingDate,
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,

                        accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                        credit = 0,
                        debit = Convert.ToDouble(txtAmountOC.Text),
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        total = Convert.ToDouble(txtAmountOC.Text) - 0,
                        deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                        companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                    };
                    transactionList.Add(transactionToDebit);
                }
                if (btnPushCredits.IsChecked == true)
                {
                    JournalTransaction transactionToCredit = new JournalTransaction()
                    {
                        coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                        creationDate = postingDate,
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        accountId = (lookupCOAcredit.SelectedItem as ChartofAccount).Id,
                        credit = Convert.ToDouble(txtAmountOC.Text),
                        debit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        total = Convert.ToDouble(txtAmountOC.Text) - 0,
                        deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                        companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                    };
                    transactionList.Add(transactionToCredit);

                }
                if (bankTransfer.bankChargesFrom != null)
                {
                    if (bankTransfer.bankChargesFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesFrom)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.bankChargesTo != null)
                {
                    if (bankTransfer.bankChargesTo.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesTo)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterCompBankTransferVATFrom != null)
                {
                    if (bankTransfer.InterCompBankTransferVATFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterCompBankTransferVATFrom)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterCompBankTransferVATto != null)
                {
                    if (bankTransfer.InterCompBankTransferVATto.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterCompBankTransferVATto)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                bankTransfer.journalTransactions = transactionList;
            }
            else
            if (bankTransfer.transferType == TransferType.Inter_Company_Multiple_Currency)
            {
                if (btnPushCredits.IsChecked == true)
                {
                    JournalTransaction transactionFromCredit = new JournalTransaction()
                    {
                        coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                        creationDate = postingDate,
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                        credit = Convert.ToDouble(txtAmountOCfrom.Text),
                        debit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                        total = 0 - Convert.ToDouble(txtAmountOCfrom.Text),
                        deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                        companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id
                    };
                    transactionList.Add(transactionFromCredit);
                }
                if (btnPushDebits.IsChecked == true)
                {
                    JournalTransaction transactionFromDebit = new JournalTransaction()
                    {
                        coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                        creationDate = postingDate,
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        accountId = (lookupCOAdebit.SelectedItem as ChartofAccount).Id,
                        credit = 0,
                        debit = Convert.ToDouble(txtAmountOCfrom.Text),
                        MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                        total = Convert.ToDouble(txtAmountOCfrom.Text) - 0,
                        deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                        companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id
                    };
                    transactionList.Add(transactionFromDebit);
                }
                if (btnPushDebits.IsChecked == true)
                {
                    JournalTransaction transactionToDebit = new JournalTransaction()
                    {
                        coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                        creationDate = postingDate,
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,

                        accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                        credit = 0,
                        debit = Convert.ToDouble(txtAmountOCto.Text),
                        MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                        total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                        deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                        companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id
                    };
                    transactionList.Add(transactionToDebit);
                }
                if (btnPushCredits.IsChecked == true)
                {
                    JournalTransaction transactionToCredit = new JournalTransaction()
                    {
                        coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                        creationDate = postingDate,
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        accountId = (lookupCOAcredit.SelectedItem as ChartofAccount).Id,
                        credit = Convert.ToDouble(txtAmountOCto.Text),
                        debit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                        total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                        deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                        companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id
                    };
                    transactionList.Add(transactionToCredit);

                }
                if (bankTransfer.bankChargesFrom != null)
                {
                    if (bankTransfer.bankChargesFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesFrom)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.bankChargesTo != null)
                {
                    if (bankTransfer.bankChargesTo.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesTo)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterCompBankTransferVATFrom != null)
                {
                    if (bankTransfer.InterCompBankTransferVATFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterCompBankTransferVATFrom)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterCompBankTransferVATto != null)
                {
                    if (bankTransfer.InterCompBankTransferVATto.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterCompBankTransferVATto)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                //bankTransfer.journalTransactions = transactionList;
            }
            return transactionList;
        }
        public List<JournalTransaction> getJournalTransactions(int transferId)
        {
            List<JournalTransaction> transactionList = new List<JournalTransaction>();
            DateTime postingDate = (DateTime)datglPostingdate.EditValue;
            JournalEntryRepo repo = new JournalEntryRepo();
            PaymentRepo paymentRepo = new PaymentRepo();
            TaxRepo taxRepo = new TaxRepo();
            var dbTrans = repo.getTransBYIBCT(transferId);


            if (bankTransfer.transferType == TransferType.Inter_Company)
            {
                if (btnPushCredits.IsChecked == true)
                {
                    if(dbTrans==null)
                    {
                        JournalTransaction transactionFromCredit = new JournalTransaction()
                        {
                            coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                            creationDate = postingDate,
                            memo = txtFinanceRefNo.Text,
                            transactionRefno = txtSystemRefNo.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                            credit = Convert.ToDouble(txtAmountOC.Text),
                            debit = 0,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            total = 0 - Convert.ToDouble(txtAmountOC.Text),
                            deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                            companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                        };
                        transactionList.Add(transactionFromCredit);
                    }
                    else
                    {
                        if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                        {
                            JournalTransaction transactionFromCredit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                credit = Convert.ToDouble(txtAmountOC.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                total = 0 - Convert.ToDouble(txtAmountOC.Text),
                                deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                reconcilationDate = null,
                                reconcilationType = ReconcilationType.Uncleared_Transactions,
                                ReconcilationId = null,
                                isReconciled = false

                            };
                            transactionList.Add(transactionFromCredit);
                        }
                        else
                        {
                            JournalTransaction transactionFromCredit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                credit = Convert.ToDouble(txtAmountOC.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                total = 0 - Convert.ToDouble(txtAmountOC.Text),
                                deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                            };
                            transactionList.Add(transactionFromCredit);
                        }
                    }
                    
                }
                if (btnPushDebits.IsChecked == true)
                {
                    if(dbTrans==null)
                    {
                        JournalTransaction transactionFromDebit = new JournalTransaction()
                        {
                            coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                            creationDate = postingDate,
                            memo = txtFinanceRefNo.Text,
                            transactionRefno = txtSystemRefNo.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            accountId = (lookupCOAdebit.SelectedItem as ChartofAccount).Id,
                            credit = 0,
                            debit = Convert.ToDouble(txtAmountOC.Text),
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            total = Convert.ToDouble(txtAmountOC.Text) - 0,
                            deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                            companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id,

                        };
                        transactionList.Add(transactionFromDebit);

                    }
                    else
                    {
                        if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                        {
                            JournalTransaction transactionFromDebit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (lookupCOAdebit.SelectedItem as ChartofAccount).Id,
                                credit = 0,
                                debit = Convert.ToDouble(txtAmountOC.Text),
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                reconcilationDate = null,
                                reconcilationType = ReconcilationType.Uncleared_Transactions,
                                ReconcilationId = null,
                                isReconciled = false
                            };
                            transactionList.Add(transactionFromDebit);
                        }
                        else
                        {
                            JournalTransaction transactionFromDebit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (lookupCOAdebit.SelectedItem as ChartofAccount).Id,
                                credit = 0,
                                debit = Convert.ToDouble(txtAmountOC.Text),
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id,

                            };
                            transactionList.Add(transactionFromDebit);
                        }
                    }
                  
                    

                }
                if (btnPushDebits.IsChecked == true)
                {

                    if(dbTrans==null)
                    {
                        JournalTransaction transactionToDebit = new JournalTransaction()
                        {
                            coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                            creationDate = postingDate,
                            memo = txtFinanceRefNo.Text,
                            transactionRefno = txtSystemRefNo.Text,
                            userId = SYSTEM_STATIC.currentUser.id,

                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                            credit = 0,
                            debit = Convert.ToDouble(txtAmountOC.Text),
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            total = Convert.ToDouble(txtAmountOC.Text) - 0,
                            deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                            companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                        };
                        transactionList.Add(transactionToDebit);
                    }
                    else
                    {
                        if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                        {
                            JournalTransaction transactionToDebit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,

                                accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                credit = 0,
                                debit = Convert.ToDouble(txtAmountOC.Text),
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                reconcilationDate = null,
                                reconcilationType = ReconcilationType.Uncleared_Transactions,
                                ReconcilationId = null,
                                isReconciled = false
                            };
                            transactionList.Add(transactionToDebit);
                        }
                        else
                        {
                            JournalTransaction transactionToDebit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,

                                accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                credit = 0,
                                debit = Convert.ToDouble(txtAmountOC.Text),
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                            };
                            transactionList.Add(transactionToDebit);
                        }
                    }
                   
                }
                if (btnPushCredits.IsChecked == true)
                {
                    if(dbTrans==null)
                    {
                        JournalTransaction transactionToCredit = new JournalTransaction()
                        {
                            coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                            creationDate = postingDate,
                            memo = txtFinanceRefNo.Text,
                            transactionRefno = txtSystemRefNo.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            accountId = (lookupCOAcredit.SelectedItem as ChartofAccount).Id,
                            credit = Convert.ToDouble(txtAmountOC.Text),
                            debit = 0,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            total = Convert.ToDouble(txtAmountOC.Text) - 0,
                            deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                            companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                        };
                        transactionList.Add(transactionToCredit);
                    }
                    else
                    {
                        if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                        {
                            JournalTransaction transactionToCredit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (lookupCOAcredit.SelectedItem as ChartofAccount).Id,
                                credit = Convert.ToDouble(txtAmountOC.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                reconcilationDate = null,
                                reconcilationType = ReconcilationType.Uncleared_Transactions,
                                ReconcilationId = null,
                                isReconciled = false
                            };
                            transactionList.Add(transactionToCredit);
                        }
                        else
                        {
                            JournalTransaction transactionToCredit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (lookupCOAcredit.SelectedItem as ChartofAccount).Id,
                                credit = Convert.ToDouble(txtAmountOC.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                            };
                            transactionList.Add(transactionToCredit);
                        }
                    }
                    

                }
                if (bankTransfer.bankChargesFrom != null)
                {
                    if (bankTransfer.bankChargesFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesFrom)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.bankChargesTo != null)
                {
                    if (bankTransfer.bankChargesTo.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesTo)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterCompBankTransferVATFrom != null)
                {
                    if (bankTransfer.InterCompBankTransferVATFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterCompBankTransferVATFrom)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterCompBankTransferVATto != null)
                {
                    if (bankTransfer.InterCompBankTransferVATto.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterCompBankTransferVATto)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                bankTransfer.journalTransactions = transactionList;
            }
            else
            if (bankTransfer.transferType == TransferType.Inter_Company_Multiple_Currency)
            {
                if (btnPushCredits.IsChecked == true)
                {

                    if(dbTrans == null)
                    {
                        JournalTransaction transactionFromCredit = new JournalTransaction()
                        {
                            coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                            creationDate = postingDate,
                            memo = txtFinanceRefNo.Text,
                            transactionRefno = txtSystemRefNo.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                            credit = Convert.ToDouble(txtAmountOCfrom.Text),
                            debit = 0,
                            MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                            total = 0 - Convert.ToDouble(txtAmountOCfrom.Text),
                            deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                            companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id
                        };
                        transactionList.Add(transactionFromCredit);
                    }
                    else
                    {
                        if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                        {
                            JournalTransaction transactionFromCredit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                credit = Convert.ToDouble(txtAmountOCfrom.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                total = 0 - Convert.ToDouble(txtAmountOCfrom.Text),
                                deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id,
                                reconcilationDate = null,
                                reconcilationType = ReconcilationType.Uncleared_Transactions,
                                ReconcilationId = null,
                                isReconciled = false
                            };
                            transactionList.Add(transactionFromCredit);
                        }
                        else
                        {
                            JournalTransaction transactionFromCredit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                credit = Convert.ToDouble(txtAmountOCfrom.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                total = 0 - Convert.ToDouble(txtAmountOCfrom.Text),
                                deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id
                            };
                            transactionList.Add(transactionFromCredit);
                        }
                    }
                   
                }
                if (btnPushDebits.IsChecked == true)
                {
                    if(dbTrans == null)
                    {
                        JournalTransaction transactionFromDebit = new JournalTransaction()
                        {
                            coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                            creationDate = postingDate,
                            memo = txtFinanceRefNo.Text,
                            transactionRefno = txtSystemRefNo.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            accountId = (lookupCOAdebit.SelectedItem as ChartofAccount).Id,
                            credit = 0,
                            debit = Convert.ToDouble(txtAmountOCfrom.Text),
                            MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                            total = Convert.ToDouble(txtAmountOCfrom.Text) - 0,
                            deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                            companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id
                        };
                        transactionList.Add(transactionFromDebit);
                    }
                    else
                    {
                        if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                        {
                            JournalTransaction transactionFromDebit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (lookupCOAdebit.SelectedItem as ChartofAccount).Id,
                                credit = 0,
                                debit = Convert.ToDouble(txtAmountOCfrom.Text),
                                MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                total = Convert.ToDouble(txtAmountOCfrom.Text) - 0,
                                deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id,
                                reconcilationDate = null,
                                reconcilationType = ReconcilationType.Uncleared_Transactions,
                                ReconcilationId = null,
                                isReconciled = false
                            };
                            transactionList.Add(transactionFromDebit);
                        }
                        else
                        {
                            JournalTransaction transactionFromDebit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (lookupCOAdebit.SelectedItem as ChartofAccount).Id,
                                credit = 0,
                                debit = Convert.ToDouble(txtAmountOCfrom.Text),
                                MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                total = Convert.ToDouble(txtAmountOCfrom.Text) - 0,
                                deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id
                            };
                            transactionList.Add(transactionFromDebit);
                        }
                    }
                   
                }
                if (btnPushDebits.IsChecked == true)
                {
                    if(dbTrans == null)
                    {
                        JournalTransaction transactionToDebit = new JournalTransaction()
                        {
                            coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                            creationDate = postingDate,
                            memo = txtFinanceRefNo.Text,
                            transactionRefno = txtSystemRefNo.Text,
                            userId = SYSTEM_STATIC.currentUser.id,

                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                            credit = 0,
                            debit = Convert.ToDouble(txtAmountOCto.Text),
                            MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                            total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                            deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                            companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id
                        };
                        transactionList.Add(transactionToDebit);
                    }
                    else
                    {
                        if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                        {
                            JournalTransaction transactionToDebit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,

                                accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                credit = 0,
                                debit = Convert.ToDouble(txtAmountOCto.Text),
                                MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                                deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id,
                                reconcilationDate = null,
                                reconcilationType = ReconcilationType.Uncleared_Transactions,
                                ReconcilationId = null,
                                isReconciled = false
                            };
                            transactionList.Add(transactionToDebit);
                        }
                        else
                        {
                            JournalTransaction transactionToDebit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,

                                accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                credit = 0,
                                debit = Convert.ToDouble(txtAmountOCto.Text),
                                MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                                deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id
                            };
                            transactionList.Add(transactionToDebit);
                        }
                    }
                    
                }
                if (btnPushCredits.IsChecked == true)
                {
                    if(dbTrans == null)
                    {
                        JournalTransaction transactionToCredit = new JournalTransaction()
                        {
                            coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                            creationDate = postingDate,
                            memo = txtFinanceRefNo.Text,
                            transactionRefno = txtSystemRefNo.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            accountId = (lookupCOAcredit.SelectedItem as ChartofAccount).Id,
                            credit = Convert.ToDouble(txtAmountOCto.Text),
                            debit = 0,
                            MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                            total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                            deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                            companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id
                        };
                        transactionList.Add(transactionToCredit);
                    }
                    else
                    {
                        if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                        {
                            JournalTransaction transactionToCredit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (lookupCOAcredit.SelectedItem as ChartofAccount).Id,
                                credit = Convert.ToDouble(txtAmountOCto.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                                deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id,
                                reconcilationDate = null,
                                reconcilationType = ReconcilationType.Uncleared_Transactions,
                                ReconcilationId = null,
                                isReconciled = false
                            };
                            transactionList.Add(transactionToCredit);
                        }
                        else
                        {
                            JournalTransaction transactionToCredit = new JournalTransaction()
                            {
                                coaTransactionsType = coaTransactionsType.InterCompanyTransfer,
                                creationDate = postingDate,
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                accountId = (lookupCOAcredit.SelectedItem as ChartofAccount).Id,
                                credit = Convert.ToDouble(txtAmountOCto.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                                deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id
                            };
                            transactionList.Add(transactionToCredit);
                        }
                    }
                    

                }
                if (bankTransfer.bankChargesFrom != null)
                {
                    if (bankTransfer.bankChargesFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesFrom)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.bankChargesTo != null)
                {
                    if (bankTransfer.bankChargesTo.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesTo)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterCompBankTransferVATFrom != null)
                {
                    if (bankTransfer.InterCompBankTransferVATFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterCompBankTransferVATFrom)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMERfrom.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentFrom.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyFrom.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterCompBankTransferVATto != null)
                {
                    if (bankTransfer.InterCompBankTransferVATto.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterCompBankTransferVATto)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMERto.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (lookupDepartmentTo.SelectedItem as Department).Id,
                                    companyId = (lookupCompanyTo.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                //bankTransfer.journalTransactions = transactionList;
            }
            return transactionList;
        }
        private void CmbxCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if (cmbxCurrency.SelectedIndex != -1)
            //{
            //    currency = cmbxCurrency.SelectedItem as Currency;
            //}
        }

        private void CmbxInterBankTransStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbxInterBankTransStatus.SelectedIndex != -1)
            {

                status = cmbxInterBankTransStatus.SelectedItem as InterBankTransferStatus;
            }



        }
        public List<ProcurementProduct> getProductsdata()
        {
            List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
            List<ProcurementProduct> bankTransItems = new List<ProcurementProduct>();
            ProcurementProduct product = new ProcurementProduct();
            procurementProducts = grdCntrlPOItems.ItemsSource as List<ProcurementProduct>;
            foreach (var procurementProduct in procurementProducts)
            {
                if (procurementProduct.Id == 0)
                {
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            //product = inquiryProduct;
                            //product.product_Id = inquiryProduct.product.Id;
                            //product.UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                            //offerItems.Add(procurementProduct);
                            bankTransItems.Add(new ProcurementProduct()
                            {
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,
                                    //product = new Product()
                                    //{
                                    //    Id = procurementProduct.inquiryProduct.product.Id,
                                    //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    //    item = procurementProduct.inquiryProduct.product.item,
                                    //    code = procurementProduct.inquiryProduct.product.code,
                                    //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    //    //nature = procurementProduct.inquiryProduct.product.nature,
                                    //    //category = procurementProduct.inquiryProduct.product.category,
                                    //    isActive = procurementProduct.inquiryProduct.product.isActive
                                    //}
                                    //,
                                    product_Id = procurementProduct.inquiryProduct.product.Id

                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,

                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                //caption1 = cmbcaption1.Text.Trim(),
                                //caption2 = cmbcaption2.Text.Trim()



                            });
                        }
                    }
                    else
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)

                            bankTransItems.Add(new ProcurementProduct()
                            {
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,
                                    //product = new Product()
                                    //{
                                    //    Id = procurementProduct.inquiryProduct.product.Id,
                                    //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    //    item = procurementProduct.inquiryProduct.product.item,
                                    //    code = procurementProduct.inquiryProduct.product.code,
                                    //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    //    //nature = procurementProduct.inquiryProduct.product.nature,
                                    //    //category = procurementProduct.inquiryProduct.product.category,
                                    //    isActive = procurementProduct.inquiryProduct.product.isActive
                                    //}
                                    // ,
                                    product_Id = procurementProduct.inquiryProduct.product.Id

                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,

                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                //caption1 = cmbcaption1.Text.Trim(),
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                    }
                    //product = procurementProduct;
                    //product.product_Id = procurementProduct.inquiryProduct.product.Id;
                    //product.inquiryProduct.UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                    //offerItems.Add(product);
                }
                else
                {
                    // if user is adding completely new prodcut first time.
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            //product = inquiryProduct;
                            //product.product_Id = inquiryProduct.product.Id;
                            //product.UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                            bankTransItems.Add(new ProcurementProduct()
                            {
                                Id = procurementProduct.Id,
                                inquiryProduct = new InquiryProduct()
                                {
                                    //Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,
                                    product = new Product()
                                    {
                                        Id = procurementProduct.inquiryProduct.product.Id,
                                        categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                        item = procurementProduct.inquiryProduct.product.item,
                                        code = procurementProduct.inquiryProduct.product.code,
                                        itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                        ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                        nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                        unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                        //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                        //nature = procurementProduct.inquiryProduct.product.nature,
                                        //category = procurementProduct.inquiryProduct.product.category,
                                        isActive = procurementProduct.inquiryProduct.product.isActive
                                    }
                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                //caption1 = cmbcaption1.Text.Trim(),
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                        }
                    }
                    else
                    {
                        // if user reloaded he offer and its product item is already there.
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {

                            bankTransItems.Add(new ProcurementProduct()
                            {
                                Id= procurementProduct.Id,
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,
                                    //product = new Product()
                                    //{
                                    //    Id = procurementProduct.inquiryProduct.product.Id,
                                    //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    //    item = procurementProduct.inquiryProduct.product.item,
                                    //    code = procurementProduct.inquiryProduct.product.code,
                                    //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    //    //nature = procurementProduct.inquiryProduct.product.nature,
                                    //    //category = procurementProduct.inquiryProduct.product.category,
                                    //    isActive = procurementProduct.inquiryProduct.product.isActive
                                    //}
                                    // ,
                                    product_Id = procurementProduct.inquiryProduct.product.Id

                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                //caption1 = cmbcaption1.Text.Trim(),
                                //caption2 = cmbcaption2.Text.Trim()
                                


                            });
                            //offerItems.Add(new ProcurementProduct()
                            //{
                          
                                 
                            //    Id = procurementProduct.Id,

                            //    //inquiryProduct = new InquiryProduct()
                            //    //{
                            //    //    Id = procurementProduct.inquiryProduct.Id,
                            //    //    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                            //    //    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                            //    //    quantity = procurementProduct.inquiryProduct.quantity,

                            //    //    product= procurementProduct.inquiryProduct.product,
                            //    //    //product = new Product()
                            //    //    //{

                            //    //    //    Id = procurementProduct.inquiryProduct.product.Id,
                            //    //    //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                            //    //    //    item = procurementProduct.inquiryProduct.product.item,
                            //    //    //    code = procurementProduct.inquiryProduct.product.code,
                            //    //    //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                            //    //    //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                            //    //    //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                            //    //    //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                            //    //    //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                            //    //    //    //nature = procurementProduct.inquiryProduct.product.nature,
                            //    //    //    //category = procurementProduct.inquiryProduct.product.category,
                            //    //    //    isActive = procurementProduct.inquiryProduct.product.isActive
                            //    //    //},
                            //    //    product_Id = procurementProduct.inquiryProduct.product.Id


                            //    //},
                            //    product_Id = procurementProduct.inquiryProduct.Id,
                            //    value1 = procurementProduct.value1,
                            //    value2 = procurementProduct.value2,
                            //    caption1 = cmbcaption1.Text.Trim(),
                            //    caption2 = cmbcaption2.Text.Trim()


                            //});
                        }
                    }
                }
            }


            return bankTransItems;
        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            //Product pro = (Product)lookupProductsinGrid.GetItemFromValue(grdPOItems.GetCellValue(e.RowHandle, "inquiryProduct.product"));
            (grdCntrlPOItems.CurrentItem as ProcurementProduct).inquiryProduct = new InquiryProduct();
            //(grdPOItems.CurrentItem as ProcurementProduct).inquiryProduct.product =  as Product;
            //grdPOItems.SetCellValue(e.RowHandle, "inquiryProduct.product.itemDescription", pro.itemDescription);

        }

        private void TxtAmountOCfrom_KeyUp(object sender, KeyEventArgs e)
        {
            if (cmbxTransferType.SelectedIndex == 1)
            {
                var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
                var amountMER = Convert.ToDouble(txtMER.Text);
                var amountER = Convert.ToDouble(txtER.Text);
                txtAmountMer.Text = (amountFrom * amountMER).ToString();

                txtAmountOCto.Text = (amountFrom * amountER).ToString();
            }
        }

        private void TxtMERfrom_KeyUp(object sender, KeyEventArgs e)
        {
            var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
            if (amountFrom > 0)
            {
                var MERfrom = Convert.ToDouble(txtMERfrom.Text);
                var total = MERfrom * amountFrom;
                txtAmountMERfrom.Text = total.ToString();
                txtAmountMERto.Text = total.ToString();

                var amountMERto = Convert.ToDouble(txtAmountMERto.Text);
                var amountTo = Convert.ToDouble(txtAmountOCto.Text);

                txtMERto.Text = (amountMERto / amountTo).ToString();
            }
        }

        private void TxtAmountMERfrom_KeyUp(object sender, KeyEventArgs e)
        {
            var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
            if (amountFrom > 0)
            {
                var amountMERfrom = Convert.ToDouble(txtAmountMERfrom.Text);
                txtMERfrom.Text = (amountMERfrom / amountFrom).ToString();
                txtAmountMERto.Text = txtAmountMERfrom.Text;

                var amountMERto = Convert.ToDouble(txtAmountMERto.Text);
                var amountTo = Convert.ToDouble(txtAmountOCto.Text);

                txtMERto.Text = (amountMERto / amountTo).ToString();
            }
        }

        private void TxtAmountMERfrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
            if (amountFrom > 0)
            {
                var amountMERfrom = Convert.ToDouble(txtAmountMERfrom.Text);
                txtMERfrom.Text = (amountMERfrom / amountFrom).ToString();
                txtAmountMERto.Text = txtAmountMERfrom.Text;

                var amountMERto = Convert.ToDouble(txtAmountMERto.Text);
                var amountTo = Convert.ToDouble(txtAmountOCto.Text);

                txtMERto.Text = (amountMERto / amountTo).ToString();
            }
        }

        private void TxtAmountOCto_KeyUp(object sender, KeyEventArgs e)
        {
            var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
            if (amountFrom > 0)
            {
                var amountTo = Convert.ToDouble(txtAmountOCto.Text);
                txtER.Text = (amountTo / amountFrom).ToString();
            }
        }

        private void TxtMERto_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void TxtAmountMERto_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void TxtER_KeyUp(object sender, KeyEventArgs e)
        {
            var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
            if (amountFrom > 0)
            {
                var amountER = Convert.ToDouble(txtER.Text);
                txtAmountOCto.Text = (amountFrom * amountER).ToString();
            }
        }

        private void BtnPaymentTo_Checked(object sender, RoutedEventArgs e)
        {
            btnDepositTo.IsChecked = false;
        }

        private void BtnDepositTo_Checked(object sender, RoutedEventArgs e)
        {
            btnPaymentTo.IsChecked = false;
        }

        private void BtnDepositFrom_Checked(object sender, RoutedEventArgs e)
        {
            btnPaymentFrom.IsChecked = false;
        }

        private void BtnPaymentFrom_Checked(object sender, RoutedEventArgs e)
        {
            btnDepositFrom.IsChecked = false;
        }

        private void ChkAmountOCfrom_Checked(object sender, RoutedEventArgs e)
        {
            chkAmountMERfrom.IsChecked = false;
        }

        private void ChkAmountMERfrom_Checked(object sender, RoutedEventArgs e)
        {
            chkAmountOCfrom.IsChecked = false;
        }

        private void ChkAmountOCto_Checked(object sender, RoutedEventArgs e)
        {
            chkAmountMERto.IsChecked = false;
        }

        private void ChkAmountMERto_Checked(object sender, RoutedEventArgs e)
        {
            chkAmountOCto.IsChecked = false;
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                if (bankTransfer.Id != 0)
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveIBTAttachmentCategories();
                    grdAttach1.Visibility = Visibility.Visible;
                }
               
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {
                if (bankTransfer.Id != 0)
                {
                    List<TreeItem> atachments = SYSTEM_STATIC.GetIBCTAttachmentsListByCategory(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer);
                    treeViewAttachments1.ItemsSource = atachments;
                }
                grdAttachments1.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (bankTransfer.Id != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\InterBank_Transfer\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += bankTransfer.Id + "_" + TransactionItemType.InterCompanyBank_Transfer.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {

                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.InterBank_Transfer);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, bankTransfer.Id, 18, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                });
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;
                            }


                            //MessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {



                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void BtnGJournal_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View General Journal") != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(getJournalTransactions());
                generalJournal.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not Allowed to View General Journal.");
            }
        }

        private void btnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (bankTransferId != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(bankTransferId, TransactionItemType.InterCompanyBank_Transfer);
                trackingWindow.ShowDialog();
            }
        }


        private void BtnAddBankChargesFrom_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    if (bankTransfer != null)
                    {
                        //var receipt = grdCntrlSalesReceipt.SelectedItem as SaleReceipts;
                        winICBTbankCharges winIBTbankCharges = new winICBTbankCharges(bankTransferId);
                        winIBTbankCharges.BankChargesFrom = true;
                        winIBTbankCharges.ShowDialog();
                        bankTransfer.bankChargesFrom = winIBTbankCharges.finalBankCharges;
                        txtBankChargesFrom.Text = winIBTbankCharges.totalBankCharges.ToString();

                        bankTransfer.InterCompBankTransferVATFrom = winIBTbankCharges.finalVATs;
                        txtVATFrom.Text = winIBTbankCharges.totalVAT.ToString();
                        //receipt.VAT = receipt.dedVAT + receipt.bankVAT;
                        //receipt.TotalDeductionSOC = receipt.Deductions + receipt.BankCharges + receipt.VAT;
                        bankTransfer.IsAdjustedVATfrom = winIBTbankCharges.IsAdjustedFrom;
                        //grdCntrlSalesReceipt.SetFocusedRowCellValue("dedVAT", ucSalesReceiptDeduction.totalVAT);

                    }
                    //else
                    //    DXMessageBox.Show("Please Select Receipt to Add Deduction.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Please Save this Inter-Bank Transfer first!");
                return;
            }
        }

        private void BtnAddBankChargesTo_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    if (bankTransfer != null)
                    {
                        //var receipt = grdCntrlSalesReceipt.SelectedItem as SaleReceipts;
                        winICBTbankCharges winIBTbankCharges = new winICBTbankCharges(bankTransferId);
                        winIBTbankCharges.BankChargesFrom = false;
                        winIBTbankCharges.ShowDialog();
                        bankTransfer.bankChargesTo = winIBTbankCharges.finalBankCharges;
                        txtBankChargesTo.Text = winIBTbankCharges.totalBankCharges.ToString();

                        bankTransfer.InterCompBankTransferVATto = winIBTbankCharges.finalVATs;
                        txtVATTo.Text = winIBTbankCharges.totalVAT.ToString();
                        //receipt.VAT = receipt.dedVAT + receipt.bankVAT;
                        //receipt.TotalDeductionSOC = receipt.Deductions + receipt.BankCharges + receipt.VAT;
                        bankTransfer.IsAdjustedVATto = winIBTbankCharges.IsAdjustedTo;
                        //grdCntrlSalesReceipt.SetFocusedRowCellValue("dedVAT", ucSalesReceiptDeduction.totalVAT);

                    }
                    //else
                    //    DXMessageBox.Show("Please Select Receipt to Add Deduction.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Please Save this Inter-Bank Transfer first!");
                return;
            }
        }
        private void loadVATBookReferenceNo()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            if (lookupCompanyFrom.SelectedItem != null)
            {
                ERP_BL.VATBook.VATBookRepo vatBookRepo = new ERP_BL.VATBook.VATBookRepo();
                var references = vatBookRepo.GetAllActiveVATBookReferenceNo((lookupCompanyFrom.SelectedItem as Company).Id);
                cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
                foreach (ERP_BL.VATBook.VATBookRefNumber _ref in references)
                {
                    cmbitems.Add(new cmbitem() { name = _ref.VATBookReferenceNo, id = _ref.Id });
                }
            }
            if (lookupCompanyTo.SelectedItem != null)
            {
                ERP_BL.VATBook.VATBookRepo vatBookRepo = new ERP_BL.VATBook.VATBookRepo();
                var references = vatBookRepo.GetAllActiveVATBookReferenceNo((lookupCompanyTo.SelectedItem as Company).Id);
                cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
                foreach (ERP_BL.VATBook.VATBookRefNumber _ref in references)
                {
                    cmbitems.Add(new cmbitem() { name = _ref.VATBookReferenceNo, id = _ref.Id });
                }
            }


            cmbxVATBookRef.ItemsSource = cmbitems;

        }

    }
}
