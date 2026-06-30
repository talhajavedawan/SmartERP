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

namespace ZAS_ERP.Procurementss.Inquiriess
{
    /// <summary>
    /// Interaction logic for ucStatuschange.xaml
    /// </summary>
    public partial class ucStatuschange : UserControl
    {
        
        public static int inquiryid;
        public static Inquiry inquiry = new Inquiry();
        public static InquiryRepo inquiryRepo = new InquiryRepo();
        public ucStatuschange()
        {
            InitializeComponent();
        }
        private void cmbInquiryStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbInquiryStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbInquiryStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmInquiryStatusAdd statusAdd = new frmInquiryStatusAdd();
                    statusAdd.ShowDialog();
                    loadInquiryStatus();
                }



            }

        }
        public void loadInquiryStatus()
        {

            List<InquiryStatus> inquiryStatuses = new List<InquiryStatus>();
            inquiryStatuses = inquiryRepo.getAllInactiveStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

           // cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (InquiryStatus status in inquiryStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbInquiryStatus.ItemsSource = cmbitems;
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
            inquiry = inquiryRepo.get(inquiryid);
            loadInquiryStatus();
            if (inquiry.Id !=0&& inquiry!=null)
            {
                txtStatus.Text = inquiry.inquiryStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(inquiry.inquiryStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbInquiryStatus.Items)
                {
                    if (cmbitem.name == inquiry.inquiryStatus.Status)
                    {

                        cmbInquiryStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                //((Panel)this.Parent).Children.Remove(this);
                //var myWindow = Window.GetWindow(this);
                //myWindow.Close();
                MessageBox.Show("Couldn't load Inquiry Data! Try again...!");
            }
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbInquiryStatus.SelectedItem as cmbitem) != null)
            {

                InquiryStatus status = inquiryRepo.getstatus((cmbInquiryStatus.SelectedItem as cmbitem).id);
                inquiry.inquiryStatus = status;
                // ((Panel)this.Parent).Children.Remove(this);
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
