using ERP_BL.Databases;
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

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucSaleReceiptsStatusList.xaml
    /// </summary>
    public partial class ucSaleReceiptsStatusList : UserControl
    {
        SalesReceiptRepo repo = new SalesReceiptRepo();
        SalesReceiptStatus status = new SalesReceiptStatus();
        List<SalesReceiptStatus> statuss = new List<SalesReceiptStatus>();

        public ucSaleReceiptsStatusList()
        {
            InitializeComponent();
        }

        private void BtnNewSaleReceiptStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt Status") != null)
                {
                    ucFrmAddSalesReceiptStatus frm = new ucFrmAddSalesReceiptStatus();

                    frm.saveEditFlag = false;
                    frm.win.Content = frm;
                    frm.win.Width = 400;
                    frm.win.Height = 250;
                    frm.win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    frm.win.ResizeMode = ResizeMode.CanMinimize;
                    frm.win.ShowDialog();
                    Load_Statuses();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Sale Receipt Status!");
                    return;
                }
            }
            catch
            {

            }
        }

        private void BtnEditSaleReceiptStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Receipt Status") != null)
                {
                    ucFrmAddSalesReceiptStatus frm = new ucFrmAddSalesReceiptStatus();

                    var selectedRow = (StatusClass)grdStatus.SelectedItem;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.Color);

                        frm.saveEditFlag = true;
                        frm.statusId = selectedRow.ID;
                        frm.txtStatus.Text = selectedRow.Status;

                        if (selectedRow.IsActive == true)
                            frm.chkisActive.IsChecked = true;
                        else
                            frm.chkisActive.IsChecked = false;

                        frm.cpStatus.Color = (Color)color;

                        frm.win.Content = frm;
                        frm.win.Width = 400;
                        frm.win.Height = 250;
                        frm.win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frm.win.ResizeMode = ResizeMode.CanMinimize;
                        frm.win.ShowDialog();
                        Load_Statuses();
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Sale Receipt Status!");
                    return;
                }
            }
            catch
            {

            }            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Statuses();
            //statuss = repo.GetAllSaleReceiptStatus();
            //grdStatus.ItemsSource = statuss;
        }

        private void Load_Statuses()
        {
            try
            {
                GetAllStatuses obj = new GetAllStatuses();
                grdStatus.ItemsSource = obj.StatusList;
            }
            catch
            {

            }
            
        }

        private void GrdStatus_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }
    }


    public class StatusClass
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string Color { get; set; }
    }

    public class GetAllStatuses
    {
        public List<StatusClass> StatusList = new List<StatusClass>();
        List<StatusClass> statusList = new List<StatusClass>();

        public GetAllStatuses()
        {
            try
            {
                SalesReceiptRepo repo = new SalesReceiptRepo();
                var allStatuses = repo.GetAllSaleReceiptStatus();

                foreach (var _status in allStatuses)
                {
                    StatusClass status = new StatusClass();

                    status.ID = _status.Id;
                    status.Status = _status.Status;
                    status.IsActive = (bool)_status.isActive;
                    status.Color = _status.backcolor;
                    //if(_collectionMethod.isActive == true)
                    //{
                    //    method.IsActive.IsChecked =true;
                    //}
                    //else
                    //{
                    //    method.IsActive.IsChecked = false;
                    //}

                    statusList.Add(status);
                }
                StatusList = statusList;
            }
            catch
            {

            }
           
        }
    }
}
