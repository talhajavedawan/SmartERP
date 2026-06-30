using DevExpress.Xpf.Core;
using ERP_BL.Payments;
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

namespace ZAS_ERP.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmPaymentMethod.xaml
    /// </summary>
    public partial class ucFrmPaymentMethod : UserControl
    {
        public Window addCategoryWindow = new Window();
        PaymentRepo newBillsRepo = new PaymentRepo();
        public PaymentMethod paymentMethod = new PaymentMethod();
        public bool editFlag = false;

        public ucFrmPaymentMethod()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            populatefIelds();
        }

        public void populatefIelds()
        {
            if (editFlag == true)
            {
                txtMethodName.Text = paymentMethod.MethodName;

                if (paymentMethod.isActive == true)
                    chkIsActive.IsChecked = true;
                else
                    chkIsActive.IsChecked = false;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtMethodName.Text))
            {
                DXMessageBox.Show("Please enter Category Name!");
                txtMethodName.Focus();
                return;
            }

            paymentMethod.MethodName = txtMethodName.Text;
            if (chkIsActive.IsChecked == true)
                paymentMethod.isActive = true;
            else
                paymentMethod.isActive = false;


            if (editFlag == false && paymentMethod.Id == 0)
            {
                newBillsRepo.AddPaymentMethod(paymentMethod);
                DXMessageBox.Show("Successfully Added!");
                addCategoryWindow.Close();
            }
            else if (editFlag == true && paymentMethod.Id != 0)
            {
                newBillsRepo.UpdatePaymentMethod(paymentMethod);
                DXMessageBox.Show("Updated Successfully!");
                addCategoryWindow.Close();
            }
        }


    }
}
