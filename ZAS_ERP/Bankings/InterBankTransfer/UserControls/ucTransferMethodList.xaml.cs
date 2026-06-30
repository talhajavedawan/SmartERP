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
    /// Interaction logic for ucTransferMethodList.xaml
    /// </summary>
    public partial class ucTransferMethodList : UserControl
    {
        public Window tranferMethodListWin = new Window();
        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
        public ucTransferMethodList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlTransferMethodList.ItemsSource = bankTransRepo.GetAllTransferMethods();
            SetColumnsVisibility();
        }

        private void RefreshData()
        {
            grdCntrlTransferMethodList.ItemsSource = bankTransRepo.GetAllTransferMethods();
            SetColumnsVisibility();
        }

        private void SetColumnsVisibility()
        {
            grdCntrlTransferMethodList.Columns["Id"].Visible = false;
        }

        private void MbtnAddTransferMethod_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAddTransferMethod frmAddTransferMethod = new ucFrmAddTransferMethod();
            frmAddTransferMethod.flagEditAdd = false;
            frmAddTransferMethod.AddTranferMethodWin.Content = frmAddTransferMethod;
            frmAddTransferMethod.AddTranferMethodWin.Height = 300;
            frmAddTransferMethod.AddTranferMethodWin.Width = 350;
            frmAddTransferMethod.AddTranferMethodWin.ResizeMode = ResizeMode.CanMinimize;
            frmAddTransferMethod.AddTranferMethodWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmAddTransferMethod.AddTranferMethodWin.ShowDialog();
            RefreshData();
        }

        private void MbtnEditTransferMethod_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAddTransferMethod frmAddTransferMethod = new ucFrmAddTransferMethod();
            var selectedRow = grdCntrlTransferMethodList.SelectedItem as TranferMethod;


            frmAddTransferMethod.flagEditAdd = true;
            frmAddTransferMethod.method = bankTransRepo.GetTransferMethod(selectedRow.Id);
            frmAddTransferMethod.AddTranferMethodWin.Content = frmAddTransferMethod;
            frmAddTransferMethod.AddTranferMethodWin.Height = 270;
            frmAddTransferMethod.AddTranferMethodWin.Width = 380;
            frmAddTransferMethod.AddTranferMethodWin.ResizeMode = ResizeMode.CanMinimize;
            frmAddTransferMethod.AddTranferMethodWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            frmAddTransferMethod.txtTransferMethod.Text = selectedRow.MethodName;
            frmAddTransferMethod.chkEdtIsActive.IsChecked = selectedRow.isActive;
            frmAddTransferMethod.AddTranferMethodWin.ShowDialog();
            RefreshData();
        }

       
    }
}
