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

namespace ZAS_ERP.BussinessLogicss
{

    /// <summary>
    /// Interaction logic for frmStatusPanel.xaml
    /// </summary>
    public partial class frmStatusPanel : Window
    {
        public frmStatusPanel()
        {
            InitializeComponent();
            Procurementss.Inquiriess.ucStatusGrid ucInquiryStatusgrid = new Procurementss.Inquiriess.ucStatusGrid();
            gridStatus.Children.Add(ucInquiryStatusgrid);
        }
        private void lsttransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsttransactions.SelectedItem != null)
            {
                if (gridStatus != null)
                    gridStatus.Children.Clear();
                int index = lsttransactions.SelectedIndex;
                
                switch (index)
                {
                    case 0:
                        {
                            Procurementss.Inquiriess.ucStatusGrid ucInquiryStatusgrid = new Procurementss.Inquiriess.ucStatusGrid();
                            gridStatus.Children.Add(ucInquiryStatusgrid);
                            break;
                        }
                    case 1:
                        Procurementss.Offerss.ucStatusGrid ucOfferStatusgrid = new Procurementss.Offerss.ucStatusGrid();
                        gridStatus.Children.Add(ucOfferStatusgrid);
                        break;
                    case 2:
                        Procurementss.SaleOrderss.ucStatusGrid saleOrderStatusGrid = new Procurementss.SaleOrderss.ucStatusGrid();
                        gridStatus.Children.Add(saleOrderStatusGrid);
                        break;
                    case 3:
                        Procurementss.PurchaseOrderss.ucStatusGrid purchaseOrderStatusGrid = new Procurementss.PurchaseOrderss.ucStatusGrid();
                        gridStatus.Children.Add(purchaseOrderStatusGrid);
                        break;


                }
            }
            

        }
    }
}
