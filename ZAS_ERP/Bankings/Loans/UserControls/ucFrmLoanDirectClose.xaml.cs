using ERP_BL.Bankings;
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

namespace ZAS_ERP.Bankings.Loans.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmLoanDirectClose.xaml
    /// </summary>
    public partial class ucFrmLoanDirectClose : UserControl
    {
        LoansRepo loansRepo = new LoansRepo();
        ucLoansList loanStatus = new ucLoansList();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;

        public ucFrmLoanDirectClose()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Sale Receipt Status 
                if ((cmbLoansStatus.SelectedItem as cmbitem) != null)
                {
                    var status = loansRepo.GetLoansStatus((cmbLoansStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        //receiptStatus.statusChanged = status;
                        if (frmFlag == true)
                        {
                            ucFrmLoans frmLoansAdd = new ucFrmLoans(status);
                        }
                        else
                        {
                            ucLoansList loansStatusList = new ucLoansList(status);

                        }
                        directCloseWin.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Select a status before saving!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> loansStatusLst = new List<cmbitem>();
                var allLoansStatus = loansRepo.GetAllCloseLoansStatus();
                if (allLoansStatus != null)
                {
                    Parallel.ForEach(allLoansStatus, delegate (LoansStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                    {
                        loansStatusLst.Add
                        (new cmbitem()
                        {
                            name = status.Status,
                            id = status.Id,
                            bcolor = status.backcolor,
                            fcolor = "#FF000000"
                        });


                    });
                    cmbLoansStatus.ItemsSource = loansStatusLst;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
