using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL;
using ERP_BL.Databases;


namespace ZAS_ERP.Companiess
{
    /// <summary>
    /// Interaction logic for frmcompanyCenter.xaml
    /// </summary>
    public partial class frmcompanyCenter : DXWindow
    {
        public static int Editit;
        public static int companyId;
        public static int addId;
        public static int contactId;
        public static int departmentId;
        public static int editdept;
        public static int sectionid;
        public static int editsection;
    
        public static string parentDept;

        public ERP_BL.Databases.DBContextERP context = new DBContextERP();
        List<ERP_BL.Databases.Department> deptList = new List<ERP_BL.Databases.Department>();
        List<ERP_BL.Databases.Department> tempDeptList = new List<ERP_BL.Databases.Department>();


        public frmcompanyCenter()
        {
            InitializeComponent();

           
          
        }
        private void loadcompdata()
        {
            CompanyRepo cont = new CompanyRepo();
            List<ERP_BL.Databases.Company> companyRepos = new List<ERP_BL.Databases.Company>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Companies") != null || MainWindow.currentUserid == 0)
            {
                companyRepos = cont.GetCompanies();

                
            }
            else
            {
                companyRepos = cont.GetActiveCompanies();
            }
            this.gridCompany.ItemsSource = companyRepos;
            Editit = 0;
            //DepartmentRepo cont1 = new DepartmentRepo();
            //List<ERP_BL.Databases.Department> deptRepo = new List<ERP_BL.Databases.Department>();
            //deptRepo = cont1.GetDepartments();
            //this.grdDepartment.ItemsSource = deptRepo;

            //// ERP_BL.Databases.Section cont2 = new ERP_BL.Databases.Section();
            //List<ERP_BL.Databases.Section> secRepo = new List<ERP_BL.Databases.Section>();
            //secRepo = cont1.GetSections();
            //this.gridSection.ItemsSource = secRepo;

        }
        private void loaddeptdata()
        {
           

            DepartmentRepo cont1 = new DepartmentRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Departments") != null || MainWindow.currentUserid == 0)
            {
                deptList = cont1.GetDepartments();

            }
            else
            {
                deptList = cont1.GetActiveDepartments();
            }
            
            this.grdDepartment.ItemsSource = deptList;
            editdept = 0;
        }
        //private void loadSectiondata()
        //{
        //    DepartmentRepo cont1 = new DepartmentRepo();
        //    List<Department> departments = new List<Department>();
        //    departments = cont1.GetDepartments();
        //    this.gridSection.ItemsSource = departments;
        //    editsection = 0;
        //}
        private void sectionlistload()
        {

            //DepartmentRepo cont1 = new DepartmentRepo();
            //List<ERP_BL.Databases.Section> secRepo = new List<ERP_BL.Databases.Section>();
            //secRepo = cont1.GetSections();
            //int i = 0;
            //List<sectdeptlist> items = new List<sectdeptlist>();
            
            
            //DepartmentRepo repo = new DepartmentRepo();
            //Department department = new Department();
            //department = repo.GetDepartment((int)grdDepartment.GetFocusedRowCellValue(this.coldeptid));
            //foreach (Section section in department.sections)
            //{
            //   items.Add(new sectdeptlist() { deptname = department.DeptName, secname = section.SectionName, secid = section.Id });
            //}
            //pnlsectionss.ItemsSource = items;

        }
        //public class sectionlist
        //{
        //    public string deptsect { get; set; }
        //    public int sectid { get; set; }
        //}
        //public ObservableCollection<BoolStringClass> TheList { get; set; }
        //public void CreateCheckBoxList()
        //{
        //    TheList = new ObservableCollection<BoolStringClass>();
        //    TheList.Add(new BoolStringClass { TheText = "EAST", TheValue = 1 });
        //    TheList.Add(new BoolStringClass { TheText = "WEST", TheValue = 2 });
        //    TheList.Add(new BoolStringClass { TheText = "NORTH", TheValue = 3 });
        //    TheList.Add(new BoolStringClass { TheText = "SOUTH", TheValue = 4 });
        //    this.DataContext = this;
        //}
        //public class sectdeptlist
        //{
        //    public string deptname { get; set; }
        //    public string secname { get; set; }
        //    public int secid { get; set; }
        //    public override string ToString()
        //    {
        //        return deptname + "_" + secname;
        //    }
        //}
        private void loa_Loaded(object sender, RoutedEventArgs e)
        {
            loadcompdata();
          //  loaddeptdata();
            //loadSectiondata();

            //SystemLogic.SetUserSettingOfCurrentWindow(grdDepartment);
            //SystemLogic.SetUserSettingOfCurrentWindow(gridCompany);
            //SystemLogic.SetUserSettingOfCurrentWindow(gridCompanyDepartment);

            // System.Windows.Data.CollectionViewSource departmentRepoViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("departmentRepoViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // departmentRepoViewSource.Source = [generic data source]
        }
        private void mapSection_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNewCompany_Click(object sender, RoutedEventArgs e)
        {
            Editit = 0;
            frmcompanyadd frmcompanyadd = new frmcompanyadd();
            frmcompanyadd.Owner = this;
            frmcompanyadd.ShowDialog();
            loadcompdata();

        }

