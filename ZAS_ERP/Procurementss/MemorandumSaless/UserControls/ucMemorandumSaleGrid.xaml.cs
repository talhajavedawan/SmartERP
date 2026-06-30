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
using ERP_BL.Databases;
using ERP_BL;
using DevExpress.Xpf.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraExport.Helpers;
using DevExpress.Xpf.Grid.Printing;
using DevExpress.Xpf.Reports.UserDesigner;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using System.Collections.ObjectModel;
using ERP_BL.Enums;

namespace ZAS_ERP.Procurementss.MemorandumSaless
{
    /// <summary>
    /// Interaction logic for ucMemorandumSaleGrid.xaml
    /// </summary>
    public partial class ucMemorandumSaleGrid : UserControl
    {
        public static int statusId;
        public static int AllActive;
        public int VoidCount { get; set; }
        public ucMemorandumSaleGrid()
        {

            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) MemorandumSale List") != null)
                {
                    ApprovalCount = memorandumSalerepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add MemorandumSale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                    {
                        ApprovalCount = memorandumSalerepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = memorandumSalerepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);

                    }

                }
            }

            else
            {
                ApprovalCount = memorandumSalerepo.getAllPendingForAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) MemorandumSale List") != null)
                {
                    ClosingCount = memorandumSalerepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                    {
                        ClosingCount = memorandumSalerepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = memorandumSalerepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                    }

                }
            }

            else
            {
                ClosingCount = memorandumSalerepo.getAllPendingForClosingAdministratorCount();
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void MemorandumSales") != null)
            {
                mbtnVoid.Visibility = Visibility.Visible;

                if (MainWindow.currentUserid != 0)
                {
                    VoidCount = memorandumSalerepo.getVoidRegisterCount(MainWindow.currentUserid);

                }
                else
                {
                    VoidCount = memorandumSalerepo.getVoidRegisterAdministratorCount();

                }


            }
            else
            {
                mbtnVoid.Visibility = Visibility.Collapsed;
            }
            //this.DataContext = this;
        }

        MemorandumSaleRepo memorandumSalerepo = new MemorandumSaleRepo();
        MemorandumSale memorandumSale = new MemorandumSale();
        public IList<ERP_BL.Databases.MemorandumSale> memorandumSales { get; set; }
        //public ObservableCollection<MemorandumSale> orders { get; set; }
        public int ApprovalCount { get; set; }
        public int ClosingCount { get; set; }
        /// <summary>
        /// runs when window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucmemorandumSaleGrid_Loaded(object sender, RoutedEventArgs e)
        {

            //SystemLogic.SetUserSettingOfCurrentWindow(grdmemorandumSale);
            loadMemorandumSalegrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

        }

        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            //if (e.IsGetData)
            {
                if (e.Column.FieldName == "PaymentDueAgeingDays" && e.IsGetData)

                {
                    if (e.GetListSourceFieldValue("PaymentDueAgeing") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("PaymentDueAgeing"));

                        //DateTime date;
                        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                        e.Value = NoDueAgeingDays;
                    }
                }

                if (e.Column.FieldName == "Department")

                {

                    // string s = "Test: FieldTwo";

                }
                if (e.Column.FieldName == "CreationAgeing")

                {
                    if (e.GetListSourceFieldValue("CreationDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("CreationDate"));

                        Double CreateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = CreateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "SODateAgeing")

                {
                    if (e.GetListSourceFieldValue("memorandumSaleDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("memorandumSaleDate"));

                        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                        e.Value = SoDateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "SODeliveryAgeing")

                {
                    if (e.GetListSourceFieldValue("deliveryDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("deliveryDate"));

                        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                        e.Value = SoDateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "totalWeight")

                {
                    if (e.GetListSourceFieldValue("products") != null)
                    {
                        List<ProcurementProduct> products = (e.GetListSourceFieldValue("products")) as List<ProcurementProduct>;

                        decimal? totalweight = 0;
                        foreach (var pro in products)
                        {
                            if (pro.inquiryProduct.Weight != null || pro.inquiryProduct.Weight != 0)
                            {
                                totalweight = pro.inquiryProduct.Weight;
                            }
                        }
                        //CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = totalweight;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "totalQuantity")

                {
                    if (e.GetListSourceFieldValue("products") != null)
                    {
                        List<ProcurementProduct> products = (e.GetListSourceFieldValue("products")) as List<ProcurementProduct>;

                        double totalquantity = 0;
                        foreach (var pro in products)
                        {
                            if (pro.inquiryProduct.quantity != 0)
                            {
                                totalquantity = pro.inquiryProduct.quantity;
                            }
                        }
                        //CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = totalquantity;
                        // string s = "Test: FieldTwo";
                    }
                }
                //CreationDate


                //int unitsOnOrder = Convert.ToInt32(e.GetListSourceFieldValue("UnitsOnOrder"));
                //e.Value = price * unitsOnOrder;
            }
        }
        /// <summary>
        /// Loads data into memorandumSale Grid and create columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadMemorandumSalegrid()

        {
            //this.grdmemorandumSale.ItemsSource = new List<MemorandumSale>();
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            //grdmemorandumSale.Columns.Clear();
            //orders = new ObservableCollection<MemorandumSale>();
            //grdmemorandumSale.ItemsSource = null;
            //grdmemorandumSale.ItemsSource = orders;
            //grdmemorandumSale.VisibleItems.Clear();

            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        memorandumSales = memorandumSalerepo.getAll();
                        //this.grdmemorandumSale.ItemsSource = memorandumSales;
                        //MessageBox.Show("You are not Authorized");

                        //return;
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Memorandum Sales") != null)
                    {
                        memorandumSales = memorandumSalerepo.getAll(MainWindow.currentUserid);
                    }
                    else
                    {
                        memorandumSales = memorandumSalerepo.getAllActive(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    memorandumSales = memorandumSalerepo.getAllActive(MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    memorandumSales = memorandumSalerepo.getAllInActive(MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending For Approvals (Memorandum Sales)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) MemorandumSale List") != null)
                        {
                            memorandumSales = memorandumSalerepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);


                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                            {
                                memorandumSales = memorandumSalerepo.getAllPendingForApproval(MainWindow.currentUserid);
                            }

                            else
                            {
                                memorandumSales = memorandumSalerepo.getAllPendingForApprovalOwn(MainWindow.currentUserid);

                            }
                        }
                    else
                        memorandumSales = memorandumSalerepo.getAllPendingForAdministrator();

                    grdmemorandumSale.ItemsSource = memorandumSales;
                }
                else if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Memorandum Sales";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) MemorandumSale List") != null)
                        {
                            memorandumSales = memorandumSalerepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);


                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                            {
                                memorandumSales = memorandumSalerepo.getAllPendingForClosing(MainWindow.currentUserid);
                            }
                            else
                            {
                                memorandumSales = memorandumSalerepo.getAllPendingForClosingOwn(MainWindow.currentUserid);

                            }
                        }
                    //memorandumSales = memorandumSalerepo.getAllPendingForApproval(MainWindow.currentUserid);
                    else
                        memorandumSales = memorandumSalerepo.getAllPendingForClosingAdministrator();
                    grdmemorandumSale.ItemsSource = memorandumSales;
                }
            }
            else
            {
                memorandumSales = memorandumSalerepo.getAllPobyStatusId(MainWindow.currentUserid, statusId);
            }
            //foreach (var item in memorandumSales)
            //    orders.Add(item);
            this.grdmemorandumSale.ItemsSource = memorandumSales;

            //grdmemorandumSale.RefreshData();

            //if (grdmemorandumSale.Columns.Count <= 2)
            {
                //// grdcutomercompanies.AutoGenerateColumns = AutoGenerateColumnsMode.AddNew;
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "Id" });
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdmemorandumSale);
                grdmemorandumSale.Columns.GetColumnByFieldName("Id").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("Id").Visible = false;
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("company"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("incoterm"));
                ////grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("offerStatus"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("paymentTerm"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("TitleValue1"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("TitleValue2"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("currency"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("bid"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("memorandumSaleStatus"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("vendor"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("employee"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("department"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("customerCompany"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("principal"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("company_Id"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("incoterm_Id"));
                ////grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("offerStatus"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("paymentterm_Id"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("TitleValue1Id"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("TitleValue2Id"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("currency_Id"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("bid_Id"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("offer_Id"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("offer"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("allocation_Id"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("dept_Id"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("customerCompany_Id"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("principal_Id"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("user_Id"));
                grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("user"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("vendorPaymentId"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("vendorPaymentStatus"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("CostSheetId"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("CostSheet"));
                grdmemorandumSale.Columns.GetColumnByFieldName("referenceNo").Header = "Refrence No";
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("margin"));
                //grdmemorandumSale.Columns.Remove(grdmemorandumSale.Columns.GetColumnByFieldName("commision"));
                //grdmemorandumSale.RefreshData();
                //foreach (int i in grdmemorandumSale.GetSelectedRowHandles())
                //grdmemorandumSale.RefreshRow(i); 
                //this.grdmemorandumSale.EndDataUpdate();
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "company.CompanyName" });
                //grdmemorandumSale.Columns.GetColumnByFieldName("company.CompanyName").Header = "Company Name";
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "contactPerson.FName" });
                //grdmemorandumSale.Columns.GetColumnByFieldName("contactPerson.FName").Header = "First Name";
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "contactPerson.LName" });
                //grdmemorandumSale.Columns.GetColumnByFieldName("contactPerson.LName").Header = "Last Name";
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "ParentID" });
                //grdmemorandumSale.Columns.GetColumnByFieldName("ParentID").Header = "Parent";
                //grdmemorandumSale.Columns.GetColumnByFieldName("ParentID").Visible = false;
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "customerCompany.CompanyName" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "customerCompany.company.CompanyName" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "MemorandumSaleReferenceNo" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "company.CompanyName" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "department.DeptName" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "memorandumSaleDate" });

                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "employee.person.FName" });
                ////grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "memorandumSaleStatus.Status" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "SalesReferenceNo" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "incoterm.term" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "paymentTerm.term" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "currency.CurrencyName" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "vendor.company.CompanyName" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "principal.company.CompanyName" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "exchangeRate" });

                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "offerReferenceNo" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "targetYear" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "deliveryDate" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "maker" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "salesTax" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "origin" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "totalFOBValue" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "totalCFRValue" });
                //grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "lCnumber" });
                ////
                ////grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "Id" });
                ////grdmemorandumSale.Columns.Add(new GridColumn() { FieldName = "Id" });
                //grdmemorandumSale.Columns.GetColumnByFieldName("department.DeptName").Header = "Department";
                //grdmemorandumSale.Columns.GetColumnByFieldName("MemorandumSaleReferenceNo").Header = "Refrence No";
                //grdmemorandumSale.Columns.GetColumnByFieldName("customerCompany.company.CompanyName").Header = "Customer Name";
                //grdmemorandumSale.Columns.GetColumnByFieldName("employee.person.FName").Header = "Referal";
                //grdmemorandumSale.Columns.GetColumnByFieldName("company.CompanyName").Header = "Company Name";
                //grdmemorandumSale.Columns.GetColumnByFieldName("memorandumSaleStatus.Status").Visible = true;
                //grdmemorandumSale.Columns.GetColumnByFieldName("memorandumSaleStatus.Status").VisibleIndex = 6;
                //grdmemorandumSale.Columns.GetColumnByFieldName("memorandumSaleStatus.isActive").Visible = true;
                //grdmemorandumSale.Columns.GetColumnByFieldName("memorandumSaleStatus.isActive").VisibleIndex = 5;
                //grdmemorandumSale.Columns.GetColumnByFieldName("memorandumSaleDate").Header = "Date";
                ////grdmemorandumSale.Columns.GetColumnByFieldName("customerCompany").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("SalesReferenceNo").Header = "File No.";


                //grdmemorandumSale.Columns.GetColumnByFieldName("incoterm.term").Header = "Incoterm";
                //grdmemorandumSale.Columns.GetColumnByFieldName("paymentTerm.term").Header = "Payment Term";
                //grdmemorandumSale.Columns.GetColumnByFieldName("currency.CurrencyName").Header = "Currency";
                //grdmemorandumSale.Columns.GetColumnByFieldName("vendor.company.CompanyName").Header = "Vendor";
                //grdmemorandumSale.Columns.GetColumnByFieldName("lCnumber").Header = "L.C #";
                //grdmemorandumSale.Columns.GetColumnByFieldName("targetYear").Header = "Target Year";
                //grdmemorandumSale.Columns.GetColumnByFieldName("principal.company.CompanyName").Header = "Prinicipal";
                //grdmemorandumSale.Columns.GetColumnByFieldName("exchangeRate").Header = "Exchange Rate";
                //grdmemorandumSale.Columns.GetColumnByFieldName("deliveryDate").Header = "Delivery Date";

                //grdmemorandumSale.Columns.GetColumnByFieldName("maker").Header = "maker";
                //grdmemorandumSale.Columns.GetColumnByFieldName("origin").Header = "Origin";
                //grdmemorandumSale.Columns.GetColumnByFieldName("salesTax").Header = "Tax";
                //grdmemorandumSale.Columns.GetColumnByFieldName("totalFOBValue").Header = "Total FOB";
                //grdmemorandumSale.Columns.GetColumnByFieldName("totalCFRValue").Header = "Total CFR";
                //grdmemorandumSale.Columns.GetColumnByFieldName("incoterm.term").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("paymentTerm.term").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("currency.CurrencyName").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("vendor.company.CompanyName").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("principal.company.CompanyName").Visible = false;

                //grdmemorandumSale.Columns.GetColumnByFieldName("lCnumber").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("targetYear").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("deliveryDate").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("maker").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("origin").Visible = false;
                //grdmemorandumSale.Columns.GetColumnByFieldName("salesTax").Visible = false;
                //tableView.BestFitColumns();
                //grdmemorandumSale.Columns.GetColumnByFieldName("company.CompanyName").Header = "Company Name";
                //grdmemorandumSale.Columns.GetColumnByFieldName("company.CompanyName").Header = "Company Name";
                //grdmemorandumSale.Columns.GetColumnByFieldName("company.CompanyName").Header = "Company Name";
                //grdmemorandumSale.Columns.GetColumnByFieldName("company.CompanyName").Header = "Company Name";
                //grdmemorandumSale.Columns["memorandumSale.ParentID"].GroupIndex = 0;
                // grdcutomercompanies.GroupBy("memorandumSale.ParentID");
            }
        }
        private void grdmemorandumSale_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditMemorandumSale();
        }

        private void EditMemorandumSale()
        {
            if (grdmemorandumSale.GetFocusedRowCellValue(grdmemorandumSale.Columns.GetColumnByFieldName("Id")) != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Memorandum_Sale, (int)grdmemorandumSale.GetFocusedRowCellValue(grdmemorandumSale.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();

            }
        }

        private void btnNewMemorandumSale_Click(object sender, RoutedEventArgs e)
        {
           

            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Memorandum_Sale,0);
            procurmentPanel.Show();
        }

        private void btnEditMemorandumSale_Click(object sender, RoutedEventArgs e)
        {
            EditMemorandumSale();

        }

        private void UcmemorandumSalegrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //AllActive = 0;
            //SystemLogic.SaveUserSettingForCurrentWindow(grdmemorandumSale);
            //statusId = 0;
        }

        private void RbtnCopy_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdmemorandumSale.View.ShowPrintPreview(this);
            //ShowDesigner(tableView);
        }
        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ShowDesigner(tableView);
        }
        // Initializes and runs a Report Designer. 
        public static void ShowDesigner(IGridViewFactory<ColumnWrapper, RowBaseWrapper> factory)
        {
            var report = new XtraReport();
            ReportGenerationExtensions<ColumnWrapper, RowBaseWrapper>.Generate(report, factory);
            Reportss.frmReportPanel frmReport = new Reportss.frmReportPanel(report);
            frmReport.Show();
            //var reportDesigner = new ReportDesigner();
            //reportDesigner.Loaded += (s, e) => {
            //    reportDesigner.OpenDocument(report);
            //};
            //reportDesigner.ShowWindow(factory as FrameworkElement);
        }

        private void MbtnReportCreate_Click(object sender, RoutedEventArgs e)
        {
            ShowDesigner(tableView);
        }
        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName();
            setReportName.ShowDialog();
            string str = Reportss.frmSetReportName.ReportName;

            if (str != "")
            {
                SYSTEM_STATIC.SaveUnBoundReport(grdmemorandumSale, str, 2);
                DXMessageBox.Show("Report Named ( " + str + " ) is Saved!");
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //this.RemoveFromVisualTree();
            MemorandumSaless.ucMemorandumSaleGrid ucMemorandumSaleGrid = new ucMemorandumSaleGrid();
            this.Content = ucMemorandumSaleGrid;
            //InitializeComponent();
            //loadMemorandumSalegrid();
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdmemorandumSale.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdmemorandumSale.View.ShowPrintPreview(this);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale without Approval") != null) ? true : false)
            {

                if (grdmemorandumSale.GetFocusedRow() != null)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    MemorandumSaless.ucStatuschange.memorandumSaleid = (int)grdmemorandumSale.GetFocusedRowCellValue(grdmemorandumSale.Columns.GetColumnByFieldName("Id"));
                    MemorandumSaless.frmMemorandumSaleStatusChange statusChange = new MemorandumSaless.frmMemorandumSaleStatusChange();
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (MemorandumSaless.ucStatuschange.memorandumSale.Id != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing MemorandumSale") != null) ? true : false)
                        {
                            MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing = false;
                            MemorandumSaless.ucStatuschange.memorandumSale.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, MemorandumSaless.ucStatuschange.memorandumSale.Id, 4, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.memorandumSaleRepo.update(Inquiriess.ucStatuschange.memorandumSale);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                        {
                            MemorandumSaless.ucStatuschange.memorandumSale.stage = TransactionStage.AwaitingApproval.ToString();
                            if (MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing == null)
                            {
                                MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, MemorandumSaless.ucStatuschange.memorandumSale.Id, 4, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null)
                        {
                            MemorandumSaless.ucStatuschange.memorandumSale.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing == null)
                            {
                                MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing = true;

                            }
                            //MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing = true;

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, MemorandumSaless.ucStatuschange.memorandumSale.Id, 4, frmInputBox.comment);
                        }
                        else
                        {
                            MemorandumSaless.ucStatuschange.memorandumSale.stage = TransactionStage.AwaitingFirstReview.ToString();

                            MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing = true;
                        }
                    //MemorandumSaless.ucStatuschange.memorandumSale.user_Id = MainWindow.currentUserid;
                    MemorandumSaless.ucStatuschange.memorandumSaleRepo.update(MemorandumSaless.ucStatuschange.memorandumSale);
                    MessageBox.Show("MemorandumSale status changed to InActive (" + MemorandumSaless.ucStatuschange.memorandumSale.memorandumSaleStatus.Status + ")");
                }

            }
            else
            {
                MessageBox.Show("You are not Allowed to Close MemorandumSale Directly.");
            }
        }
        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Pending Memorandum Sales";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) MemorandumSale List") != null)
                {
                    memorandumSales = memorandumSalerepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                    {
                        memorandumSales = memorandumSalerepo.getAllPendingForApproval(MainWindow.currentUserid);
                    }

                    else
                    {
                        memorandumSales = memorandumSalerepo.getAllPendingForApprovalOwn(MainWindow.currentUserid);

                    }
                }
            else
                memorandumSales = memorandumSalerepo.getAllPendingForAdministrator();

            grdmemorandumSale.ItemsSource = memorandumSales;
            //grdmemorandumSale.ClearGrouping();
            //grdmemorandumSale.FilterString = "";
            //grdmemorandumSale.Columns["stage"].GroupIndex = 0;
            //grdmemorandumSale.GroupBy("stage");
            //grdmemorandumSale.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);
            //grdmemorandumSale.View = new CardView();
        }
        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            lblHeading.Text = "(Pending for Closing) Memorandum Sales";
            if (MainWindow.currentUserid != 0)
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) MemorandumSale List") != null)
            {
                    memorandumSales = memorandumSalerepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);


                }
                else
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                {
                    memorandumSales = memorandumSalerepo.getAllPendingForClosing(MainWindow.currentUserid);
                }
                else
                {
                    memorandumSales = memorandumSalerepo.getAllPendingForClosingOwn(MainWindow.currentUserid);

                }
            }
            //memorandumSales = memorandumSalerepo.getAllPendingForApproval(MainWindow.currentUserid);
            else
                memorandumSales = memorandumSalerepo.getAllPendingForClosingAdministrator();
            grdmemorandumSale.ItemsSource = memorandumSales;
            //grdmemorandumSale.ClearGrouping();
            //grdmemorandumSale.FilterString = "";
            //grdmemorandumSale.ItemsSource = memorandumSales;
            //grdmemorandumSale.Columns["stage"].GroupIndex = 0;
            //grdmemorandumSale.GroupBy("stage");
            //grdmemorandumSale.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);

            //grdmemorandumSale.View =new CardView();
        }
        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorandum Sale without Approval") != null) ? true : false)
            {
                if (grdmemorandumSale.GetFocusedRow() != null)
                {
                    MemorandumSale memorandumSale = new MemorandumSale();
                    //Inquiriess.ucStatuschange.memorandumSaleid = (int)grdmemorandumSale.GetFocusedRowCellValue(grdmemorandumSale.Columns.GetColumnByFieldName("Id"));
                    memorandumSale = grdmemorandumSale.GetFocusedRow() as MemorandumSale;
                    //Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                    //statusChange.Owner = this;
                    //var myWindow = Window.GetWindow(this);
                    //statusChange.Owner = myWindow;
                    //statusChange.ShowDialog();
                    UsersRepo usersRepo = new UsersRepo();

                    if (memorandumSale.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added MemorandumSale") != null) ? true : false)
                        {
                            memorandumSale.isApproved = true;
                            memorandumSale.stage = TransactionStage.Approved.ToString();
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Adding, memorandumSale.Id, 4, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                        {
                            memorandumSale.stage = TransactionStage.AwaitingApproval.ToString();
                            if (memorandumSale.isApproved == null)
                            {
                                memorandumSale.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, memorandumSale.Id, 4, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null)
                        {
                            memorandumSale.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (memorandumSale.isApproved == null)
                            {
                                memorandumSale.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, memorandumSale.Id, 4, frmInputBox.comment);
                        }
                        else
                        {
                            memorandumSale.isApproved = false;
                        }




                    memorandumSalerepo.update(memorandumSale);
                    MessageBox.Show("MemorandumSale is Approved (" + memorandumSale.Id + ")");
                    SystemLog.LogInfo(this.GetType(), "MemorandumSale is Approved (" + memorandumSale.Id + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Approve MemorandumSale Directly");
                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve MemorandumSale Directly user id=(" + MainWindow.currentUserid + ")");

            }

        }
        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdmemorandumSale);
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            //_usersRepo usersRepo = new _usersRepo();

            lblHeading.Text = "Void Memorandum Sales";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void MemorandumSales") != null)
                {
                    memorandumSales = memorandumSalerepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    memorandumSales = memorandumSalerepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                memorandumSales = memorandumSalerepo.getVoidRegisterAdministrator();
            grdmemorandumSale.ItemsSource = memorandumSales;
            grdmemorandumSale.Columns["CreationDate"].VisibleIndex = 0;
        }

        private void MbtnMemorandumRegister_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Memorandum Sales") != null)
            {
                memorandumSales = memorandumSalerepo.getAll(MainWindow.currentUserid);
            }
            else
            {
                memorandumSales = memorandumSalerepo.getAllActive(MainWindow.currentUserid);
            }
            this.grdmemorandumSale.ItemsSource = memorandumSales;
        }
    }
}
