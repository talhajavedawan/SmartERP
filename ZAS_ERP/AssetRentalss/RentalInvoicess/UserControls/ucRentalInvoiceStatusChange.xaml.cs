using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals.RentalInvoices;
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

namespace ZAS_ERP.AssetRentalss.RentalInvoicess.UserControls
{
    /// <summary>
    /// Interaction logic for ucRentalInvoiceStatusChange.xaml
    /// </summary>
    public partial class ucRentalInvoiceStatusChange : UserControl
    {
        RentalInvoiceRepo rentalInvoiceRepo = new RentalInvoiceRepo();
        ucRentalInvoiceList rentalInvoiceList = new ucRentalInvoiceList();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;
        public ucRentalInvoiceStatusChange()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> billStatusLst = new List<cmbitem>();
                var allRentalsStatus = rentalInvoiceRepo.GetAllClosedStatus();
                if (allRentalsStatus != null)
                {
                    Parallel.ForEach(allRentalsStatus, delegate (RentalInvoiceStatus status) // foreach (PurchaseInvoiceStatus status in PurchaseInvoiceStatuses)
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
                    var status = rentalInvoiceRepo.GetRentalInvoiceStatus((cmbStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        //receiptStatus.statusChanged = status;
                        if (frmFlag == true)
                        {
                            ucRentalInvoiceAdd frmBillAdd = new ucRentalInvoiceAdd(status);
                        }
                        else
                        {
                            ucRentalInvoiceList rentalStatus = new ucRentalInvoiceList(status);

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
