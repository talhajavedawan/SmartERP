using ERP_BL.Procurements.Memos;
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

namespace ZAS_ERP.Memos.PerformanceReview
{
    /// <summary>
    /// Interaction logic for PerformanceIndicatorWindow.xaml
    /// </summary>
    public partial class PerformanceIndicatorWindow : Window
    {
        MemoRepo memoRepo = new MemoRepo();
        private int? selectedIndicatorId = null;
        PerformanceIndicatorDefinition indicator = new PerformanceIndicatorDefinition();
        public PerformanceIndicatorWindow()
        {
            InitializeComponent();
            LoadIndicators();
        }

        private void LoadIndicators()
        {
            // Call your own method to get all indicators from DB
            var indicators = memoRepo.GetAllPerformanceIndicators(); // Return as List<PerformanceIndicatorDefinition>
            dgIndicators.ItemsSource = indicators.OrderBy(i => i.DisplayOrder).ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate Inputs
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                !int.TryParse(txtDisplayOrder.Text, out int order) ||
                !int.TryParse(txtWeightage.Text, out int weight))
            {
                MessageBox.Show("Please provide valid inputs.");
                return;
            }

            if (selectedIndicatorId == null || selectedIndicatorId == 0 || indicator == null)
                indicator = new PerformanceIndicatorDefinition();

            indicator.Name = txtName.Text;
            indicator.Description = txtDescription.Text;
            indicator.DisplayOrder = order;
            indicator.Weightage = weight;
            indicator.IsActive = chkIsActive.IsChecked.Value;
            indicator.IsAdminType = chkIsAdmin.IsChecked.Value;






            if (selectedIndicatorId.HasValue)
            {
                // Set ID for updating existing record
                indicator.Id = selectedIndicatorId.Value;
                memoRepo.UpdatePerformanceIndicatorDefinition(indicator); // You implement this
            }
            else
            {
                memoRepo.AddPerformanceIndicatorDefinition(indicator); // You implement this
            }

            LoadIndicators();
            ClearForm();
        }


        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            indicator = new PerformanceIndicatorDefinition();

            txtName.Text = "";
            txtDescription.Text = "";
            txtDisplayOrder.Text = "";
            txtWeightage.Text = "";
            chkIsActive.IsChecked = true;
            chkIsAdmin.IsChecked = false;
            selectedIndicatorId = null;
            dgIndicators.UnselectAll();
        }

        private void DgIndicators_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            indicator = dgIndicators.SelectedItem as PerformanceIndicatorDefinition;
            if (dgIndicators.SelectedItem is PerformanceIndicatorDefinition selected)
            {
                selectedIndicatorId = selected.Id;

                txtName.Text = selected.Name;
                txtDescription.Text = selected.Description;
                txtDisplayOrder.Text = selected.DisplayOrder.ToString();
                txtWeightage.Text = selected.Weightage.ToString();
                chkIsActive.IsChecked = selected.IsActive;
                chkIsAdmin.IsChecked = selected.IsAdminType;
            }
        }
    }
}
