using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Procurements.StatusClass;
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

namespace ZAS_ERP.Procurementss.StatusClasses.Windows
{
    /// <summary>
    /// Interaction logic for winStatusClassAdd.xaml
    /// </summary>
    public partial class winStatusClassAdd : DXWindow
    {
        ProcurementRepo procurementRepo = new ProcurementRepo();
        StatusClass statusClass = new StatusClass();

        public winStatusClassAdd()
        {
            InitializeComponent();
        }  
        public winStatusClassAdd(StatusClass _StatusClass)
        {
            InitializeComponent();
            statusClass = _StatusClass;
        }
        public void LoadTransactionTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.TransactionItemType.InventoryAdjustment; i++)
            {
                if (((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Inquiry" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Sale_Order")
                {
                    gridTransactionType.Items.Add(((ERP_BL.Enums.ReportTransactionType)i).ToString());
                }
                else
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null && ((ERP_BL.Enums.ReportTransactionType)i).ToString() == "Offer")
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
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTransactionTypes();
            if (statusClass.Id != 0)
            {
                statusClass = procurementRepo.GetStatusClass(statusClass.Id);
                txtStatus.Text = statusClass.ClassName;
                chkisactive.IsChecked = statusClass.isActive;
                chkDisable.IsChecked = statusClass.isDisable;
                gridTransactionType.Text = statusClass.transactionType.ToString();
            }

        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            var transactionType = gridTransactionType.SelectedItem.ToString();

            if (statusClass.Id != 0)
            {
                statusClass.ClassName = txtStatus.Text;
                if (chkisactive.IsChecked == true)
                {
                    statusClass.isActive = true;
                }
                else
                {
                    statusClass.isActive = false;

                }
                if (chkDisable.IsChecked == true)
                {
                    statusClass.isDisable = true;
                }
                else
                {
                    statusClass.isDisable = false;
                }
                statusClass.backcolor = cpStatus.Text.Trim();

                switch (transactionType)
                {
                    case "Inquiry":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Inquiry;
                            break;
                        }
                    case "Offer":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Offer;
                            break;
                        }
                    case "Sale_Order":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Sale_Order;
                            break;
                        }
                    case "Sale_Invoice":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Sale_Invoice;
                            break;
                        }
                    case "Purchase_Order":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Purchase_Order;
                            break;
                        }
                    case "Purchase_Invoice":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Purchase_Invoice;
                            break;
                        }
                    case "Bill":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Bill;
                            break;
                        }
                    case "Sale_Receipt":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Sale_Receipt;
                            break;
                        }
                    case "InterBank_Transfer":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.InterBank_Transfer;
                            break;
                        }
                    case "Admin_Bill":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Admin_Bill;
                            break;
                        }
                    case "InterCompanyBank_Transfer":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.InterCompanyBank_Transfer;
                            break;
                        }
                    case "Payments":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Payments;
                            break;
                        }


                    case "TargetReward":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.TargetReward;
                            break;
                        }
                    case "Tasks":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Tasks;
                            break;
                        }
                    //case "Chart_of_Account":
                    //    {
                    //        statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Chart_of_Account;
                    //        break;
                    //    }
                    case "STL":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.STL;
                            break;
                        }
                    case "TravelingRecord":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.TravelingRecord;
                            break;
                        }
                }
                procurementRepo.UpdateStatusClass(statusClass);
                MessageBox.Show("Status class (" + txtStatus.Text + ") updated", "Congratulations");
                this.Close();
            }
            else
            {
                statusClass.ClassName = txtStatus.Text;
                if (chkisactive.IsChecked == true)
                {
                    statusClass.isActive = true;
                }
                else
                {
                    statusClass.isActive = false;

                }
                if (chkDisable.IsChecked == true)
                {
                    statusClass.isDisable = true;
                }
                else
                {
                    statusClass.isDisable = false;
                }
                statusClass.backcolor = cpStatus.Text.Trim();

                switch (transactionType)
                {
                    case "Inquiry":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Inquiry;
                            break;
                        }
                    case "Offer":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Offer;
                            break;
                        }
                    case "Sale_Order":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Sale_Order;
                            break;
                        }
                    case "Sale_Invoice":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Sale_Invoice;
                            break;
                        }
                    case "Purchase_Order":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Purchase_Order;
                            break;
                        }
                    case "Purchase_Invoice":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Purchase_Invoice;
                            break;
                        }
                    case "Bill":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Bill;
                            break;
                        }
                    case "Sale_Receipt":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Sale_Receipt;
                            break;
                        }
                    case "InterBank_Transfer":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.InterBank_Transfer;
                            break;
                        }
                    case "Admin_Bill":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Admin_Bill;
                            break;
                        }
                    case "InterCompanyBank_Transfer":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.InterCompanyBank_Transfer;
                            break;
                        }
                    case "Payments":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Payments;
                            break;
                        }
                 
                   
                    case "TargetReward":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.TargetReward;
                            break;
                        } 
                    case "Tasks":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Tasks;
                            break;
                        }
                    //case "Chart_of_Account":
                    //    {
                    //        statusClass.transactionType = ERP_BL.Enums.TransactionItemType.Chart_of_Account;
                    //        break;
                    //    }
                    case "STL":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.STL;
                            break;
                        }
                    case "TravelingRecord":
                        {
                            statusClass.transactionType = ERP_BL.Enums.TransactionItemType.TravelingRecord;
                            break;
                        }

                }
                procurementRepo.AddStatusClass(statusClass);
                MessageBox.Show("New Status Class (" + txtStatus.Text + ") Added", "Congratulations");
                this.Close();
            }
        }
        private void ColorEdit_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (cpStatus.Color.R <= 120 || cpStatus.Color.G <= 120 || cpStatus.Color.B <= 120)
            {
                var myColor = "#FFFFFFFF";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                txtStatus.Foreground = new SolidColorBrush(newColor);
                statusClass.forecolor = myColor;

            }
            else
            {
                var myColor = "#FF000000";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                txtStatus.Foreground = new SolidColorBrush(newColor);
                statusClass.forecolor = myColor;
            }
            txtStatus.Background = new SolidColorBrush(cpStatus.Color);
        }
    }
}
