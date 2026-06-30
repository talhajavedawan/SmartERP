using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

namespace ZAS_ERP.Reportss
{
    /// <summary>
    /// Interaction logic for winVendorFinder.xaml
    /// </summary>
    public partial class winVendorFinder : DXWindow
    {
        PurchaseOrderRepo poRepo = new PurchaseOrderRepo();
        public winVendorFinder()
        {
            InitializeComponent();
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var purchaseOrders = poRepo.getAll();
            List<VendorFinder> vendorFinders = new List<VendorFinder>();
            foreach (var PO in purchaseOrders)
            {
                VendorFinder vendorFinder = new VendorFinder();
                if (PO.vendors.Count > 0)
                {
                    vendorFinder.vendor = PO.vendors[0];
                    vendorFinders.Add(vendorFinder);
                }
                if(!string.IsNullOrEmpty(PO.OwnDescription))
                {
                    vendorFinder.OwnDescription = PO.OwnDescription;
                }
            }
            grdVendorFinder.ItemsSource = vendorFinders;
        }
    }
    public class VendorFinder
    {
        public virtual Vendor vendor { get; set; }
        public string OwnDescription { get; set; }
    }
}
