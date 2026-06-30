using DevExpress.Xpf.Core;
using ERP_BL.Databases;
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

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for winPerformanceSheetHeadAdd.xaml
    /// </summary>
    public partial class winPerformanceSheetHeadAdd : DXWindow
    {
        public static int performanceSheetFieldId;
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
        PerformanceSheetHead performanceSheetField = new PerformanceSheetHead();
        public winPerformanceSheetHeadAdd()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            performanceSheetFieldId = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (performanceSheetFieldId != 0)
            {
                this.Title = "Edit performance sheet Head";
                performanceSheetField = repo.getPerformanceSheetHead(performanceSheetFieldId);
                txtcostSheetField.Text = performanceSheetField.HeadName;
                chkisactive.IsChecked = performanceSheetField.isActive;
                spnHierarchicalIndex.Value = performanceSheetField.SortId;
                txtTotalPoints.Text = performanceSheetField.totalPoints.ToString();

            }
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
                performanceSheetField.HeadName = txtcostSheetField.Text.Trim();
                performanceSheetField.SortId = Convert.ToInt32(spnHierarchicalIndex.Text);


                if (chkisactive.IsChecked == true)
                    performanceSheetField.isActive = true;
                else
                    performanceSheetField.isActive = false;


                if(!string.IsNullOrEmpty(txtTotalPoints.Text))
                {
                    performanceSheetField.totalPoints = Convert.ToDouble(txtTotalPoints.Text);
                }
                else
                {
                    DXMessageBox.Show("Please input total points");
                    return;
                }



                

                if (performanceSheetField.Id == 0)
                {
                    if (MainWindow.currentUserid != 0)
                        performanceSheetField.creatorId = MainWindow.currentUserid;
                    repo.AddPerformanceSheetField(performanceSheetField);

                    MessageBox.Show("New Field (" + txtcostSheetField.Text + ") Added", "Congratulations");
                }
                else
                {
                    repo.UpdatePerformanceSheetField(performanceSheetField);

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
