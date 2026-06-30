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
using System.Xml.Linq;

namespace ZAS_ERP.Interest.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmInterestAdd.xaml
    /// </summary>
    public partial class ucFrmInterestAdd : UserControl
    {
        public bool editFlag = false;
        STLInterest interestName = new STLInterest();
        TaxRepo taxRepo = new TaxRepo();
        public int interestNameId = 0;
        public Window addInterestWin = new Window();

        public ucFrmInterestAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadTaxTypes();
            loadCOA();

            if (editFlag == true && interestNameId != 0)
            {
                interestName = taxRepo.getInterestName(interestNameId);

                if (interestName.Name != null)
                    txtInterestName.Text = interestName.Name;

                txtPercent.Text = interestName.percentage.ToString();
                if (interestName.isAdjusted == true)
                {
                    checkIsAdjusted.IsChecked = true;
                }
                else
                {
                    checkIsAdjusted.IsChecked = false;
                }
                int index;

                //Select Tax Type
                var interestTypeList = (cmbxInterestType.ItemsSource as List<TaxType>) == null ? new List<TaxType>() : cmbxInterestType.ItemsSource as List<TaxType>;
                if (interestName.interestType != null)
                {
                    index = 0;
                    foreach (var _type in interestTypeList)
                    {
                        if (_type.Id == interestName.interestType.Id)
                        {
                            cmbxInterestType.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }


                //Select COA
                var coaList = (cmbxCOA.ItemsSource as List<ChartofAccount>) == null ? new List<ChartofAccount>() : cmbxCOA.ItemsSource as List<ChartofAccount>;
                if (interestName.chartofAccount != null)
                {
                    index = 0;
                    foreach (var _coa in coaList)
                    {
                        if (_coa.Id == interestName.chartofAccount.Id)
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
            if (String.IsNullOrEmpty(txtInterestName.Text))
            {
                DXMessageBox.Show("Please enter Interest Name!");
                txtInterestName.Focus();
                return;
            }
            if (cmbxInterestType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Interest Type!");
                cmbxInterestType.Focus();
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
                interestName.isAdjusted = true;
            }
            else
            {
                interestName.isAdjusted = false;
            }

            interestName.Name = txtInterestName.Text;
            interestName.interestTypeId = (cmbxInterestType.SelectedItem as STLInterestType).Id;
            interestName.percentage = Convert.ToDouble(txtPercent.Text);
            interestName.COA_Id = (cmbxCOA.SelectedItem as ChartofAccount).Id;
            if (checkIsManual.IsChecked != true)
            {
                interestName.isManual = false;
            }
            else
            {
                interestName.isManual = true;
            }

            if (editFlag == true && interestNameId != 0)
            {
                taxRepo.updateInterest(interestName);
                DXMessageBox.Show("Successfully Updated!");
                addInterestWin.Close();
            }
            else if (editFlag == false && interestNameId == 0)
            {
                taxRepo.addInterest(interestName);
                DXMessageBox.Show("Successfully Added!");
                addInterestWin.Close();
            }
        }
        private void loadTaxTypes()
        {
            cmbxInterestType.ItemsSource = taxRepo.getAllInterestType();
        }

        private void loadCOA()
        {
            ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
            cmbxCOA.ItemsSource = coaRepo.getAllforTax();
        }
    }
}
