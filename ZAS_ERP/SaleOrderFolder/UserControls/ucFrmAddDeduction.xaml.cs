using ERP_BL.ChartofAccounts;
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
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddDeduction.xaml
    /// </summary>
    public partial class ucFrmAddDeduction : UserControl
    {
        public UcListWindow addDeductionWin = new UcListWindow();
        public UcListWindow updateDeductionWin = new UcListWindow();

        public int flagEditAdd;
        public int Id;
        bool flag = false;

        SalesReceiptRepo repo = new SalesReceiptRepo();


        public ucFrmAddDeduction()
        {
            InitializeComponent();
        }

        private void ChkEdtIsActive_Checked(object sender, RoutedEventArgs e)
        {
            flag = true;
        }

        private void ChkEdtIsActive_Unchecked(object sender, RoutedEventArgs e)
        {
            flag = false;

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (flagEditAdd == 0)
            //    {
            //        if (String.IsNullOrEmpty(txtDeductionTitle.Text) || String.IsNullOrWhiteSpace(txtDeductionTitle.Text))
            //        {
            //            MessageBox.Show("Collection Method cannot be empty..");
            //        }
            //        else
            //        {
            //            Deduction deduction = new Deduction();
            //            deduction.title = txtDeductionTitle.Text;
            //            deduction.isActive = flag;

            //            repo.addDeduction(deduction);


            //            MessageBox.Show("Deduction added successfully.");
            //            GetAllDeductions collectMthd = new GetAllDeductions();
            //            ucDeductionList obj = new ucDeductionList();
            //            obj.grdCntrlDeductionList.ItemsSource = collectMthd.DeductionsList;
            //            addDeductionWin.Close();
            //        }
            //    }
            //    else
            //    {
            //        if (String.IsNullOrEmpty(txtDeductionTitle.Text) || String.IsNullOrWhiteSpace(txtDeductionTitle.Text))
            //        {
            //            MessageBox.Show("Title cannot be empty");
            //        }
            //        else
            //        {
            //            Deduction deduction = new Deduction();
            //            deduction.Id = Id;
            //            deduction.title = txtDeductionTitle.Text;
            //            deduction.isActive = flag;

            //            repo.updateDeduction(deduction);

            //            //updateCollectionMethodFrm = 0;
            //            MessageBox.Show("Deduction updated successfully.");
            //            GetAllDeductions deductions = new GetAllDeductions();
            //            ucDeductionList obj = new ucDeductionList();
            //            obj.grdCntrlDeductionList.ItemsSource = deductions.DeductionsList;
            //            updateDeductionWin.Close();

            //        }
            //    }
            //}
            //catch
            //{

            //}
           


        }
        public void LoadAccounts()
        {
            ChartofAccountsRepo repo = new ChartofAccountsRepo();

            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            if (MainWindow.currentUserid == 0)
            {
                chartofAccounts = repo.getAll();

            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Chart of Accounts") != null)
            {
                chartofAccounts = repo.getAll(MainWindow.currentUserid);
            }
            else
            {
                chartofAccounts = repo.getAllActive(MainWindow.currentUserid);
            }
            cmbcoaAccounts.ItemsSource = chartofAccounts;
        }
        private void CmbcoaAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAccounts();
        }
    }
}

