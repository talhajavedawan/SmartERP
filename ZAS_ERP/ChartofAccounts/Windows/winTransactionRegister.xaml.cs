using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
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
using ZAS_ERP.ChartofAccounts.UserControls;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winTransactionRegister.xaml
    /// </summary>
    public partial class winTransactionRegister : DXWindow
    {
        JournalVoucherRepo repo = new JournalVoucherRepo();
        List<cmbitem> TreeItems = new List<cmbitem>();
        ucTransactions uc = new ucTransactions();
        List<JournalVoucher> journalVouchers = new List<JournalVoucher>();

        ChartofAccounts.UserControls.ucTransactions transactionsGrid;
        public winTransactionRegister()
        {
            InitializeComponent();
            this.Activate();
            try
            {
                cmbitem treeItem1 = new cmbitem() { name = "Journal Vouchers" };
                cmbitem treeItema = new cmbitem() { name = "Journal Vouchers(Open)" };
                ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                foreach (JournalVoucherStatus status in repo.GetAllOpenJournalVoucherStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Journal Vouchers" });
                }
                treeItema.Items = cmbItemsa;
                treeItem1.Items.Add(treeItema);
                cmbitem treeItemb = new cmbitem() { name = "Journal Vouchers(Closed)" };

                ICollection<cmbitem> cmbItems = new List<cmbitem>();
                foreach (JournalVoucherStatus status in repo.GetAllCloseJournalVoucherStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Journal Vouchers" });
                }
                treeItemb.Items = cmbItems;
                treeItem1.Items.Add(treeItemb);
                TreeItems.Add(treeItem1);
                treeViewTransactionStatus.ItemsSource = TreeItems;
            }
            catch
            {

            }

        }
        public winTransactionRegister(int Tabindex, TransactionItemType SelectedItem, DataType datatype)
        {
            InitializeComponent();
            SystemLog.LogInfo(this.GetType(), "Form Intialized");
            switch ((SelectedItem))
            {
                
                case (TransactionItemType.JV):
                    {
                        string item = "Journal Vouchers";
                        switch (datatype)
                        {
                            case DataType.All:
                                LoadTransactionGrid(item);
                                break;
                            case DataType.Open:
                                LoadTransactionGrid($"{item}({datatype.ToString()})");

                                break;
                            case DataType.Closed:
                                LoadTransactionGrid($"{item}({datatype.ToString()})");
                                break;
                        }
                    }
                    break;
            }

        }
        private void LoadTransactionGrid(string Name)
        {
            SYSTEM_STATIC.gridTitle = Name;
            switch (Name)
            {
                case "Journal Vouchers":

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "JV") != null)
                    {
                        ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                        ChartofAccounts.UserControls.ucTransactions.AllActive = 0;
                        transactionsGrid = new ChartofAccounts.UserControls.ucTransactions();
                        faRightGrid.Children.Add(transactionsGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Journal Vouchers");
                    }

                    break;
                case "Journal Vouchers(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "JV") != null)
                    {
                        ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                        ChartofAccounts.UserControls.ucTransactions.AllActive = 1;

                        transactionsGrid = new ChartofAccounts.UserControls.ucTransactions();
                        faRightGrid.Children.Add(transactionsGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Journal Vouchers");
                    }
                    break;
                case "Journal Vouchers(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "JV") != null)
                    {
                        ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                        ChartofAccounts.UserControls.ucTransactions.AllActive = 2;

                        transactionsGrid = new ChartofAccounts.UserControls.ucTransactions();
                        faRightGrid.Children.Add(transactionsGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to viewJournal Vouchers");
                    }
                    break;
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

        private void AddTransactionBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void TreeViewtransactionStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = treeViewTransactionStatus.SelectedItem;
            var type = item.GetType();

            if (type.Name == "TreeItem")
            {
                if (faRightGrid != null)
                    faRightGrid.Children.Clear();
                TreeItem treeItem = (TreeItem)treeViewTransactionStatus.SelectedItem;
                SYSTEM_STATIC.gridTitle = treeItem.name;

            }
            else if (type.Name == "cmbitem")
            {
                if (faRightGrid != null)
                    faRightGrid.Children.Clear();
                cmbitem cmbitem = (cmbitem)treeViewTransactionStatus.SelectedItem;
                SYSTEM_STATIC.gridTitle = cmbitem.description + " (" + cmbitem.name + ")";
                if (cmbitem.id == 0)
                {
                    LoadTransactionGrid(cmbitem.name);
                }
                switch (cmbitem.description)
                {
                    case "Journal Vouchers":
                        ChartofAccounts.UserControls.ucTransactions.statusId = cmbitem.id;
                        transactionsGrid = new ChartofAccounts.UserControls.ucTransactions();
                        faRightGrid.Children.Add(transactionsGrid);
                        break;
                  

                }
            }
        }
    }
}
