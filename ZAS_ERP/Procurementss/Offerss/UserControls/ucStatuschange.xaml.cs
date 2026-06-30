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

namespace ZAS_ERP.Procurementss.Offerss
{
    /// <summary>
    /// Interaction logic for ucStatuschange.xaml
    /// </summary>
    public partial class ucStatuschange : UserControl
    {
        
        public static int offerid;
        public static Offer offer = new Offer();
        public static OfferRepo offerRepo = new OfferRepo();
        public ucStatuschange()
        {
            InitializeComponent();
        }
        private void cmbOfferStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbOfferStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbOfferStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmOfferStatusAdd statusAdd = new frmOfferStatusAdd();
                    statusAdd.ShowDialog();
                    loadOfferStatus();
                }



            }

        }
        public void loadOfferStatus()
        {

            List<OfferStatus> offerStatuses = new List<OfferStatus>();
            offerStatuses = offerRepo.getAllInactiveStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            foreach (OfferStatus status in offerStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }

            cmbOfferStatus.ItemsSource = cmbitems;
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
            offer = offerRepo.get(offerid);
            loadOfferStatus();
            if (offer != null && offer.Id!=0)
            {
                txtStatus.Text = offer.offerStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(offer.offerStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbOfferStatus.Items)
                {
                    if (cmbitem.name == offer.offerStatus.Status)
                    {
                        cmbOfferStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                //((Panel)this.Parent).Children.Remove(this);
                MessageBox.Show("Couldn't load Offer Data! Try again...!");
            }
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbOfferStatus.SelectedItem as cmbitem) != null)
            {

                OfferStatus status = offerRepo.getstatus((cmbOfferStatus.SelectedItem as cmbitem).id);
                offer.offerStatus = status;
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
