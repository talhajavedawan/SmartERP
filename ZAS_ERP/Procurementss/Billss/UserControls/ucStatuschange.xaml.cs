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

namespace ZAS_ERP.Procurementss.Billss
{
    /// <summary>
    /// Interaction logic for ucStatuschange.xaml
    /// </summary>
    public partial class ucStatuschange : UserControl
    {
        public static int inActiveStatuses;
        public static int billid;
        public int internalBillid;
        public static Bill bill = new Bill();
        BillRepo billRepo { get; set; }

        public ucStatuschange(BillRepo _billRepo)
        {
            InitializeComponent();
            internalBillid = billid;
            bill = new Bill();

            billRepo = _billRepo;
        }
        public ucStatuschange()
        {
            InitializeComponent();
            internalBillid = billid;
            bill = new Bill();
            billRepo = new BillRepo();
        }
        public static void UpdateBill()
        {
            //saleORepo.updateStatus(bill.Id,bill.BillStatus);
            //saleORepo.update(bill);

            //Updatestatus();
        }

        private void Updatestatus()
        {
            BillRepo PORepo = new BillRepo();

            PORepo.updateStatusById(bill.Id, bill.BillStatus.Id);
        }

        private void cmbBillStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbBillStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbBillStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmBillStatusAdd statusAdd = new frmBillStatusAdd();
                    statusAdd.ShowDialog();
                    loadBillStatus();
                }



            }

        }
        public void loadBillStatus()
        {

            List<BillStatus> BillStatuses = new List<BillStatus>();
            if (inActiveStatuses == 0)
                BillStatuses = billRepo.getAllActiveBillStatus();
            else
                BillStatuses = billRepo.getAllInActiveBillStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (BillStatus status in BillStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbBillStatus.ItemsSource = cmbitems;
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

            bill = billRepo.get(internalBillid);
            loadBillStatus();
            if (bill != null && bill.Id != 0)
            {
                txtStatus.Text = bill.BillStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(bill.BillStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbBillStatus.Items)
                {
                    if (cmbitem.name == bill.BillStatus.Status)
                    {
                        cmbBillStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                //((Panel)this.Parent).Children.Remove(this);
                MessageBox.Show("Couldn't load Bill Data! Try again...!");
            }
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbBillStatus.SelectedItem as cmbitem) != null)
            {

                BillStatus status = billRepo.getstatus((cmbBillStatus.SelectedItem as cmbitem).id);
                bill.BillStatus = status;
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {

                MessageBox.Show("Please select New Status First");

            }
        }
    }
}
