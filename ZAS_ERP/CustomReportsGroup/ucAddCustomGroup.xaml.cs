using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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
using ERP_BL.Databases;
using ERP_BL.CustomReports;
using DevExpress.Xpf.Grid.LookUp;

namespace ZAS_ERP.CustomReportsGroup
{
    /// <summary>
    /// Interaction logic for ucAddCustomGroup.xaml
    /// </summary>
    public partial class ucAddCustomGroup : UserControl
    {
        List<CustomReport> reportList = new List<CustomReport>();
        
        public ucAddCustomGroup()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] extraEnums = { "UnDefined", "BackgroundImages" };
                var enums = Enum.GetNames(typeof(ERP_BL.Enums.TransactionItemType)).Except(extraEnums);
                lookupModule.ItemsSource = enums;

                lookupReportType.ItemsSource = Enum.GetNames(typeof(ERP_BL.Enums.GridReportType));

                grdCustomReports.ItemsSource = reportList;
                lookupCompanyGrid.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
                txtCreator.Text = SYSTEM_STATIC.currentUser.userName;
                txtUpdator.Text = SYSTEM_STATIC.currentUser.userName;

                datCreationDate.DateTime = DateTime.Now;
                datUpdateDate.DateTime = DateTime.Now;

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "On Loaded");
            }
          
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //if (lookupModule.SelectedIndex < 0)
            //{
            //    DXMessageBox.Show("Please select Module first!");
            //    lookupModule.Focus();
            //    return;
            //}
            //if (lookupFields.SelectedIndex < 0)
            //{
            //    DXMessageBox.Show("Please select Field first!");
            //    lookupFields.Focus();
            //    return;
            //}
        }

        private void LookupCompanyGrid_LostFocus(object sender, RoutedEventArgs e)
        {

            
        }

        private void View_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
           
            var selectedCompany = e.Cell.Value as Company;

            var row = e.Row as CustomReport;
            var colum = e.Column;

            if (row != null && colum.FieldName == "company")
            {
                var departments = selectedCompany.departments;
                List<Department> departmentList = new List<Department>();
                foreach (var dept in departments)
                {
                    if (dept.isActive == true)
                    {
                        List<int> empyoyeeIds = new List<int>();
                        foreach (var emp in dept.employees)
                        {
                            empyoyeeIds.Add(emp.EmpId);
                        }
                        if (empyoyeeIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                            departmentList.Add(dept);
                    }
                }

                lookupDepartmentGrid.ItemsSource = departmentList;

                //lookupDepartmentGrid.ItemsSource = SYSTEM_STATIC.LoadCurrentUserDepartments();
            }
            if(row != null && colum.FieldName == "department")
            {

                //var lookupDepartment = (ControlTemplate)lookupDepartmentGrid.PopupContentTemplate;
                //var template = lookupDepartment.Template;
                var tabView = (TableView)view;

                //var selectedDepartment = reportList[(int)(grdCustomReports.GetFocusedRow() as GridRow).RowPosition].department as Department;

         
                var selectedDepartment = reportList[0].department as Department;
                selectedDepartment = null;
                //var v = grdCustomReports.GetRowByListIndex(selectedDepartment.Id);
                //var Dcoloumn = (GridColumn)lookupDepartmentGrid;

            }
            //if (e.Value != null || String.IsNullOrEmpty(e.Value.ToString()))
            //{
            //    DateTime cellValue = (DateTime)e.Value;
            //    grdCustomReports.SetCellValue(e.RowHandle, "DateFrom", cellValue); 
            //}

        }
        private void SetVisibilitiesSO() 
        {
            colAmountPERsum.Visible = true;
            colSystemCost.Visible = true;
            budgetCostSER.Visible = true;
            budgetCostMER.Visible = true;
            actualCostSER.Visible = true;
            actualCostMER.Visible = true;
            totalCollectionSER.Visible = true;
            totalCollectionMER.Visible = true;
            pendingCollectionSER.Visible = true;
            pendingCollectionMER.Visible = true;
        }
        private void HideVisibilitiesSO()  
        {
            colAmountPERsum.Visible = false;
            colSystemCost.Visible = false;
            budgetCostSER.Visible = false;
            budgetCostMER.Visible = false;
            actualCostSER.Visible = false;
            actualCostMER.Visible = false;
            totalCollectionSER.Visible = false;
            totalCollectionMER.Visible = false;
            pendingCollectionSER.Visible = false;
            pendingCollectionMER.Visible = false;
        }

        private void LookupModule_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            lookupModule.SelectedText = null;
            var selectedItem = lookupModule.SelectedText;
            switch (selectedItem)
            {
                case "Inquiry":
                    
                    break;
                case "Offer":
                    
                    break;
                case "Sale_Order":
                    SetVisibilitiesSO();
                    break;
                case "Memorandum_Sale":
                    
                    break;
                case "Sale_Invoice":
                    
                    break;

                case "Purchase_Order":
                 
                    break;
                case "Purchase_Invoice":
                    
                    break;
                case "CostCenter":
                   
                    break;
                case "SummarySheet":
                   
                    break;
                case "Bill":
                   
                    break;
                case "Sale_Receipt":
                    
                    break;
                case "FixedAssets":
                   
                    break;
                case "InterBank_Transfer":
                  
                    break;
                case "Employee":
                    
                    break;
                case "Leave":
                   
                    break;
                case "JV":
                   
                    break;
                case "Admin_Bill":
                    
                    break;
                case "InterCompanyBank_Transfer":
                   
                    break;
                case "PurchaseInvoice":
                 
                    break;
                case "Payments":
                    
                    break;
                case "ToDo_Task":
                   
                    break;
            }
        }

        private void PART_GridControl_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            

        }

        private void PART_GridControl_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
           
        }
        private void GrdCustomReports_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {

        }
    }
}
