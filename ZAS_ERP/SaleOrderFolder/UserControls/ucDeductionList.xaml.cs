using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using ERP_BL.ChartofAccounts;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucDeductionList.xaml
    /// </summary>
    public partial class ucDeductionList : UserControl
    {
        SalesReceiptRepo repo = new SalesReceiptRepo();

        public ucDeductionList()
        {
            InitializeComponent();
           
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //Load_Deduction();
        }


       
        private void GrdCntrlDeductionList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            

        }

        private void Save_click(object sender, RoutedEventArgs e)
        {
            try
            {
                

               
            }
            catch
            {

            }
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Deduction") != null)
                {
                    winFrmDeduction deductionForm = new winFrmDeduction();

                    deductionForm.Title = "Add Deduction Form";
                    deductionForm.ShowDialog();

                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Deductions!");
                    return;
                }
            }
            catch
            {

            }
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {

            var deduction=grdCntrlDeductionList.SelectedItem as Deduction;
            winFrmDeduction deductionForm = new winFrmDeduction(deduction);
            deductionForm.ShowDialog();
        }
        private void GrdCntrlDeductionList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SalesReceiptRepo repo = new SalesReceiptRepo();
            var deductions = repo.GetAllDeductions();
            if (deductions != null)
            {
                grdCntrlDeductionList.ItemsSource = deductions;
            }
        }
    }
}