        private void btnEditCompany_Click(object sender, RoutedEventArgs e)
        {
            Editit = 1;
            if (gridCompany.GetFocusedRowCellValue(this.colId) != null)
            {

                companyId = (int)gridCompany.GetFocusedRowCellValue(this.colId);
                //addId = (int)gridCompany.GetFocusedRowCellValue(this.coladressId);
                //contactId= (int)gridCompany.GetFocusedRowCellValue(this.colcontactId);
                //MessageBox.Show(newid.ToString());
            }
            
            frmcompanyadd frmcompanyadd = new frmcompanyadd();
            frmcompanyadd.Owner = this;
            frmcompanyadd.ShowDialog();
            loadcompdata();
        }

        private void btnDeleteCompany_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            frmDepartmentAdd frmDepartmentAdd = new frmDepartmentAdd();
            editdept = 0;
            frmDepartmentAdd.Owner = this;
            frmDepartmentAdd.ShowDialog();
            //loaddeptdata();
        }

        private void btnEditDepartment_Click(object sender, RoutedEventArgs e)
        {
            editdept = 1;
            if (grdDepartment.GetFocusedRowCellValue(this.coldeptid) != null)
            {

                departmentId = (int)grdDepartment.GetFocusedRowCellValue(this.coldeptid);

                //MessageBox.Show(newid.ToString());
                //StringBuilder fullName = new StringBuilder();
                //var node = griddeptview.FocusedNode;

                //while (node != null)
                //{
                //    if (fullName.Length != 0) 
                //        fullName.Insert(0, " | ");
                //    fullName.Insert(0, (string)grdDepartment.GetCellValue(node.RowHandle, "DeptName"));

                //    node = node.ParentNode;
                //}
                //parentDept = fullName.ToString();

            }

            frmDepartmentAdd frmdeptadd = new frmDepartmentAdd();
           frmdeptadd.Owner = this;
            
            frmdeptadd.ShowDialog();
            
        }

        private void btnDeleteDepartment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteSection_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void btnEditSection_Click(object sender, RoutedEventArgs e)
        //{
        //    editsection = 1;
        //    if (gridSection.GetFocusedRowCellValue(this.colsectid) != null)
        //    {

        //       sectionid = (int)gridSection.GetFocusedRowCellValue(this.colsectid);

        //        //MessageBox.Show(newid.ToString());
        //    }

        //    //frmSectionAdd frmsectionadd = new frmSectionAdd();
        //    //frmsectionadd.Owner = this;
        //    //frmsectionadd.ShowDialog();
        //    //loadSectiondata();

        //}

        private void btnAddSection_Click(object sender, RoutedEventArgs e)
        {
           // frmSectionAdd frmsectionadd = new frmSectionAdd();
           //frmsectionadd.Owner = this;
           // frmsectionadd.ShowDialog();
           // loadSectiondata();
        }

        private void gridCompany_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

            ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
            company = gridCompany.SelectedItem as ERP_BL.Databases.Company;
            if(company!=null && company.departments!=null)
            gridCompanyDepartment.ItemsSource = company.departments;
            //MessageBox.Show("Selection changes");
            //int newid = (int)gridCompany.GetFocusedRowCellValue("colId");
            //MessageBox.Show(newid.ToString());
            //if (Convert.ToDouble(gridCompany.GetCellValue(rowHandle, "UnitPrice")) < 10)
            // gridCompany.UnselectItem(rowHandle);

            //foreach (int i in gridCompany.Get)
            //{
            //    DataRow row = gridCompany.GetDataRow(i);
            //    MessageBox.Show(row[0].ToString());
            //}

