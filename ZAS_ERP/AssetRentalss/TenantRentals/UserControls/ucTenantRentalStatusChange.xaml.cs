using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals.TenantRentals;
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
using ZAS_ERP.AssetRentalss.UserControls;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.AssetRentalss.TenantRentals
{
    /// <summary>
    /// Interaction logic for ucTenantRentalStatusChange.xaml
    /// </summary>
    public partial class ucTenantRentalStatusChange : UserControl
    {
        TenantRentalRepo tenantRentalRepo = new TenantRentalRepo();
        ucTenantRentalList tenantRentalList = new ucTenantRentalList();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;
        public ucTenantRentalStatusChange()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> billStatusLst = new List<cmbitem>();
                var allTenantStatus = tenantRentalRepo.GetAllTenantRentalClosedStatus();
                if (allTenantStatus != null)
                {
                    Parallel.ForEach(allTenantStatus, delegate (TenantRentalStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
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
                    cmbStatus.ItemsSource = billStatusLst;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if ((cmbStatus.SelectedItem as cmbitem) != null)
                {
                    var status = tenantRentalRepo.GetTenantRentalStatus((cmbStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        //receiptStatus.statusChanged = status;
                        if (frmFlag == true)
                        {
                            ucFrmTenantRentalAdd frmBillAdd = new ucFrmTenantRentalAdd(status);
                        }
                        else
                        {
                            ucTenantRentalList rentalStatus = new ucTenantRentalList(status);
                        }
                        directCloseWin.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Select a status before saving!");
                }




            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }
    }
}
