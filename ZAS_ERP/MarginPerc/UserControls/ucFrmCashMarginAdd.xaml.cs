using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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
    /// Interaction logic for ucFrmCashMarginAdd.xaml
    /// </summary>
    public partial class ucFrmCashMarginAdd : UserControl
    {
        public bool editFlag = false;
        MarginPercentage marginName = new MarginPercentage();
        TaxRepo taxRepo = new TaxRepo();
        public int marginNameId = 0;
        public Window addMarginWin = new Window();

        public ucFrmCashMarginAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadMarginTypes();
            loadCOA();

            if (editFlag == true && marginNameId != 0)
            {
                marginName = taxRepo.getMarginName(marginNameId);

                if (marginName.Name != null)
                    txtMarginName.Text = marginName.Name;

                txtPercent.Text = marginName.percentage.ToString();
                if (marginName.isAdjusted == true)
                {
                    checkIsAdjusted.IsChecked = true;
                }
                else
                {
                    checkIsAdjusted.IsChecked = false;
                }
                int index;

                //Select Tax Type
                var interestTypeList = (cmbxMarginType.ItemsSource as List<TaxType>) == null ? new List<TaxType>() : cmbxMarginType.ItemsSource as List<TaxType>;
                if (marginName.marginpercentageType != null)
                {
                    index = 0;
                    foreach (var _type in interestTypeList)
                    {
                        if (_type.Id == marginName.marginpercentageType.Id)
                        {
                            cmbxMarginType.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }


                //Select COA
                var coaList = (cmbxCOA.ItemsSource as List<ChartofAccount>) == null ? new List<ChartofAccount>() : cmbxCOA.ItemsSource as List<ChartofAccount>;
                if (marginName.chartofAccount != null)
                {
                    index = 0;
                    foreach (var _coa in coaList)
                    {
                        if (_coa.Id == marginName.chartofAccount.Id)
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
            if (String.IsNullOrEmpty(txtMarginName.Text))
            {
                DXMessageBox.Show("Please enter Margin Percentage Name!");
                txtMarginName.Focus();
                return;
            }
            if (cmbxMarginType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Margin Percentage Type!");
                cmbxMarginType.Focus();
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
            if (checkIsAdjusted.IsChecked == true)
            {
                marginName.isAdjusted = true;
            }
            else
            {
                marginName.isAdjusted = false;
            }

            marginName.Name = txtMarginName.Text;
            marginName.marginpercentageTypeId = (cmbxMarginType.SelectedItem as MarginPercentageType).Id;
            marginName.percentage = Convert.ToDouble(txtPercent.Text);
            marginName.COA_Id = (cmbxCOA.SelectedItem as ChartofAccount).Id;
            if (checkIsManual.IsChecked != true)
            {
                marginName.isManual = false;
            }
            else
            {
                marginName.isManual = true;
            }

            if (editFlag == true && marginNameId != 0)
            {
                taxRepo.updateMarginPerc(marginName);
                DXMessageBox.Show("Successfully Updated!");
                addMarginWin.Close();
            }
            else if (editFlag == false && marginNameId == 0)
            {
                taxRepo.addMarginPercentage(marginName);
                DXMessageBox.Show("Successfully Added!");
                addMarginWin.Close();
            }
        }
        private void loadMarginTypes()
        {
            cmbxMarginType.ItemsSource = taxRepo.getAllMarginPercType();
        }

        private void loadCOA()
        {
            ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
            cmbxCOA.ItemsSource = coaRepo.getAllforTax();
        }
    }
}
