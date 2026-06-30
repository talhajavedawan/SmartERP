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
    /// Interaction logic for ucLenderTypeList.xaml
    /// </summary>
    public partial class ucLenderTypeList : UserControl
    {
        AdvanceRepo repo = new AdvanceRepo();
        ucFrmLenderType frmApplicantTypeAdd = new ucFrmLenderType();
        public ucLenderTypeList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            repo = new AdvanceRepo();
            grdCntrlApplicantTypeList.ItemsSource = repo.GetAllLenderTypes();
        }

        private void MbtnAddApplicantType_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            frmApplicantTypeAdd = new ucFrmLenderType();
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Height = 250;
            win.Width = 350;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmApplicantTypeAdd.editFlag = false;
            win.Content = frmApplicantTypeAdd;
            win.ShowDialog();
        }

        private void MbtnEditApplicantType_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            repo = new AdvanceRepo();
            var selectedRow = grdCntrlApplicantTypeList.SelectedItem as LoanApplicantType;
            if (selectedRow != null)
            {
                frmApplicantTypeAdd = new ucFrmLenderType();
                frmApplicantTypeAdd.typeId = selectedRow.Id;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Height = 250;
                win.Width = 350;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmApplicantTypeAdd.editFlag = true;
                win.Content = frmApplicantTypeAdd;
                win.ShowDialog();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            repo = new AdvanceRepo();
            grdCntrlApplicantTypeList.ItemsSource = repo.GetAllLenderTypes();
        }

    }
}
