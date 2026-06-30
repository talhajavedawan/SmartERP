using ERP_BL.ToDoTasks;
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

namespace ZAS_ERP.ToDoTasks.TargetRewardss
{
    /// <summary>
    /// Interaction logic for frmTargetRewardStatusChange.xaml
    /// </summary>
    public partial class frmTargetRewardStatusChange : UserControl
    {
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;
        public frmTargetRewardStatusChange()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> billStatusLst = new List<cmbitem>();
                var allBillsStatus = taskRepo.GetAllClosedStatus();
                if (allBillsStatus != null)
                {
                    Parallel.ForEach(allBillsStatus, delegate (TargetRewardStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                    {
                        billStatusLst.Add
                        (new cmbitem()
                        {
                            name = status.Status,
                            id = status.Id,
                            bcolor = status.backcolor,
                            fcolor = "#FF000000"
                        });


                    });
                    cmbBillStatus.ItemsSource = billStatusLst;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Sale Receipt Status 
                if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                {
                    var status = taskRepo.GetTargetRewardStatus((cmbBillStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        //receiptStatus.statusChanged = status;
                        if (frmFlag == true)
                        {
                            ucFrmTargetRewardAdd frmBillAdd = new ucFrmTargetRewardAdd(status);
                        }
                        else
                        {
                            ucTargetRewardsRegister receiptStatus = new ucTargetRewardsRegister(status);
                        }
                        directCloseWin.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Select a status before saving!");
                }
            }
            catch
            {

            }
        }
    }
}
