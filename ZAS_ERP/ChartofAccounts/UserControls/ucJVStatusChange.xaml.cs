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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.ChartofAccounts.UserControls
{
    /// <summary>
    /// Interaction logic for ucJVStatusChange.xaml
    /// </summary>
    public partial class ucJVStatusChange : UserControl
    {
        public static int inActiveStatuses;
        public static int journalVoucherid;
        public int internalJournalVoucherid;
        public static JournalVoucher journalVoucher = new JournalVoucher();
        JournalVoucherRepo journalVoucherRepo { get; set; }
        
        public ucJVStatusChange(JournalVoucherRepo _journalVoucherRepo)
        {
            InitializeComponent();
            internalJournalVoucherid = journalVoucherid;
            journalVoucher = new JournalVoucher();
            journalVoucherRepo = _journalVoucherRepo;
        }
        public ucJVStatusChange()
        {
            InitializeComponent();
            internalJournalVoucherid = journalVoucherid;
            journalVoucher = new JournalVoucher();
            journalVoucherRepo = new JournalVoucherRepo();
        }
        public static void UpdateJournalVoucher()
        {
            JournalVoucherRepo JVRepo = new JournalVoucherRepo();
            JVRepo.updateStatusById(journalVoucher.Id, journalVoucher.JournalVoucherStatus.Id);
        }
        private void Updatestatus()
        {
            JournalVoucherRepo JVRepo = new JournalVoucherRepo();

            JVRepo.updateStatusById(journalVoucher.Id, journalVoucher.JournalVoucherStatus.Id);
        }
        private void CmbJournalVoucherStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbJournalVoucherStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbJournalVoucherStatus.SelectedItem as cmbitem).id;
            }
        }
        public void loadJournalVoucherStatus()
        {
            List<JournalVoucherStatus> JournalVoucherStatuses = new List<JournalVoucherStatus>();
            if (inActiveStatuses == 0)
                JournalVoucherStatuses = journalVoucherRepo.getAllActiveJournalVoucherStatus();
            else
                JournalVoucherStatuses = journalVoucherRepo.getAllInActiveJournalVoucherStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();


            foreach (JournalVoucherStatus status in JournalVoucherStatuses)
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

        private void Ucjvstatuschange_Loaded(object sender, RoutedEventArgs e)
        {
            journalVoucher = journalVoucherRepo.get(internalJournalVoucherid);
            loadJournalVoucherStatus();
            if (journalVoucher != null && journalVoucher.Id != 0)
            {
                txtStatus.Text = journalVoucher.JournalVoucherStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(journalVoucher.JournalVoucherStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbJournalVoucherStatus.Items)
                {
                    if (cmbitem.name == journalVoucher.JournalVoucherStatus.Status)
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
                MessageBox.Show("Couldn't load JournalVoucher Data! Try again...!");
            }
        }
        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbJournalVoucherStatus.SelectedItem as cmbitem) != null)
            {

                JournalVoucherStatus status = journalVoucherRepo.getstatus((cmbJournalVoucherStatus.SelectedItem as cmbitem).id);
                journalVoucher.JournalVoucherStatus = status;
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
