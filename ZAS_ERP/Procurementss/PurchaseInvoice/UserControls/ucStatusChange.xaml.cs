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

namespace ZAS_ERP.Procurementss.PurchaseInvoice.UserControls
{
    /// <summary>
    /// Interaction logic for ucStatusChange.xaml
    /// </summary>
    /// 
    public partial class ucStatusChange : UserControl
    {
        public static int purchaseInvoiceid;
        public static ERP_BL.Databases.PurchaseInvoice purchaseInvoice = new ERP_BL.Databases.PurchaseInvoice();
        public static PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();
        public ucStatusChange()
        {
            InitializeComponent();
        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }
            public string bcolor { get; set; }
            public string fcolor { get; set; }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            purchaseInvoice = purchaseInvoiceRepo.get(purchaseInvoiceid);
            loadPurchaseInvoiceStatus();
            if (purchaseInvoice != null && purchaseInvoice.Id != 0)
            {
                txtStatus.Text = purchaseInvoice.PurchaseInvoiceStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(purchaseInvoice.PurchaseInvoiceStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbPurchaseInvoiceStatus.Items)
                {
                    if (cmbitem.name == purchaseInvoice.PurchaseInvoiceStatus.Status)
                    {
                        cmbPurchaseInvoiceStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                //((Panel)this.Parent).Children.Remove(this);
                MessageBox.Show("Couldn't load PurchaseInvoice Data! Try again...!");
            }
        }

        public void loadPurchaseInvoiceStatus()
        {

            List<PurchaseInvoiceStatus> purchaseInvoiceStatuses = new List<PurchaseInvoiceStatus>();
            purchaseInvoiceStatuses = purchaseInvoiceRepo.getAllInActivePurchaseInvoiceStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (PurchaseInvoiceStatus status in purchaseInvoiceStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbPurchaseInvoiceStatus.ItemsSource = cmbitems;
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbPurchaseInvoiceStatus.SelectedItem as cmbitem) != null)
            {

                PurchaseInvoiceStatus status = purchaseInvoiceRepo.getstatus((cmbPurchaseInvoiceStatus.SelectedItem as cmbitem).id);
                purchaseInvoice.PurchaseInvoiceStatus = status;
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {

                MessageBox.Show("Please select New Status First");

            }
        }

        private void CmbPurchaseInvoiceStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbPurchaseInvoiceStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbPurchaseInvoiceStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmPurchaseInvoiceStatusAdd statusAdd = new frmPurchaseInvoiceStatusAdd();
                    statusAdd.ShowDialog();
                    loadPurchaseInvoiceStatus();
                }
            }
        }
    }
}
