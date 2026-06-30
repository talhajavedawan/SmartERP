using DevExpress.Xpf.Core;
using ERP_BL.Procurements.Budget;
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

namespace ZAS_ERP.Procurementss.Budget
{
    /// <summary>
    /// Interaction logic for frmBudgetSatatusChange.xaml
    /// </summary>
    public partial class frmBudgetSatatusChange : DXWindow
    {
        public frmBudgetSatatusChange(BudgetCostCenterRepo repo)
        {
            InitializeComponent();
            ZAS_ERP.Procurementss.Budget.UserControls.ucStatusChange status = new ZAS_ERP.Procurementss.Budget.UserControls.ucStatusChange(repo);
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
        public frmBudgetSatatusChange()
        {
            InitializeComponent();
            ZAS_ERP.Procurementss.Budget.UserControls.ucStatusChange status = new ZAS_ERP.Procurementss.Budget.UserControls.ucStatusChange();
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
    }

}
