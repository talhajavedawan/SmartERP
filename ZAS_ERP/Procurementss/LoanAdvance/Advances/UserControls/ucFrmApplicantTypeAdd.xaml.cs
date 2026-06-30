using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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
    /// Interaction logic for ucFrmApplicantTypeAdd.xaml
    /// </summary>
    public partial class ucFrmApplicantTypeAdd : UserControl
    {
        AdvanceRepo repo = new AdvanceRepo();
        LoanApplicantType applicantType = new LoanApplicantType();
        public int typeId = 0;
        public bool editFlag = false;
        List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
        ChartofAccountsRepo chartofAccountRepo = new ChartofAccountsRepo();


        public ucFrmApplicantTypeAdd()
        {
            InitializeComponent();
        }

        public void populatefIelds()
        {
            if (editFlag == true && typeId > 0)
            {
                applicantType = repo.GetApplicantType(typeId);
                txtTypeName.Text = applicantType.TypeName;

                //if (applicantType.isActive == true)
                //    chkIsActive.IsChecked = true;
                //else
                //    chkIsActive.IsChecked = false;
                if(applicantType.account!=null)
                {
                    lookupAccounts.Text = applicantType.account.accountName;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtTypeName.Text))
            {
                DXMessageBox.Show("Please enter Type Name!");
                txtTypeName.Focus();
                return;
            }

            applicantType.TypeName = txtTypeName.Text;
            //if (chkIsActive.IsChecked == true)
            //    bank.isActive = true;
            //else
            //    bank.isActive = false;

            if(lookupAccounts.SelectedIndex>-1)            
            {
                ChartofAccount account = lookupAccounts.SelectedItem as ChartofAccount;
                applicantType.accountId = account.Id;
            }



            if (editFlag == false && typeId == 0)
            {
                repo.AddApplicantType(applicantType);
                DXMessageBox.Show("Successfully Added!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (editFlag == true && typeId != 0)
            {
                repo.UpdateApplicantType(applicantType);
                DXMessageBox.Show("Updated Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChartofAccounts();
            populatefIelds();
        }
        public void LoadChartofAccounts()
        {
            if (MainWindow.currentUserid == 0)
            {
                chartofAccounts = chartofAccountRepo.getAll();

            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Chart of Accounts") != null)
            {
                chartofAccounts = chartofAccountRepo.getAll(MainWindow.currentUserid);
            }
            else
            {
                chartofAccounts = chartofAccountRepo.getAllActive(MainWindow.currentUserid);
            }
            lookupAccounts.ItemsSource = chartofAccounts;
        }
    }
}
