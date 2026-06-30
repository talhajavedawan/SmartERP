using DevExpress.Xpf.Core;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucManagementSummaryAdd.xaml
    /// </summary>
    public partial class ucManagementSummaryAdd : UserControl
    {
        public bool editFlag = false;
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        public ManagementSummary managementSummary = new ManagementSummary();

        public ucManagementSummaryAdd()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtManagementSummary.Text))
            {
                DXMessageBox.Show("Please Enter Summary Name!");
                txtManagementSummary.Focus();
                return;
            }

            if (lookupParentSUmmary.SelectedIndex > -1)
                managementSummary.ParentId = (lookupParentSUmmary.SelectedItem as ManagementSummary).Id;

            if (chkIsActive.IsChecked == true)
                managementSummary.isActive = true;
            else
                managementSummary.isActive = false;

            managementSummary.SummaryName = txtManagementSummary.Text;
            billsRepo = new AdminBillsRepo();
            if (editFlag == false)
            {
                billsRepo.AddManagementSummary(managementSummary);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                billsRepo.UpdateManagementSummary(managementSummary);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lookupParentSUmmary.ItemsSource = billsRepo.GetAllManagementSummary();
            if (editFlag == true)
            {
                if(managementSummary.parentSummary != null)
                {
                    var managementSummaries = (lookupParentSUmmary.ItemsSource as List<ManagementSummary>) == null ? new List<ManagementSummary>() : lookupParentSUmmary.ItemsSource as List<ManagementSummary>;
                    
                        int indexx = 0;
                        foreach (var _summary in managementSummaries)
                        {
                            if (_summary.Id == managementSummary.ParentId)
                            {
                                lookupParentSUmmary.SelectedIndex = indexx;
                                indexx = 0;
                                break;
                            }
                            indexx++;
                        }
                }
                    

                txtManagementSummary.Text = managementSummary.SummaryName;
                chkIsActive.IsChecked = managementSummary.isActive;
            }
        }
    }
}
