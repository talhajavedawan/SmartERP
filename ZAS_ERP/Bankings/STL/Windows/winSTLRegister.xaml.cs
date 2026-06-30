using DevExpress.Xpf.Core;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.InterBankTransfers;
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
using ZAS_ERP.Bankings.STL.UserControls;
using ZAS_ERP.Payments.UserControls;
using ZAS_ERP.Payments.Windows;

namespace ZAS_ERP.Bankings.STL.Windows
{
    /// <summary>
    /// Interaction logic for winSTLRegister.xaml
    /// </summary>
    public partial class winSTLRegister : DXWindow
    {
        winSTLAdd frmSTLAdd = new winSTLAdd();

        List<ERP_BL.Procurements.InterBankTransfers.STL> stlList = new List<ERP_BL.Procurements.InterBankTransfers.STL>();
        List<cmbitem> TreeItems = new List<cmbitem>();
        STLRepo stlRepo = new STLRepo();

        ucSTLList stlRegister = new ucSTLList();
        public string transctions;
        ZAS_ERP.Bankings.STL.UserControls.ucSTLList ucSTLList = new ucSTLList();
        public winSTLRegister(int Tabindex, TransactionItemType SelectedItem, DataType datatype)
        {
            InitializeComponent();
            stlList = new List<ERP_BL.Procurements.InterBankTransfers.STL>();
            //faRightGrid.Children.Add(stlRegister);
            cmbitem treeItem1 = new cmbitem() { name = "STL" };
            cmbitem treeItema = new cmbitem() { name = "STL(Open)" };
            ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
            foreach (STLStatus status in stlRepo.GetAllOpenSTLStatus().Where(x => x.isActive == true).ToList())
            {
                cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Payments" });
            }
            treeItema.Items = cmbItemsa;
            treeItem1.Items.Add(treeItema);
            cmbitem treeItemb = new cmbitem() { name = "STL(Closed)" };

            ICollection<cmbitem> cmbItems = new List<cmbitem>();
            foreach (STLStatus status in stlRepo.GetAllCloseSTLStatus().Where(x => x.isActive == false).ToList())
            {
                cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Payments" });
            }
            treeItemb.Items = cmbItems;
            treeItem1.Items.Add(treeItemb);

            //}


            TreeItems.Add(treeItem1);
            treeViewSTLStatus.ItemsSource = TreeItems;

            switch (SelectedItem)
            {
                case TransactionItemType.STL:
                    {
                        var item = "STL";
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
                            case DataType.PendingForApproval:
                                LoadTransactionGrid($"{item}({datatype.ToString()})");
                                break;
                            case DataType.PendingForReApproval:
                                LoadTransactionGrid($"{item}({datatype.ToString()})");
                                break;
                            case DataType.PendingForClosing:
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
                case "STL":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLList.statusId = 0;
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLList.AllActive = 0;
                            ucSTLList = new ZAS_ERP.Bankings.STL.UserControls.ucSTLList();

                            faRightGrid.Children.Add(ucSTLList);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission STL");
                        }
                        break;
                    }

                case "STL(Open)":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
                        {
                          
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLList.statusId = 0;
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLList.AllActive = 1;
                            ucSTLList = new ZAS_ERP.Bankings.STL.UserControls.ucSTLList();
                            faRightGrid.Children.Add(ucSTLList);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission STL");
                        }
                        break;
                    }
                case "STL(Closed)":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLList.statusId = 0;
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLList.AllActive = 2;
                            ucSTLList = new ZAS_ERP.Bankings.STL.UserControls.ucSTLList();
                            faRightGrid.Children.Add(ucSTLList);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission STL");
                        }
                        break;
                    }
                
            }
        }      
        public winSTLRegister()
        {
            InitializeComponent();
            stlList = new List<ERP_BL.Procurements.InterBankTransfers.STL>();
            //faRightGrid.Children.Add(stlRegister);
            cmbitem treeItem1 = new cmbitem() { name = "STL" };
            cmbitem treeItema = new cmbitem() { name = "STL(Open)" };
            ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
            foreach (STLStatus status in stlRepo.GetAllOpenSTLStatus().Where(x => x.isActive == true).ToList())
            {
                cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Payments" });
            }
            treeItema.Items = cmbItemsa;
            treeItem1.Items.Add(treeItema);
            cmbitem treeItemb = new cmbitem() { name = "STL(Closed)" };

            ICollection<cmbitem> cmbItems = new List<cmbitem>();
            foreach (STLStatus status in stlRepo.GetAllCloseSTLStatus().Where(x => x.isActive == false).ToList())
            {
                cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Payments" });
            }
            treeItemb.Items = cmbItems;
            treeItem1.Items.Add(treeItemb);

            //}


            TreeItems.Add(treeItem1);
            treeViewSTLStatus.ItemsSource = TreeItems;

        }
           

        private void treeViewSTLStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            stlList = new List<ERP_BL.Procurements.InterBankTransfers.STL>();
            stlRepo = new STLRepo();
            var item = (cmbitem)treeViewSTLStatus.SelectedItem;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of STL") != null)
            {
                if (item.name == "STL")
                {
                    stlList = stlRepo.GetAllTransactionsOpenAndClosed(MainWindow.currentUserid);
                }
                else if (item.name == "STL(Open)")
                {
                    stlList = stlRepo.getAllActiveandUnapprovedTransactions(MainWindow.currentUserid);
                }
                else if (item.name == "STL(Closed)")
                {
                    stlList = stlRepo.getAllInActiveandUnapprovedReceipts(MainWindow.currentUserid);
                }
                else
                {
                    stlList = stlRepo.getAllSaleReceiptsbyStatusId(MainWindow.currentUserid, item.id);
                }


                stlRegister.grdSTLRegister.ItemsSource = stlList;
                //stlRegister.GridControlSetUserSettings();
                stlRegister.lblHeading.Text = item.name;
            }
            else
            {
                stlRegister.grdSTLRegister.ItemsSource = null;
                stlRegister.lblHeading.Text = item.name;
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

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public void GridControlSetUserSettings()
        {
            //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPaymentRegister);
        }
    }
}