            //if (gridCompany.SelectedItems != null)
            //{
            //    DataViewBase view = gridCompany.View;
            //    object row = gridCompany.SelectedItem;
            //    MessageBox.Show(String.Format("Selected row: {0}", row.ToString()));
            //}
        }

        private void select_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {   //get id from company grid 
            //if (gridCompany.GetFocusedRowCellValue(this.colId) != null)
            //{
                
            //    int newid = (int)gridCompany.GetFocusedRowCellValue(this.colId);
            //    MessageBox.Show(newid.ToString());
            //}
        }

        private void griddeptview_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //sectionlistload();
            
            
        }

        private void Loa_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdDepartment);
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(gridCompany);
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(gridCompanyDepartment);
        }

        private void Griddeptview_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                if (e.Column.FieldName == "Companiess")
                {
                    var dept = e.Node.Content as Department;
                    if (dept.companies != null && dept.companies.Count != 0)
                    {
                        var res = String.Join(", ", dept.companies.Select(x => x.CompanyName));
                        e.Value = res;
                    }
                }

                if (e.Column.FieldName == "DeptLevel")
                {
                    var row = e.Node.Content as Department;
                    string deptLevel = "";
                    if (row != null)
                    {
                        ERP_BL.Databases.DepartmentLevel level = new ERP_BL.Databases.DepartmentLevel();
                        level = row.departmentLevel;
                        while (level != null)
                        {
                            deptLevel = level.Title + " | " + deptLevel;
                            level = level.parentLevel;
                        }
                    }
                    e.Value = deptLevel;
                }
            }
        }

        private void Griddeptview_UnboundExpressionEditorCreated(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdDepartment);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loaddeptdata();
        }

        private void MbtnAddNew_Click(object sender, RoutedEventArgs e)
        {
            frmDepartmentAdd frmDepartmentAdd = new frmDepartmentAdd();
            frmDepartmentAdd.Owner = this;
            frmDepartmentAdd.ShowDialog();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {

            editdept = 1;
            if (grdDepartment.GetFocusedRowCellValue(this.coldeptid) != null)
            {

                departmentId = (int)grdDepartment.GetFocusedRowCellValue(this.coldeptid);

                //MessageBox.Show(newid.ToString());
            }

            frmDepartmentAdd frmdeptadd = new frmDepartmentAdd();
            frmdeptadd.Owner = this;

            frmdeptadd.ShowDialog();
        }
        public void loadActiveDepartment() 
        {
            try
            {
                DepartmentRepo cont1 = new DepartmentRepo();
                deptList = cont1.GetActiveDepartments();
                this.grdDepartment.ItemsSource = deptList;
                lblDepartment.Text = "Active Departments";
                editdept = 0;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }

           
        }
        public void loadInActiveDepartment() 
        {
            try
            {
                DepartmentRepo cont1 = new DepartmentRepo();
                deptList = cont1.GetInActiveDepartments();
                this.grdDepartment.ItemsSource = deptList;
                lblDepartment.Text = "In Active Departments";
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void mbtnAllDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DepartmentRepo cont1 = new DepartmentRepo();
                List<ERP_BL.Databases.Department> deptRepo = new List<ERP_BL.Databases.Department>();
                deptRepo = cont1.GetDepartments();
                this.grdDepartment.ItemsSource = deptRepo;
                lblDepartment.Text = "All Departments";
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
            


        }

        private void mbtnActiveDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loadActiveDepartment();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
            
        }

        private void mbtnInActiveDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loadInActiveDepartment();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
           
        }
        public bool checkDept;
        private void tabDepartment_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if(checkDept == false)
                {
                    checkDept = true;
                    loadActiveDepartment();
                }
               
            }
              catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void ChkProcurementType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkProcurementType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkInventoryType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkInventoryType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkVendorBillType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkVendorBillType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkInterBankTransType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkInterBankTransType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkInterCompanyTransType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkInterCompanyTransType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkLoansAdvancesType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkLoansAdvancesType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkTaskType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkTaskType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkTravelingRecordType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkTravelingRecordType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void LoadFilteredDepartments()
        {
            tempDeptList = new List<Department>();

            if(chkProcurementType.IsChecked == true)
            {
                tempDeptList = deptList.Where(x=>x.IsProcurementType == true).ToList();
            }
            if (chkInventoryType.IsChecked == true)
            {
                tempDeptList.AddRange(deptList.Where(x => x.IsInventoryType == true));
            }
            if (chkVendorBillType.IsChecked == true)
            {
                tempDeptList.AddRange(deptList.Where(x => x.IsVendorBillType == true));
            }
            if (chkInterBankTransType.IsChecked == true)
            {
                tempDeptList.AddRange( deptList.Where(x => x.IsInterBankTransferType == true));
            }
            if (chkInterCompanyTransType.IsChecked == true)
            {
                tempDeptList.AddRange(deptList.Where(x => x.IsInterCompTransferType == true));
            }
            if (chkLoansAdvancesType.IsChecked == true)
            {
                tempDeptList.AddRange( deptList.Where(x => x.IsLoansAdvancesType == true));
            }
            if (chkTaskType.IsChecked == true)
            {
                tempDeptList.AddRange( deptList.Where(x => x.IsTaskType == true));
            }
            if (chkTravelingRecordType.IsChecked == true)
            {
                tempDeptList.AddRange(deptList.Where(x => x.IsTravelingRecordType == true));
            }
            if (chkHRMType.IsChecked == true)
            {
                tempDeptList.AddRange(deptList.Where(x => x.IsManagerial == true));
            }
            if (chkAdminBillType.IsChecked == true)
            {
                tempDeptList.AddRange(deptList.Where(x => x.IsAdminBillType == true));
            }
            if (chkDocumentType.IsChecked == true)
            {
                tempDeptList.AddRange(deptList.Where(x => x.IsDocumentType == true));
            }

            tempDeptList = tempDeptList.Distinct().ToList();
            grdDepartment.ItemsSource = tempDeptList;
            grdDepartment.RefreshData();
        }

        private void ChkHRMType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkHRMType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkAdminBillType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkAdminBillType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void GrdDepartment_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
           

        }

        private void ChkDocumentType_Checked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }

        private void ChkDocumentType_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadFilteredDepartments();
        }
    }
}
