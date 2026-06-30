using ERP_BL.Enums;
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
    /// Interaction logic for StageSelectionDialog.xaml
    /// </summary>
    public partial class StageSelectionDialog : Window
    {
        public PerformanceReviewerStage SelectedStage { get; private set; }

        public StageSelectionDialog()
        {
            InitializeComponent();
            cmbStages.ItemsSource = Enum.GetValues(typeof(PerformanceReviewerStage)).Cast<PerformanceReviewerStage>();
            cmbStages.SelectedIndex = 0; // Optional: set default selection
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStages.SelectedItem is PerformanceReviewerStage selected)
            {
                SelectedStage = selected;
                DialogResult = true;
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}
