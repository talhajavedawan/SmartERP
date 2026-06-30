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
    /// Interaction logic for ReviewSummaryWindow.xaml
    /// </summary>
    public partial class ReviewSummaryWindow : Window
    {
        MemoRepo memoRepo = new MemoRepo();

        public ReviewSummaryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var ratings = memoRepo.GetAllPerformanceRatings();
            grdCntrlSummary.ItemsSource = ratings;
        }        
    }
}
