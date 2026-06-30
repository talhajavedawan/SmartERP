using ERP_BL.Databases;
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
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for ucFrmDirectCloseEmp.xaml
    /// </summary>
    public partial class ucFrmDirectCloseEmp : UserControl
    {
        EmployeeRepo repo = new EmployeeRepo();
        ucEmployeeInfo empStatus = new ucEmployeeInfo();

        public UcListWindow directCloseWin = new UcListWindow();

        public ucFrmDirectCloseEmp()
        {
            InitializeComponent();

            List<cmbitem> empStausLst = new List<cmbitem>();
            var AllEmpStatus = repo.GetAllInActiveEmployeeStatus();
            if (AllEmpStatus != null)
            {
                Parallel.ForEach(AllEmpStatus, delegate (EmployeeWorkingStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                {
                    empStausLst.Add
                    (new cmbitem()
                    {
                        name = status.Status,
                        id = status.Id,
                        bcolor = status.backcolor,
                        fcolor = "#FF000000"
                    });


                });
                cmbEmpStatus.ItemsSource = empStausLst;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           if ((cmbEmpStatus.SelectedItem as cmbitem) != null)
            {
                var status = repo.GetEmployeeStatus((cmbEmpStatus.SelectedItem as cmbitem).id);
                if (status != null)
                {
                    //receiptStatus.statusChanged = status;
                    ucEmployeeInfo empStatus = new ucEmployeeInfo(status);
                    directCloseWin.Close();
                }
            }
            else
            {
                MessageBox.Show("Select a status before saving!");
            }

        }
    }
}
