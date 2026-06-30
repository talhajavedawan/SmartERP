using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
    /// Interaction logic for winStatusClassList.xaml
    /// </summary>
    public partial class winStatusClassList : DXWindow
    {
        public winStatusClassList()
        {
            InitializeComponent();
        }
        private void grdStatusClass_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdStatusClass.SelectedItem != null)
            {

                var selectedClasss = grdStatusClass.SelectedItem as StatusClass;
                winStatusClassAdd winStatusClassEdit = new winStatusClassAdd(selectedClasss);
                winStatusClassEdit.Show();
            }
        }
        private void btnNewStatusClass_Click(object sender, RoutedEventArgs e)
        {
            winStatusClassAdd winStatusClassAdd = new winStatusClassAdd();
            winStatusClassAdd.Show();
        }

        private void btnEditStatusClass_Click(object sender, RoutedEventArgs e)
        {
            if (grdStatusClass.SelectedItem != null)
            {

                var selectedClasss = grdStatusClass.SelectedItem as StatusClass;
                winStatusClassAdd winStatusClassEdit = new winStatusClassAdd(selectedClasss);
                winStatusClassEdit.Show();
            }
        }

        private void mbtnNewStatusClass_Click(object sender, RoutedEventArgs e)
        {
            winStatusClassAdd winStatusClassAdd = new winStatusClassAdd();
            winStatusClassAdd.Show();
        }

        private void mbtnEditStatusClass_Click(object sender, RoutedEventArgs e)
        {
            if (grdStatusClass.SelectedItem != null)
            {
                var selectedClasss = grdStatusClass.SelectedItem as StatusClass;
                winStatusClassAdd winStatusClassEdit = new winStatusClassAdd(selectedClasss);
                winStatusClassEdit.Show();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProcurementRepo repo = new ProcurementRepo();
            grdStatusClass.ItemsSource= repo.GetActiveStatusClasses();
            //grdStatusClass.ItemsSource= repo.GetActiveStatusClasses();
        }

        private void grdStatusClass_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            if (grdStatusClass.SelectedItem != null)
            {
                var statusClass = grdStatusClass.SelectedItem as StatusClass;

                switch(statusClass.transactionType)
                {
                    case TransactionItemType.Inquiry:
                        {
                            grdStatuses.ItemsSource = statusClass.inquiryStatuses;
                            break;
                        }
                    case TransactionItemType.Offer:
                        {
                            grdStatuses.ItemsSource = statusClass.offerStatuses;

                            break; 
                        }
                    case TransactionItemType.Sale_Order:
                        {
                            grdStatuses.ItemsSource = statusClass.soStatuses;

                            break;
                        }
                    case TransactionItemType.Purchase_Order:
                        {
                            grdStatuses.ItemsSource = statusClass.poStatuses;

                            break;
                        }
                    case TransactionItemType.Sale_Invoice:
                        {
                            grdStatuses.ItemsSource = statusClass.siStatuses;

                            break;
                        }
                    case TransactionItemType.Sale_Receipt:
                        {
                            grdStatuses.ItemsSource = statusClass.srStatuses;

                            break;
                        }
                    case TransactionItemType.Purchase_Invoice:
                        {
                            grdStatuses.ItemsSource = statusClass.piStatuses;

                            break;
                        }
                    case TransactionItemType.PurchaseInvoice:
                        {
                            grdStatuses.ItemsSource = statusClass.piStatuses;

                            break;
                        }
                    case TransactionItemType.Payments:
                        {
                            grdStatuses.ItemsSource = statusClass.paymentStatuses;

                            break;
                        }
                    case TransactionItemType.InterBank_Transfer:
                        {
                            grdStatuses.ItemsSource = statusClass.ibtStatuses;

                            break;
                        }
                    case TransactionItemType.InterCompanyBank_Transfer:
                        {
                            grdStatuses.ItemsSource = statusClass.ibtStatuses;

                            break;
                        }
                    case TransactionItemType.Bill:
                        {
                            grdStatuses.ItemsSource = statusClass.billStatuses;

                            break;
                        }
                    case TransactionItemType.Admin_Bill:
                        {
                            grdStatuses.ItemsSource = statusClass.adminBillStatuses;

                            break;
                        }
                    case TransactionItemType.Tasks:
                        {
                            grdStatuses.ItemsSource = statusClass.taskStatuses;

                            break;
                        }
                    case TransactionItemType.ToDo_Task:
                        {
                            grdStatuses.ItemsSource = statusClass.todoTaskStatuses;

                            break;
                        }
                    case TransactionItemType.TargetReward:
                        {
                            grdStatuses.ItemsSource = statusClass.targetRewardStatuses;

                            break;
                        }
                    case TransactionItemType.LoansAdvances:
                        {
                            grdStatuses.ItemsSource = statusClass.loanStatuses;

                            break;
                        }
                    case TransactionItemType.Loans:
                        {
                            grdStatuses.ItemsSource = statusClass.loanStatuses;

                            break;
                        }
                    case TransactionItemType.STL:
                        {
                            //grdStatuses.ItemsSource = statusClass.st;

                            break;
                        }
                    case TransactionItemType.JV:
                        {
                            //grdStatuses.ItemsSource = statusClass.offerStatuses;

                            break;
                        }
                    case TransactionItemType.TravelingRecord:
                        {
                            //grdStatuses.ItemsSource = statusClass.tra;

                            break;
                        }
                }
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ProcurementRepo repo = new ProcurementRepo();
            grdStatusClass.ItemsSource = repo.GetActiveStatusClasses();
        }
    }
}
