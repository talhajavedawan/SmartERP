using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
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

namespace ZAS_ERP.Procurementss.Billss.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmVendorBillNatureAdd.xaml
    /// </summary>
    public partial class ucFrmVendorBillNatureAdd : UserControl
    {
        public bool editFlag = false;
        BillRepo billsRepo = new BillRepo();
        VendorBillNature billNature = new VendorBillNature();
        public int billNatureId = 0;
        public ucFrmVendorBillNatureAdd()
        {
            InitializeComponent();

            gridCompanyView.NodeCheckStateChanged += OnCompanyGirdNodeCheckStateChanged;
            grdCntrlCompanies.SelectionChanged += OnCompanyGridSelectionChanged;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //if(lookupCompany.SelectedIndex < 0)
            //{
            //    DXMessageBox.Show("Please select Company!");
            //    lookupCompany.Focus();
            //    return;
            //}
            if (String.IsNullOrEmpty(txtVendorBillNature.Text))
            {
                DXMessageBox.Show("Please Enter Bill Nature!");
                txtVendorBillNature.Focus();
                return;
            }

            if (chkIsActive.IsChecked == true)
                billNature.isActive = true;
            else
                billNature.isActive = false;

            //billNature.companyId = (lookupCompany.SelectedItem as Company).Id; 
            billNature.Nature = txtVendorBillNature.Text;

            if (grdCntrlCompanies.SelectedItems.Count != 0)
            {
                billNature.Companies = new List<Company>();
                foreach (Company _company in grdCntrlCompanies.SelectedItems)
                {
                    if (!billNature.Companies.Contains(_company))
                    {
                        billNature.Companies.Add(_company);
                    }
                }
            }

            //billsRepo = new AdminBillsRepo();
            if (editFlag == false)
            {
                billsRepo.AddVendorBillNature(billNature);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                billsRepo.UpdateVendorBillNature(billNature);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCompanies();

            //lookupCompany.ItemsSource = userCompanies;
            if (editFlag == true && billNatureId != 0)
            {
                billNature = billsRepo.GetVendorBillNature(billNatureId);
                //lookupCompany.Text = billNature.company.CompanyName;
                txtVendorBillNature.Text = billNature.Nature;
                chkIsActive.IsChecked = billNature.isActive;

                foreach (Company _comp in billNature.Companies)
                    grdCntrlCompanies.SelectItem(grdCntrlCompanies.FindRowByValue(grdCntrlCompanies.Columns.GetColumnByFieldName("Id"), _comp.Id));

                //foreach (var _company in billNature.Companies)
                //{
                //    grdCntrlCompanies.SelectItem(grdCntrlCompanies.FindRowByValue(grdCntrlCompanies.Columns.GetColumnByFieldName("Id"), _company.Id));
                //}
            }
        }

        private void LoadCompanies()
        {
            var userCompanies = billsRepo.GetUserCompanies(SYSTEM_STATIC.currentUser.id);
            grdCntrlCompanies.ItemsSource = userCompanies;
        }

        private void OnCompanyGirdNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdCntrlCompanies.SelectItem(e.Node.RowHandle);
            else
                grdCntrlCompanies.UnselectItem(e.Node.RowHandle);
        }

        private void OnCompanyGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = gridCompanyView;
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
                    var selectedRows = grdCntrlCompanies.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }
    }
}
