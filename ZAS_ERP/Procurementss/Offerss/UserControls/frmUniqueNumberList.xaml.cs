using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.Procurementss.Offerss.UserControls
{
    /// <summary>
    /// Interaction logic for frmUniqueNumberList.xaml
    /// </summary>
    public partial class frmUniqueNumberList : DXWindow
    {
        public static UniqueNumber uniqueNumber;
        OfferRepo offerRepo = new OfferRepo();
        public frmUniqueNumberList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdUniqueNumberList.ItemsSource = offerRepo.GetAllActiveUniqueNumbers();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (grdUniqueNumberList.SelectedItem != null)
            {
                uniqueNumber = grdUniqueNumberList.SelectedItem as UniqueNumber;
                this.Close();
            }
            
        }
    }
}
