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
    /// Interaction logic for ucTravelingSummary.xaml
    /// </summary>
    public partial class ucTravelingSummary : UserControl
    {
        VisitingRecordRepo recordRepo = new VisitingRecordRepo();
        public ucTravelingSummary()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdTravelingRecordRegister.ItemsSource = recordRepo.GetAllTravelRecordss(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdTravelingRecordRegister);
        }

        private void grdTravelingRecordRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void grdTravelingRecordRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void mbtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mbtnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
