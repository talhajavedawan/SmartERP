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
    /// Interaction logic for ucAirlineList.xaml
    /// </summary>
    public partial class ucAirlineList : UserControl
    {
        VisitingRecordRepo recordRepo = new VisitingRecordRepo();
        public ucAirlineList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucAirlineAdd ucAirline = new ucAirlineAdd();
            Window win = new Window();
            ucAirline.editFlag = false;
            win.Content = ucAirline;
            win.Width = 300;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlAirlineList.SelectedItem != null)
            {
                var selectedRow = grdCntrlAirlineList.SelectedItem as Airline;
                ucAirlineAdd ucAirline = new ucAirlineAdd();
                Window win = new Window();
                ucAirline.typeId = selectedRow.Id;
                ucAirline.editFlag = true;
                win.Content = ucAirline;
                win.Width = 300;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            recordRepo = new VisitingRecordRepo();
            grdCntrlAirlineList.ItemsSource = recordRepo.GetAllAirlines();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            recordRepo = new VisitingRecordRepo();
            grdCntrlAirlineList.ItemsSource = recordRepo.GetAllAirlines();
        }

    }
}
