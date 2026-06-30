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
    /// Interaction logic for ucFrmDocumentTypeAdd.xaml
    /// </summary>
    public partial class ucFrmDocumentTypeAdd : UserControl
    {
        public bool editFlag = false;
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        DocumentRepo documentRepo = new DocumentRepo();
        DocumentType documentType = new DocumentType();
        public int documentTypeId = 0;
        List<Department> deptList = new List<Department>();
        public ucFrmDocumentTypeAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdTemplates.ItemsSource = documentRepo.GetAllDocumentTemplate();
            LoadCompanies();
            if (editFlag == true)
            {
                documentType = documentRepo.GetDocumentType(documentTypeId);

                

                txtDocumentType.Text = documentType.TypeName;

                if (documentType.companies != null && documentType.companies.Count > 0)
                {
                    foreach (Company company in documentType.companies)
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
                foreach (var _dept in documentType.departments)
                    deptIds.Add(_dept.Id);

                foreach (Department _deptt in grdDepts)
                {
                    if (deptIds.Contains(_deptt.Id))
                        CallRecursion(gridDeptView, _deptt.Id, true);
                    else
                        CallRecursion(gridDeptView, _deptt.Id, false);
                    index++;
                }

                if (documentType.DocumentTemplates != null && documentType.DocumentTemplates.Count > 0)
                {
                    foreach (DocumentTemplate template in documentType.DocumentTemplates)
                    {
                        grdTemplates.SelectItem(grdTemplates.FindRowByValue(grdTemplates.Columns.GetColumnByFieldName("Id"), template.Id));
                    }
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
            if (String.IsNullOrEmpty(txtDocumentType.Text))
            {
                DXMessageBox.Show("Please Enter Document Type!");
                txtDocumentType.Focus();
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
            if (grdTemplates.SelectedItems == null || grdTemplates.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Plase select Templates!");
                grdTemplates.Focus();
                return;
            }

            documentType.TypeName = txtDocumentType.Text;

            documentType.companies = new List<Company>();
            foreach (Company company in grdCompanies.SelectedItems)
                documentType.companies.Add(company);


            documentType.departments = new List<Department>();
            foreach (Department dept in deptList)
                documentType.departments.Add(dept);

            documentType.DocumentTemplates = new List<DocumentTemplate>();
            foreach (DocumentTemplate template in grdTemplates.SelectedItems)
                documentType.DocumentTemplates.Add(template);

            if (editFlag == false)
            {
                documentRepo.AddDocumentType(documentType);
                DXMessageBox.Show("Added Succesfully!");
            }
            else
            {
                documentRepo.UpdateDocumentType(documentType);
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
                deptList.Add(row);
            else
                deptList.Remove(row);
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
