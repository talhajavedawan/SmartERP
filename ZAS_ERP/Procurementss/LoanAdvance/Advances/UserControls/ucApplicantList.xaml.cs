using ERP_BL.Procurements.LoansAdvances;
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

namespace ZAS_ERP.Procurementss.LoanAdvance.UserControls
{
    /// <summary>
    /// Interaction logic for ucApplicantList.xaml
    /// </summary>
    public partial class ucApplicantList : UserControl
    {
        AdvanceRepo repo = new AdvanceRepo();
        ucFrmApplicantAdd frmApplicantAdd = new ucFrmApplicantAdd();
        public ucApplicantList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            repo = new AdvanceRepo();
            var allApplicants= repo.GetAllApplicants();

            grdCntrlApplicantList.ItemsSource = allApplicants;
        }

        private void MbtnAddApplicant_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            frmApplicantAdd = new ucFrmApplicantAdd();
            win.WindowState = WindowState.Maximized;
            
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmApplicantAdd.editFlag = false;
            win.Content = frmApplicantAdd;
            win.ShowDialog();
        }

        private void MbtnEditApplicant_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            repo = new AdvanceRepo();
            var selectedRow = grdCntrlApplicantList.SelectedItem as LoanApplicant;
            if (selectedRow != null)
            {
                frmApplicantAdd = new ucFrmApplicantAdd();
                frmApplicantAdd.applicantId = selectedRow.Id;
                win.WindowState = WindowState.Maximized;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmApplicantAdd.editFlag = true;
                win.Content = frmApplicantAdd;
                win.ShowDialog();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            repo = new AdvanceRepo();
            grdCntrlApplicantList.ItemsSource = repo.GetAllApplicants();
        }


        private void GrdCntrlApplicantList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                var row = grdCntrlApplicantList.GetRowByListIndex(e.ListSourceRowIndex) as LoanApplicant;
                switch (e.Column.FieldName)
                {
                    case "Companies":
                        if(row != null && row.companies != null)
                        {
                            var companies = String.Join(" | ", row.companies.Select(x => x.CompanyName));
                            e.Value = companies;
                        }
                        break;
                    case "Departments":
                        if(row != null && row.departments != null)
                        {
                            var depts = row.departments.Where(x => row.departments.Select(y => y.ParentID).Contains(x.Id) != true);
                            var deptNames = String.Join(" | ", depts.Select(x => x.DeptName));
                            e.Value = deptNames;
                        }
                        break;
                }
            }
        }
    }
}
