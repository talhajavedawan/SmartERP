using DevExpress.Xpf.Core;
using ERP_BL.Procurements.LoansAdvances;
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
using ZAS_ERP.Procurementss.Advances.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;

namespace ZAS_ERP.Procurementss.LoanAdvance.Windows
{
    /// <summary>
    /// Interaction logic for LoansAdvanceRegister.xaml
    /// </summary>
    public partial class LoansAdvanceRegister : Window
    {
        ucFrmLoansAdvances frmLoansAdvanceAdd = new ucFrmLoansAdvances();

        List<LoansAdvance> loansAdvanceList = new List<LoansAdvance>();
        List<cmbitem> TreeItems = new List<cmbitem>();
        AdvanceRepo loansAdvanceRepo = new AdvanceRepo();

        ucAdvancesList loansAdvanceRegister = new ucAdvancesList();
        public string transctions;
        public LoansAdvanceRegister()
        {
            InitializeComponent();
            try
            {

                loansAdvanceList = new List<LoansAdvance>();
                //paymentList = paymentsRepo.GetAllOpenPayments(SYSTEM_STATIC.currentUser.id);
                faRightGrid.Children.Add(loansAdvanceRegister);

                //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Payments") != null)
                //{
                //    paymentRegister.grdPaymentRegister.ItemsSource = paymentList;
                //}

                cmbitem treeItem1 = new cmbitem() { name = "Loans and Advances" };
                cmbitem treeItema = new cmbitem() { name = "Loans and Advances(Open)" };
                ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                foreach (LoansAdvanceStatus status in loansAdvanceRepo.GetAllOpenStatus())
                {
                    cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Payments" });
                }
                treeItema.Items = cmbItemsa;
                treeItem1.Items.Add(treeItema);
                cmbitem treeItemb = new cmbitem() { name = "Loans and Advances(Closed)" };

                ICollection<cmbitem> cmbItems = new List<cmbitem>();
                foreach (LoansAdvanceStatus status in loansAdvanceRepo.GetAllClosedStatus())
                {
                    cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Loans and Advances" });
                }
                treeItemb.Items = cmbItems;
                treeItem1.Items.Add(treeItemb);

                //}


                TreeItems.Add(treeItem1);
                treeViewLoansAdvanceStatus.ItemsSource = TreeItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddLoansAdvanceBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances") != null)
            {
                ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                Window win = new Window();
                win.Content = frmLoansAdvances;
                win.WindowState = WindowState.Maximized;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void GrdCollapse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Collapsed;
            GridSplitter.Visibility = Visibility.Collapsed;
            faRightGrid.SetValue(Grid.ColumnProperty, 0);

            faRightGrid.SetValue(Grid.ColumnSpanProperty, 3);
        }

        private void GrdExpand_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Visible;
            GridSplitter.Visibility = Visibility.Visible;

            faRightGrid.SetValue(Grid.ColumnProperty, 2);
            faRightGrid.SetValue(Grid.ColumnSpanProperty, 1);
        }

        private void TreeViewLoansAdvanceStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            loansAdvanceList = new List<LoansAdvance>();
            loansAdvanceRepo = new AdvanceRepo();
            var item = (cmbitem)treeViewLoansAdvanceStatus.SelectedItem;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
            {
                if (item.name == "Loans and Advances")
                {
                    loansAdvanceList = loansAdvanceRepo.GetAllLoansAdvances(MainWindow.currentUserid);
                }
                else if (item.name == "Loans and Advances(Open)")
                {
                    loansAdvanceList = loansAdvanceRepo.GetAllActiveLoansAdvances(MainWindow.currentUserid);
                }
                else if (item.name == "Loans and Advances(Closed)")
                {
                    loansAdvanceList = loansAdvanceRepo.GetAllClosedLoansAdvances(MainWindow.currentUserid);
                }
                else
                {
                    loansAdvanceList = loansAdvanceRepo.GetAllLoansAdvancesByStatusId(MainWindow.currentUserid, item.id);
                }


                loansAdvanceRegister.grdLoansAdvancesRegister.ItemsSource = loansAdvanceList;
                loansAdvanceRegister.GridControlSetUserSettings();
                //bankTransferRegister.SetColumnsVisibility();
                loansAdvanceRegister.lblHeading.Text = item.name;
            }
            else
            {
                loansAdvanceRegister.grdLoansAdvancesRegister.ItemsSource = null;
                loansAdvanceRegister.lblHeading.Text = item.name;
            }
        }
    }
}
