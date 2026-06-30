using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.Tax
{
    /// <summary>
    /// Interaction logic for ucFrmTaxType.xaml
    /// </summary>
    public partial class ucFrmTaxType : UserControl
    {
        public bool editFlag = false;
        TaxType taxType = new TaxType();
        TaxRepo taxRepo = new TaxRepo();
        public int taxTypeId;

        public Window addTaxTypeWin = new Window();
        public ucFrmTaxType()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(editFlag == true)
            {
                taxType = taxRepo.getTaxType(taxTypeId);

                txtTaxType.Text = taxType.TypeName;
                chkIsActive.IsChecked = taxType.isActive;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtTaxType.Text))
            {
                DXMessageBox.Show("Please enter Tax Type!");
                txtTaxType.Focus();
                return;
            }
            taxType.TypeName = txtTaxType.Text;

            if (chkIsActive.IsChecked == true)
                taxType.isActive = true;
            else
                taxType.isActive = false;

            if(editFlag == false)
            {
                taxRepo.addTaxType(taxType);
                DXMessageBox.Show("Successfully Added!");
                addTaxTypeWin.Close();
            }
            else if(editFlag == true)
            {
                taxRepo.updateTaxType(taxType);
                DXMessageBox.Show("Successfully Updated!");
                addTaxTypeWin.Close();
            }
                

        }

        
    }
}
