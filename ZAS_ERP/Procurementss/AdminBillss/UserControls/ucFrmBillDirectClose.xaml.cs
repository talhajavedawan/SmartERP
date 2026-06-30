using ERP_BL.Databases;
using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmBillDirectClose.xaml
    /// </summary>
    public partial class ucFrmBillDirectClose : UserControl
    {
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        ucBillList billStatus = new ucBillList();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;

        public ucFrmBillDirectClose()
        {
            InitializeComponent();

            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> billStatusLst = new List<cmbitem>();
                var allBillsStatus = billsRepo.GetAllCloseBillStatus();
                if (allBillsStatus != null)
                {
                    Parallel.ForEach(allBillsStatus, delegate (AdminBillStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                    {
                        billStatusLst.Add
                        (new cmbitem()
                        {
                            name = status.Status,
                            id = status.Id,
                            bcolor = status.backcolor,
                            fcolor = "#FF000000"
                        });


                    });
                    cmbBillStatus.ItemsSource = billStatusLst;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Sale Receipt Status 
                if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                {
                    var status = billsRepo.GetBillStatus((cmbBillStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        //receiptStatus.statusChanged = status;
                        if(frmFlag == true)
                        {
                            ucFrmBillAdd frmBillAdd = new ucFrmBillAdd(status);
                        }
                        else
                        {
                            ucBillList receiptStatus = new ucBillList(status);

                        }
                        directCloseWin.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Select a status before saving!");
                }
            }
            catch
            {

            }


        }

        
    }
}
