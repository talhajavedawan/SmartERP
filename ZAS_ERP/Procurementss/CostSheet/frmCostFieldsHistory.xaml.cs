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
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for frmCostFieldsHistory.xaml
    /// </summary>
    public partial class frmCostFieldsHistory : Window
    {
        public frmCostFieldsHistory(List<CostFieldHistory> costFieldHistories)
        {
            InitializeComponent();
            grdCostItems.ItemsSource = costFieldHistories;
        }
        public frmCostFieldsHistory()
        {
            InitializeComponent();
        }
    }
}
