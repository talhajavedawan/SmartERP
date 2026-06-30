using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Documents;
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

namespace ZAS_ERP.FilesAndDocss.Documentss
{
    /// <summary>
    /// Interaction logic for ucFrmDocumentTemplateAdd.xaml
    /// </summary>
    public partial class ucFrmDocumentTemplateAdd : UserControl
    {
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        public bool editFlag = false;
        DocumentRepo documentRepo = new DocumentRepo();
        DocumentTemplate documentTemplate = new DocumentTemplate();
        public int documentTemplateId = 0;
        List<Department> deptList = new List<Department>();
        public ucFrmDocumentTemplateAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCompanies();
            if (editFlag == true)
            {
                documentTemplate = documentRepo.GetDocumentTemplate(documentTemplateId);

                txtDocumentTemplate.Text = documentTemplate.TemplateName;


                if (documentTemplate.companies != null && documentTemplate.companies.Count > 0)
                {
                    foreach (Company company in documentTemplate.companies)
                    {
                        grdCompanies.SelectItem(grdCompanies.FindRowByValue(grdCompanies.Columns.GetColumnByFieldName("Id"), company.Id));
                    }
                }

                //if (documentType.departments != null && documentType.departments.Count > 0)
                //{
                //    foreach (Department dept in documentType.departments)
                //    {
                //        grdDepartments.SelectItem(grdDepartments.FindRowByValue(grdDepartments.Columns.GetColumnByFieldName("Id"), dept.Id));
                //    }
                //}

                int index = 0;
                var grdDepts = (grdDepartments.ItemsSource as List<Department>) == null ? new List<Department>() : grdDepartments.ItemsSource as List<Department>;

                List<int> deptIds = new List<int>();
                foreach (var _dept in documentTemplate.departments)
                    deptIds.Add(_dept.Id);

                foreach (Department _deptt in grdDepts)
                {
                    if (deptIds.Contains(_deptt.Id))
                        CallRecursion(gridDeptView, _deptt.Id, true);
                    else
                        CallRecursion(gridDeptView, _deptt.Id, false);
                    index++;
                }

            }
        }

        private void LoadCompanies()
        {
            empUser = documentRepo.GetEmployeeForDocuments(SYSTEM_STATIC.currentUser.employeeId);
            grdCompanies.ItemsSource = empUser.Companies;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtDocumentTemplate.Text))
            {
                DXMessageBox.Show("Please Enter Document Template!");
                txtDocumentTemplate.Focus();
                return;
            }
            if (grdCompanies.SelectedItems == null || grdCompanies.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Plase select Companies!");
                grdCompanies.Focus();
                return;
            }
            if (deptList == null || deptList.Count == 0)
            {
                DXMessageBox.Show("Plase select Departments!");
                grdDepartments.Focus();
                return;
            }

            documentTemplate.TemplateName = txtDocumentTemplate.Text;

            documentTemplate.companies = new List<Company>();
            foreach (Company company in grdCompanies.SelectedItems)
                documentTemplate.companies.Add(company);


            documentTemplate.departments = new List<Department>();
            foreach (Department dept in deptList)
                documentTemplate.departments.Add(dept);

            if (editFlag == false)
            {
                documentRepo.AddDocumentTemplate(documentTemplate);
                DXMessageBox.Show("Added Succesfully!");
            }
            else
            {
                documentRepo.UpdateDocumentTemplate(documentTemplate);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }


        private void grdCompanies_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            List<Company> companies = new List<Company>();
            foreach (Company company in grdCompanies.SelectedItems)
            {
                companies.Add(company);
            }

            List<Department> selectedDepts = new List<Department>();
            foreach (Department department in deptList)
            {
                selectedDepts.Add(department);
            }

            var deptIds = selectedDepts.Select(x => x.Id);

            if (companies != null && companies.Count > 0)
            {
                List<Department> departments = new List<Department>();
                //grdDepartments.ItemsSource = rentalRepo.GetAllAssetBrand().Where(x => x.assetNatureId == nature.Id && subNatureIds.Intersect(x.AssetSubNatures.Select(y => y.Id)).Count() > 0).ToList();
                foreach (Company comp in companies)
                {
                    departments.AddRange(comp.departments.Where(x => x.IsDocumentType == true && x.isActive == true).ToList());
                }
                departments = departments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                grdDepartments.ItemsSource = departments;

                foreach (Department dept in selectedDepts)
                {
                    //grdDepartments.SelectItem(grdDepartments.FindRowByValue(grdDepartments.Columns.GetColumnByFieldName("Id"), dept.Id));

                    CallRecursion(gridDeptView, dept.Id, true);
                }
                //grdDepartments.RefreshData();
            }
        }

        private void gridDeptView_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            var row = grdDepartments.GetRow(e.Node.RowHandle) as Department;

            if (e.Node.IsChecked == true)
            {
                if (deptList.FirstOrDefault(x => x.Id == row.Id) == null)
                    deptList.Add(row);
            }
            else
            {
                if (deptList.FirstOrDefault(x => x.Id == row.Id) != null)
                    deptList.Remove(row);
            }

        }

        private void CheckNodes(TreeListNode treeNode, int id, bool check)
        {

            var row = grdDepartments.GetRow(treeNode.RowHandle) as Department;
            //deptList.Add(row);
            if (row.Id == id)
            {
                treeNode.IsChecked = check;
                return;
            }
            // }


            // Visit each node recursively.  
            foreach (TreeListNode tn in treeNode.Nodes)
            {
                CheckNodes(tn, id, check);
            }
        }

        // Call the procedure using the TreeView.  
        private void CallRecursion(TreeListView treeView, int id, bool check)
        {
            // Print each node recursively.  
            foreach (TreeListNode n in treeView.Nodes)
            {
                //recursiveTotalNodes++;
                CheckNodes(n, id, check);
            }
        }

    }
}
