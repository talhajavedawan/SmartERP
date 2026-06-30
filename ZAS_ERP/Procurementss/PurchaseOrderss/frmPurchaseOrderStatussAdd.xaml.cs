using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP.Procurementss.PurchaseOrderss
{
    /// <summary>
    /// Interaction logic for frmPurchaseOrderStatusAdd.xaml
    /// </summary>
    public partial class frmPurchaseOrderStatussAdd : DXWindow
    {
        public frmPurchaseOrderStatussAdd()
        {
            InitializeComponent();
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdStatusClasses.SelectionChanged += OnGridSelectionChanged;
        }
        public static int StatusId;
        PurchaseOrderRepo repo = new PurchaseOrderRepo();
        PurchaseOrderStatus status = new PurchaseOrderStatus();
        private void OnNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdStatusClasses.SelectItem(e.Node.RowHandle);
            else
                grdStatusClasses.UnselectItem(e.Node.RowHandle);
        }
        private void OnGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)grdStatusClasses.View;
            var node = view.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (node != null)
                        node.IsChecked = true;
                    break;
                case CollectionChangeAction.Remove:
                    if (node != null)
                        node.IsChecked = false;
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = grdStatusClasses.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }
        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                status.Status = txtStatus.Text.Trim();
                if (chkisactive.IsChecked == true)
                    status.isActive = true;
                else
                    status.isActive = false;
                if (chkDisable.IsChecked == true)
                    status.isDisable = true;
                else
                    status.isDisable = false;
                if (grdStatusClasses.SelectedItems.Count != 0)
                {
                    status.poStatusSubClasses = new List<ERP_BL.Procurements.StatusClass.StatusClass>();
                    foreach (ERP_BL.Procurements.StatusClass.StatusClass subClass in grdStatusClasses.SelectedItems)
                    {
                        if (!status.poStatusSubClasses.Contains(subClass))
                        {
                            status.poStatusSubClasses.Add(subClass);
                        }
                    }
                }
                //repo.addStatus(status);
                if (status.Id == 0)
            {
                    //if (MainWindow.currentUserid != 0)
                    //    PurchaseOrderStatus.user_Id = MainWindow.currentUserid;
                    //else
                    //    PurchaseOrderStatus.user_Id = null;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order Status") != null)
                    {
                        repo.addStatus(status);
                        SystemLog.LogInfo(this.GetType(), "New PurchaseOrder Payment Status(" + txtStatus.Text + ") Added");
                        MessageBox.Show("New PurchaseOrder Payment Status (" + txtStatus.Text + ") Added", "Congratulations");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new PurchaseOrder status.");
            }
            else
            {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Order Status") != null)
                    {
                        repo.updateStatus(status);

                        MessageBox.Show("PurchaseOrder Payment Status (" + txtStatus.Text + ") updated", "Congratulations");
                        SystemLog.LogInfo(this.GetType(), " PurchaseOrder Payment Status(" + txtStatus.Text + ") updated");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new PurchaseOrder status.");
                //    repo.updateStatus(PurchaseOrderStatus);

                //MessageBox.Show("PurchaseOrder Payment Status (" + txtStatus.Text + ") updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                SystemLog.LogError(this.GetType(), " PurchaseOrder Status error" + ex.ToString());

                MessageBox.Show(ex.ToString());
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
                status.forecolor = myColor;

            }
            else
            {
                var myColor = "#FF000000";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                txtStatus.Foreground = new SolidColorBrush(newColor);
                status.forecolor = myColor;
            }
            txtStatus.Background = new SolidColorBrush(cpStatus.Color);
        }
        private void winPurchaseOrderStatusAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StatusId = 0;
        }

        private void winPurchaseOrderStatusAdd_Loaded(object sender, RoutedEventArgs e)
        {
            loadStatusClasses();
            if (StatusId != 0)
            {
                status = repo.getstatus(StatusId);
                txtStatus.Text = status.Status;
                chkisactive.IsChecked = status.isActive;
                chkDisable.IsChecked = status.isDisable;
                foreach (ERP_BL.Procurements.StatusClass.StatusClass subClass in status.poStatusSubClasses)
                    grdStatusClasses.SelectItem(grdStatusClasses.FindRow(subClass));
            }
        }
        public void loadStatusClasses()
        {
            grdStatusClasses.ItemsSource = repo.GetActiveStatusClasses();
        }
    }
}
