using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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
    /// Interaction logic for ucFrmPayeeName.xaml
    /// </summary>
    public partial class ucFrmPayeeName : UserControl
    {
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        public Payee payee = new Payee();
        public bool editFlag = false;
        public Window addPayeeWindow = new Window();
        List<Vendor> selectedVendors = new List<Vendor>();
        public ucFrmPayeeName()
        {
            InitializeComponent();

            gridCompanyView.NodeCheckStateChanged += OnCompanyGirdNodeCheckStateChanged;
            grdCntrlCompanies.SelectionChanged += OnCompanyGridSelectionChanged;

            griddeptview.NodeCheckStateChanged += OndeptgirdNodeCheckStateChanged;
            gridDepartment.SelectionChanged += OndeptGridSelectionChanged;

            gridVendorView.NodeCheckStateChanged += OnVendorGirdNodeCheckStateChanged;
            grdCntrlVendors.SelectionChanged += OnVendorGridSelectionChanged;

            gridAdminBillTypeView.NodeCheckStateChanged += OnAdminBillTypeGirdNodeCheckStateChanged;
            grdCntrlAdminBillTypes.SelectionChanged += OnAdminBillTypeGridSelectionChanged;
        }

        private void OndeptgirdNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                gridDepartment.SelectItem(e.Node.RowHandle);
            else
                gridDepartment.UnselectItem(e.Node.RowHandle);
        }

        private void OndeptGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = griddeptview;
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
                    var selectedRows = gridDepartment.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtPayeeName.Text))
            {
                DXMessageBox.Show("Please enter Name!");
                txtPayeeName.Focus();
                return;
            }

            if (grdCntrlCompanies.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Please Select Companies!");
                return;
            }
            if (gridDepartment.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Please Select Departments!");
                return;
            }

            //if(cmbIndustry.SelectedIndex < 0)
            //{
            //    DXMessageBox.Show("Please select Vendor Type!");
            //    return;
            //}

            if (grdCntrlAdminBillTypes.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Please Select Vendors Types!");
                return;
            }

            if (grdCntrlVendors.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Please Select Vendors!");
                return;
            }

            payee.PayeeName = txtPayeeName.Text;

            if(chkIsSubsidiary.IsChecked == true)
            {
                payee.isSubsidiary = true;
                if(lookupParentPayee.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please Select Parent Payee!");
                    return;
                }
                payee.ParentId = (lookupParentPayee.SelectedItem as Payee).Id;
            }
            else
            {
                payee.isSubsidiary = false;
                payee.ParentId = null;
            }

            if(chkIsVehicleType.IsChecked == true)
            {
                payee.isVehicleType = true;
                if(chkIsOwned.IsChecked == true)
                {
                    if (lookupCompany.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select company!");
                        lookupCompany.Focus();
                        return;
                    }
                    payee.isOwned = true;
                    payee.companyId = (lookupCompany.SelectedItem as Company).Id;
                }
                if (chkIsRented.IsChecked == true)
                {
                    if (lookupVehicleOwner.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Vehicle Owner!");
                        lookupVehicleOwner.Focus();
                        return;
                    }
                    payee.isOwned = false;
                    payee.vehicleOwnerId = (lookupVehicleOwner.SelectedItem as RentedVehicleOwner).Id;
                }
            }
            else
            {
                payee.isVehicleType = true;
                payee.isOwned = false;
                payee.companyId = null;
                payee.vehicleOwnerId = null;
            }

            //payee.IndustryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;

            if (chkIsActive.IsChecked == true)
                payee.isActive = true;
            else
                payee.isActive = false;

            if (grdCntrlCompanies.SelectedItems.Count != 0)
            {
                payee.Companies = new List<Company>();
                foreach (Company _company in grdCntrlCompanies.SelectedItems)
                {
                    if (!payee.Companies.Contains(_company))
                    {
                        payee.Companies.Add(_company);
                    }
                }
            }

            if (gridDepartment.SelectedItems.Count != 0)
            {
                payee.departments = new List<Department>();
                foreach (Department _dept in gridDepartment.SelectedItems)
                {
                    if (!payee.departments.Contains(_dept))
                    {
                        payee.departments.Add(_dept);
                    }
                }
            }

            if (grdCntrlAdminBillTypes.SelectedItems.Count != 0)
            {
                payee.AdminBillTypes = new List<AdminBillType>();
                foreach (AdminBillType _billType in grdCntrlAdminBillTypes.SelectedItems)
                {
                    if (!payee.AdminBillTypes.Contains(_billType))
                    {
                        payee.AdminBillTypes.Add(_billType);
                    }
                }
            }

            if (grdCntrlVendors.SelectedItems.Count != 0)
            {
                payee.vendors = new List<Vendor>();
                foreach (Vendor _vendor in grdCntrlVendors.SelectedItems)
                {
                    if (!payee.vendors.Contains(_vendor))
                    {
                        payee.vendors.Add(_vendor);
                    }
                }
            }

            billsRepo = new AdminBillsRepo();
            if (editFlag == false && payee.Id == 0)
            {
                billsRepo.AddPayee(payee);
                DXMessageBox.Show("Successfully Added!");
                addPayeeWindow.Close();
            }
            else if (editFlag == true && payee.Id != 0)
            {
                billsRepo.UpdatePayee(payee);
                DXMessageBox.Show("Updated Successfully!");
                addPayeeWindow.Close();
            }
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


        private void OnVendorGirdNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdCntrlVendors.SelectItem(e.Node.RowHandle);
            else
                grdCntrlVendors.UnselectItem(e.Node.RowHandle);
        }

        private void OnVendorGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = gridVendorView;
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
                    var selectedRows = grdCntrlVendors.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }

        }

        private void OnAdminBillTypeGirdNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdCntrlAdminBillTypes.SelectItem(e.Node.RowHandle);
            else
                grdCntrlAdminBillTypes.UnselectItem(e.Node.RowHandle);
        }

        private void OnAdminBillTypeGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = gridAdminBillTypeView;
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
                    var selectedRows = grdCntrlAdminBillTypes.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }

            List<AdminBillType> adminBillTypes = new List<AdminBillType>();
            List<Vendor> resultVendors = new List<Vendor>();
            foreach (AdminBillType _type in grdCntrlAdminBillTypes.SelectedItems)
            {
                adminBillTypes.Add(_type);
            }

            selectedVendors = new List<Vendor>();
            if (grdCntrlVendors.SelectedItems.Count != 0)
            {
                foreach (Vendor _vendor in grdCntrlVendors.SelectedItems)
                {
                    if (!selectedVendors.Contains(_vendor))
                    {
                        selectedVendors.Add(_vendor);
                    }
                }
            }

            if (adminBillTypes != null)
            {
                foreach(var _type in adminBillTypes)
                {
                    var vendorList = _type.vendors;
                    foreach (var _vendor in vendorList)
                        if(!resultVendors.Contains(_vendor))
                            resultVendors.Add(_vendor);

                    //vendorList.ForEach(item => resultVendors.Add(item));
                }
                grdCntrlVendors.ItemsSource = null;
                grdCntrlVendors.ItemsSource = resultVendors;
            }

            foreach(var _vendor in selectedVendors)
            {
                grdCntrlVendors.SelectItem(grdCntrlVendors.FindRowByValue(grdCntrlVendors.Columns.GetColumnByFieldName("Id"), _vendor.Id));
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //loadPayeeCategories();
            loadParentPayee();
            LoadCompanies();
            LoadVehicleOwner();
            LoadDepartments();
            loadAdminBillTypes();

            if (editFlag == true && payee.Id > 0)
            {
                if(payee.PayeeName != null)
                {
                    txtPayeeName.Text = payee.PayeeName;
                }

                if (payee.isSubsidiary == true)
                    chkIsSubsidiary.IsChecked = true;
                else
                    chkIsSubsidiary.IsChecked = false;


                if(chkIsSubsidiary.IsChecked == true)
                {
                    var payeeList = (lookupParentPayee.ItemsSource as List<Payee>) == null ? new List<Payee>() : lookupParentPayee.ItemsSource as List<Payee>;
                    if (payee.parentPayee != null)
                    {
                        int index = 0;
                        foreach (var _payee in payeeList)
                        {
                            if (_payee.Id == payee.ParentId)
                            {
                                lookupParentPayee.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }
                }


                if (payee.isVehicleType == true)
                    chkIsVehicleType.IsChecked = true;
                else
                    chkIsVehicleType.IsChecked = false;


                if (chkIsVehicleType.IsChecked == true)
                {
                    if( payee.isOwned == true)
                    {
                        chkIsOwned.IsChecked = true;
                        if(payee.company != null)
                        {
                            lookupCompany.Text = payee.company.CompanyName;
                        }
                    }
                    else
                    {
                        chkIsRented.IsChecked = true;
                        if (payee.VehicleOwner != null)
                        {
                            lookupVehicleOwner.Text = payee.VehicleOwner.OwnerName;
                        }
                    }
                }


                if (payee.isActive == true)
                    chkIsActive.IsChecked = true;
                else
                    chkIsActive.IsChecked = false;

                foreach (var _company in payee.Companies)
                {
                    grdCntrlCompanies.SelectItem(grdCntrlCompanies.FindRowByValue(grdCntrlCompanies.Columns.GetColumnByFieldName("Id"), _company.Id));
                }

                foreach (var _dept in payee.departments)
                {
                    gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), _dept.Id));
                }

                //var VendorTypeList = (cmbIndustry.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbIndustry.ItemsSource as List<cmbitem>;
                //if (payee.industryType != null)
                //{
                //    int index = 0;
                //    foreach (var _vendorType in VendorTypeList)
                //    {
                //        if (_vendorType.id == payee.IndustryTypeId)
                //        {
                //            cmbIndustry.SelectedIndex = index;
                //            index = 0;
                //            break;
                //        }
                //        index++;
                //    }
                //}

               

                if(payee.AdminBillTypes != null)
                    foreach (var _billType in payee.AdminBillTypes)
                    {

                        grdCntrlAdminBillTypes.SelectItem(grdCntrlAdminBillTypes.FindRowByValue(grdCntrlAdminBillTypes.Columns.GetColumnByFieldName("Id"), _billType.Id));
                    }

                foreach (var _vendor in payee.vendors)
                {

                    grdCntrlVendors.SelectItem(grdCntrlVendors.FindRowByValue(grdCntrlVendors.Columns.GetColumnByFieldName("Id"), _vendor.Id));
                }
            }
        }

        private void loadParentPayee()
        {
            lookupParentPayee.ItemsSource = billsRepo.GetAllPayees();
        }

        private void LoadCompanies()
        {
            CompanyRepo companyRepo = new CompanyRepo();
            var companies = companyRepo.getAll();
            grdCntrlCompanies.ItemsSource = companies;
            lookupCompany.ItemsSource = companies;
        }

        private void LoadVehicleOwner()
        {
            lookupVehicleOwner.ItemsSource = billsRepo.GetAllRentedVehicleOwner();
        }
            

        private void LoadDepartments()
        {
            DepartmentRepo deptRepo = new DepartmentRepo();
            gridDepartment.ItemsSource = deptRepo.GetActiveDepartments();
        }

        private void loadPayeeCategories()
        {
            //grdCntrlPayeeCategory.ItemsSource = billsRepo.GetAllPayeeCategories();
        }

        

        private void CmbxDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        

        private void ChkIsSubsidiary_Checked(object sender, RoutedEventArgs e)
        {
            lookupParentPayee.IsEnabled = true;
        }

        private void ChkIsSubsidiary_Unchecked(object sender, RoutedEventArgs e)
        {
            lookupParentPayee.IsEnabled = false;
            LoadCompanies();
            LoadDepartments();
        }

        private void LookupParentPayee_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(lookupParentPayee.SelectedIndex > -1)
            {
                var parentPayee = lookupParentPayee.SelectedItem as Payee;
                while(parentPayee.ParentId != null)
                {
                    parentPayee = billsRepo.GetPayee((int)parentPayee.ParentId);
                }
                if(parentPayee != null)
                {
                    gridDepartment.ItemsSource = parentPayee.departments;
                    grdCntrlCompanies.ItemsSource = parentPayee.Companies;
                }
            }
        }

        public void loadAdminBillTypes()
        {


            List<AdminBillType> adminBillTypes = new List<AdminBillType>();
            
            
            adminBillTypes = billsRepo.GetAllAdminBillTypes();
            //List<cmbitem> cmbitems = new List<cmbitem>();
            grdCntrlAdminBillTypes.ItemsSource = adminBillTypes;


            //foreach (IndustryType industry in industryTypes)
            //{

            //    cmbitems.Add(new cmbitem() { name = industry.name, id = industry.Id });


            //}
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            //cmbIndustry.ItemsSource = cmbitems;

        }

        private void CmbIndustry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if ((cmbIndustry.SelectedItem as cmbitem) != null)
            //{
            //    int idd = (cmbIndustry.SelectedItem as cmbitem).id;
            //    if (idd == 0)
            //    {
            //        frmIndustryTypeAdd industryadd = new frmIndustryTypeAdd();
            //        industryadd.ShowDialog();
            //        loadIndustryTypes();

            //    }
            //    else
            //    {
            //        VendorRepo vendorRepo = new VendorRepo();
            //        var vendorsList = vendorRepo.getAllByIndustryType(idd);
            //        grdCntrlVendors.ItemsSource = vendorsList;
            //    }
            //}
        }

        private void ChkIsVehicleType_Checked(object sender, RoutedEventArgs e)
        {
            layoutItemIsOwned.Visibility = Visibility.Visible;
           
        }

        private void ChkIsVehicleType_Unchecked(object sender, RoutedEventArgs e)
        {
            layoutItemIsOwned.Visibility = Visibility.Collapsed;
        }

        private void ChkIsOwned_Checked(object sender, RoutedEventArgs e)
        {
            chkIsRented.IsChecked = false;
            layoutItemCompany.Visibility = Visibility.Visible;
            layoutItemOwner.Visibility = Visibility.Collapsed;
        }

        private void ChkIsOwned_Unchecked(object sender, RoutedEventArgs e)
        {
            layoutItemCompany.Visibility = Visibility.Collapsed;
        }

        private void ChkIsRented_Checked(object sender, RoutedEventArgs e)
        {
            chkIsOwned.IsChecked = false;
            layoutItemCompany.Visibility = Visibility.Collapsed;
            layoutItemOwner.Visibility = Visibility.Visible;
        }

        private void ChkIsRented_Unchecked(object sender, RoutedEventArgs e)
        {
            layoutItemOwner.Visibility = Visibility.Collapsed;
        }
    }
}
