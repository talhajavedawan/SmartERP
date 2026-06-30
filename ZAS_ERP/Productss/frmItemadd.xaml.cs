using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
using System.Windows.Shapes;

namespace ZAS_ERP.Productss
{
    /// <summary>
    /// Interaction logic for frmItemadd.xaml
    /// </summary>
    public partial class frmItemadd : ThemedWindow
    {

        ProductRepo repo = new ProductRepo();
        List<Product> products = new List<Product>();
        Product product = new Product();
        ProductCategory category = new ProductCategory();
        ProductNature nature = new ProductNature();
        UnitOfMeasure unitOfMeasure = new UnitOfMeasure();
        ChartofAccountsRepo chartofAccountRepo = new ChartofAccountsRepo();
        List<ChartofAccount> incomeAccounts = new List<ChartofAccount>();
        List<ChartofAccount> cgsAccounts = new List<ChartofAccount>();
        ChartofAccount incomeAccount = new ChartofAccount();
        ChartofAccount cgsAccount = new ChartofAccount();
       
        ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
        Product parentProduct = new Product();
        List<Department> departments = new List<Department>();
        public static bool duplicate;

        public static int productId;

        public frmItemadd()
        {
            InitializeComponent();
            griddeptview.NodeCheckStateChanged += OndeptgirdNodeCheckStateChanged;

            gridDepartment.SelectionChanged += OndeptGridSelectionChanged;
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
        public void loadproductnature()
        {



            List<ProductNature> natures = new List<ProductNature>();
            natures = repo.getActiveProductNatures();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (ProductNature nat in natures)
            {

                cmbitems.Add(new cmbitem() { name = nat.nature, id = nat.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


            cmbItemNature.ItemsSource = cmbitems;
        }
        public void loadUnitOfMeasures()
        {



            List<UnitOfMeasure> unitOfMeasures = new List<UnitOfMeasure>();
            unitOfMeasures = repo.getActiveUnitOfMeasures();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (UnitOfMeasure unit in unitOfMeasures)
            {

                cmbitems.Add(new cmbitem() { name = unit.unitOfMeasure, id = unit.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


            cmbUnitOfMeasure.ItemsSource = cmbitems;

        }
        public void loadCategoris()
        {


            List<ProductCategory> categories = new List<ProductCategory>();
            categories = repo.getActiveProductCategories();
            lookupCategory.ItemsSource = categories;

        }


        private void cmbUnitOfMeasure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbUnitOfMeasure.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbUnitOfMeasure.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmUnitOfMeasureAdd frmUnit = new frmUnitOfMeasureAdd();
                    frmUnit.ShowDialog();
                    loadUnitOfMeasures();

                }
            }
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            frmProductCategoryAdd productCategoryAdd = new frmProductCategoryAdd();
            productCategoryAdd.ShowDialog();
            loadCategoris();
        }

        private void lookupCategory_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            category = lookupCategory.SelectedItem as ProductCategory;
            if (category != null)
            {
                string selectedcust = category.category;
                lookupCategory.EditValue = selectedcust;


            }
        }

        private void cmbItemNature_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbItemNature.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbItemNature.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmProductNatureAdd frmProductNature = new frmProductNatureAdd();
                    frmProductNature.ShowDialog();
                    loadproductnature();

                }
            }
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkIsTitle.IsChecked == true)
                {
                    if (txtItemName.Text == "")
                    {
                        MessageBox.Show("Enter Item Name");
                        txtItemName.Focus();
                        return;
                    }
                    else if (txtItemDiscription.Text == "")
                    {
                        MessageBox.Show("Enter Item Discription");
                        txtItemDiscription.Focus();
                        return;

                    }
                    else if (txtItemCode.Text == "")
                    {
                        MessageBox.Show("Enter Item Code");
                        txtItemCode.Focus();
                        return;

                    }
                    else if (cmbItemNature.SelectedIndex == -1)
                    {
                        MessageBox.Show("Select Item Nature");
                        cmbItemNature.Focus();
                        return;

                    }
                    else if (cmbUnitOfMeasure.SelectedIndex == -1)
                    {
                        MessageBox.Show("Select Unit Of Measure");
                        cmbUnitOfMeasure.Focus();
                        return;

                    }
                    else if (cmbItemType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Select Item type");
                        cmbItemType.Focus();
                        return;

                    }
                    else if (lookupCompany.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please Select Company");
                        lookupCategory.Focus();
                        return;

                    }
                    else if (txtItemName.Text != "" && txtItemDiscription.Text != "" && txtItemCode.Text != "" && cmbItemNature.Text != "" && cmbUnitOfMeasure.Text != "" && cmbItemType.Text != "")
                    {
                        if (duplicate == true)
                        {
                            product = new Product();
                        }
                        if (lookupCompany.SelectedIndex != -1)
                        {
                            product.company_id = (lookupCompany.SelectedItem as Company).Id;
                        }
                        if(chkIsTitle.IsChecked==true)
                        {
                            product.isTitle = true;
                        }
                        else
                        {
                            product.isTitle = false;

                        }
                        if (gridDepartment.SelectedItems.Count != 0)
                        {
                            product.departments = new List<Department>();
                            
                                foreach (Department dept in gridDepartment.SelectedItems)
                                {
                                    
                                        if (!product.departments.Contains(dept))
                                        {
                                            departments.Add(dept);
                                        }
                                }
                            product.departments = departments;
                           
                        }
                        product.productType = (ERP_BL.Enums.ProductType)cmbItemType.SelectedIndex;
                        product.code = txtItemCode.Text.Trim();
                        product.item = txtItemName.Text.Trim();
                        product.itemDescription = txtItemDiscription.Text.Trim();
                        product.isActive = (chkIsActive.IsChecked == true) ? true : false;
                        product.unitOfMeasureId = (cmbUnitOfMeasure.SelectedItem as cmbitem).id;
                        product.nature_Id = (cmbItemNature.SelectedItem as cmbitem).id;
                        if (MainWindow.currentUserid != 0)
                            product.user_Id = MainWindow.currentUserid;
                        product.categoryId = category.Id;
                        if (lookupParentProduct.SelectedIndex == -1)
                        {
                            product.parent = null;
                            product.parentId = null;
                        }
                        else
                        {
                            parentProduct = lookupParentProduct.SelectedItem as Product;
                            product.parentId = parentProduct.Id;
                        }
                        if (product.Id == 0)
                        {
                            repo.Add(product);
                            MessageBox.Show(txtItemName.Text + " Added Succesfully!");
                            this.Close();
                        }
                        else
                        {
                            repo.Update(product);
                            MessageBox.Show(txtItemName.Text + " Updated Succesfully!");
                            this.Close();
                        }
                    }
                }
                else
                {

                    if (txtItemName.Text == "")
                    {
                        MessageBox.Show("Enter Item Name");
                        txtItemName.Focus();
                        return;
                    }
                    else if (txtItemCode.Text == "")
                    {
                        MessageBox.Show("Enter Item Code");
                        txtItemCode.Focus();
                        return;

                    }
                    else if (txtItemDiscription.Text == "")
                    {
                        MessageBox.Show("Enter Item Discription");
                        txtItemDiscription.Focus();
                        return;

                    }
                    else if (cmbItemNature.SelectedIndex == -1)
                    {
                        MessageBox.Show("Select Item Nature");
                        cmbItemNature.Focus();
                        return;

                    }
                    else if (cmbUnitOfMeasure.SelectedIndex == -1)
                    {
                        MessageBox.Show("Select Unit Of Measure");
                        cmbUnitOfMeasure.Focus();
                        return;

                    }
                    else if (cmbItemType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Select Item type");
                        cmbItemType.Focus();
                        return;

                    }
                    else if (lookupIncomeAccounts.SelectedIndex == -1)
                    {
                        MessageBox.Show("Select Income Account");

                        return;

                    }
                    else if (category.Id == 0 || category == null)
                    {
                        MessageBox.Show("Select Category");
                        lookupCategory.Focus();
                        return;

                    }
                    else if (lookupCompany.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please Select Company");
                        lookupCategory.Focus();
                        return;

                    }


                    else if (txtItemName.Text != "" && txtItemDiscription.Text != "" && txtItemCode.Text != "" && cmbItemNature.Text != "" && cmbUnitOfMeasure.Text != "" && cmbItemType.Text != "" && incomeAccount != null)
                    {
                        if (duplicate == true)
                        {
                            product = new Product();
                        }
                        if (lookupCGSAccounts.SelectedIndex != -1)
                        {
                            product.cgsAccount_id = (lookupCGSAccounts.SelectedItem as ChartofAccount).Id;
                        }
                        if (lookupIncomeAccounts.SelectedIndex != -1)
                        {
                            product.incomeAccount_id = (lookupIncomeAccounts.SelectedItem as ChartofAccount).Id;
                        }
                        if (lookupCompany.SelectedIndex != -1)
                        {
                            product.company_id = (lookupCompany.SelectedItem as Company).Id;
                        }
                        if (gridDepartment.SelectedItems.Count != 0)
                        {
                            int CheckFlag = 0;
                            product.departments = new List<Department>();
                            foreach (Department dept in gridDepartment.SelectedItems)
                            {
                                if (dept.AccountPayable != null && dept.ChartofAccount != null)
                                {
                                    if (!product.departments.Contains(dept))
                                    {
                                        //DepartmentRepo repo = new DepartmentRepo();
                                        //var depart=repo.get(dept.Id);
                                        departments.Add(dept);
                                    }
                                }
                                else
                                {
                                    DXMessageBox.Show("No Account mapped with " + dept.DeptName, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                                    CheckFlag = 1;
                                    break;
                                }

                            }
                            product.departments = departments;
                            if (CheckFlag == 1)
                            {
                                return;
                            }

                        }
                        if (lookupInvenCGSAccounts.SelectedIndex != -1)
                        {
                            product.cgsInvenAccount_id = (lookupInvenCGSAccounts.SelectedItem as ChartofAccount).Id;
                        }
                        if (lookupCAssetAccounts.SelectedIndex != -1)
                        {
                            product.cAssetAccount_id = (lookupCAssetAccounts.SelectedItem as ChartofAccount).Id;
                        }
                        if (chkIsTitle.IsChecked == true)
                        {
                            product.isTitle = true;
                        }
                        else
                        {
                            product.isTitle = false;

                        }

                        product.productType = (ERP_BL.Enums.ProductType)cmbItemType.SelectedIndex;

                        product.code = txtItemCode.Text.Trim();
                        product.item = txtItemName.Text.Trim();
                        product.itemDescription = txtItemDiscription.Text.Trim();
                        product.isActive = (chkIsActive.IsChecked == true) ? true : false;
                        product.unitOfMeasureId = (cmbUnitOfMeasure.SelectedItem as cmbitem).id;
                        product.nature_Id = (cmbItemNature.SelectedItem as cmbitem).id;
                        if (MainWindow.currentUserid != 0)
                            product.user_Id = MainWindow.currentUserid;
                        product.categoryId = category.Id;

                        if (lookupParentProduct.SelectedIndex == -1)
                        {
                            //parentProduct = lookupParentProduct.SelectedItem as Product;
                            product.parent = null;
                            product.parentId = null;
                        }
                        else
                        {
                            parentProduct = lookupParentProduct.SelectedItem as Product;
                            product.parentId = parentProduct.Id;
                        }

                        if (product.Id == 0)
                        {
                            repo.Add(product);

                            MessageBox.Show(txtItemName.Text + " Added Succesfully!");
                            this.Close();

                        }
                        else
                        {
                            repo.Update(product);

                            MessageBox.Show(txtItemName.Text + " Updated Succesfully!");
                            this.Close();

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winItemAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            productId = 0;
        }
        private void loaddepartments()
        {
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            gridDepartment.ItemsSource = userDepartments;
        }
        private void winItemAdd_Loaded(object sender, RoutedEventArgs e)
        {
            loaddepartments();
            loadCompanies();
            loadCategoris();
            loadproductnature();
            loadUnitOfMeasures();
            LoadChartofAccounts();
            LoadItemTypes();
            loaditeminfo();
            loadParentProducts();
                
        }
        public void loadCompanies()
        {
            lookupCompany.ItemsSource = SYSTEM_STATIC.LoadCurrentUserCompanies();
        }
        public void loadParentProducts()
        {
           lookupParentProduct.ItemsSource=repo.getAllParentProducts();
        }
        public void LoadItemTypes()
        {
            List<string> productTypes = new List<string>();
            for (int i = 0; i <= (int)ERP_BL.Enums.ProductType.Services; i++)
            {
                productTypes.Add(((ERP_BL.Enums.ProductType)i).ToString());
            }

            cmbItemType.ItemsSource = productTypes;
            return;
        }

        public void LoadChartofAccounts()
        {
            
            incomeAccounts = chartofAccountRepo.GetAllIncomeAccountsforItem(SYSTEM_STATIC.currentUser.id);
            cgsAccounts = chartofAccountRepo.GetAllCGSAccountsforItem(SYSTEM_STATIC.currentUser.id);
            if (incomeAccounts != null)
            {
                lookupIncomeAccounts.ItemsSource = incomeAccounts;
            }
            if (cgsAccounts != null)
            {
                lookupCGSAccounts.ItemsSource = cgsAccounts;
                lookupInvenCGSAccounts.ItemsSource = cgsAccounts;
            }

        }
        public void loaditeminfo()
        {
            if (productId != 0)
            {
                product = repo.get(productId);
                txtItemCode.Text = product.code;
                txtItemDiscription.Text = product.itemDescription;
                txtItemName.Text = product.item;
                if (product.isTitle == true)
                {
                    chkIsTitle.IsChecked = true;
                }
                else
                {
                    chkIsTitle.IsChecked = false;

                }
                foreach (cmbitem item in cmbItemNature.Items)
                {
                    if (item.id == product.nature_Id)
                    {
                        cmbItemNature.SelectedItem = item;
                        break;
                    }
                }
                foreach (cmbitem item in cmbUnitOfMeasure.Items)
                {
                    if (item.id == product.unitOfMeasureId)
                    {
                        cmbUnitOfMeasure.SelectedItem = item;
                        break;
                    }
                }
                if (product.categoryId != 0 && product.category != null)
                {
                    lookupCategory.Text = product.category.category;

                    lookupCategory.SelectedItem = lookupCategory.GetItemByKeyValue(product.category);

                    category = product.category;
                }
                chkIsActive.IsChecked = product.isActive;
                foreach (Department dept in product.departments)
                {
                    gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), dept.Id));
                }
                if (product.incomeAccount_id != null )
                {
                    var chartofAccount=chartofAccountsRepo.GetAccountById((int)product.incomeAccount_id);
                    if (chartofAccount != null)
                    {
                        lookupIncomeAccounts.Text = chartofAccount.accountName;
                        incomeAccount = chartofAccount;
                    }
                }
                if (product.cgsAccount_id != null)
                {
                    //var chartofAccount = chartofAccountsRepo.GetAccountById((int)product.incomeAccount_id);
                    if (product.cgsAccount != null)
                    {
                        lookupCGSAccounts.Text = product.cgsAccount.accountName;
                        cgsAccount = product.cgsAccount;
                    }
                }
                if (product.cgsInvenAccount_id != null)
                {
                    //var chartofAccount = chartofAccountsRepo.GetAccountById((int)product.incomeAccount_id);
                    if (product.cgsInvenAccount_id != null)
                    {
                        lookupInvenCGSAccounts.Text = product.cgsInvenAccount.accountName;
                        
                    }
                }
                if (product.cAssetAccount != null)
                {
                    //var chartofAccount = chartofAccountsRepo.GetAccountById((int)product.incomeAccount_id);
                    if (product.cAssetAccount != null)
                    {
                        lookupCAssetAccounts.Text = product.cAssetAccount.accountName;
                        txtCreditCurrentAsset.Text = product.cAssetAccount.accountName;
                        txtDebitCurrentAsset.Text = product.cAssetAccount.accountName;
                    }
                }
                if (product.company_id != null)
                {
                    if (product.company != null)
                    {
                        lookupCompany.Text = product.company.CompanyName;
                    }
                }

                cmbItemType.Text = product.productType.ToString();
                if (product.parent != null)
                {
                    lookupParentProduct.Text = product.parent.item;
                }
                else
                {
                }
            }
        }

