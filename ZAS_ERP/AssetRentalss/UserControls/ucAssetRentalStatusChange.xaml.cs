using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals;
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
using ZAS_ERP.AssetRentalss.RentalOrderss.UserControls;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.AssetRentalss.UserControls
{
    /// <summary>
    /// Interaction logic for ucAssetRentalStatusChange.xaml
    /// </summary>
    public partial class ucAssetRentalStatusChange : UserControl
    {
        AssetRentalRepo assetRentalRepo = new AssetRentalRepo();
        ucAssetRentalsList rentalOrderList = new ucAssetRentalsList();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;
        public ucAssetRentalStatusChange()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> billStatusLst = new List<cmbitem>();
                var allAssetStatus = assetRentalRepo.GetAllAssetRentalClosedStatus();
                if (allAssetStatus != null)
                {
                    Parallel.ForEach(allAssetStatus, delegate (AssetRentalStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
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
                    var status = assetRentalRepo.GetAssetRentalStatus((cmbStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        //receiptStatus.statusChanged = status;
                        if (frmFlag == true)
                        {
                            ucFrmAddAssetRental frmBillAdd = new ucFrmAddAssetRental(status);
                        }
                        else
                        {
                            ucFrmAddAssetRental rentalStatus = new ucFrmAddAssetRental(status);

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
