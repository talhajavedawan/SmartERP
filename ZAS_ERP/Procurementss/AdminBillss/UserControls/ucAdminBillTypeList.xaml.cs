using DevExpress.Xpf.Core;
using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucAdminBillTypeList.xaml
    /// </summary>
    public partial class ucAdminBillTypeList : UserControl
    {
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        public ucAdminBillTypeList()
        {
            InitializeComponent();
        }

        private void MbtnEditAdminBillLink_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Admin Bill Link") != null)
            {
                var selectedRow = grdCntrlAdminBillType.SelectedItem as AdminBillType;

                if (selectedRow != null)
                {
                    ucFrmAdminBillTypeAdd adminBillTypeAdd = new ucFrmAdminBillTypeAdd();
                    adminBillTypeAdd.editFlag = true;
                    adminBillTypeAdd.billType = billsRepo.GetAdminBillType(selectedRow.Id);

                    Window win = new Window();
                    win.Height = 450;
                    win.Width = 550;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //win.ResizeMode = ResizeMode.CanMinimize;

                    win.Content = adminBillTypeAdd;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Edit Admin Bill Link!");
            }
        }

        private void MbtnAddAdminBillLink_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill Link") != null)
            {
                ucFrmAdminBillTypeAdd adminBillTypeAdd = new ucFrmAdminBillTypeAdd();
                adminBillTypeAdd.editFlag = false;

                Window win = new Window();
                win.Height = 450;
                win.Width = 550;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //win.ResizeMode = ResizeMode.CanMinimize;

                win.Content = adminBillTypeAdd;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Bill Link!");
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            billsRepo = new AdminBillsRepo();
            var billTypes = billsRepo.GetAllAdminBillTypes();
            grdCntrlAdminBillType.ItemsSource = billTypes;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billsRepo = new AdminBillsRepo();
            grdCntrlAdminBillType.ItemsSource = billsRepo.GetAllAdminBillTypes();
        }

        private void GrdCntrlAdminBillType_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "vendors" && e.IsGetData)
            {
                
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    if (id > 0)
                    {
                        var type = billsRepo.GetAdminBillType(id);
                        var vendors = type.vendors;
                        string vendorsNames = "";

                        foreach(var _vendor in vendors)
                        {
                            vendorsNames = vendorsNames + _vendor.company.CompanyName+" | ";
                        }
                        
                        e.Value = vendorsNames;
                    }
                }

            if (e.Column.FieldName == "ChartofAccounts" && e.IsGetData)
            {

                var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                if (id > 0)
                {
                    var type = billsRepo.GetAdminBillType(id);
                    var Coas = type.ChartofAccounts;
                    string coaNames = "";

                    foreach (var _coa in Coas)
                    {
                        coaNames = coaNames + _coa.accountName + " | ";
                    }

                    e.Value = coaNames;
                }
            }
        }
        }
    }

