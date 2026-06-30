using ERP_BL.Bankings;
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

namespace ZAS_ERP.Bankings.Loans
{
    /// <summary>
    /// Interaction logic for ucLoansRow.xaml
    /// </summary>
    public partial class ucLoansRow : UserControl
    {
        List<FacilityNature> facilityNatures = new List<FacilityNature>();
        LoansRepo loansRepo = new LoansRepo();
        public ucLoansRow()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            facilityNatures = loansRepo.GetAllFacilityNatures();
            lookupMainLimitNature.ItemsSource = facilityNatures;
            lookupSubLimitNature.ItemsSource = facilityNatures;
        }
    }
}
