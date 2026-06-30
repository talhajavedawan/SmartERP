using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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

namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for ucFrmEmployeeDataRetrievalDateAdd.xaml
    /// </summary>
    public partial class ucFrmEmployeeDataRetrievalDateAdd : UserControl
    {
        List<ERP_BL.Databases.Employee> Employees = new List<ERP_BL.Databases.Employee>();
        EmployeeRepo repo = new EmployeeRepo();

        public ucFrmEmployeeDataRetrievalDateAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Employees = repo.GetAllEmployees();
            grdCntrlDataRetrievalDates.ItemsSource = Employees;
        }

        private void grdCntrlDataRetrievalDates_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void grdCntrlDataRetrievalDates_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
           
        }

        private void tableViewDataRetrievalDates_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "RetrievalDate")
            {
                var grid = sender as TableView;
                if (grid != null)
                {
                    var row = e.Row; // Get the current row
                    if (row != null)
                    {
                        if (e.Value == null || e.Value == DBNull.Value)
                        {
                            row.GetType().GetProperty("InquiryDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("OfferDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("SODataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("MemorandumSaleDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("SIDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("PODataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("PIDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("VendorBillDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("SRDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("FixedAssetsDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("IBTDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("AdminBillDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("PaymentDataRetrievalDate")?.SetValue(row, null);
                            row.GetType().GetProperty("LoansAdvancesDataRetrievalDate")?.SetValue(row, null);
                            grid.Grid.RefreshData();
                            return; // Exit if the value is null to prevent exception
                        }

                        if (DateTime.TryParse(e.Value.ToString(), out DateTime newDate))
                        {
                            // Update all other date fields
                            row.GetType().GetProperty("InquiryDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("OfferDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("SODataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("MemorandumSaleDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("SIDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("PODataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("PIDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("VendorBillDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("SRDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("FixedAssetsDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("IBTDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("AdminBillDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("PaymentDataRetrievalDate")?.SetValue(row, newDate);
                            row.GetType().GetProperty("LoansAdvancesDataRetrievalDate")?.SetValue(row, newDate);

                            // Refresh the row to show updated values
                            grid.Grid.RefreshData();
                        }
                    }
                }
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            repo.UpdateMultipleEmployees(Employees);

            DXMessageBox.Show("Employees Updated!");

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        
    }
}
