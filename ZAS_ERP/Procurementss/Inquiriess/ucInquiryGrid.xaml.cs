using DevExpress.Xpf.Grid;
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
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERP_BL;
using ERP_BL.Databases;
using ERP_BL.Enums;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid.Themes;
using DevExpress.XtraReports.UI;
using DevExpress.Xpf.Grid.Printing;
using DevExpress.XtraExport.Helpers;
using DevExpress.Xpf.Printing;
using ERP_BL.Reports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.Inquiriess
{
    /// <summary>
    /// Interaction logic for ucInquiryGrid.xaml
    /// </summary>
    public partial class ucInquiryGrid : UserControl
    {
        InquiryRepo inquiryrepo = new InquiryRepo();

        Inquiry inquiry = new Inquiry();
        List<ERP_BL.Databases.Inquiry> inquiries = new List<ERP_BL.Databases.Inquiry>();
        public static int statusid;
        public static int AllActive;
        public int VoidCount { get; set; }
        public int ApprovalCount { get; set; }
        public int ClosingCount { get; set; }

        public bool firstLoad;
        public InquiryStatus oldStatus = new InquiryStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();

        public ucInquiryGrid(bool _firstLoad)
        {
            InitializeComponent();

            firstLoad = _firstLoad;
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inquiry List") != null)
                {
                    ApprovalCount = inquiryrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                    {
                        ApprovalCount = inquiryrepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = inquiryrepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);

                    }

                }
            }

            else
            {
                ApprovalCount = inquiryrepo.getAllPendingInquiriesForApprovalAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inquiry List") != null)
                {
                    ClosingCount = inquiryrepo.getAllPendingForClosingCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                    {
                        ClosingCount = inquiryrepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = inquiryrepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                    }

                }
            }

            else
            {
                ClosingCount = inquiryrepo.getAllPendingInquiriesForClosingAdministratorCount();
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inquiries") != null)
            {
                mbtnVoid.Visibility = Visibility.Visible;

                if (MainWindow.currentUserid != 0)
                {
                    VoidCount = inquiryrepo.getVoidRegisterCount(MainWindow.currentUserid);

                }
                else
                {
                    VoidCount = inquiryrepo.getVoidRegisterAdministratorCount();

                }
            }
            else
            {
                mbtnVoid.Visibility = Visibility.Collapsed;
            }

        }

        /// <summary>
        /// runs when window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucinquiryGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null )
            //{
            //    mbtnPending.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    mbtnPending.Visibility = Visibility.Collapsed;

            //}
            loadInquirygrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

        }

        /// <summary>
        /// Loads data into inquiry Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadInquirygrid()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Inquiries(Open)" || lblHeading.Text == "Inquiries(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusid == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        inquiries = inquiryrepo.getAll();
                        //this.grdinquiry.ItemsSource = inquiries;
                        //grdinquiry.Columns.GetColumnByFieldName("Id").Visible = false;
                        //SystemLogic.SetUserSettingOfCurrentWindow(grdinquiry);
                        //MessageBox.Show("You are not Authorized");

                        //return;
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inquiries") != null)
                    {
                        if (firstLoad)
                        {

                            inquiries = inquiryrepo.getAllActiveFirst(MainWindow.currentUserid);
                            firstLoad = false;
                        }
                        else
                        {
                            inquiries = inquiryrepo.getAllFirst(MainWindow.currentUserid);
                        }
                    }
                    else
                    {
                        inquiries = inquiryrepo.getAllActiveFirst(MainWindow.currentUserid);
                    }
                }
                if (AllActive == 1)
                {
                    inquiries = inquiryrepo.getAllActiveFirst(MainWindow.currentUserid);
                }
                if (AllActive == 2)
                {
                    inquiries = inquiryrepo.getAllInActiveFirst(MainWindow.currentUserid);
                }
                if (AllActive == 3)
                {
                    lblHeading.Text = "(Pending for Approval) Inquiries";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inquiry List") != null)
                        {
                            inquiries = inquiryrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inquiry") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                            {
                                inquiries = inquiryrepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                            }
                            else
                            {
                                inquiries = inquiryrepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);

                            }
                        }
                    //inquiries = inquiryrepo.getAllPendingForApproval(MainWindow.currentUserid);
                    else
                        inquiries = inquiryrepo.getAllPendingForAdministratorFirst();

                    //grdinquiry.ItemsSource = inquiries;
                }
                if (AllActive == 4)
                {

                }
                if (AllActive == 5)
                {
                    lblHeading.Text = "(Pending for Closing) Inquiries";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inquiry List") != null)
                        {
                            inquiries = inquiryrepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                            {
                                inquiries = inquiryrepo.getAllPendingForClosingFirst(MainWindow.currentUserid);
                            }
                            else
                            {
                                inquiries = inquiryrepo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);

                            }
                        }
                    //inquiries = inquiryrepo.getAllPendingForApproval(MainWindow.currentUserid);
                    else
                        inquiries = inquiryrepo.getAllPendingForClosingAdministratorFirst();

                    //grdinquiry.ItemsSource = inquiries;
                }

            }
            else
            {
                inquiries = inquiryrepo.getInquiriesByStatusIdFirst(MainWindow.currentUserid, statusid);
            }

            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdinquiry);
            RemoveSourceObjects();
            this.grdinquiry.ItemsSource = inquiries;

        }

        private void grdinquiry_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditInquiry();
            inquiry = ((Inquiry)grdinquiry.GetFocusedRow());

        }

        private void EditInquiry()
        {
            if (grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Inquiry, (int)grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.inq.Add(inquiry);
                procurmentPanel.Show();
                inquiry = null;

            }
        }
        private void btnNewInquiry_Click(object sender, RoutedEventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Inquiry, 0);
            procurmentPanel.Show();

        }

        private void btnEditInquiry_Click(object sender, RoutedEventArgs e)
        {
            EditInquiry();

        }

        private void grdinquiry_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "totalWeight")

            {
                if (e.GetListSourceFieldValue("products") != null)
                {
                    List<InquiryProduct> products = (e.GetListSourceFieldValue("products")) as List<InquiryProduct>;

                    decimal? totalweight = 0;
                    foreach (var pro in products)
                    {
                        if (pro.Weight != null || pro.Weight != 0)
                        {
                            totalweight = pro.Weight;
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
                    List<InquiryProduct> products = (e.GetListSourceFieldValue("products")) as List<InquiryProduct>;

                    double totalquantity = 0;
                    foreach (var pro in products)
                    {
                        if (pro.quantity != 0)
                        {
                            totalquantity = pro.quantity;
                        }
                    }
                    //CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                    e.Value = totalquantity;
                    // string s = "Test: FieldTwo";
                }
            }
            if (e.Column.FieldName == "inquiryStatus.Status")
            {
            }
        }
        private void grdinquiry_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {

            Inquiry inq = grdinquiry.CurrentItem as Inquiry;
            if (e.Column.FieldName == "inquiryStatus.Status" && inq != null)
            {   //foreach (ItemCollection item in grdinquiry.VisibleItems)

                //string str = grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("inquiryStatus.backcolor")).ToString();
                //grdinquiry.CurrentColumn.CellTemplate.Template= new TemplateContent() { Background= new Brush() { col } }
                //var mycolor=grdinquiry.GetCellValue((int)grdinquiry.CurrentItem, grdinquiry.Columns.GetColumnByFieldName("inquiryStatus.backcolor"));
                string mycolor = inq.inquiryStatus.backcolor;
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(mycolor.ToString());
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);

                //myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                //BindingOperations.SetBinding( LightweightCellEditor.BackgroundProperty, myBinding);
                Style st = new Style(typeof(LightweightCellEditor));
                object key = new DevExpress.Xpf.Grid.Themes.GridRowThemeKeyExtension() { ResourceKey = DevExpress.Xpf.Grid.Themes.GridRowThemeKeys.CellStyle };
                SolidColorBrush s = new SolidColorBrush(newColor);
                Binding myBinding = new Binding();
                myBinding.ElementName = "grdinquiry";
                myBinding.Path = new PropertyPath("CurrentItem.inquiryStatus.backcolor");
                myBinding.Mode = BindingMode.TwoWay;
                //st.BasedOn = (Style)this.FindResource(key);
                //Style baseStyle = FindResource(new GridRowThemeKeyExtension() { ResourceKey = GridRowThemeKeys.LightweightCellStyle, ThemeName = ThemeManager.ActualApplicationThemeName }) as Style;
                //st.BasedOn = baseStyle;
                Style myStyle = new Style(typeof(DataGridCell));
                myStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new Binding("SelectedColour[0]")));
                st.Setters.Add(new Setter(LightweightCellEditor.BackgroundProperty, newColor));
                //new Binding("RowData.Row.inquiryStatus.backcolor")
                //st.Setters.Add(new Setter(LightweightCellEditor.BackgroundProperty, new Binding(new SolidColorBrush(newColor))));
                // GridCell gridCell = grdinquiry.GetCellValue((int)grdinquiry.CurrentItem, grdinquiry.Columns.GetColumnByFieldName("inquiryStatus.Status")) as GridCell;

                //next statment sets the cell style to grds desired column 
                // grdinquiry.Columns.GetColumnByFieldName("inquiryStatus.Status").CellStyle = st;

            }
        }

        private void grdinquiry_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {

            if (e.Column.FieldName == "inquiryStatus.Status")
            {
                Inquiry inq = grdinquiry.CurrentItem as Inquiry;
                var cb = new GridColumn();
                cb.Header = "Status";

                //products = productrepo.getAll();
                //List<cmbitem> cmbitems = new List<cmbitem>();
                //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
                //foreach (Product prod in products)
                //{
                //    cmbitems.Add(new cmbitem() { name = prod.item, id = prod.Id });
                //}           
                //cb.ItemsSource = loadproducts();
                //cb.ce = ;
                //cb.SelectedValuePath = "id";/*new List<string> { "C50", "C40", "C30" };*/
                //cb.SelectedValueBinding = new Binding("name");
                //var style = new Style(typeof(ComboBox));
                //style.Setters.Add(new EventSetter(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(ComboBox_SelectionChanged)));
                //cb.EditingElementStyle = style;
                //dGitems.Columns.Add( cb);


            }

        }

        private void tableView_CustomCellAppearance(object sender, CustomCellAppearanceEventArgs e)
        {
            e.Result = e.ConditionalValue;
            e.Handled = true;
        }

        private void UcinquiryGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //SystemLogic.SaveUserSettingForCurrentWindow(grdinquiry);
            //statusid = 0;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdinquiry.View.ShowPrintPreview(this);
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Inquiries Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblHeading.Text);
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdinquiry, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Inquiries Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



        }
        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Inquiriess.ucInquiryGrid ucInquiryGrid = new ucInquiryGrid(false);
            this.Content = ucInquiryGrid;
            //lblHeading.Text = "Inquiries";
            //loadInquirygrid();
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            PrintableControlLink link = new PrintableControlLink((TableView)grdinquiry.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            //link.ReportHeaderTemplate= text as DataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdinquiry.View.ShowPrintPreview(this);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry") != null) ? true : false)
            {
                if (grdinquiry.GetFocusedRow() != null)
                {
                    Inquiriess.ucStatuschange.inquiryid = (int)grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id"));

                    var row = grdinquiry.GetFocusedRow() as Inquiry;
                    if (row.inquiryStatus != null)
                    {
                        oldStatus = row.inquiryStatus;
                    }
                    Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                    //statusChange.Owner = this;
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (Inquiriess.ucStatuschange.inquiry.Id != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null) ? true : false)
                        {
                            Inquiriess.ucStatuschange.inquiry.PendingForClosing = false;
                            Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.Closed.ToString();
                            //Inquiriess.ucStatuschange.inquiryRepo.update(Inquiriess.ucStatuschange.inquiry);

                        }
                        else
                        {
                            Inquiriess.ucStatuschange.inquiry.PendingForClosing = true;
                            Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.AwaitingFirstReview.ToString();

                        }
                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null) ? true : false)
                    {
                        Inquiriess.ucStatuschange.inquiry.PendingForClosing = false;

                        //Inquiriess.ucStatuschange.inquiryRepo.update(Inquiriess.ucStatuschange.inquiry);

                    }
                    UsersRepo usersRepo = new UsersRepo();

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null) ? true : false)
                    {
                        Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.Closed.ToString();

                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Closing, Inquiriess.ucStatuschange.inquiry.Id, 1, frmInputBox.comment);
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                    {
                        Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.AwaitingApproval.ToString();
                        if (Inquiriess.ucStatuschange.inquiry.PendingForClosing == null)
                        {
                            Inquiriess.ucStatuschange.inquiry.PendingForClosing = true;
                        }
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Inquiriess.ucStatuschange.inquiry.Id, 1, frmInputBox.comment);
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null)
                    {
                        Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.AwaitingSecondReview.ToString();
                        if (Inquiriess.ucStatuschange.inquiry.PendingForClosing == null)
                        {
                            Inquiriess.ucStatuschange.inquiry.PendingForClosing = true;
                        }
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Inquiriess.ucStatuschange.inquiry.Id, 1, frmInputBox.comment);
                    }
                    //Adding Signaure
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Inquiry has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Inquiry);
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

                    }
                    string oldStat = "";
                    if (oldStatus != null)
                    {
                        oldStat = oldStatus.Status;
                    }
                    string newStat = Inquiriess.ucStatuschange.inquiry.inquiryStatus.Status;
                    string symbolCurr = "";

                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of Inquiry has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.Inquiry, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Inquiry No " + row.SalesReferenceNo, row.Id, TransactionItemType.Inquiry, comment.Comment, user.id, "New Comment", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Inquiry No " + row.SalesReferenceNo, row.Id, TransactionItemType.Inquiry, comment.Comment, user.id, "New Comment", null);
                        }
                    }

                    //}

                    Inquiriess.ucStatuschange.inquiry.user_Id = MainWindow.currentUserid;
                    Inquiriess.ucStatuschange.inquiry.ClosingDate = System.DateTime.Now;
                    Inquiriess.ucStatuschange.inquiry.LastStatusChangeDate = System.DateTime.Now;
                    Inquiriess.ucStatuschange.inquiryRepo.update(Inquiriess.ucStatuschange.inquiry);


                    MessageBox.Show("Inquiry status changed to InActive (" + Inquiriess.ucStatuschange.inquiry.inquiryStatus.Status + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to close Inquiry Directly");
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            AllActive = 3;
            statusid = 0;
            lblHeading.Text = "(Pending for Approval) Inquiries";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inquiry List") != null)
                {
                    inquiries = inquiryrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inquiry") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                    {
                        inquiries = inquiryrepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        inquiries = inquiryrepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);

                    }
                }
            //inquiries = inquiryrepo.getAllPendingForApproval(MainWindow.currentUserid);
            else
                inquiries = inquiryrepo.getAllPendingForAdministratorFirst();

            grdinquiry.ItemsSource = inquiries;
            //grdinquiry.ClearGrouping();
            //grdinquiry.FilterString = "";
            //grdinquiry.Columns["stage"].GroupIndex = 0;
            //grdinquiry.GroupBy("stage");
            //grdinquiry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdinquiry);
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            AllActive = 5;
            statusid = 0;
            lblHeading.Text = "(Pending for Closing) Inquiries";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inquiry List") != null)
                {
                    inquiries = inquiryrepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                    {
                        inquiries = inquiryrepo.getAllPendingForClosingFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        inquiries = inquiryrepo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);

                    }
                }
            //inquiries = inquiryrepo.getAllPendingForApproval(MainWindow.currentUserid);
            else
                inquiries = inquiryrepo.getAllPendingForClosingAdministrator();

            grdinquiry.ItemsSource = inquiries;
            //grdinquiry.ClearGrouping();
            //grdinquiry.FilterString = "";
            //grdinquiry.Columns["stage"].GroupIndex = 0;
            //grdinquiry.GroupBy("stage");
            //grdinquiry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inquiry") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null) ? true : false)
            {
                if (grdinquiry.GetFocusedRow() != null)
                {
                    Inquiry inquiry = new Inquiry();
                    UsersRepo usersRepo = new UsersRepo();
                    //Inquiriess.ucStatuschange.inquiryid = (int)grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id"));
                    inquiry = grdinquiry.GetFocusedRow() as Inquiry;
                    //Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                    //statusChange.Owner = this;
                    //var myWindow = Window.GetWindow(this);
                    //statusChange.Owner = myWindow;
                    //statusChange.ShowDialog();
                    if (inquiry.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inquiry") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null) ? true : false)
                        {
                            inquiry.isApproved = true;
                            inquiry.stage = TransactionStage.Approved.ToString();
                            //Inquiriess.ucStatuschange.inquiryRepo.update(Inquiriess.ucStatuschange.inquiry);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                        {
                            inquiry.stage = TransactionStage.AwaitingApproval.ToString();
                            if (inquiry.isApproved == null)
                                inquiry.isApproved = false;


                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, Inquiriess.ucStatuschange.inquiry.Id, 1, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null)
                        {
                            inquiry.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (inquiry.isApproved == null)
                                inquiry.isApproved = false;

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, Inquiriess.ucStatuschange.inquiry.Id, 1, frmInputBox.comment);
                        }
                        else
                        {
                            inquiry.isApproved = false;
                            //inquiry.stage = TransactionStage.AwaitingApproval.ToString();
                        }





                    //usersRepo.Add(TransactionInfo.Approved_Closing, Inquiriess.ucStatuschange.inquiry.Id, 1,"");
                    inquiryrepo.update(inquiry);
                    MessageBox.Show("Inquiry is Approved (" + inquiry.Id + ")");
                    SystemLog.LogInfo(this.GetType(), "Inquiry is Approved (" + inquiry.Id + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Approve Inquiry Directly");
                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Inquiry Directly user id=(" + MainWindow.currentUserid + ")");

            }

        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Void Inquiries";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inquiries") != null)
                {
                    inquiries = inquiryrepo.getVoidRegister(MainWindow.currentUserid);
                    //inquiries = inquiryrepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    inquiries = inquiryrepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                inquiries = inquiryrepo.getVoidRegisterAdministrator();
            grdinquiry.ItemsSource = inquiries;
            grdinquiry.Columns["CreationDate"].VisibleIndex = 0;
        }

        private void BtnDateFilter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (dateFrom.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateFrom.Focus();

                return;
            }
            else
                if (dateTo.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateTo.Focus();

                return;
            }
            else
            {

                if (SYSTEM_STATIC.currentUser.employee != null && SYSTEM_STATIC.currentUser.employee.InquiryDataRetrievalDate != null)
                {
                    if (SYSTEM_STATIC.currentUser.employee.InquiryDataRetrievalDate > dateFrom.DateTime)
                    {
                        if (SYSTEM_STATIC.currentUser.employee.AllowOpenTransactions != true)
                        {
                            DXMessageBox.Show("Date from cannot be less than " + SYSTEM_STATIC.currentUser.employee.InquiryDataRetrievalDate);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Mouse.OverrideCursor = null;
                            });
                            return;
                        }
                    }

                    if (SYSTEM_STATIC.currentUser.employee.InquiryDataRetrievalDate > dateTo.DateTime)
                    {
                        if (SYSTEM_STATIC.currentUser.employee.AllowOpenTransactions != true)
                        {
                            DXMessageBox.Show("Date To cannot be less than " + SYSTEM_STATIC.currentUser.employee.InquiryDataRetrievalDate);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Mouse.OverrideCursor = null;
                            });
                            return;
                        }
                    }
                }

                LoadOrdersByDate();
                RemoveSourceObjects();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });

            }

        }
        private void LoadOrdersByDate()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Inquiries(Open)" || lblHeading.Text == "Inquiries(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusid == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        inquiries = inquiryrepo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inquiries") != null)
                    {
                        if (firstLoad)
                        {

                            inquiries = inquiryrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            firstLoad = false;
                        }
                        else
                        {
                            inquiries = inquiryrepo.getAllByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                    }
                    else
                    {
                        inquiries = inquiryrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                }
                if (AllActive == 1)
                {
                    inquiries = inquiryrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                if (AllActive == 2)
                {
                    inquiries = inquiryrepo.getAllInActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                if (AllActive == 3)
                {
                    lblHeading.Text = "(Pending for Approval) Inquiries";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inquiry List") != null)
                        {
                            inquiries = inquiryrepo.getAllPendingForApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inquiry") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                            {
                                inquiries = inquiryrepo.getAllPendingForApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                inquiries = inquiryrepo.getAllPendingForApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    //inquiries = inquiryrepo.getAllPendingForApproval(MainWindow.currentUserid);
                    else
                        inquiries = inquiryrepo.getAllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);

                    //grdinquiry.ItemsSource = inquiries;
                }
                if (AllActive == 4)
                {

                }
                if (AllActive == 5)
                {
                    lblHeading.Text = "(Pending for Closing) Inquiries";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inquiry List") != null)
                        {
                            inquiries = inquiryrepo.getAllPendingForClosingDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                            {
                                inquiries = inquiryrepo.getAllPendingForClosingByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                inquiries = inquiryrepo.getAllPendingForClosingOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    //inquiries = inquiryrepo.getAllPendingForApproval(MainWindow.currentUserid);
                    else
                        inquiries = inquiryrepo.getAllPendingForClosingAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);

                    //grdinquiry.ItemsSource = inquiries;
                    RemoveSourceObjects();
                }

            }
            else
            {
                inquiries = inquiryrepo.getInquiriesByStatusId(MainWindow.currentUserid, statusid);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdinquiry);
            RemoveSourceObjects();
            this.grdinquiry.ItemsSource = inquiries;
            
        }


        private void RemoveSourceObjects()
        {
           
            //grdinquiry.Columns.GetColumnByFieldName("Id").Visible = false;

            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("company_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("company"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("customerCompany_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("customerCompany"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("TitleValue1Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("TitleValue2Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("department"));
            //grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("bid_Id"));
            //grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("offer_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("employee"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("allocation_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("dept_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("department"));
            //grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("principal_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("user_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("user"));


            //grdinquiry.Columns.GetColumnByFieldName("Id").Visible = false;

            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("company_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("company"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("customerCompany_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("customerCompany"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("TitleValue1Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("TitleValue2Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("department"));
            //grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("bid_Id"));
            //grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("offer_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("employee"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("allocation_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("dept_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("department"));
            //grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("principal_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("user_Id"));
            grdinquiry.Columns.Remove(grdinquiry.Columns.GetColumnByFieldName("user"));

        }
        public class ColorValueConverter : MarkupExtension, IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                InquiryStatus cellValue = (value as InquiryStatus);
                if (cellValue.Status == "Active") return Brushes.Green;
                return Brushes.Red;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public override object ProvideValue(IServiceProvider serviceProvider)
            {
                return this;
            }

        }


    }

}
