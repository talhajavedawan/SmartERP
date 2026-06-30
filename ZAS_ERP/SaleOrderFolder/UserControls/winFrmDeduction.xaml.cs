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
using System.Windows.Shapes;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for winFrmDeduction.xaml
    /// </summary>
    public partial class winFrmDeduction : Window
    {
        public Deduction deduction = new Deduction();
        SalesReceiptRepo repo = new SalesReceiptRepo();
        ChartofAccount chartofAccount = new ChartofAccount();

        public winFrmDeduction()
        {
            InitializeComponent();


        }
        public winFrmDeduction(Deduction dbDeduction)
        {
            InitializeComponent();
            deduction = dbDeduction;
        }

        private void ChkEdtIsActive_Unchecked(object sender, RoutedEventArgs e)
        {
            deduction.isActive = false;
        }

        private void ChkEdtIsActive_Checked(object sender, RoutedEventArgs e)
        {
            deduction.isActive = true;
        }

        private void CmbcoaAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if ( lookupcoaAccounts.SelectedIndex !=-1)
            {
                chartofAccount = lookupcoaAccounts.SelectedItem as ChartofAccount;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAccounts();
            if (deduction != null && deduction.Id != 0)
                PopulateDeduction(deduction);
        }
        public void PopulateDeduction(Deduction deduction)
        {
            if (deduction.title != null)
            {
                txtDeductionTitle.Text = deduction.title;

            }
            if (deduction.isActive == true)
            {
                chkEdtIsActive.IsChecked = true;
            }
            if (deduction.COA != null)
            {
                lookupcoaAccounts.Text = deduction.COA.accountName;
            }

        }
        public void LoadAccounts()
        {
            ChartofAccountsRepo repo = new ChartofAccountsRepo();

            var chartofAccounts = repo.GetChartofAccountsForDeductions(SYSTEM_STATIC.currentUser.id);
            lookupcoaAccounts.ItemsSource = chartofAccounts;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (deduction.Id == 0)
                {
                    if (String.IsNullOrEmpty(txtDeductionTitle.Text) || String.IsNullOrWhiteSpace(txtDeductionTitle.Text))
                    {
                        MessageBox.Show("Collection Method cannot be empty..");
                    }
                    else
                    {
                       
                        deduction.title = txtDeductionTitle.Text;

                        if (chartofAccount != null)
                            deduction.chartofAccountId = chartofAccount.Id;
                            repo.addDeduction(deduction);

                        

                        MessageBox.Show("Deduction added successfully.");
                        
                       
                        this.Close();

                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(txtDeductionTitle.Text) || String.IsNullOrWhiteSpace(txtDeductionTitle.Text))
                    {
                        MessageBox.Show("Title cannot be empty");
                    }
                    else
                    {
                        
                        deduction.title = txtDeductionTitle.Text;
                        if (chartofAccount.Id !=0)
                        {
                            deduction.chartofAccountId = chartofAccount.Id;
                            deduction.COA = null;
                        }
                        repo.updateDeduction(deduction);

                        MessageBox.Show("Deduction updated successfully.");
                        
                        ucDeductionList obj = new ucDeductionList();
                        this.Close();

                    }
                }
            }
            catch
            {

            }
        }
    }
}
