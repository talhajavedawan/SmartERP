using ERP_BL.FilesAndDocs;
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

namespace ZAS_ERP.FilesAndDocss.TravellingRecords.UserControls
{
    /// <summary>
    /// Interaction logic for ucTravelerList.xaml
    /// </summary>
    public partial class ucTravelerList : UserControl
    {
        VisitingRecordRepo recordRepo = new VisitingRecordRepo();
        ucFrmTraveler frmTraveler = new ucFrmTraveler();
        public ucTravelerList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            recordRepo = new VisitingRecordRepo();
            grdCntrlTravelerList.ItemsSource = recordRepo.GetAllTravelers();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            frmTraveler = new ucFrmTraveler();
            frmTraveler.addCategoryWindow.ResizeMode = ResizeMode.CanMinimize;
            frmTraveler.addCategoryWindow.Height = 500;
            frmTraveler.addCategoryWindow.Width = 450;
            frmTraveler.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmTraveler.editFlag = false;
            frmTraveler.addCategoryWindow.Content = frmTraveler;
            frmTraveler.addCategoryWindow.ShowDialog();
        }

        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            recordRepo = new VisitingRecordRepo();
            var selectedRow = grdCntrlTravelerList.SelectedItem as Traveler;
            if (selectedRow != null)
            {
                frmTraveler = new ucFrmTraveler();
                frmTraveler.travelerId = selectedRow.Id;
                frmTraveler.addCategoryWindow.ResizeMode = ResizeMode.CanMinimize;
                frmTraveler.addCategoryWindow.Height = 500;
                frmTraveler.addCategoryWindow.Width = 450;
                frmTraveler.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmTraveler.editFlag = true;
                frmTraveler.addCategoryWindow.Content = frmTraveler;
                frmTraveler.addCategoryWindow.ShowDialog();
            }

        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            recordRepo = new VisitingRecordRepo();
            grdCntrlTravelerList.ItemsSource = recordRepo.GetAllTravelers();
        }
    }
}
