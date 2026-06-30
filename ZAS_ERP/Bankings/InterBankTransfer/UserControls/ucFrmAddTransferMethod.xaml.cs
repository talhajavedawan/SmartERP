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
    /// Interaction logic for ucFrmAddTransferMethod.xaml
    /// </summary>


    public partial class ucFrmAddTransferMethod : UserControl
    {
        public Window AddTranferMethodWin = new Window();
        public bool flagEditAdd = false;
        public bool flag = false;
        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
        public TranferMethod method = new TranferMethod();

        public ucFrmAddTransferMethod()
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
            try
            {
                if (String.IsNullOrEmpty(txtTransferMethod.Text) || String.IsNullOrWhiteSpace(txtTransferMethod.Text))
                {
                    MessageBox.Show("Transfer Method cannot be empty!");
                    return;
                }
                else
                {
                    method.MethodName = txtTransferMethod.Text;
                    method.isActive = flag;

                    if (flagEditAdd == true && method.Id != 0)
                    {
                        bankTransRepo.updatedTransferMethod(method);
                        MessageBox.Show("Transfer Method updated successfully!");
                        AddTranferMethodWin.Close();
                    }
                    else if (flagEditAdd == false && method.Id == 0)
                    {
                        bankTransRepo.addTransMethod(method);
                        MessageBox.Show("Transfer Method added successfully!");
                        AddTranferMethodWin.Close();
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
