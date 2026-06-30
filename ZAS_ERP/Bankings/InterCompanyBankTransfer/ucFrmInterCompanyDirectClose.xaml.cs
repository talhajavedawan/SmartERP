using ERP_BL.Procurements;
using ERP_BL.Procurements.InterBankTransfers;
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
using ZAS_ERP.Bankings.InterCompanyBankTransfer;
using ZAS_ERP.Bankings.UserControls;

namespace ZAS_ERP.Bankings
{
    /// <summary>
    /// Interaction logic for ucFrmInterCompanyDirectClose.xaml
    /// </summary>
    public partial class ucFrmInterCompanyDirectClose : UserControl      
    {

        InterBankTransRepo repo = new InterBankTransRepo();
        InterCompanyBankTransferRepo transferRepo = new InterCompanyBankTransferRepo();
        ucInterCompBankTransRegister ucInterCompBankTrans = new ucInterCompBankTransRegister();
        public Window directCloseWin = new Window();
        public InterBankTransferStatus IBTstatus = new InterBankTransferStatus();
        public bool IBTflag = false;
        public ucFrmInterCompanyDirectClose()

        {
            InitializeComponent();
            try
            {
                List<cmbitem> interBankTransStatusLst = new List<cmbitem>();
                var allClosedInterBankTransStatus = repo.GetAllCloseInterBankTransferStatus();
                if (allClosedInterBankTransStatus != null)
                {
                    Parallel.ForEach(allClosedInterBankTransStatus, delegate (InterBankTransferStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                    {
                        interBankTransStatusLst.Add
                        (new cmbitem()
                        {
                            name = status.Status,
                            id = status.Id,
                            bcolor = status.backcolor,
                            fcolor = "#FF000000"
                        });


                    });
                    cmbInterBankTransStatus.ItemsSource = interBankTransStatusLst;
                }
                 
            } 
        
            catch
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Sale Receipt Status 
                if (IBTflag == false)
                {
                    if ((cmbInterBankTransStatus.SelectedItem as cmbitem) != null)
                    {
                        var status = repo.GetInterBankTransStatus((cmbInterBankTransStatus.SelectedItem as cmbitem).id);
                        if (status != null)
                        {
                            //receiptStatus.statusChanged = status;
                            ucInterCompBankTransRegister interBankStatus = new ucInterCompBankTransRegister(status);
                          //  ucBankTransferRegister interBankStatus = new ucBankTransferRegister(status);

                            directCloseWin.Close();
                            //IBstatus = status;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select a status before saving!");
                    }
                }
                else if (IBTflag == true)
                {
                    if ((cmbInterBankTransStatus.SelectedItem as cmbitem) != null)
                    {
                        var status = repo.GetInterBankTransStatus((cmbInterBankTransStatus.SelectedItem as cmbitem).id);
                        if (status != null)
                        {
                            //receiptStatus.statusChanged = status;
                            ucBankTransferInterCompany interBankStatus = new ucBankTransferInterCompany(status);

                            directCloseWin.Close();
                            //IBstatus = status;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select a status before saving!");
                    }
                }

            }
            catch
            {

            }


        }
    }
}
