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

namespace ZAS_ERP.Interest.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmInterestType.xaml
    /// </summary>
    public partial class ucFrmInterestType : UserControl
    {
        public bool editFlag = false;
        STLInterestType interestType = new STLInterestType();
        TaxRepo taxRepo = new TaxRepo();
        public int interestTypeId;

        public Window addInterestTypeWin = new Window();
        public ucFrmInterestType()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtInterestType.Text))
            {
                DXMessageBox.Show("Please enter Interest Type!");
                txtInterestType.Focus();
                return;
            }
            interestType.TypeName = txtInterestType.Text;

            if (chkIsActive.IsChecked == true)
                interestType.isActive = true;
            else
                interestType.isActive = false;

            if (editFlag == false)
            {
                taxRepo.addInterestType(interestType);
                DXMessageBox.Show("Successfully Added!");
                addInterestTypeWin.Close();
            }
            else if (editFlag == true)
            {
                taxRepo.updateInterestType(interestType);
                DXMessageBox.Show("Successfully Updated!");
                addInterestTypeWin.Close();
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                interestType = taxRepo.getInterestType(interestTypeId);

                txtInterestType.Text = interestType.TypeName;
                chkIsActive.IsChecked = interestType.isActive;
            }
        }
    }
}
