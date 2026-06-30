using ERP_BL.ToDoTasks.Taskss;
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
using ZAS_ERP.ToDoTasks.Taskks.UserControls.TaxTasks;

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmTasksDirectClose.xaml
    /// </summary>
    public partial class ucFrmTasksDirectClose : UserControl
    {
        TaskRepo taskRepo = new TaskRepo();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;
        public string template = "";

        public ucFrmTasksDirectClose()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> billStatusLst = new List<cmbitem>();
                var allBillsStatus = taskRepo.GetAllCloseTasksStatus();
                if (allBillsStatus != null)
                {
                    Parallel.ForEach(allBillsStatus, delegate (TasksStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
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
                    cmbTaskStatus.ItemsSource = billStatusLst;
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
                if ((cmbTaskStatus.SelectedItem as cmbitem) != null)
                {
                    var status = taskRepo.GetTaskStatus((cmbTaskStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        //receiptStatus.statusChanged = status;
                        if (frmFlag == true)
                        {
                            switch (template)
                            {
                                case "NA":
                                    ucTaskAdd taskAdd = new ucTaskAdd(status);
                                    break;
                                case "TaxRecord":
                                    ucTaxTaskAdd ucTaxTaskAdd = new ucTaxTaskAdd(status);
                                    break;
                            }
                        }
                        //else
                        //{
                        //    ucTaskGrid taskGrid = new ucTaskGrid(status);

                        //}
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
