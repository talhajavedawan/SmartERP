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

namespace ZAS_ERP.Procurementss.SaleInvoicess
{
    /// <summary>
    /// Interaction logic for ucStatuschange.xaml
    /// </summary>
    public partial class ucStatuschange : UserControl
    {
        
        public static int saleInvoiceid;
        public static SaleInvoice saleInvoice = new SaleInvoice();
        public static SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
        List<ERP_BL.Procurements.StatusClass.StatusClass> StatusClasses = new List<ERP_BL.Procurements.StatusClass.StatusClass>();

        public ucStatuschange()
        {
            InitializeComponent();
        }
        private void cmbSaleInvoiceStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbSaleInvoiceStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbSaleInvoiceStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmSaleInvoiceStatusAdd statusAdd = new frmSaleInvoiceStatusAdd();
                    statusAdd.ShowDialog();
                    loadSaleInvoiceStatus();

                }
                else
                {
                    loadSaleInvoiceStatusClass(idd);
                }
            }
        }
        public void loadSaleInvoiceStatusClass(int _statusId)
        {
            cmbSaleOrderStatusClass.ItemsSource = null;
            var status = saleInvoiceRepo.getstatus(_statusId);
            if (status.siStatusSubClasses.Count > 0)
            {
                StatusClasses.Clear();
                StatusClasses.AddRange(status.siStatusSubClasses.Where(x => x.isDisable != true).ToList());
                List<cmbitem> cmbitems = new List<cmbitem>();
                Parallel.ForEach(StatusClasses, delegate (ERP_BL.Procurements.StatusClass.StatusClass subClass)
                {
                    cmbitems.Add(new cmbitem() { name = subClass.ClassName, id = subClass.Id, bcolor = subClass.backcolor, fcolor = "#FF000000" });
                });
                cmbSaleOrderStatusClass.ItemsSource = cmbitems;
            }

        }
        public void loadSaleInvoiceStatus()
        {

            List<SaleInvoiceStatus> saleInvoiceStatuses = new List<SaleInvoiceStatus>();
            saleInvoiceStatuses = saleInvoiceRepo.getAllInActiveSaleInvoiceStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (SaleInvoiceStatus status in saleInvoiceStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbSaleInvoiceStatus.ItemsSource = cmbitems;
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
            saleInvoice = saleInvoiceRepo.get(saleInvoiceid);
            loadSaleInvoiceStatus();
            if (saleInvoice != null && saleInvoice.Id!=0)
            {
                txtStatus.Text = saleInvoice.saleInvoiceStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(saleInvoice.saleInvoiceStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbSaleInvoiceStatus.Items)
                {
                    if (cmbitem.name == saleInvoice.saleInvoiceStatus.Status)
                    {
                        cmbSaleInvoiceStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                MessageBox.Show("Couldn't load SaleInvoice Data! Try again...!");
            }
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbSaleInvoiceStatus.SelectedItem as cmbitem) != null)
            {

                SaleInvoiceStatus status = saleInvoiceRepo.getstatus((cmbSaleInvoiceStatus.SelectedItem as cmbitem).id);
                saleInvoice.saleInvoiceStatus = status;
                if (cmbSaleOrderStatusClass.SelectedIndex > -1)
                {
                    ERP_BL.Procurements.StatusClass.StatusClass statusClass = saleInvoiceRepo.GetStatusClass((cmbSaleOrderStatusClass.SelectedItem as cmbitem).id);
                    saleInvoice.StatusClass = new ERP_BL.Procurements.StatusClass.StatusClass();
                    saleInvoice.StatusClass = statusClass;
                }
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {
                MessageBox.Show("Please select New Status First");
            }
        }

        private void cmbSaleOrderStatusClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
