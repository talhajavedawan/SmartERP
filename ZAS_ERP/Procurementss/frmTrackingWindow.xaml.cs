using ERP_BL.Enums;
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
using ERP_BL.Databases;
using ERP_BL;
using DevExpress.Xpf.Core;
using System.Diagnostics;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ERP_BL.Payments;
using ERP_BL.Procurements.AdminBills;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ERP_BL.Procurements;
using ZAS_ERP.Bankings;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.Procurementss.SaleOrderss;
using DevExpress.Data;
using DevExpress.Xpf.Grid;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.Bankings.STL;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ZAS_ERP.Payments.UserControls.CompanyLoanPayments;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.AdminBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoan;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ERP_BL.AssetsRentals;
using ZAS_ERP.AssetRentalss.UserControls;

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for frmTrackingWindow.xaml
    /// </summary>
    public partial class frmTrackingWindow : DXWindow
    {
        public int Id = 0;
        public int GroupId;
        public ERP_BL.Enums.TransactionItemType Type = TransactionItemType.Inquiry;
        ProcurementRepo repo = new ProcurementRepo();
        public bool isParent = false;
        decimal sum = 0;
        decimal sumChild = 0;
        public List<CostSheetBillField> costSheetBillFieldsParent = new List<CostSheetBillField>();
        public List<CostSheetPOField> costSheetPOFieldsParent = new List<CostSheetPOField>();
        public List<CostSheetSIField> costSheetSIFieldsParent = new List<CostSheetSIField>();
        public List<CostSheetSOField> costSheetSOFieldsParent = new List<CostSheetSOField>();
        public List<CostSheetSaleReceiptField> costSheetSaleReceiptFieldsParent = new List<CostSheetSaleReceiptField>();
        public List<CostSheetPaymentField> costSheetPaymentFieldsParent = new List<CostSheetPaymentField>();
        public List<CostSheetBillField> costSheetBillFieldsChild = new List<CostSheetBillField>();
        public List<CostSheetPOField> costSheetPOFieldsChild = new List<CostSheetPOField>();
        public List<CostSheetSIField> costSheetSIFieldsChild = new List<CostSheetSIField>();
        public List<CostSheetSOField> costSheetSOFieldsChild = new List<CostSheetSOField>();
        public List<CostSheetSaleReceiptField> costSheetSaleReceiptFieldsChild = new List<CostSheetSaleReceiptField>();
        public List<CostSheetPaymentField> costSheetPaymentFieldsChild = new List<CostSheetPaymentField>();
        public List<CostFieldValues> costParentFieldValue = new List<CostFieldValues>();
        public List<CostFieldValues> costChildFieldValue = new List<CostFieldValues>();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        public frmTrackingWindow(int id, TransactionItemType type)
        {
            InitializeComponent();
            Id = id;
            Type = type;
        }

        public frmTrackingWindow(int id,int groupId ,TransactionItemType type)
        {
            InitializeComponent();
            Id = id;
            GroupId = groupId;
            Type = type;
        }
        public frmTrackingWindow(int id, TransactionItemType type, bool _isParent)
        {
            InitializeComponent();
            Id = id;
            Type = type;
            isParent = _isParent;
        }

        private void GrdTransactionListing_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<AllTransactionsView> allTransactions = new List<AllTransactionsView>();
            List<AllTransactionsView> allSOTransactions = new List<AllTransactionsView>();
            List<SaleOrder> linkedSaleOrders = new List<SaleOrder>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Transactions in Tracking 360 Window") != null)
            {
                if (isParent != true)
                {
                    tabTransactions.Visibility = Visibility.Visible;

                    if (Type == TransactionItemType.Payments)
                    {
                        allTransactions = repo.getAllTransactionsNew(Id, Type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.employeeId);
                    }
                    else if(Type == TransactionItemType.TargetReward  || Type==TransactionItemType.STL)
                    {
                        allTransactions = repo.getAllTransactions(Id, Type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.id);
                    }
                    else
                    {
                        allTransactions = repo.getAllTransactions(Id, Type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.id);
                 
                        linkedSaleOrders = repo.GetLInkedSaleOrders(Id, Type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.id);

                        List<int> deptIds=  new List<int>();
                        var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
                        deptIds = userDepartments.Select(x => x.Id).ToList();

                        List<int> companytIds = new List<int>();
                        var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();
                        companytIds = userCompanies.Select(x => x.Id).ToList();

                        if (linkedSaleOrders[0] != null)
                        {

                            linkedSaleOrders = linkedSaleOrders.Where(x =>x.company_Id!=null &&  companytIds.Contains((int)x.company_Id)).ToList();

                            linkedSaleOrders = linkedSaleOrders.Where(x => deptIds.Contains(x.dept_Id)).ToList();
                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View CostSheet in Tracking 360 Window") != null)
                        {
                            try
                            {
                                foreach (var SO in linkedSaleOrders)
                                {
                                    if (SO != null && SO.CostSheet_Id != null)
                                    {
                                        if (SO != null)
                                        {
                                            if (SO.ParentSO_Id == null)
                                            {
                                                if (SO.Id != 0)
                                                {
                                                    lblParentSO.Text = lblParentSO.Text + " / " + SO.company.CompanyName;
                                                    txtCompany.Text = SO.company.CompanyName;
                                                    txtCustomer.Text = SO.customerCompany.company.CompanyName;
                                                    txtDepartment.Text = SO.department.DeptName;
                                                    txtPrincipal.Text = SO.principal.company.CompanyName;
                                                    txtSaleOrderDate.Text = SO.saleOrderDate.ToString();
                                                    txtSORef.Text = SO.referenceNo;
                                                    txtFinanceRef.Text = SO.FinanceRefrenceNo;
                                                    txtSalesRef.Text = SO.SalesReferenceNo;
                                                    txtCreationDate.Text = SO.CreationDate.ToString();
                                                    if(SO.saleOrdertype==InquiryType.SupplyCCC)
                                                    {
                                                        txtTotalAmount.Text = SO.costCenterAmount.ToString();
                                                        txtCurrency.Text = SO.costcenterCurrency.CurrencyName;

                                                    }
                                                    else
                                                    {
                                                        txtCurrency.Text = SO.currency.CurrencyName;

                                                        txtTotalAmount.Text = SO.totalCFRValue.ToString();

                                                    }
                                                    foreach (var item in saleOrderRepo.getActiveCostSheetFields())
                                                    {
                                                        costParentFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title });
                                                    }
                                                    grdParentCostItems.ItemsSource = costParentFieldValue;
                                                    var costfieldValues = GetCostSheetParentValues(SO);
                                                    var costfields = costfieldValues.Where(x => x.actualValue != 0 || x.SRBC != 0 || x.budgetedValue != 0 || x.revisedValue != 0 /*&& x.systemPayment != 0*/ && x.systemValue <= 0 && x.vendor == null).ToList();
                                                    grdParentCostItems.ItemsSource = costfields;
                                                }
                                            }
                                            else
                                            {
                                                if (SO.Id != 0)
                                                {
                                                    lblChildSO.Text = lblChildSO.Text + " / " + SO.company.CompanyName;
                                                    txtCompanyChild.Text = SO.company.CompanyName;
                                                    txtCustomerChild.Text = SO.customerCompany.company.CompanyName;
                                                    txtDepartmentChild.Text = SO.department.DeptName;
                                                    txtPrincipalChild.Text = SO.principal.company.CompanyName;
                                                    txtSaleOrderDateChild.Text = SO.saleOrderDate.ToString();
                                                    txtSORefChild.Text = SO.referenceNo;
                                                    txtFinanceRefChild.Text = SO.FinanceRefrenceNo;
                                                    txtSalesRefChild.Text = SO.SalesReferenceNo;
                                                    txtCreationDateChild.Text = SO.CreationDate.ToString();
                                                    if (SO.saleOrdertype == InquiryType.SupplyCCC)
                                                    {
                                                        txtCurrencyChild.Text = SO.costcenterCurrency.CurrencyName;
                                                        txtTotalAmountChild.Text = SO.costCenterAmount.ToString();
                                                    }
                                                    else
                                                    {
                                                        txtTotalAmountChild.Text = SO.totalCFRValue.ToString();
                                                        txtCurrencyChild.Text = SO.currency.CurrencyName;

                                                    }

                                                    foreach (var item in saleOrderRepo.getActiveCostSheetFields())
                                                    {
                                                        costChildFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title });
                                                    }
                                                    grdChildCostItems.ItemsSource = costChildFieldValue;
                                                    var costfieldValues = GetCostSheetChildValues(SO);
                                                    var costfields = costfieldValues.Where(x => x.actualValue != 0 || x.SRBC != 0 || x.budgetedValue != 0 || x.revisedValue != 0 /*&& x.systemPayment != 0*/ && x.systemValue <= 0 && x.vendor == null).ToList();
                                                    grdChildCostItems.ItemsSource = costfields;
                                                }
                                            }
                                            if (linkedSaleOrders.Count != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
                                            {

                                                foreach (var order in linkedSaleOrders)
                                                {
                                                    if (order.Id != 0)
                                                    {
                                                        allSOTransactions.Add(new AllTransactionsView()
                                                        {
                                                            Id = order.Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString(),
                                                            TransactionType = TransactionItemType.Sale_Order.ToString(),
                                                            Company = order.company.CompanyName,
                                                            Employee = order.employee.person.FName + " " + order.employee.person.FName,
                                                            CreationDate = order.CreationDate,
                                                            Currency = order.customerCompany.company.currency.CurrencyName,
                                                            Department = (order.department.parentDepartment == null) ? order.department.DeptName : order.department.parentDepartment.DeptName + " " + order.department.DeptName,
                                                            Customer = order.customerCompany.company.CompanyName,
                                                            SalesReferenceNo = order.SalesReferenceNo,
                                                            BackColor = order.saleOrderStatus.backcolor,
                                                            Status = order.saleOrderStatus.Status,
                                                            totalCFRValue = order.totalCFRValue,
                                                            amountSOC = order.totalCFRValue,
                                                            amountME = order.totalBaseCFRValue,
                                                            Parent_Id = order.ParentSO_Id.ToString()
                                                        });
                                                    }
                                                }
                                                grdTransactionSO.ItemsSource = allSOTransactions;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                        }
                       
                    }
                    grdTransactionListing.ItemsSource = allTransactions;
                }
                else
                {

                    allTransactions = repo.getLinkedSO(Id, Type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.employeeId);
                    grdTransactionListing.ItemsSource = allTransactions;

                }
            }
            else
            {
                tabTransactions.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files in Tracking 360 Window") != null)
            {
                tabAttachments.Visibility = Visibility.Visible;
                List<Attachment> attachments = new List<Attachment>();
                AttachmentsRepo attachmentsRepo = new AttachmentsRepo();

                if(Type == TransactionItemType.Payments)
                {
                    foreach (var item in allTransactions)
                    {
                        TransactionItemType type = (TransactionItemType)Enum.Parse(typeof(TransactionItemType), item.TransactionType, true);
                        List<Attachment> attaches = new List<Attachment>();
                        if(type == TransactionItemType.Admin_Bill)
                            attaches = attachmentsRepo.getAttachmentOrderDsc(Convert.ToInt32(item.Id.Split('_').Last()), type);
                        //else if(type == TransactionItemType.Bill)
                        //    attaches = attachmentsRepo.getAttachmentOrderDsc(Convert.ToInt32(item.Id.Split('_').First()), type);
                        else /*if (type == TransactionItemType.Purchase_Invoice)*/
                            attaches = attachmentsRepo.getAttachmentOrderDsc(Convert.ToInt32(item.Id.Split('_').First()), type);

                        foreach (var attach in attaches)
                            attachments.Add(attach);
                    }
                }
                else
                {
                    foreach (var item in allTransactions)
                    {
                        TransactionItemType type = (TransactionItemType)Enum.Parse(typeof(TransactionItemType), item.TransactionType, true);
                        var attaches = attachmentsRepo.getAttachmentOrderDsc(Convert.ToInt32(item.Id.Split('_').First()), type);
                        foreach (var attach in attaches)
                            attachments.Add(attach);
                    }
                }
               
                grdTransactionAttachments.ItemsSource = attachments;
            }
            else
            {
                tabAttachments.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View CostSheet in Tracking 360 Window") != null)
            {
                tabCostSheet.Visibility = Visibility.Visible;
                grdCostSheet.ItemsSource = repo.getSoFromTransaction(Id, Type);
            }
            else
            {
                tabCostSheet.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Bill Tracking in 360 tracking Window") != null)
            {
                tabBillMappings.Visibility = Visibility.Visible;
                LoadBillTracking(allTransactions);
            }
            else
            {
                tabBillMappings.Visibility = Visibility.Collapsed;
            }
            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "View Sale Recipt Tracking in 360 tracking Window") != null)
            {
                tabSaleReciptListings.Visibility = Visibility.Visible;
                LoadSegregatePOTracking(allTransactions);
                LoadSaleReciptTracking(allTransactions);
            }
            //else
            //{
            //    tabSaleReciptListings.Visibility = Visibility.Collapsed;
            //}

        }
        public List<CostFieldValues> GetCostSheetParentValues(SaleOrder saleOrder)
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costSheetBillFieldsParent = new List<CostSheetBillField>();
            costSheetPOFieldsParent = new List<CostSheetPOField>();
            costSheetSIFieldsParent = new List<CostSheetSIField>();
            costSheetSOFieldsParent = new List<CostSheetSOField>();
            costSheetSaleReceiptFieldsParent = new List<CostSheetSaleReceiptField>();
            costSheetPaymentFieldsParent = new List<CostSheetPaymentField>();
            costfieldValues = grdParentCostItems.ItemsSource as List<CostFieldValues>;
            if (saleOrder.Id != 0 && saleOrder.CostSheet_Id != 0 )
            {
                costSheetSOFieldsParent = saleOrderRepo.GetCostSheetSOFields(saleOrder.Id, (int)saleOrder.CostSheet_Id);
            }

            foreach (var item in costfieldValues)
            {

                foreach (var field in saleOrder.CostSheet.FieldValues)
                    if (item.Id == field.FieldId)
                    {
                        if (field.Type == 2)
                            item.actualValue = field.Value;

                        else if (field.Type == 0)
                        {
                            item.revisedValue = field.Value;
                            if (item.revisedValue == 0)
                            {
                                item.revisedValue = item.budgetedValue;
                            }
                        }
                        else if (field.Type == 1)
                        {
                            item.budgetedValue = field.Value;
                            if (item.revisedValue == 0 || saleOrder.isApproved != true)
                            {
                                item.revisedValue = field.Value;
                            }
                        }
                        else if (field.Type == 3)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                var value = saleOrderRepo.GetSystemCost((int)saleOrder.CostSheet_Id, item.Id);
                                if (item.systemValue == 0 || saleOrder.isApproved != true)
                                {
                                    if (item.systemValue == 0)
                                    {
                                        item.systemValue = value + field.adjSCost;
                                        item.adjSCost = field.adjSCost;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 4)
                        {
                            VendorRepo vendorRepo = new VendorRepo();
                            var vendor = vendorRepo.Get((int)field.Value);
                            item.vendor = vendor;
                        }
                        else if (field.Type == 10)
                        {
                            if (costSheetSOFieldsParent.Count != 0)
                            {
                                var dbField = costSheetSOFieldsParent.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.soAmountSRBC && x.CostSheetId == saleOrder.CostSheet_Id && x.SO_Id == saleOrder.Id);
                                if (dbField != null)
                                {
                                    var soFieldAmount = dbField.Value;
                                    if (soFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.soAmountSRBC = soFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 11)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                var value = saleOrderRepo.GetSBRC((int)saleOrder.CostSheet_Id, item.Id);
                                if (item.SRBC == 0 || saleOrder.isApproved != true)
                                {
                                    item.SRBC = value;
                                }
                            }
                        }
                        else if (field.Type == 12)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.Maker = field.stringValue;

                            }
                        }
                        else if (field.Type == 13)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.Origin = field.stringValue;
                            }
                        }
                        else if (field.Type == 14)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.Packing = field.isPacking;
                            }
                        }
                        else if (field.Type == 15)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.deliveryDate = field.dateValue;
                            }
                        }
                        else if (field.Type == 16)
                        {
                            PaymentTermRepo paymentTermRepo = new PaymentTermRepo();
                            var paymentTerm = paymentTermRepo.get((int)field.Value);
                            item.PaymentTerm = paymentTerm;
                        }
                        else if (field.Type == 17)
                        {
                            IncotermRepo incotermRepo = new IncotermRepo();
                            var incoterm = incotermRepo.get((int)field.Value);
                            IncoTermName incoTermName = new IncoTermName()
                            {
                                Id = incoterm.Id,
                                termName = incoterm.term,
                                discription = incoterm.discription,
                                isActive = incoterm.isActive
                            };

                            item.IncotermName = incoTermName;
                        }
                        else if (field.Type == 18)
                        {
                            ProcurementRepo repo = new ProcurementRepo();
                            var warranty = repo.GetWarranty((int)field.Value);

                            item.Warranty = warranty;
                        }
                        else if (field.Type == 19)
                        {
                            item.Dg_Goods = field.isdgGood;
                        }
                        else if (field.Type == 20)
                        {
                            CurrencyRepo currencyRepo = new CurrencyRepo();
                            var currency = currencyRepo.get((int)field.Value);
                            item.OC = currency;
                        }
                        else
                        if (field.Type == 21)
                            item.OCamount = field.Value;
                        else if (field.Type == 22)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.LoadingPort = field.stringValue;
                            }

                        }
                        else if (field.Type == 23)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.DestinationPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 24)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var document = repo.GetPQ((int)field.Value);
                                item.PQ = document;
                            }
                        }
                        else if (field.Type == 25)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var term = repo.GetST((int)field.Value);
                                item.ST = term;
                            }
                        }
                        else if (field.Type == 26)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.HScode = field.stringValue;
                            }
                        }
                        else if (field.Type == 27)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.DrawaingRequired = field.drawingRequired;
                            }
                        }
                        else if (field.Type == 28)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.AttestedCOO = field.stringValue;
                            }
                        }
                        else if (field.Type == 29)
                        {
                            item.exchangeRate = field.Value;
                            if (item.exchangeRate == 0 || saleOrder.isApproved != true)
                            {
                                item.exchangeRate = field.Value;
                            }
                        }
                        else if (field.Type == 30)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.isExportLicense = field.isExportLicense;
                            }
                        }
                        //else if (field.Type == 31)
                        //{
                        //    item.billAmount = field.Value;
                        //    if (costSheetSaleReceiptFields.Count != 0)
                        //    {
                        //        var dbField = costSheetSaleReceiptFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.Receipt_Cost && x.CostSheetId == costSheet.Id && x.Receipt_Id == salesReceipt.Id);
                        //        if (dbField != null)
                        //        {
                        //            var ReceiptFieldAmount = dbField.Value;
                        //            if (ReceiptFieldAmount != 0 || saleOrder.isApproved != true)
                        //            {
                        //                item.receiptAmount = ReceiptFieldAmount;
                        //            }
                        //        }
                        //    }
                        //}
                        //else if (field.Type == 32)
                        //{
                        //    //if (field.Value != 0)
                        //    //{
                        //    //    //item.addedSystemValue = field.Value;
                        //    //    item.paymentAmount = field.Value;
                        //    //}
                        //    if (costSheetPaymentFields.Count != 0)
                        //    {
                        //        var dbField = costSheetPaymentFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.System_Payment && x.CostSheetId == costSheet.Id && x.Payment_Id == payment.Id);
                        //        if (dbField != null)
                        //        {
                        //            var paymentFieldAmount = dbField.Value;
                        //            if (paymentFieldAmount != 0 || saleOrder.isApproved != true)
                        //            {
                        //                item.paymentAmount = paymentFieldAmount;
                        //                item.addedSystemValue = paymentFieldAmount;
                        //            }
                        //        }
                        //    }
                        //}
                        else if (field.Type == 33)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.IntermediaryPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 34)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.deliveryDays = field.stringValue;
                            }
                        }

                        //else if (field.Type == 35)
                        //{
                        //    if (costSheet != null && costSheet.Id != 0)
                        //    {
                        //        if(item.adjSCost==0)
                        //        item.adjSCost = field.adjSCost;
                        //    }
                        //}

                        //else if (field.Type == 36)
                        //{
                        //    //item.billAmount = field.Value;
                        //    if (costSheetSIFields.Count != 0)
                        //    {
                        //        var dbField = costSheetSIFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.SI_Cost && x.CostSheetId == costSheet.Id && x.SI_Id == saleInvoice.Id);
                        //        if (dbField != null)
                        //        {
                        //            var SIFieldAmount = dbField.Value;
                        //            if (SIFieldAmount != 0 || saleOrder.isApproved != true)
                        //            {
                        //                item.poAmount = SIFieldAmount;
                        //            }
                        //        }
                        //    }
                        //}

                    }
                costfields.Add(item);

            }
            return costfields;
        }
        public List<CostFieldValues> GetCostSheetChildValues(SaleOrder saleOrder)
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costSheetBillFieldsParent = new List<CostSheetBillField>();
            costSheetPOFieldsParent = new List<CostSheetPOField>();
            costSheetSIFieldsParent = new List<CostSheetSIField>();
            costSheetSOFieldsParent = new List<CostSheetSOField>();
            costSheetSaleReceiptFieldsParent = new List<CostSheetSaleReceiptField>();
            costSheetPaymentFieldsParent = new List<CostSheetPaymentField>();
            costfieldValues = grdChildCostItems.ItemsSource as List<CostFieldValues>;
            if (saleOrder.Id != 0 && saleOrder.CostSheet_Id != 0)
            {
                costSheetSOFieldsParent = saleOrderRepo.GetCostSheetSOFields(saleOrder.Id, (int)saleOrder.CostSheet_Id);
            }

            foreach (var item in costfieldValues)
            {

                foreach (var field in saleOrder.CostSheet.FieldValues)
                    if (item.Id == field.FieldId)
                    {
                        if (field.Type == 2)
                            item.actualValue = field.Value;

                        else if (field.Type == 0)
                        {
                            item.revisedValue = field.Value;
                            if (item.revisedValue == 0)
                            {
                                item.revisedValue = item.budgetedValue;
                            }
                        }
                        else if (field.Type == 1)
                        {
                            item.budgetedValue = field.Value;
                            if (item.revisedValue == 0 || saleOrder.isApproved != true)
                            {
                                item.revisedValue = field.Value;
                            }
                        }
                        else if (field.Type == 3)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                var value = saleOrderRepo.GetSystemCost((int)saleOrder.CostSheet_Id, item.Id);
                                if (item.systemValue == 0 || saleOrder.isApproved != true)
                                {
                                    if (item.systemValue == 0)
                                    {
                                        item.systemValue = value + field.adjSCost;
                                        item.adjSCost = field.adjSCost;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 4)
                        {
                            VendorRepo vendorRepo = new VendorRepo();
                            var vendor = vendorRepo.Get((int)field.Value);
                            item.vendor = vendor;
                        }
                        else if (field.Type == 10)
                        {
                            if (costSheetSOFieldsParent.Count != 0)
                            {
                                var dbField = costSheetSOFieldsParent.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.soAmountSRBC && x.CostSheetId == saleOrder.CostSheet_Id && x.SO_Id == saleOrder.Id);
                                if (dbField != null)
                                {
                                    var soFieldAmount = dbField.Value;
                                    if (soFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.soAmountSRBC = soFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 11)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                var value = saleOrderRepo.GetSBRC((int)saleOrder.CostSheet_Id, item.Id);
                                if (item.SRBC == 0 || saleOrder.isApproved != true)
                                {
                                    item.SRBC = value;
                                }
                            }
                        }
                        else if (field.Type == 12)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.Maker = field.stringValue;

                            }
                        }
                        else if (field.Type == 13)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.Origin = field.stringValue;
                            }
                        }
                        else if (field.Type == 14)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.Packing = field.isPacking;
                            }
                        }
                        else if (field.Type == 15)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.deliveryDate = field.dateValue;
                            }
                        }
                        else if (field.Type == 16)
                        {
                            PaymentTermRepo paymentTermRepo = new PaymentTermRepo();
                            var paymentTerm = paymentTermRepo.get((int)field.Value);
                            item.PaymentTerm = paymentTerm;
                        }
                        else if (field.Type == 17)
                        {
                            IncotermRepo incotermRepo = new IncotermRepo();
                            var incoterm = incotermRepo.get((int)field.Value);
                            IncoTermName incoTermName = new IncoTermName()
                            {
                                Id = incoterm.Id,
                                termName = incoterm.term,
                                discription = incoterm.discription,
                                isActive = incoterm.isActive
                            };

                            item.IncotermName = incoTermName;
                        }
                        else if (field.Type == 18)
                        {
                            ProcurementRepo repo = new ProcurementRepo();
                            var warranty = repo.GetWarranty((int)field.Value);

                            item.Warranty = warranty;
                        }
                        else if (field.Type == 19)
                        {
                            item.Dg_Goods = field.isdgGood;
                        }
                        else if (field.Type == 20)
                        {
                            CurrencyRepo currencyRepo = new CurrencyRepo();
                            var currency = currencyRepo.get((int)field.Value);
                            item.OC = currency;
                        }
                        else
                        if (field.Type == 21)
                            item.OCamount = field.Value;
                        else if (field.Type == 22)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.LoadingPort = field.stringValue;
                            }

                        }
                        else if (field.Type == 23)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.DestinationPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 24)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var document = repo.GetPQ((int)field.Value);
                                item.PQ = document;
                            }
                        }
                        else if (field.Type == 25)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var term = repo.GetST((int)field.Value);
                                item.ST = term;
                            }
                        }
                        else if (field.Type == 26)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.HScode = field.stringValue;
                            }
                        }
                        else if (field.Type == 27)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.DrawaingRequired = field.drawingRequired;
                            }
                        }
                        else if (field.Type == 28)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.AttestedCOO = field.stringValue;
                            }
                        }
                        else if (field.Type == 29)
                        {
                            item.exchangeRate = field.Value;
                            if (item.exchangeRate == 0 || saleOrder.isApproved != true)
                            {
                                item.exchangeRate = field.Value;
                            }
                        }
                        else if (field.Type == 30)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.isExportLicense = field.isExportLicense;
                            }
                        }
                        else if (field.Type == 33)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.IntermediaryPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 34)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.deliveryDays = field.stringValue;
                            }
                        }

           

                    }
                costfields.Add(item);

            }
            return costfields;
        }
        private void LoadBillTracking(List<AllTransactionsView> allTransactions)
        {
            try
            {
                var BillTrackingList = allTransactions.Distinct().Where(x => x.TransactionType == TransactionItemType.Purchase_Order.ToString() || x.TransactionType == TransactionItemType.Sale_Order.ToString() || x.TransactionType == TransactionItemType.Bill.ToString() || x.TransactionType == TransactionItemType.Purchase_Invoice.ToString()).ToList();
                var group = allTransactions.GroupBy(x => x.Id);
                grdBillListing.ItemsSource = BillTrackingList;
            }
            catch (Exception ex)
            {

            }
            
        }
        private void LoadSaleReciptTracking(List<AllTransactionsView> allTransactions)
        {
            var SaleReciptTrackingList = allTransactions.Where(x => x.TransactionType == TransactionItemType.Sale_Invoice.ToString() || x.TransactionType == TransactionItemType.Sale_Order.ToString() || x.TransactionType == TransactionItemType.Sale_Receipt.ToString());
            grdSaleReciptListing.ItemsSource = SaleReciptTrackingList;
        }
        private void LoadSegregatePOTracking(List<AllTransactionsView> allTransactions)
        {
            var POTrackingList = allTransactions.Where(x => x.TransactionType == TransactionItemType.Purchase_Order.ToString() || x.TransactionType == TransactionItemType.Purchase_Invoice.ToString() || x.TransactionType == TransactionItemType.Payments.ToString());
            grdPOListing.ItemsSource = POTrackingList;
        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void CardView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdTransactionAttachments.SelectedItem != null)
            {
                try
                {


                    var attach = (Attachment)grdTransactionAttachments.SelectedItem;
                    //TransactionItemType type = (TransactionItemType)Enum.Parse(typeof(TransactionItemType), attach.transactionType, true);

                    string str = attach.fileServerAdress;
                    if (!string.IsNullOrEmpty(str))
                    {
                        System.Threading.Thread thread = new System.Threading.Thread(() =>
                        {
                            //Button thisButton = (Button)sender;

                            ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                            var result = attachment.startDownload(str, attach.transactionType);
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
        private void GrdSaleReceiptListLoad(int id)
        {
            try
            {
                var repo = new SalesReceiptRepo();
                var saleReceipt = repo.GetSalesReceipt(id);

                if (saleReceipt.receiptType == ReceiptType.Customer_Credits)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Customer Credit Receipts") != null)
                    {
                        ucFrmCustomerCreditReceipt frmLAreceipt = new ucFrmCustomerCreditReceipt();
                        //paymentRepo = new PaymentRepo();
                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                        Window frmPiPaymentWindow = new Window();
                        if (saleReceipt.saleReceiptStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                            {
                                frmLAreceipt.editFlag = true;
                                frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                frmLAreceipt.receiptId = saleReceipt.Id;
                                frmPiPaymentWindow.Content = frmLAreceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Sale Receipts";
                                frmPiPaymentWindow.Show();
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                return;
                            }
                        }
                        else
                        {
                            frmLAreceipt.editFlag = true;
                            frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                            frmLAreceipt.receiptId = saleReceipt.Id;
                            frmPiPaymentWindow.Content = frmLAreceipt;
                            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                            frmPiPaymentWindow.Title = "Sale Receipts";
                            frmPiPaymentWindow.Show();
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to View Customer Credit Receipts!");
                    }
                    
                }
                else if (saleReceipt.receiptType == ReceiptType.Loans_Advances)
                {
                    switch (saleReceipt.loansAdvance.advanceTemplate)
                    {
                        case LoansAdvanceTemplate.Loan:
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                            {
                                ucFrmCompanyLoanSaleReceipt frmLAreceipt = new ucFrmCompanyLoanSaleReceipt();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                                Window frmPiPaymentWindow = new Window();
                                if (saleReceipt.saleReceiptStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                    {
                                        frmLAreceipt.editFlag = true;
                                        frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                        frmLAreceipt.receiptId = saleReceipt.Id;
                                        frmPiPaymentWindow.Content = frmLAreceipt;
                                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPiPaymentWindow.Title = "Sale Receipts";
                                        frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                    frmLAreceipt.receiptId = saleReceipt.Id;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Sale Receipts";
                                    frmPiPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                            }
                            break;
                        default:
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                            {
                                ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                                Window frmPiPaymentWindow = new Window();
                                if (saleReceipt.saleReceiptStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                    {
                                        frmLAreceipt.editFlag = true;
                                        frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                        frmLAreceipt.receiptId = saleReceipt.Id;
                                        frmPiPaymentWindow.Content = frmLAreceipt;
                                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPiPaymentWindow.Title = "Sale Receipts";
                                        frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                    frmLAreceipt.receiptId = saleReceipt.Id;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Sale Receipts";
                                    frmPiPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                            }
                            break;
                    }

                    
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                    {
                        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                        updateSaleReceiptObj.saveEditFlag = 1;


                        if (saleReceipt == null)
                        {
                            return;
                        }

                        if (saleReceipt.saleReceiptStatus != null)
                        {
                            var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                            updateSaleReceiptObj.selectedStatus = status;
                        }

                        updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
                        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                        updateSaleReceiptObj.receiptId = saleReceipt.Id;
                        updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;


                        //updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                        ////ucFrmAddAccount obj = new ucFrmAddAccount();
                        //updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                        //updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                        //updateSaleReceiptObj.enter_receipt_win.Show();

                        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanMinimize;
                        updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                        updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                        updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                        updateSaleReceiptObj.enterReceiptWindowFlag = true;

                        if (saleReceipt.CreditedDate != null)
                            updateSaleReceiptObj.dateEditcreditedDate.EditValue = (DateTime)saleReceipt.CreditedDate;

                        if (saleReceipt.DepositedDate != null)
                            updateSaleReceiptObj.datDepositedDate.EditValue = (DateTime)saleReceipt.DepositedDate;

                        if (saleReceipt.InstrumentDate != null)
                            updateSaleReceiptObj.datInstrumentDate.EditValue = (DateTime)saleReceipt.InstrumentDate;

                        if (saleReceipt.InstrumentNo != null)
                            updateSaleReceiptObj.txtInstrumentNo.Text = saleReceipt.InstrumentNo;

                        updateSaleReceiptObj.enter_receipt_win.ShowDialog();
                        //Load_Receipts();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                        return;
                    }
                }

               

            }
            catch
            {

            }

            //MessageBox.Show("Mission Successful!");
        }
        private void GrdTransactionListing_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = (DevExpress.Xpf.Grid.GridControl)sender;
            if (grid.SelectedItem != null)
            {

                var item = (AllTransactionsView)grid.SelectedItem;

                if (item.TransactionType == TransactionItemType.AssetRental.ToString())
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Assets") != null)
                    {
                        AssetRentalRepo rentalRepo = new AssetRentalRepo();

                        var selectedAsset = rentalRepo.GetAssetRental(Convert.ToInt32(item.Id.Split('_').First()));
                        ucFrmAddAssetRental frmAddAssetRental = new ucFrmAddAssetRental();
                        frmAddAssetRental.editFlag = true;
                        frmAddAssetRental.assetId = selectedAsset.Id;

                        Window win = new Window();
                        win.Content = frmAddAssetRental;
                        win.WindowState = WindowState.Maximized;
                        win.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View STL");
                    }
                }
                else
                if (item.TransactionType == TransactionItemType.STL.ToString())
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL") != null)
                    {
                        STLRepo sTLRepo = new STLRepo();

                        var selectedStl = sTLRepo.Get(Convert.ToInt32(item.Id.Split('_').First()));
                        winSTLAdd stl = new winSTLAdd(true, Convert.ToInt32(selectedStl.paymentGroupId));
                        stl.stl = selectedStl;
                        stl.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View STL");
                    }
                }
                else
                    
                if (item.TransactionType == TransactionItemType.InterBank_Transfer.ToString())
                {
                    try
                    {
                        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                        {

                            var selectedBankTransfer = bankTransRepo.GetInterBankTransfer((Convert.ToInt32(item.Id.Split('_').First())));

                            if (selectedBankTransfer != null)
                            {
                                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                                CompanyRepo compRepo = new CompanyRepo();
                                bankTransRepo = new InterBankTransRepo();

                                //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                                ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedBankTransfer.interBankTransStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                    {
                                        ucFrmBankTransfer.editFlag = true;

                                        ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                        ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                                        ucFrmBankTransfer.frmBankTranfer.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBankTransfer.editFlag = true;

                                    ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                    ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                                    ucFrmBankTransfer.frmBankTranfer.Show();
                                }


                            }
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    if (item.TransactionType == TransactionItemType.Sale_Receipt.ToString())
                    {
                        GrdSaleReceiptListLoad(Convert.ToInt32(item.Id.Split('_').First()));
                        return;
                    }

                    if(item.TransactionType == TransactionItemType.Tasks.ToString())
                    {
                        ucTaskAdd taskAdd = new ucTaskAdd();
                        taskAdd.taskId = Convert.ToInt32(item.Id.Split('_').First());
                        taskAdd.editFlag = true;
                        Window win = new Window();
                        win.Content = taskAdd;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }

                    if (item.TransactionType == TransactionItemType.LoansAdvances.ToString())
                    {
                        AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
                        LoansAdvance loansAdvance = new LoansAdvance();
                        loansAdvance = loansAdvanceRepo.GetLoansAdvance(Convert.ToInt32(item.Id.Split('_').First()));

                        switch (loansAdvance.advanceTemplate)
                        {
                            case LoansAdvanceTemplate.Advance:

                                switch (loansAdvance.loansAdvanceType)
                                {
                                    case LoansAdvanceType.Admin_Bill:
                                        ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                                        frmLoansAdvances.loansAdvanceId = Convert.ToInt32(item.Id.Split('_').First());
                                        frmLoansAdvances.editFlag = true;
                                        Window win = new Window();
                                        win.Content = frmLoansAdvances;
                                        win.WindowState = WindowState.Maximized;
                                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                        win.Show();
                                        return;
                                        break;
                                    case LoansAdvanceType.Vendor_Bill:
                                        ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();
                                        frmBillLoansAdvance.loansAdvanceId = Convert.ToInt32(item.Id.Split('_').First());
                                        frmBillLoansAdvance.editFlag = true;
                                        Window wind = new Window();
                                        wind.Content = frmBillLoansAdvance;
                                        wind.WindowState = WindowState.Maximized;
                                        wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                        wind.Show();
                                        return;
                                        break;
                                }
                                break;

                            case LoansAdvanceTemplate.Loan:
                                switch (loansAdvance.loansAdvanceType)
                                {
                                    case LoansAdvanceType.Admin_Bill:
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                        {
                                            ucFrmAdminBillLoan frmCompanyLoan = new ucFrmAdminBillLoan();
                                            frmCompanyLoan.loansAdvanceId = loansAdvance.Id;
                                            frmCompanyLoan.editFlag = true;
                                            Window wind = new Window();
                                            wind.Content = frmCompanyLoan;
                                            wind.WindowState = WindowState.Maximized;
                                            wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                            wind.Show();
                                            return;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Permission denied!");
                                        }
                                        break;
                                    case LoansAdvanceType.Vendor_Bill:
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                        {
                                            ucFrmVendorBillLoan frmCompanyLoan = new ucFrmVendorBillLoan();
                                            frmCompanyLoan.loansAdvanceId = loansAdvance.Id;
                                            frmCompanyLoan.editFlag = true;
                                            Window wind = new Window();
                                            wind.Content = frmCompanyLoan;
                                            wind.WindowState = WindowState.Maximized;
                                            wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                            wind.Show();
                                            return;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Permission denied!");
                                        }
                                        break;
                                }
                                break;
                        }
                    
                    }

                    if (item.TransactionType == TransactionItemType.TargetReward.ToString())
                    {
                        ucFrmBasicTargetRewards frmTargetRewards = new ucFrmBasicTargetRewards();
                        frmTargetRewards.rewardId = Convert.ToInt32(item.Id.Split('_').First());
                        //frmTargetRewards.editFlag = true;
                        Window win = new Window();
                        win.Content = frmTargetRewards;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }


                    if (item.TransactionType == TransactionItemType.Admin_Bill.ToString())
                    {
                        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                        AdminBillsRepo billsRepo = new AdminBillsRepo();
                        DXWindow frmBill = new DXWindow();

                        var bill = billsRepo.GetBill(Convert.ToInt32(item.Id.Split('_').First()));

                        frmBillAdd = new ucFrmBillAdd();

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBillAdd.isProgressiveCost = bill.isProgressiveCost;
                                    frmBillAdd.billTypes = bill.billTypes;
                                    frmBill.Content = frmBillAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmBill.WindowState = WindowState.Maximized;
                                    
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmBill.Title = "Admin Bill";
                                    frmBill.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                            else
                            {
                                frmBillAdd.editFlag = true;
                                frmBillAdd.groupId = bill.transactionGroupId; 
                                frmBillAdd.isProgressiveCost = bill.isProgressiveCost;
                                frmBillAdd.billTypes = bill.billTypes;
                                frmBill.Content = frmBillAdd;
                                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                frmBill.WindowState = WindowState.Maximized;
                                //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                frmBill.Title = "Admin Bill";
                                frmBill.Show();
                            }
                        }
                        return;
                    }
                    if (item.TransactionType == TransactionItemType.Payments.ToString())
                    {
                        ucFrmPayments frmPayments = new ucFrmPayments();
                        ucFrmBillPaymentAdd ucFrmBillPayment = new ucFrmBillPaymentAdd();
                        ucFrmPInvoicePaymentAdd frmPInvoicePaymentAdd = new ucFrmPInvoicePaymentAdd();
                        ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                        ucFrmTargetRewardPayment frmTRpayment = new ucFrmTargetRewardPayment();
                        CompanyRepo compRepo = new CompanyRepo();
                        PaymentRepo paymentRepo = new PaymentRepo();


                        var payment = paymentRepo.GetPayment(Convert.ToInt32(item.Id.Split('_').First()));
                        //var payments = paymentRepo.GetPaymentsByGroupId(payment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;



                        switch (payment.transactionType)
                        {
                            case PaymentTransactionType.Loans_Advances:

                                switch (payment.loansAdvance.advanceTemplate)
                                {
                                    case LoansAdvanceTemplate.Loan:
                                        ucFrmCompanyLoanPayment frmCompanyLoanPayment = new ucFrmCompanyLoanPayment();
                                        if (payment.Status.isActive == false)
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                            {
                                                frmCompanyLoanPayment.editFlag = true;
                                                frmCompanyLoanPayment.groupId = payment.transactionGroupId;
                                                frmCompanyLoanPayment.frmPiPaymentWindow.Content = frmCompanyLoanPayment;
                                                frmCompanyLoanPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                frmCompanyLoanPayment.frmPiPaymentWindow.Title = "Payments";
                                                frmCompanyLoanPayment.frmPiPaymentWindow.Show();
                                            }
                                            else
                                            {
                                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            frmCompanyLoanPayment.editFlag = true;
                                            frmCompanyLoanPayment.groupId = payment.transactionGroupId;
                                            frmCompanyLoanPayment.frmPiPaymentWindow.Content = frmCompanyLoanPayment;
                                            frmCompanyLoanPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmCompanyLoanPayment.frmPiPaymentWindow.Title = "Payments";
                                            frmCompanyLoanPayment.frmPiPaymentWindow.Show();
                                        }
                                        break;

                                    default:
                                        if (payment.Status.isActive == false)
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                            {
                                                frmLApayment.editFlag = true;
                                                frmLApayment.groupId = payment.transactionGroupId;
                                                frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                                frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                                frmLApayment.frmPiPaymentWindow.Show();
                                            }
                                            else
                                            {
                                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            frmLApayment.editFlag = true;
                                            frmLApayment.groupId = payment.transactionGroupId;
                                            frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                            frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                            frmLApayment.frmPiPaymentWindow.Show();
                                        }
                                        break;
                                }
                                
                                break;
                            case PaymentTransactionType.Target_Reward:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRpayment.editFlag = true;
                                        frmTRpayment.groupId = payment.transactionGroupId;
                                        frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                        frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRpayment.editFlag = true;
                                    frmTRpayment.groupId = payment.transactionGroupId;
                                    frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                    frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRpayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Admin_Bills:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = payment.transactionGroupId;
                                        frmPayments.frmPaymentWindow.Content = frmPayments;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPayments.frmPaymentWindow.Title = "Payments";
                                        frmPayments.frmPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPayments.editFlag = true;
                                    frmPayments.groupId = payment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Vendor_Bills:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        ucFrmBillPayment.editFlag = true;
                                        ucFrmBillPayment.groupId = payment.transactionGroupId;
                                        ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                        ucFrmBillPayment.frmBillPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBillPayment.editFlag = true;
                                    ucFrmBillPayment.groupId = payment.transactionGroupId;
                                    ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                    ucFrmBillPayment.frmBillPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Purchase_Invoice:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPInvoicePaymentAdd.editFlag = true;
                                        frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPInvoicePaymentAdd.editFlag = true;
                                    frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                }
                                break;
                        }



                        return;
                    }
                    

                    Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel((TransactionItemType)Enum.Parse(typeof(TransactionItemType), item.TransactionType), Convert.ToInt32(item.Id.Split('_').First()));
                    procurmentPanele.Show();
                }
                //if (item.TransactionType == TransactionItemType.Inquiry.ToString())
                //{

                //    Procurementss.frmProcurmentPanel.childid = 0;
                //    Procurementss.Inquiriess.ucInquiryAdd.editinquiry = 1;
                //    Procurementss.frmProcurmentPanel.inquiryid = item.Id;

                //    Procurementss.Inquiriess.ucInquiryAdd.inquiryid = Procurementss.frmProcurmentPanel.inquiryid;
                //    Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel();
                //    procurmentPanele.Show();
                //}
                //else if (item.TransactionType == TransactionItemType.Offer.ToString())
                //{
                //    Procurementss.frmProcurmentPanel.childid = 1;

                //    Procurementss.Offerss.ucOfferAdd.editoffer = 1;
                //    Procurementss.frmProcurmentPanel.offerid = item.Id;

                //    Procurementss.Offerss.ucOfferAdd.offerid = Procurementss.frmProcurmentPanel.offerid;
                //    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel();
                //    procurmentPanel.Show();


                //}
                //else if (item.TransactionType == TransactionItemType.Sale_Order.ToString())
                //{
                //    Procurementss.frmProcurmentPanel.childid = 2;
                //    Procurementss.SaleOrderss.ucSaleOrderAdd.editsaleOrder = 1;
                //    Procurementss.frmProcurmentPanel.saleOrderid = item.Id;

                //    Procurementss.SaleOrderss.ucSaleOrderAdd.saleOrderid = Procurementss.frmProcurmentPanel.saleOrderid;
                //    Procurementss.frmProcurmentPanel procurmentPane = new Procurementss.frmProcurmentPanel();
                //    procurmentPane.Show();
                //}
                ////else if (item.TransactionType == TransactionItemType.Inquiry.ToString())
                ////{
                ////    Procurementss.frmProcurmentPanel.childid = 3;
                ////    Procurementss.MemorandumSaless.ucMemorandumSaleAdd.editmemorandumSale = 1;
                ////    Procurementss.frmProcurmentPanel.memorandumSaleid = item.Id;

                ////    Procurementss.MemorandumSaless.ucMemorandumSaleAdd.memorandumSaleid = Procurementss.frmProcurmentPanel.memorandumSaleid;
                ////    Procurementss.frmProcurmentPanel procurmentPan = new Procurementss.frmProcurmentPanel();
                ////    procurmentPan.Show();
                ////}
                //else if (item.TransactionType == TransactionItemType.Sale_Invoice.ToString())
                //{
                //    Procurementss.frmProcurmentPanel.childid = 4;
                //    Procurementss.SaleInvoicess.ucSaleInvoiceAdd.editsaleInvoice = 1;
                //    Procurementss.frmProcurmentPanel.saleInvoiceid = item.Id;
                //    Procurementss.frmProcurmentPanel.saleOrderid = 0;

                //    Procurementss.SaleInvoicess.ucSaleInvoiceAdd.saleInvoiceid = Procurementss.frmProcurmentPanel.saleInvoiceid;
                //    Procurementss.frmProcurmentPanel procurmentPane = new Procurementss.frmProcurmentPanel();
                //    procurmentPane.Show();
                //}

            }
        }

        private void ViewCostSheet_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdCostSheet.SelectedItem != null)
            {
                SaleOrderRepo orderRepo = new SaleOrderRepo();
                var saleOrder = orderRepo.get((grdCostSheet.SelectedItem as SaleOrder).Id);
                if (saleOrder != null && saleOrder.CostSheet != null)
                {
                    List<ViewInfo> views = new List<ViewInfo>();

                    SaleOrderss.frmCostSheet frmCostSheet = new SaleOrderss.frmCostSheet(saleOrder,saleOrder.FinanceRefrenceNo,saleOrder.SalesReferenceNo ,saleOrder.department.DeptName,saleOrder.customerCompany.company.CompanyName ,saleOrder.currency.CurrencyName,saleOrder.totalCFRValue.ToString() ,saleOrder.incoterm.term,saleOrder.CreationDate.ToString(),saleOrder.paymentTerm.term,saleOrder.maker,saleOrder.origin , views, (InquiryType.Supply), saleOrder.principal.company.CompanyName, saleOrder.packing, /*saleOrder.deliveryDate != null ? */System.DateTime.Now /*: (DateTime)saleOrder.deliveryDate*/, (saleOrder.Warranty == null) ? "" : saleOrder.Warranty.name, saleOrder.referenceNo ,saleOrder.saleOrderDate.ToString(), saleOrder.isApproved, saleOrder.isReApproved);
                    frmCostSheet.ShowDialog();

                }

            }

        }

        private void grdTransactionSO_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = (DevExpress.Xpf.Grid.GridControl)sender;
            if (grid.SelectedItem != null)
            {

                var item = (AllTransactionsView)grid.SelectedItem;
                if (item.TransactionType == TransactionItemType.InterBank_Transfer.ToString())
                {
                    try
                    {
                        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                        {

                            var selectedBankTransfer = bankTransRepo.GetInterBankTransfer((Convert.ToInt32(item.Id.Split('_').First())));

                            if (selectedBankTransfer != null)
                            {
                                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                                CompanyRepo compRepo = new CompanyRepo();
                                bankTransRepo = new InterBankTransRepo();

                                //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                                ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedBankTransfer.interBankTransStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                    {
                                        ucFrmBankTransfer.editFlag = true;

                                        ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                        ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                                        ucFrmBankTransfer.frmBankTranfer.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBankTransfer.editFlag = true;

                                    ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                    ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                                    ucFrmBankTransfer.frmBankTranfer.Show();
                                }


                            }
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    if (item.TransactionType == TransactionItemType.Sale_Receipt.ToString())
                    {
                        GrdSaleReceiptListLoad(Convert.ToInt32(item.Id.Split('_').First()));
                        return;
                    }

                    if (item.TransactionType == TransactionItemType.LoansAdvances.ToString())
                    {
                        ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                        frmLoansAdvances.loansAdvanceId = Convert.ToInt32(item.Id.Split('_').First());
                        frmLoansAdvances.editFlag = true;
                        Window win = new Window();
                        win.Content = frmLoansAdvances;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }


                    if (item.TransactionType == TransactionItemType.Admin_Bill.ToString())
                    {
                        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                        AdminBillsRepo billsRepo = new AdminBillsRepo();
                        DXWindow frmBill = new DXWindow();

                        var bill = billsRepo.GetBill(Convert.ToInt32(item.Id.Split('_').First()));

                        frmBillAdd = new ucFrmBillAdd();

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBill.Content = frmBillAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmBill.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmBill.Title = "Admin Bill";
                                    frmBill.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                            else
                            {
                                frmBillAdd.editFlag = true;
                                frmBillAdd.groupId = bill.transactionGroupId;
                                frmBill.Content = frmBillAdd;
                                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                frmBill.WindowState = WindowState.Maximized;
                                //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                frmBill.Title = "Admin Bill";
                                frmBill.Show();
                            }
                        }
                        return;
                    }
                    if (item.TransactionType == TransactionItemType.Payments.ToString())
                    {
                        ucFrmPayments frmPayments = new ucFrmPayments();
                        ucFrmBillPaymentAdd ucFrmBillPayment = new ucFrmBillPaymentAdd();
                        ucFrmPInvoicePaymentAdd frmPInvoicePaymentAdd = new ucFrmPInvoicePaymentAdd();
                        ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                        CompanyRepo compRepo = new CompanyRepo();
                        PaymentRepo paymentRepo = new PaymentRepo();


                        var payment = paymentRepo.GetPayment(Convert.ToInt32(item.Id.Split('_').First()));
                        //var payments = paymentRepo.GetPaymentsByGroupId(payment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;



                        switch (payment.transactionType)
                        {
                            case PaymentTransactionType.Loans_Advances:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmLApayment.editFlag = true;
                                        frmLApayment.groupId = payment.transactionGroupId;
                                        frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                        frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                        frmLApayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLApayment.editFlag = true;
                                    frmLApayment.groupId = payment.transactionGroupId;
                                    frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                    frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                    frmLApayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Admin_Bills:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = payment.transactionGroupId;
                                        frmPayments.frmPaymentWindow.Content = frmPayments;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPayments.frmPaymentWindow.Title = "Payments";
                                        frmPayments.frmPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPayments.editFlag = true;
                                    frmPayments.groupId = payment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Vendor_Bills:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        ucFrmBillPayment.editFlag = true;
                                        ucFrmBillPayment.groupId = payment.transactionGroupId;
                                        ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                        ucFrmBillPayment.frmBillPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBillPayment.editFlag = true;
                                    ucFrmBillPayment.groupId = payment.transactionGroupId;
                                    ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                    ucFrmBillPayment.frmBillPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Purchase_Invoice:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPInvoicePaymentAdd.editFlag = true;
                                        frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPInvoicePaymentAdd.editFlag = true;
                                    frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                }
                                break;
                        }



                        return;
                    }


                    Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel((TransactionItemType)Enum.Parse(typeof(TransactionItemType), item.TransactionType), Convert.ToInt32(item.Id.Split('_').First()));
                    procurmentPanele.Show();
                }
          

            }
        }

        private void grdTransactionSO_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTotalAmountChild.Text))
                {
                    var totalSO = Convert.ToDecimal(txtTotalAmountChild.Text);

                    if (e.Column.FieldName == "budgetedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("budgetedValue") != null)
                        {
                            decimal budgetValue = Convert.ToDecimal(e.GetListSourceFieldValue("budgetedValue"));
                            var percent = (((/*totalSO - */budgetValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";
                        }
                    }
                    else
                if (e.Column.FieldName == "revisedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("revisedValue") != null)
                        {
                            decimal revisedValue = Convert.ToDecimal(e.GetListSourceFieldValue("revisedValue"));


                            var percent = (((/*totalSO -*/revisedValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }
                    else
                if (e.Column.FieldName == "SRBCValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("SRBC") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("SRBC"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }
                    else
                if (e.Column.FieldName == "systemValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("systemValue") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("systemValue"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void grdParentCostItems_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                sum = sum + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {
                                    sum = Convert.ToDecimal(txtTotalAmount.Text) - sum;
                                }
                                e.TotalValue = sum;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "revisedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "revisedValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("revisedValue"));
                                sum = sum + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    sum = Convert.ToDecimal(txtTotalAmount.Text) - sum;
                                }
                                e.TotalValue = sum;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBC")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBC")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));
                                var budgetFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                if (fieldValue != 0)
                                {
                                    sum = sum + fieldValue;
                                }
                                else
                                {
                                    sum = sum + budgetFieldValue;
                                }
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {
                                    sum = Convert.ToDecimal(txtTotalAmount.Text) - sum;
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = sum;



                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "actualValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "actualValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValue"));
                                sum = sum + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {
                                    sum = Convert.ToDecimal(txtTotalAmount.Text) - sum;
                                }
                                e.TotalValue = sum;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));
                                var adjSCOst = Convert.ToDecimal(e.GetValue("adjSCost"));
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {
                                    sum = Convert.ToDecimal(txtTotalAmount.Text) - sum;
                                }
                                e.TotalValue = sum;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmount.Text) - sum) / Convert.ToDecimal(txtTotalAmount.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBCValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBCValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));

                                var revisedFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                if (fieldValue != 0)
                                {
                                    sum = sum + fieldValue;
                                }
                                else
                                if (revisedFieldValue != 0)
                                {
                                    sum = sum + revisedFieldValue;
                                }
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmount.Text) - sum) / Convert.ToDecimal(txtTotalAmount.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;



                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));

                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmount.Text) - sum) / Convert.ToDecimal(txtTotalAmount.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void grdParentCostItems_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTotalAmount.Text))
                {
                    var totalSO = Convert.ToDecimal(txtTotalAmount.Text);

                    if (e.Column.FieldName == "budgetedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("budgetedValue") != null)
                        {
                            decimal budgetValue = Convert.ToDecimal(e.GetListSourceFieldValue("budgetedValue"));
                            var percent = (((/*totalSO - */budgetValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);

                        }
                    }
                    else
                if (e.Column.FieldName == "revisedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("revisedValue") != null)
                        {
                            decimal revisedValue = Convert.ToDecimal(e.GetListSourceFieldValue("revisedValue"));


                            var percent = (((/*totalSO -*/revisedValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);


                        }
                    }
                    else
                if (e.Column.FieldName == "SRBCValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("SRBC") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("SRBC"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);


                        }
                    }
                    else
                if (e.Column.FieldName == "systemValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("systemValue") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("systemValue"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);


                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grdChildCostItems_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                sumChild = sumChild + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                }
                                e.TotalValue = sumChild;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "revisedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "revisedValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("revisedValue"));
                                sumChild = sumChild + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {

                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                }
                                e.TotalValue = sumChild;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBC")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBC")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));
                                var budgetFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                if (fieldValue != 0)
                                {
                                    sumChild = sumChild + fieldValue;
                                }
                                else
                                {
                                    sumChild = sumChild + budgetFieldValue;
                                }
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                    e.TotalValue = sumChild;
                                }
                                else
                                    e.TotalValue = sumChild;



                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "actualValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "actualValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValue"));
                                sumChild = sumChild + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                }
                                e.TotalValue = sumChild;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));
                                var adjSCOst = Convert.ToDecimal(e.GetValue("adjSCost"));
                                sumChild = sumChild + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                }
                                e.TotalValue = sumChild;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValuePercChild")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValuePercChild")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValueChild"));
                                sumChild = sumChild + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild) / Convert.ToDecimal(txtTotalAmountChild.Text)) * 100);
                                    sumChild = decimal.Round(percent, 2);
                                    e.TotalValue = sumChild;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBCValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBCValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));

                                var revisedFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                if (fieldValue != 0)
                                {
                                    sumChild = sumChild + fieldValue;
                                }
                                else
                                if (revisedFieldValue != 0)
                                {
                                    sumChild = sumChild + revisedFieldValue;
                                }
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild) / Convert.ToDecimal(txtTotalAmountChild.Text)) * 100);
                                    sumChild = decimal.Round(percent, 2);
                                    e.TotalValue = sumChild;
                                }
                                else
                                    e.TotalValue = 100;



                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));

                                sumChild = sumChild + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild) / Convert.ToDecimal(txtTotalAmountChild.Text)) * 100);
                                    sumChild = decimal.Round(percent, 2);
                                    e.TotalValue = sumChild;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValueChild"));
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmount.Text) - sum) / Convert.ToDecimal(txtTotalAmount.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
                
            }
            catch (Exception ex)
            {

            }
        }

        private void grdChildCostItems_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTotalAmountChild.Text))
                {
                    var totalSO = Convert.ToDecimal(txtTotalAmountChild.Text);

                    if (e.Column.FieldName == "budgetedValuePercChild" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("budgetedValue") != null)
                        {
                            decimal budgetValue = Convert.ToDecimal(e.GetListSourceFieldValue("budgetedValue"));
                            var percent = (((/*totalSO - */budgetValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";
                        }
                    }
                    else
                if (e.Column.FieldName == "revisedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("revisedValue") != null)
                        {
                            decimal revisedValue = Convert.ToDecimal(e.GetListSourceFieldValue("revisedValue"));


                            var percent = (((/*totalSO -*/revisedValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }
                    else
                if (e.Column.FieldName == "SRBCValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("SRBC") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("SRBC"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }
                    else
                if (e.Column.FieldName == "systemValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("systemValue") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("systemValue"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grdPOListing_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
