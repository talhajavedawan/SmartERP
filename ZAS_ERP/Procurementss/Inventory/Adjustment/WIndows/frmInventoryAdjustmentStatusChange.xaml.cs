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
using ZAS_ERP.Procurementss.Inventory.Adjustment.UserControls;

namespace ZAS_ERP.Procurementss.Inventory.Adjustment.WIndows
{
    /// <summary>
    /// Interaction logic for frmInventoryAdjustmentStatusChange.xaml
    /// </summary>
    public partial class frmInventoryAdjustmentStatusChange : Window
    {
        AdjustmentRepo repo = new AdjustmentRepo();
        public frmInventoryAdjustmentStatusChange()
        {
            InitializeComponent();
            ZAS_ERP.Procurementss.Inventory.Adjustment.UserControls.ucStatusChange status = new ZAS_ERP.Procurementss.Inventory.Adjustment.UserControls.ucStatusChange();
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
        public frmInventoryAdjustmentStatusChange(AdjustmentRepo repo)
        {
            InitializeComponent();
            ZAS_ERP.Procurementss.Inventory.Adjustment.UserControls.ucStatusChange status = new ZAS_ERP.Procurementss.Inventory.Adjustment.UserControls.ucStatusChange();
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
    }
}
