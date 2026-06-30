using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucAddVehicleExpenses.xaml
    /// </summary>
    public partial class ucAddVehicleExpenses : DXWindow
    {
        public double totalExpenses = 0;
        public List<VehicleExpenses> finalVehicleExpenses = new List<VehicleExpenses>();
        public List<VehicleExpenses> vehicleExpenses = new List<VehicleExpenses>();

        public AdminBill adminBill;
        Payee payee;

        public bool editFlag = false;
        public bool saveflag = false;

        int intGroupId = 0;

        //AdminBillsRepo billsRepo = new AdminBillsRepo();
        public ucAddVehicleExpenses()
        {
            InitializeComponent();
        }

        public ucAddVehicleExpenses(AdminBill _adminBill, Payee _payee)
        {
            InitializeComponent();
            adminBill = _adminBill;
            payee = _payee;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //grdVehicleExpenses.ItemsSource = vehicleExpenses;
            
            //var maintenanceHeads = billsRepo.GetAllMaintenanceHead();
            //lookupMaintenanceHead.ItemsSource = maintenanceHeads;

            for (int i = 0; i <= (int)ERP_BL.Enums.VehicleExpenseType.Maintenance; i++)
            {
                    cmbExpenseType.Items.Add(((ERP_BL.Enums.VehicleExpenseType)i).ToString());
            }

            LoadOnVehicleExpenses();
        }

        public void loadonVehicledata()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(adminBill.vehicleExpenses[0].transactionGroupId, TransactionItemType.VehicleExpenses);
            cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
        }

        public void LoadOnVehicleExpenses()
        {
            if(editFlag == false)
            {
                if (adminBill != null)
                {
                    datCreationDate.EditValue = DateTime.Now;
                    if (adminBill.payee != null)
                    {
                        lookupPayee.Text = adminBill.payee.PayeeName;
                        AdminBillsRepo billsRepo = new AdminBillsRepo();


                        var vehicleExpense = billsRepo.GetLastVehicleExpenseByPayee(adminBill.payee_Id.Value);

                        if (vehicleExpense != null)
                            txtLastMeterReading.Text = vehicleExpense.CurrentMeterReading.ToString();
                    }
                        
                    cmbExpenseType.SelectedIndex = 1;

                    
                }

            }
            else
            {
                if (adminBill != null)
                {
                    if (adminBill.vehicleExpenses != null && adminBill.vehicleExpenses.Count > 0)
                    {
                        loadonVehicledata();
                        intGroupId = adminBill.vehicleExpenses[0].transactionGroupId;
                        if (adminBill.vehicleExpenses[0].CreationDate != null)
                            datCreationDate.EditValue = adminBill.vehicleExpenses[0].CreationDate;

                        cmbExpenseType.SelectedIndex = (int)adminBill.vehicleExpenses[0].expenseType;

                        if (adminBill.vehicleExpenses[0].payee != null)
                            lookupPayee.Text = adminBill.vehicleExpenses[0].payee.PayeeName;
                        else if (adminBill != null && adminBill.payee != null)
                            lookupPayee.Text = adminBill.payee.PayeeName;

                        txtCurrenctMeterReading.Text = adminBill.vehicleExpenses[0].CurrentMeterReading.ToString();
                        txtLastMeterReading.Text = adminBill.vehicleExpenses[0].LastMeterReading.ToString();
                        txtReadingDifference.Text = adminBill.vehicleExpenses[0].MeterReadingDifference.ToString();
                        txtSystemRefNo.Text = adminBill.vehicleExpenses[0].SystemRefNo;
                    }
                    grdVehicleExpenses.ItemsSource = adminBill.vehicleExpenses;
                }
            }
            
        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            //var expense = e.Source as VehicleExpenses;
            //expense.payee = payee;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            saveflag = true;
            this.Close();
        }

        private void btnAddDeduction_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (saveflag == true)
            {
                if(editFlag == false)
                {
                    GroupIdCalculation();
                    txtSystemRefNo.Text = "VehicleExpense-" + intGroupId;
                }
                foreach (var expense in grdVehicleExpenses.ItemsSource as List<VehicleExpenses>)
                {
                    //expense.maintenanceHeadId = expense.maintenanceHead?.Id;
                    //expense.maintenanceHead = null;

                    if (datCreationDate.EditValue != null)
                        expense.CreationDate = datCreationDate.DateTime;

                    if (cmbExpenseType.SelectedIndex > -1)
                        expense.expenseType = (ERP_BL.Enums.VehicleExpenseType)cmbExpenseType.SelectedIndex;

                    if (lookupPayee.SelectedIndex > -1)
                        expense.payeeId = (lookupPayee.SelectedItem as Payee).Id;

                    expense.CurrentMeterReading = Convert.ToDouble(txtCurrenctMeterReading.Text);
                    expense.LastMeterReading = Convert.ToDouble(txtLastMeterReading.Text);
                    expense.MeterReadingDifference = Convert.ToDouble(txtReadingDifference.Text);
                    expense.transactionGroupId = intGroupId;
                    expense.SystemRefNo = txtSystemRefNo.Text;

                    finalVehicleExpenses.Add(expense);
                }

                totalExpenses = finalVehicleExpenses.Sum(x => x.ExpenseAmount);
            }
            
        }

        private void View_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var row = e.Row as VehicleExpenses;

            if(e.Column.FieldName == "payee")
            {
                if(row.payee != null && row.payee.Id != payee.Id)
                {
                    DXMessageBox.Show("Payee should be the same as Admin Bill");
                    row.payee = payee;
                }
            }


        }

        private void GroupIdCalculation()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            string groupId;
            string year;
            string month;
            string id;

            var lastReceipt = BillsRepo.GetLastBill();
            if (lastReceipt == 0)
            {
                year = DateTime.Now.Year.ToString();
                month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                id = "1";
                groupId = year + month + id;
            }
            else
            {
                // var last = history.Last();
                var lastId = lastReceipt/*.transactionGroupId*/;
                string fullId = lastId.ToString();
                int length = fullId.Length;
                //length = length - 1;

                year = fullId.Substring(0, 4);
                if (year != DateTime.Now.Year.ToString())
                {
                    year = DateTime.Now.Year.ToString();
                }
                month = fullId.Substring(4, 2);

                var currentMonth = DateTime.Now.Month.ToString();
                if (currentMonth.Length == 1)
                {
                    currentMonth = "0" + currentMonth;
                }

                int idLen = length - 6;
                id = fullId.Substring(6, idLen);

                if (month != currentMonth)
                {
                    month = currentMonth;

                    id = "1";
                }
                else
                {
                    int intId = Convert.ToInt32(id);
                    intId = intId + 1;

                    id = intId.ToString();
                }
                groupId = year + month + id;
            }

            intGroupId = Convert.ToInt32(groupId);
        }

        private void TxtCurrenctMeterReading_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var curr = Convert.ToDouble( txtCurrenctMeterReading.Text);
            var last = Convert.ToDouble(txtLastMeterReading.Text);

            txtReadingDifference.Text = Math.Round( (curr - last), 2).ToString();
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            if(payee != null)
            {
                AdminBillsRepo billsRepo = new AdminBillsRepo();
                Window win = new Window();
                ucVehicleHistory vehicleHistory = new ucVehicleHistory();
                vehicleHistory.grdVehicleHistory.ItemsSource = billsRepo.GetAllVehicleExpensesByPayee(payee.Id);
                win.Content = vehicleHistory;
                win.ShowDialog();
            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Admin Bill") != null)
            {
                if (grdAttachments.Visibility == Visibility.Visible)
                    grdAttachments.Visibility = Visibility.Collapsed;
                else
                {
                    grdAttachments.Visibility = Visibility.Visible;
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Attached Files!");
            }
        }


        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sSelectedPath = "";

                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);

                        if (str.Contains("VehicleExpenses"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.VehicleExpenses);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else if (str.Contains("Sale_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Offer"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Offer);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Invoice"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Invoice);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Purchase_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Purchase_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Receipt"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Receipt);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Payments"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Payments);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Admin_Bill"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Admin_Bill);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }


                    });
                    thread.Start();
                    //grdProgressBar.Visibility = Visibility.Collapsed;
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

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Admin Bill") != null)
            {
                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    grdAttach1.Visibility = Visibility.Visible;
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveVehicleExpensesAttachmentCategories();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Attach File!");
            }
        }

        UsersRepo UsersRepo = new UsersRepo();
        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if( adminBill.vehicleExpenses != null && adminBill.vehicleExpenses.Count > 0)
                {
                    int groupId = adminBill.vehicleExpenses[0].transactionGroupId;
                    if (groupId != 0)
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
                            destination += "Attachments\\VehicleExpenses\\ToUpload\\";
                            //string destination = @"D:\MovedFiles\new\";
                            System.IO.Directory.CreateDirectory(destination);
                            if (fileDialog.ShowDialog() == true) // Test result.
                            {
                                imgAttachNew1.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                                btnAttachNew1.ToolTip = "Uploading";
                                btnAttachNew1.IsEnabled = true;

                                btnAttachment1.Content = "Uploading File . . .";
                                sourceFile = fileDialog.FileName;
                                destination += groupId + "_" + TransactionItemType.VehicleExpenses.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                                if (sourceFile.Length < 74)
                                {
                                    System.IO.File.Move(sourceFile, destination);

                                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                                    {
                                        ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                        var result = attachment.startUploading(TransactionItemType.VehicleExpenses);
                                        if (result.Item1)
                                        {
                                            AttachmentsRepo repo = new AttachmentsRepo();
                                            //Attachment attachmen= new Attachment();
                                            repo.Add(System.IO.Path.GetFileName(result.Item2), groupId, TransactionItemType.VehicleExpenses, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                            UsersRepo.Add(TransactionInfo.Attachment_Uploaded, groupId, 17, "Added a New attachment");

                                            this.Dispatcher.Invoke(() =>
                                            {
                                                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.VehicleExpenses);
                                                imgAttachNew1.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                                btnAttachNew1.ToolTip = "Attach";
                                                btnAttachNew1.IsEnabled = true;
                                                btnAttachment1.Content = "Select";
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
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            imgAttachNew1.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                            btnAttachNew1.ToolTip = "Attach";
                            btnAttachNew1.IsEnabled = true;
                        }
                        finally
                        {



                        }
                    }
                    else
                        return;
                }

                
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
    }
}
