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
    /// Interaction logic for ucPaymentMethodList.xaml
    /// </summary>
    public partial class ucPaymentMethodList : UserControl
    {
        PaymentRepo paymentRepo = new PaymentRepo();
        ucFrmPaymentMethod frmPaymentMethod = new ucFrmPaymentMethod();
        public ucPaymentMethodList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            paymentRepo = new PaymentRepo();
            grdCntrlPaymentMethodList.ItemsSource = paymentRepo.GetAllPaymentMethods();
        }

        private void MbtnAddPaymentMethod_Click(object sender, RoutedEventArgs e)
        {
            frmPaymentMethod = new ucFrmPaymentMethod();
            frmPaymentMethod.addCategoryWindow.ResizeMode = ResizeMode.CanMinimize;
            frmPaymentMethod.addCategoryWindow.Height = 250;
            frmPaymentMethod.addCategoryWindow.Width = 350;
            frmPaymentMethod.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmPaymentMethod.editFlag = false;
            frmPaymentMethod.addCategoryWindow.Content = frmPaymentMethod;
            frmPaymentMethod.addCategoryWindow.ShowDialog();
        }

        private void MbtnEditPaymentMethod_Click(object sender, RoutedEventArgs e)
        {
            paymentRepo = new PaymentRepo();
            var selectedRow = grdCntrlPaymentMethodList.SelectedItem as PaymentMethod;
            if (selectedRow != null)
            {
                frmPaymentMethod = new ucFrmPaymentMethod();
                frmPaymentMethod.paymentMethod = paymentRepo.GetPaymentMethod(selectedRow.Id);
                frmPaymentMethod.addCategoryWindow.ResizeMode = ResizeMode.CanMinimize;
                frmPaymentMethod.addCategoryWindow.Height = 250;
                frmPaymentMethod.addCategoryWindow.Width = 350;
                frmPaymentMethod.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmPaymentMethod.editFlag = true;
                frmPaymentMethod.addCategoryWindow.Content = frmPaymentMethod;
                frmPaymentMethod.addCategoryWindow.ShowDialog();
            }

        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            paymentRepo = new PaymentRepo();
            grdCntrlPaymentMethodList.ItemsSource = paymentRepo.GetAllPaymentMethods();
        }
    }
}
