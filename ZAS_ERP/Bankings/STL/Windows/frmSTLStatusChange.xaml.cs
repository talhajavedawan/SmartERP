using ERP_BL.Procurements.InterBankTransfers;
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

namespace ZAS_ERP.Bankings.STL.Windows
{
    /// <summary>
    /// Interaction logic for frmSTLStatusChange.xaml
    /// </summary>
    public partial class frmSTLStatusChange : Window
    {
       
        public frmSTLStatusChange(STLRepo repo)
        {
            InitializeComponent();
             ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange  status = new ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange(repo);
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
        public frmSTLStatusChange()
        {
            InitializeComponent();
            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange status = new ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange();
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
    }
}
