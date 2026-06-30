using ERP_BL.Databases;
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

namespace ZAS_ERP.Bankings.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmInterBankTransStatus.xaml
    /// </summary>
    public partial class ucFrmInterBankTransStatus : UserControl
    {
        public Window FrmBankTransStatusWin = new Window();
        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
        public InterBankTransferStatus status = new InterBankTransferStatus();


        public bool saveEditFlag = false;
        public ucFrmInterBankTransStatus()
        {
            InitializeComponent();
        }

        private void CpStatus_ColorChanged(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtStatus.Text) || String.IsNullOrWhiteSpace(txtStatus.Text))
                {
                    MessageBox.Show("Collection Method cannot be empty!");
                    return;
                }
                else
                {
                    status.Status = txtStatus.Text;
                    status.backcolor = cpStatus.Text;

                    if (chkisActive.IsChecked == true)
                        status.isActive = true;
                    else
                        status.isActive = false;

                    if (saveEditFlag == true && status.Id != 0)
                    {
                        bankTransRepo.UpdateInterBankTransferStatus(status);
                        MessageBox.Show("Successfully Updated!");
                        FrmBankTransStatusWin.Close();
                    }
                    else if (saveEditFlag == false && status.Id == 0)
                    {
                        bankTransRepo.AddInterBankTransferStatus(status);
                        MessageBox.Show("Successfully Added!");
                        FrmBankTransStatusWin.Close();
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
