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

namespace ZAS_ERP.Procurementss.PurchaseOrderss
{
    /// <summary>
    /// Interaction logic for ucStatuschange.xaml
    /// </summary>
    public partial class ucStatuschange : UserControl
    {
        public static int inActiveStatuses;
        public static int purchaseOrderid;
        public int internalPurchaseOrderid;
        public static PurchaseOrder purchaseOrder = new PurchaseOrder();
        PurchaseOrderRepo purchaseOrderRepo { get; set; }

        public ucStatuschange(PurchaseOrderRepo _purchaseOrderRepo)
        {
            InitializeComponent();
            internalPurchaseOrderid = purchaseOrderid;
            purchaseOrder = new PurchaseOrder();

            purchaseOrderRepo = _purchaseOrderRepo;
        }
        public ucStatuschange()
        {
            InitializeComponent();
            internalPurchaseOrderid = purchaseOrderid;
            purchaseOrder = new PurchaseOrder();
            purchaseOrderRepo = new PurchaseOrderRepo();
        }
        public static void UpdatePurchaseOrder()
        {
            PurchaseOrderRepo PORepo = new PurchaseOrderRepo();
            PORepo.updateStatusById(purchaseOrder.Id, purchaseOrder.PurchaseOrderStatus.Id);
            //saleORepo.updateStatus(purchaseOrder.Id,purchaseOrder.PurchaseOrderStatus);
            //saleORepo.update(purchaseOrder);

            //Updatestatus();
        }

        private void Updatestatus()
        {
            PurchaseOrderRepo PORepo = new PurchaseOrderRepo();

            PORepo.updateStatusById(purchaseOrder.Id, purchaseOrder.PurchaseOrderStatus.Id);
        }

        private void cmbPurchaseOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbPurchaseOrderStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbPurchaseOrderStatus.SelectedItem as cmbitem).id;
                //if (idd == 0)
                //{
                //    frmPurchaseOrderStatusAdd statusAdd = new frmPurchaseOrderStatusAdd();
                //    statusAdd.ShowDialog();
                //    loadPurchaseOrderStatus();
                //}



            }

        }
        public void loadPurchaseOrderStatus()
        {

            List<PurchaseOrderStatus> PurchaseOrderStatuses = new List<PurchaseOrderStatus>();
            if (inActiveStatuses == 0)
                PurchaseOrderStatuses = purchaseOrderRepo.getAllActivePurchaseOrderStatus();
            else
                PurchaseOrderStatuses = purchaseOrderRepo.getAllInActivePurchaseOrderStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbPurchaseOrderStatus.ItemsSource = cmbitems;
        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }
            public string bcolor { get; set; }
            public string fcolor { get; set; }
        }

        private void ucinqstatuschange_Loaded(object sender, RoutedEventArgs e)
        {

            purchaseOrder = purchaseOrderRepo.get(internalPurchaseOrderid);
            loadPurchaseOrderStatus();
            if (purchaseOrder != null && purchaseOrder.Id != 0)
            {
                txtStatus.Text = purchaseOrder.PurchaseOrderStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(purchaseOrder.PurchaseOrderStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbPurchaseOrderStatus.Items)
                {
                    if (cmbitem.name == purchaseOrder.PurchaseOrderStatus.Status)
                    {
                        cmbPurchaseOrderStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                //((Panel)this.Parent).Children.Remove(this);
                MessageBox.Show("Couldn't load PurchaseOrder Data! Try again...!");
            }
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbPurchaseOrderStatus.SelectedItem as cmbitem) != null)
            {

                PurchaseOrderStatus status = purchaseOrderRepo.getstatus((cmbPurchaseOrderStatus.SelectedItem as cmbitem).id);
                purchaseOrder.PurchaseOrderStatus = status;
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {

                MessageBox.Show("Please select New Status First");

            }
        }
        internal static void UpdatePurchaseOrderStatusforInvoice()
        {
            PurchaseOrderRepo repo = new PurchaseOrderRepo();
            repo.updateStatusById(purchaseOrder.Id, purchaseOrder.PurchaseOrderStatus.Id);
        }
       
    }
}