        private void CmbItemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if ((ERP_BL.Enums.ProductType)cmbItemType.SelectedIndex == ERP_BL.Enums.ProductType.Services || (ERP_BL.Enums.ProductType)cmbItemType.SelectedIndex == ERP_BL.Enums.ProductType.Non_Inventory)
            {
                lblCGS.Visibility = Visibility.Visible;
                currAssetAccount.Visibility = Visibility.Collapsed;
                lblInvenCGS.Visibility = Visibility.Collapsed;
                tabInventory.Visibility = Visibility.Collapsed;
                txtCreditCurrentAsset.Visibility = Visibility.Collapsed;
                txtDebitCurrentAsset.Visibility = Visibility.Collapsed;
                siItemName.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblInvenCGS.Visibility = Visibility.Visible;
                lblCGS.Visibility = Visibility.Collapsed;
                currAssetAccount.Visibility = Visibility.Visible;
                tabInventory.Visibility = Visibility.Visible;
                txtCreditCurrentAsset.Visibility = Visibility.Visible;
                txtDebitCurrentAsset.Visibility = Visibility.Visible;
                siItemName.Visibility = Visibility.Visible;

                lookupCAssetAccounts.ItemsSource = chartofAccountRepo.GetAll();
            }


        }

        private void LookupIncomeAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupIncomeAccounts.SelectedIndex != -1)
                incomeAccount = lookupIncomeAccounts.SelectedItem as ChartofAccount;
        }
        private void LookupParentProduct_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupParentProduct.SelectedIndex!=-1)
                parentProduct = lookupParentProduct.SelectedItem as Product;
        }

        private void LookupCGSAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupCGSAccounts.SelectedIndex != -1)
                cgsAccount = lookupCGSAccounts.SelectedItem as ChartofAccount;
        }

        private void GridDepartment_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (chkIsTitle.IsChecked != true)
            {
                List<Department> receivableList = new List<Department>();
                var departments = gridDepartment.SelectedItems;

                foreach (Department department in departments)
                {
                    if (department.ChartofAccount != null)
                    {
                        receivableList.Add(department);
                    }
                    else
                    {
                        DXMessageBox.Show("No Account found at " + department.DeptName, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                        break;

                    }
                }
                grdSIDepartment.ItemsSource = receivableList.Distinct();
                grdPIDepartment.ItemsSource = receivableList.Distinct();
            }
        }

        private void LookupCAssetAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(lookupCAssetAccounts.SelectedIndex!=-1 && (ERP_BL.Enums.ProductType)cmbItemType.SelectedIndex == ERP_BL.Enums.ProductType.Inventory)
            {
                txtCreditCurrentAsset.Text = (lookupCAssetAccounts.SelectedItem as ChartofAccount).accountName;
                txtDebitCurrentAsset.Text = (lookupCAssetAccounts.SelectedItem as ChartofAccount).accountName;
            }
        }
        private void TxtItemName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtInventoryItem.Text= txtItemName.Text;
        }

        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void chkIsTitle_Checked(object sender, RoutedEventArgs e)
        {
            tabAccountant.IsEnabled = false;
        }

        private void chkIsTitle_Unchecked(object sender, RoutedEventArgs e)
        {
            tabAccountant.IsEnabled = true;
        }
    }
}
