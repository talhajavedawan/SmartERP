using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAdminBillTypeAdd.xaml
    /// </summary>
    public partial class ucFrmAdminBillTypeAdd : UserControl
    {
        List<Vendor> allVendors = new List<Vendor>();
        List<Vendor> selectedVendors = new List<Vendor>();
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        public AdminBillType billType = new AdminBillType();
        public bool editFlag = false;
        //public bool editFlag = false;
        public ucFrmAdminBillTypeAdd()
        {
            InitializeComponent();

            gridCOAview.NodeCheckStateChanged += OnCOAGirdNodeCheckStateChanged;
            grdCntrlCOA.SelectionChanged += OnCOAGridSelectionChanged;

            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadVendorTypes();
            loadVendors();
            //loadCoaTypes();
            LoadCoa();

            

            if (editFlag == true && billType != null)
            {
                if (billType.name != null)
                    txtName.Text = billType.name;

                if (billType.isActive == true)
                    chkIsActive.IsChecked = true;

                int index = 0;

                //for (int i = 0; i <= (int)ERP_BL.Enums.COA_AccountType.Other_Expense; i++)
                //{
                //    if (billType.COA_AccountType == ((ERP_BL.Enums.COA_AccountType)i))
                //    {
                //        cmbxCoaType.SelectedIndex = i;
                //        break;
                //    }
                //}

                foreach (var _coa in billType.ChartofAccounts)
                {
                    grdCntrlCOA.SelectItem(grdCntrlCOA.FindRowByValue(grdCntrlCOA.Columns.GetColumnByFieldName("Id"), _coa.Id));
                }


                //if (billType.isIndustryType == true)
                //{
                //    chkVendorType.IsChecked = true;
                //    //Select Vendor Type
                //    var vendorTypeList = (cmbxVendorType.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxVendorType.ItemsSource as List<cmbitem>;
                //    if (billType.IndustryType != null)
                //    {
                //        index = 0;
                //        foreach (var _type in vendorTypeList)
                //        {
                //            if (_type.id == billType.IndustryTypeId)
                //            {
                //                cmbxVendorType.SelectedIndex = index;
                //                index = 0;
                //                break;
                //            }
                //            index++;
                //        }
                //    }
                //}


                foreach (var _vendor in billType.vendors)
                {
                    selectedVendors.Add(_vendor);
                }
                foreach (var _selectedVendor in selectedVendors.ToList())
                {
                    if (allVendors.FirstOrDefault(x=>x.Id == _selectedVendor.Id) != null)
                    {
                        allVendors.Remove(allVendors.FirstOrDefault(x => x.Id == _selectedVendor.Id));
                        var findParet = allVendors.Find(x => x.ParentID == _selectedVendor.Id);
                        if (findParet != null)
                        {
                            allVendors.Add(_selectedVendor);
                        }
                    }
                }
                grdVendorsSelected.ItemsSource = null;
                grdVendorsSelected.ItemsSource = selectedVendors;
                grdVendor.ItemsSource = null;
                grdVendor.ItemsSource = allVendors;

            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            
            if (String.IsNullOrEmpty(txtName.Text))
            {
                DXMessageBox.Show("Please Enter name!");
                txtName.Focus();
                return;
            }
            //if(cmbxCoaType.SelectedIndex < 0)
            //{
            //    DXMessageBox.Show("Please select COA Type!");
            //    cmbxCoaType.Focus();
            //    return;
            //}
            if(grdCntrlCOA.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Please select Chart of Accounts!");
                return;
            }
            //if (chkVendorType.IsChecked == true)
            //{
            //    if(cmbxVendorType.SelectedIndex < 0)
            //    {
            //        DXMessageBox.Show("Please select Vendor Type!");
            //        cmbxVendorType.Focus();
            //        return;
            //    }
            //}
            if (selectedVendors.Count == 0)
            {
                DXMessageBox.Show("Please select vendors!");
                return;
            }

            billType.name = txtName.Text;
            if (chkIsActive.IsChecked == true)
                billType.isActive = true;
            else
                billType.isActive = false;

            //billType.COA_AccountType = (ERP_BL.Enums.COA_AccountType)cmbxCoaType.SelectedIndex;

            if (grdCntrlCOA.SelectedItems.Count != 0)
            {
                billType.ChartofAccounts = new List<ChartofAccount>();
                foreach (ChartofAccount _coa in grdCntrlCOA.SelectedItems)
                {
                    if (!billType.ChartofAccounts.Contains(_coa))
                    {
                        billType.ChartofAccounts.Add(_coa);
                    }
                }
            }

            //if(chkVendorType.IsChecked == true)
            //{
            //    billType.isIndustryType = true;
            //    billType.IndustryTypeId = (cmbxVendorType.SelectedItem as cmbitem).id;
            //}
            //else
            //{
            //    billType.isIndustryType = false;
            //    billType.IndustryTypeId = null;
            //}

            if (selectedVendors.Count != 0)
            {
                billType.vendors = new List<Vendor>();
                foreach (var _ven in selectedVendors)
                {
                    if (!billType.vendors.Contains(_ven))
                    {
                        billType.vendors.Add(_ven);
                    }
                }
            }
            

            if (editFlag == true && billType.Id > 0)
            {
                billsRepo.UpdateAdminBillType(billType);
                DXMessageBox.Show("Updated Successfully!");
            }
            else if(editFlag == false && billType.Id == 0)
            {
                billType.user_Id = SYSTEM_STATIC.currentUser.id;
                billsRepo.AddAdminBillType(billType);
                DXMessageBox.Show("Added Successfully!");
            }
            Window win = Window.GetWindow(this);
            win.Close();

        }

        private void btnRightMoveVendor_Click(object sender, RoutedEventArgs e)
        {
            if (grdVendor.SelectedItem != null)
            {
                var vendor = grdVendor.SelectedItem as Vendor;
                if (allVendors.Find(x => x.ParentID == vendor.Id) == null)
                {

                    allVendors.Remove(vendor);
                    if (!selectedVendors.Contains(vendor))
                        selectedVendors.Add(vendor);

                    var parent = vendor.ParentVendor;
                    while (parent != null)
                    {
                        if (allVendors.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedVendors.Contains(parent))
                            {
                                selectedVendors.Add(parent);
                            }
                            var findParet = allVendors.Find(x => x.ParentVendor == parent);
                            if (findParet == null)
                            {
                                allVendors.Remove(parent);
                            }
                        }
                        parent = parent.ParentVendor;
                    }
                    grdVendor.ItemsSource = null;
                    grdVendorsSelected.ItemsSource = null;
                    grdVendor.ItemsSource = allVendors;
                    grdVendorsSelected.ItemsSource = selectedVendors;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Vendor which you want to Insert to Selected Vendor!");
            }
        }
        private void btnLeftMoveVendor_Click(object sender, RoutedEventArgs e)
        {
            if (grdVendorsSelected.SelectedItem != null)
            {
                var vendor = grdVendorsSelected.SelectedItem as Vendor;
                if (selectedVendors.Find(x => x.ParentID == vendor.Id) == null)
                {
                    selectedVendors.Remove(vendor);
                    if (!allVendors.Contains(vendor))
                        allVendors.Add(vendor);
                    var parent = vendor.ParentVendor;
                    while (parent != null)
                    {
                        if (selectedVendors.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!allVendors.Contains(parent))
                                allVendors.Add(parent);
                            var findParet = selectedVendors.Find(x => x.ParentVendor == parent);
                            if (findParet == null)
                            {
                                selectedVendors.Remove(parent);
                            }
                        }
                        parent = parent.ParentVendor;
                    }
                    grdVendor.ItemsSource = null;
                    grdVendorsSelected.ItemsSource = null;
                    grdVendor.ItemsSource = allVendors;
                    grdVendorsSelected.ItemsSource = selectedVendors;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Vendor which you want to Remove from Selected Vendor !");
            }
        }

        private void OnCOAGirdNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdCntrlCOA.SelectItem(e.Node.RowHandle);
            else
                grdCntrlCOA.UnselectItem(e.Node.RowHandle);
        }

        private void OnCOAGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = gridCOAview;
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
                    var selectedRows = grdCntrlCOA.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }

        public void loadVendors()
        {
            VendorRepo vendorRepo = new VendorRepo();
            var vendors = vendorRepo.getAll();
            allVendors = vendors;
        }

        public void loadVendorTypes()
        {
            //List<cmbitem> cmbitems = new List<cmbitem>();
            //List<IndustryType> industryTypes = new List<IndustryType>();
            //CompanyRepo companyRepo = new CompanyRepo();
            //industryTypes = companyRepo.GetIndustryTypes();

            //foreach (IndustryType industry in industryTypes)
            //{
            //    cmbitems.Add(new cmbitem() { name = industry.name, id = industry.Id });
            //}
            ////cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            //cmbxVendorType.ItemsSource = cmbitems;
        }

        public void LoadCoa()
        {
            ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
            var chartofAccounts = coaRepo.getAll().Where(x=>x.isActive == true && x.isApproved == true);
            grdCntrlCOA.ItemsSource = chartofAccounts;
        }

        public void loadCoaTypes()
        {
            //for (int i = 0; i <= (int)ERP_BL.Enums.COA_AccountType.Other_Expense; i++)
            //{
            //    cmbxCoaType.Items.Add(((ERP_BL.Enums.COA_AccountType)i).ToString());
            //}
        }

       
        private void CmbIndustry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var cmbitem = cmbxVendorType.SelectedItem as cmbitem;

            //if(cmbitem != null)
            //{
            //    VendorRepo vendorRepo = new VendorRepo();
            //    var vendors =  vendorRepo.getAllByIndustryType(cmbitem.id);
            //    grdCntrlVendors.ItemsSource = vendors;
            //}
            
        }

        private void CmbxCoaType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
            //try
            //{
            //    var selectedType = ((ERP_BL.Enums.COA_AccountType)cmbxCoaType.SelectedIndex);
            //    var chartofAccounts = coaRepo.getChartofAccountsByType(SystemLogic.currentUser.id, selectedType);
            //    if (chartofAccounts.Count != 0)
            //    {
            //        grdCntrlCOA.ItemsSource = chartofAccounts;
            //    }
            //    else
            //        grdCntrlCOA.ItemsSource = null;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void ChkVendorType_Checked(object sender, RoutedEventArgs e)
        {
            //cmbxVendorType.IsEnabled = true;
            

            //if(cmbxVendorType.SelectedIndex > -1)
            //{
            //    var cmbitem = cmbxVendorType.SelectedItem as cmbitem;

            //    if (cmbitem != null)
            //    {
            //        VendorRepo vendorRepo = new VendorRepo();
            //        var vendors = vendorRepo.getAllByIndustryType(cmbitem.id);
            //        if(vendors.Count > 0)
            //            grdCntrlVendors.ItemsSource = vendors;
            //        else
            //            grdCntrlVendors.ItemsSource = null;
            //    }
            //}
            //else
            //{
            //    grdCntrlVendors.ItemsSource = null;
            //}
        }

        private void ChkVendorType_Unchecked(object sender, RoutedEventArgs e)
        {
            //cmbxVendorType.IsEnabled = false;
            //VendorRepo vendorRepo = new VendorRepo();
            //var vendors = vendorRepo.getAll();
            //grdCntrlVendors.ItemsSource = vendors;
        }
    }
}
