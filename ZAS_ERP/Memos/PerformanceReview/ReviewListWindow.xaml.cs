using ERP_BL.Databases;
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
    /// Interaction logic for ReviewListWindow.xaml
    /// </summary>
    public partial class ReviewListWindow : Window
    {
        MemoRepo memoRepo = new MemoRepo();

        public ReviewListWindow()
        {
            InitializeComponent();
            LoadFilters();
            LoadReviews();
        }

        private void LoadFilters()
        {
            filterDepartment.ItemsSource = memoRepo.GetDepartments();
            filterEmployee.ItemsSource = memoRepo.getAllusers();
        }

        private void LoadReviews()
        {
            var year = filterYear.DateTime;
            var deptId = (filterDepartment.SelectedItem as Department)?.Id;
            var empId = (filterEmployee.SelectedItem as ERP_BL.Databases.User)?.id;

            var allReviews = memoRepo.GetAllPerformanceReviews();

            var filtered = allReviews.Where(r =>
                (year != null || r.PerformanceYear?.Year == year.Year) &&
                (!deptId.HasValue || r.DeptId == deptId) &&
                (!empId.HasValue || r.EmployeeId == empId)).ToList();

            dgReviews.ItemsSource = filtered;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadReviews();
        }

        private void BtnEditReview_Click(object sender, RoutedEventArgs e)
        {
            if (dgReviews.SelectedItem is EmployeePerformanceReview selectedReview)
            {
                var editWindow = new PerformanceReviewWindow(); // Step 3.2 will use this
                editWindow.memoId = selectedReview.MemoId.Value;
                editWindow.reviewIdToEdit = selectedReview.Id;
                editWindow.editFlag = true;
                editWindow.ShowDialog();
                LoadReviews(); // Refresh after edit
            }
            else
            {
                MessageBox.Show("Please select a review to edit.");
            }
        }
    }
}
