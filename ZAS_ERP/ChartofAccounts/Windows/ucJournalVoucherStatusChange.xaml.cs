using ERP_BL.ChartofAccounts;
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
using System.Windows.Shapes;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for ucJournalVoucherStatusChange.xaml
    /// </summary>
    public partial class ucJournalVoucherStatusChange : Window
    {
        public static int inActiveStatuses;
        public static int journalVoucherid;
        public int internalJournalVoucherid;
        public static JournalVoucher voucher = new JournalVoucher();
        JournalVoucherRepo journalVoucherRepo { get; set; }


        public ucJournalVoucherStatusChange(JournalVoucherRepo _journalVoucherRepo)
        {
            InitializeComponent();
            internalJournalVoucherid = journalVoucherid;
            voucher = new JournalVoucher();

            journalVoucherRepo = _journalVoucherRepo;
        }
        public ucJournalVoucherStatusChange()
        {
            InitializeComponent();
            internalJournalVoucherid = journalVoucherid;
            voucher = new JournalVoucher();
            journalVoucherRepo = new JournalVoucherRepo();
        }
       
       
        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbJournalVoucherStatus.SelectedItem as cmbitem) != null)
            {

                JournalVoucherStatus status = journalVoucherRepo.getstatus((cmbJournalVoucherStatus.SelectedItem as cmbitem).id);
                voucher.JournalVoucherStatus = status;
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {
                MessageBox.Show("Please select New Status First");

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            voucher = journalVoucherRepo.get(internalJournalVoucherid);
            loadJournalVoucherStatus();
            if (voucher != null && voucher.Id != 0)
            {
                txtStatus.Text = voucher.JournalVoucherStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(voucher.JournalVoucherStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbJournalVoucherStatus.Items)
                {
                    if (cmbitem.name == voucher.JournalVoucherStatus.Status)
                    {
                        cmbJournalVoucherStatus.SelectedItem = cmbitem;
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
        public void loadJournalVoucherStatus()
        {
            List<JournalVoucherStatus> journalVoucherStatuses = new List<JournalVoucherStatus>();
            if (inActiveStatuses == 0)
                journalVoucherStatuses = journalVoucherRepo.getAllActiveJournalVoucherStatus();
            else
                journalVoucherStatuses = journalVoucherRepo.getAllActiveJournalVoucherStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (JournalVoucherStatus status in journalVoucherStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbJournalVoucherStatus.ItemsSource = cmbitems;
        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }
            public string bcolor { get; set; }
            public string fcolor { get; set; }
        }

        private void CmbJournalVoucherStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbJournalVoucherStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbJournalVoucherStatus.SelectedItem as cmbitem).id;
            }
        }
        public static void UpdateJournalVoucher()
        {
           JournalVoucherRepo journalVoucherRepo = new JournalVoucherRepo();
            journalVoucherRepo.updateStatusById(voucher.Id, voucher.JournalVoucherStatus.Id);
        }
    }
}
