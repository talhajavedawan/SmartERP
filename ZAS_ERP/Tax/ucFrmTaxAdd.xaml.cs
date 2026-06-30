using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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
    /// Interaction logic for ucFrmTaxAdd.xaml
    /// </summary>
    public partial class ucFrmTaxAdd : UserControl
    {
        public bool editFlag = false;
        TaxName taxName = new TaxName();
        TaxRepo taxRepo = new TaxRepo();
        public int taxNameId = 0;

        public Window addTaxWin = new Window();
        public ucFrmTaxAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadTaxTypes();
            loadCOA();

            if(editFlag == true && taxNameId != 0)
            {
                taxName = taxRepo.getTaxName(taxNameId);

                if(taxName.Name != null)
                    txtTaxName.Text = taxName.Name;

                txtPercent.Text = taxName.percentage.ToString();
                if(taxName.isAdjusted==true)
                {
                    checkIsAdjusted.IsChecked = true;
                }
                else
                {
                    checkIsAdjusted.IsChecked = false;
                }
                int index;

                //Select Tax Type
                var taxTypeList = (cmbxTaxType.ItemsSource as List<TaxType>) == null ? new List<TaxType>() : cmbxTaxType.ItemsSource as List<TaxType>;
                if (taxName.taxType != null)
                {
                    index = 0;
                    foreach (var _type in taxTypeList)
                    {
                        if (_type.Id == taxName.taxType.Id)
                        {
                            cmbxTaxType.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }


                //Select COA
                var coaList = (cmbxCOA.ItemsSource as List<ChartofAccount>) == null ? new List<ChartofAccount>() : cmbxCOA.ItemsSource as List<ChartofAccount>;
                if (taxName.chartofAccount != null)
                {
                    index = 0;
                    foreach (var _coa in coaList)
                    {
                        if (_coa.Id == taxName.chartofAccount.Id)
                        {
                            cmbxCOA.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtTaxName.Text))
            {
                DXMessageBox.Show("Please enter Tax Name!");
                txtTaxName.Focus();
                return;
            }
            if(cmbxTaxType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Tax Type!");
                cmbxTaxType.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtPercent.Text))
            {
                DXMessageBox.Show("Please enter Percent!");
                txtPercent.Focus();
                return;
            }
            if (cmbxCOA.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select COA!");
                cmbxCOA.Focus();
                return;
            }
            if(checkIsAdjusted.IsChecked==true)
            {
                taxName.isAdjusted = true;
            }
            else
            {
                taxName.isAdjusted = false;
            }

            taxName.Name = txtTaxName.Text;
            taxName.taxTypeId = (cmbxTaxType.SelectedItem as TaxType).Id;
            taxName.percentage = Convert.ToDouble(txtPercent.Text);
            taxName.COA_Id = (cmbxCOA.SelectedItem as ChartofAccount).Id;
            if(checkIsManual.IsChecked!=true)
            {
                taxName.isManual = false;
            }
            else
            {
                taxName.isManual = true;
            }

            if(editFlag == true && taxNameId != 0)
            {
                taxRepo.updateTax(taxName);
                DXMessageBox.Show("Successfully Updated!");
                addTaxWin.Close();
            }
            else if(editFlag == false && taxNameId == 0)
            {
                taxRepo.addTax(taxName);
                DXMessageBox.Show("Successfully Added!");
                addTaxWin.Close();
            }
        }

        private void loadTaxTypes()
        {
            cmbxTaxType.ItemsSource = taxRepo.getAllTaxType();
        }

        private void loadCOA()
        {
            ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
            cmbxCOA.ItemsSource = coaRepo.getAllforTax();
        }
    }
}