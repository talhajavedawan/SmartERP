using DevExpress.Xpf.Core;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Tax;
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

namespace ZAS_ERP.MarginPerc.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmCashMarginType.xaml
    /// </summary>
    public partial class ucFrmCashMarginType : UserControl
    {
        public bool editFlag = false;
        MarginPercentageType marginPercType = new MarginPercentageType();
        TaxRepo taxRepo = new TaxRepo();
        public int marginPercTypeId;
        public Window addMarginPercTypeWin = new Window();

        public ucFrmCashMarginType()
        {
            InitializeComponent();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtCashMarginType.Text))
            {
                DXMessageBox.Show("Please enter Interest Type!");
                txtCashMarginType.Focus();
                return;
            }
            marginPercType.TypeName = txtCashMarginType.Text;

            if (chkIsActive.IsChecked == true)
                marginPercType.isActive = true;
            else
                marginPercType.isActive = false;

            if (editFlag == false)
            {
                taxRepo.addCashMarginType(marginPercType);
                DXMessageBox.Show("Successfully Added!");
                addMarginPercTypeWin.Close();
            }
            else if (editFlag == true)
            {
                taxRepo.updateMarginPercType(marginPercType);
                DXMessageBox.Show("Successfully Updated!");
                addMarginPercTypeWin.Close();
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                marginPercType = taxRepo.getMarginPercType(marginPercTypeId);
                txtCashMarginType.Text = marginPercType.TypeName;
                chkIsActive.IsChecked = marginPercType.isActive;
            }
        }
    }
}
