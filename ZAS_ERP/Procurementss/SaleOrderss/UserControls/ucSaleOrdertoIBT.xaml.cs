using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.Procurementss.SaleOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucSaleOrdertoIBT.xaml
    /// </summary>
    public partial class ucSaleOrdertoIBT : UserControl
    {
        SaleOrder saleOrder = new SaleOrder();
        public ucSaleOrdertoIBT()
        {
            InitializeComponent();
        }
        public ucSaleOrdertoIBT(SaleOrder _saleOrder)
        {
            InitializeComponent();
            saleOrder = _saleOrder;
        }
        private void BtnLinkIBt_Click(object sender, RoutedEventArgs e)
        {
            //SaleOrderRepo repo = new SaleOrderRepo();
            //saleOrder=repo.get(saleOrder.Id);
            //List<InterBankTransfer> ibts = new List<InterBankTransfer>();
            //foreach (InterBankTransfer ibt in grdIbt.SelectedItems)
            //{
            //    ibts.Add(ibt);
            //}
          
            //repo.update(saleOrder);
            //    DXMessageBox.Show("IBT has been linked successfully ", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            //    var myWindow = Window.GetWindow(this);
            //    myWindow.Close();
            ////}
            ////else
            ////{
            ////    DXMessageBox.Show("Total Sale Orders amount is greater than IBT value ", "Information", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            ////    return;
            ////}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           if(saleOrder.Id!=0)
            {
                txtCompany.Text = saleOrder.company.CompanyName;
                txtDeparment.Text = saleOrder.department.DeptName;
                txtSOAmount.Text = saleOrder.totalCFRValue.ToString();
                InterBankTransRepo ibtRepo = new InterBankTransRepo();
                var allDbIbts= ibtRepo.GetAllInterBankTransfersByUserID(SYSTEM_STATIC.currentUser.id);
                grdIbt.ItemsSource = allDbIbts;
                //if(allDbIbts.Count!=0)
                //{
                //    var ibts = allDbIbts.Where(x => x.dept_Id == saleOrder.dept_Id && x.company_Id == saleOrder.company_Id).ToList();
                //    grdIbt.ItemsSource = ibts;
                //}
            }
        }
    }
}
