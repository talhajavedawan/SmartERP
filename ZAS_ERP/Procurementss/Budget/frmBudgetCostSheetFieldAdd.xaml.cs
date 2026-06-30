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
    /// Interaction logic for frmBudgetCostSheetFieldAdd.xaml
    /// </summary>
    public partial class frmBudgetCostSheetFieldAdd : DXWindow
    {
        public static int costSheetFieldId;
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
        BudgetSheetHead costSheetField = new BudgetSheetHead();
        BudgetSheetHead parentCategory = new BudgetSheetHead();
     
        public frmBudgetCostSheetFieldAdd()
        {
            InitializeComponent();
        }

        private void winBudgetCostSheetFieldAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (costSheetFieldId != 0)
            {
                this.Title = "Edit Product Category";
                costSheetField = repo.getCostSheetField(costSheetFieldId);
                txtcostSheetField.Text = costSheetField.HeadName;
                chkisactive.IsChecked = costSheetField.isActive;
                chkisExpense.IsChecked = costSheetField.isExpense;
                chkisIncome.IsChecked = costSheetField.isIncome;
                chkisCGS.IsChecked = costSheetField.isCGS;
                spnHierarchicalIndex.Value = costSheetField.SortId;

            }
        }

        private void winBudgetCostSheetFieldAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            costSheetFieldId = 0;
        }

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
                costSheetField.HeadName = txtcostSheetField.Text.Trim();
                costSheetField.SortId = Convert.ToInt32(spnHierarchicalIndex.Text);


                if (chkisactive.IsChecked == true)
                    costSheetField.isActive = true;
                else
                    costSheetField.isActive = false;


                if (chkisIncome.IsChecked == true)
                    costSheetField.isIncome = true;
                else
                    costSheetField.isIncome = false;


                if (chkisExpense.IsChecked == true)
                    costSheetField.isExpense = true;
                else
                    costSheetField.isExpense = false;




                if (chkisCGS.IsChecked == true)
                    costSheetField.isCGS = true;
                else
                    costSheetField.isCGS = false;

                if (costSheetField.Id == 0)
                {
                    if (MainWindow.currentUserid != 0)
                        costSheetField.creatorId = MainWindow.currentUserid;
                    //costSheetField.Timestamp = System.DateTime.Now;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
