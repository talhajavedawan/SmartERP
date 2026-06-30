using DevExpress.Xpf.Grid;
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
using ERP_BL;

using ERP_BL.Config;
using ERP_BL.Enums;

namespace ZAS_ERP.Termss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmPaymentTermList : Window
    {


        List<PaymentTerm> paymentTerms= new List<PaymentTerm>();
        PaymentTermRepo repo = new PaymentTermRepo();

        public frmPaymentTermList()
        {
            InitializeComponent();
                       
        }

        private void winPaymentTermList_Loaded(object sender, RoutedEventArgs e)
        {
            loadPaymentTerm();
            //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPaymentTerm);


        }



        private void loadPaymentTerm()
        {
           
            if (MainWindow.currentUserid == 0)
                paymentTerms = repo.getAll();
            else
                paymentTerms = repo.getActivePaymentTerm();
                       
            this.grdPaymentTerm.ItemsSource = paymentTerms;
            

        }

        public void newPaymentTerm()
        {
            Termss.frmPaymentTermAdd frmPaymentTermadd = new Termss.frmPaymentTermAdd();
            frmPaymentTermadd.ShowDialog();
            loadPaymentTerm();
        }
        

        private void mbtnNewPaymentTerm_Click(object sender, RoutedEventArgs e)
        {
            newPaymentTerm();

        }

        private void mbtnEditPaymentTerm_Click(object sender, RoutedEventArgs e)
        {
            if (grdPaymentTerm.SelectedItem != null)
            {
                Termss.frmPaymentTermAdd.paymentTermId = (grdPaymentTerm.SelectedItem as PaymentTerm).Id;
                newPaymentTerm();
            }
            else
            {
                MessageBox.Show("Please select a Payment Term to Edit");
            }
        }

        private void grdPaymentTerm_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Termss.frmPaymentTermAdd.paymentTermId = (grdPaymentTerm.SelectedItem as PaymentTerm).Id;
            newPaymentTerm();
        }

        private void WinPaymentTermList_Unloaded(object sender, RoutedEventArgs e)
        {
            //SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdPaymentTerm);
        }
    }
}
