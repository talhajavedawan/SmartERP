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
using DevExpress.Xpf.Core;
using ERP_BL;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for frmCostSheetFieldAdd.xaml
    /// </summary>
    public partial class frmCostSheetFieldAdd : Window
    {
        public frmCostSheetFieldAdd()
        {
            InitializeComponent();
        }
        public static int costSheetFieldId;
        SaleOrderRepo repo = new SaleOrderRepo();
        CostSheetField costSheetField = new CostSheetField();
        CostSheetField parentCategory = new CostSheetField();
        ChartofAccount debitAccount = new ChartofAccount();
        ChartofAccount creditAccount = new ChartofAccount();

        private void btncostSheetFieldSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtcostSheetField.Text == "")
                {
                    MessageBox.Show("Please enter title", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtcostSheetField.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(spnHierarchicalIndex.Text))
                {
                    DXMessageBox.Show("Select a Hierarchical Index to continue");

                    return;
                }
                costSheetField.Title = txtcostSheetField.Text.Trim();
                costSheetField.SortId = Convert.ToInt32(spnHierarchicalIndex.Text);
                //if(cmbCreditAccounts.SelectedIndex>-1)
                //{
                //    costSheetField.creditCoaId = creditAccount.Id;
                //}
                //if (cmbDebitAccounts.SelectedIndex > -1)
                //{
                //    costSheetField.debitCoaId = debitAccount.Id;
                //}

                if (chkisactive.IsChecked == true)
                costSheetField.isActive = true;
            else
                costSheetField.isActive = false;
            if (costSheetField.Id == 0)
            {
                    if (MainWindow.currentUserid != 0)
                        costSheetField.AddedbyUserId = MainWindow.currentUserid;
                    costSheetField.Timestamp=System.DateTime.Now;
                    repo.AddCostSheetField(costSheetField);

                MessageBox.Show("New Field (" + txtcostSheetField.Text + ") Added", "Congratulations");
            }
            else
            {
                repo.UpdateCostSheetField(costSheetField);

                MessageBox.Show(" (" + txtcostSheetField.Text + ") updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //public void loadCategoris()
        //{


        //    List<CostSheetField> categories = new List<CostSheetField>();
        //    categories = repo.getActiveProductCategories();
        //    lookupCategory.ItemsSource = categories;

        //}
        //private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void lookupCategory_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    parentCategory = lookupCategory.SelectedItem as CostSheetField;
        //    if (parentCategory != null)
        //    {
        //        string selectedcust = parentCategory.category;
        //        lookupCategory.EditValue = selectedcust;


        //    }
        //}
        private void winCostSheetFieldAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            costSheetFieldId = 0;
        }

        private void winCostSheetFieldAdd_Loaded(object sender, RoutedEventArgs e)
        {
            //loadCategoris();
            //LoadChartofAccounts();
            if (costSheetFieldId != 0)
            {
                this.Title = "Edit Product Category";
                costSheetField = repo.getCostSheetField(costSheetFieldId);
                txtcostSheetField.Text = costSheetField.Title;
                chkisactive.IsChecked = costSheetField.isActive;
                spnHierarchicalIndex.Value = costSheetField.SortId;
                //if(costSheetField.debitAccount!=null)
                //{
                //    cmbDebitAccounts.Text = costSheetField.debitAccount.accountName;
                //}
                //if (costSheetField.creditAccount != null)
                //{
                //    cmbCreditAccounts.Text = costSheetField.creditAccount.accountName;
                //}
                //if (costSheetField.parentCategory != null && costSheetField.parentId != 0)
                //{
                //    parentCategory = costSheetField.parentCategory;

                //    lookupCategory.SelectedItem = lookupCategory.GetItemByKeyValue(parentCategory);
                //}
            }
        }
        //public void LoadChartofAccounts()
        //{
        //    ChartofAccountsRepo repo = new ChartofAccountsRepo();
        //    var chartofAccounts= repo.getAllActive(SYSTEM_STATIC.currentUser.id);
        //    cmbCreditAccounts.ItemsSource = chartofAccounts;
        //    cmbDebitAccounts.ItemsSource = chartofAccounts;

        //}

        //private void CmbCreditAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    creditAccount = cmbCreditAccounts.SelectedItem as ChartofAccount;
        //}

        //private void CmbDebitAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    debitAccount = cmbDebitAccounts.SelectedItem as ChartofAccount;
        //}
    }
}
