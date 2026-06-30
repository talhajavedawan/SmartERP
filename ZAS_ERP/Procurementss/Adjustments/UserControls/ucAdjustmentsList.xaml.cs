using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Procurementss.Adjustments.UserControls
{
    /// <summary>
    /// Interaction logic for ucAdjustmentsList.xaml
    /// </summary>
    public partial class ucAdjustmentsList : Window
    {
        AdminBillsRepo BillsRepo = new AdminBillsRepo();
        public List<Adjustment> adjustments = new List<Adjustment>();
        List<Adjustment> finalAdjustments = new List<Adjustment>();
        public int billId = 0;
        public ucAdjustmentsList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (billId != 0)
            {
                adjustments = BillsRepo.GetAdjustmentsByBillId(billId);
                LoadCounters();
                //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustments") == null)
                //{
                //    btnSave.IsEnabled = false;
                //}
            }
            //finalAdjustments = adjustments;
            grdAdjustments.ItemsSource = adjustments.Where(x => x.isApproved == true).ToList();
        }

        private void LoadAdjustments()
        {
            if (billId != 0)
            {
                LoadCounters();
            }
            //finalAdjustments = adjustments;
            grdAdjustments.ItemsSource = adjustments.Where(x => x.isApproved == true).ToList();
        }

        private void LoadCounters()
        {
            mbtnApprovalsCount.Header = adjustments.Where(x=>x.isApproved != true).Count();
        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            //View.AddNewRow();
        }

        private void View1_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void GrdListTransactions_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            

            //totalDeduction = finalDeductions.Sum(x => x.Amount);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //finalAdjustments = new List<Adjustment>();
            foreach (var _adj in grdAdjustments.ItemsSource as List<Adjustment>)
            {
                if(_adj.Id == 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Adjustments without Approval") != null)
                    {
                        adjustments.Add(new Adjustment()
                        {
                            Id = _adj.Id,
                            AdjustmentDate = _adj.AdjustmentDate,
                            adminBillId = _adj.adminBillId,
                            AdjustmentAmount = _adj.AdjustmentAmount,
                            ReferenceNo = _adj.ReferenceNo,
                            isApproved = true,
                            ApprovedDate = _adj.ApprovedDate
                        });
                    }
                    else
                    {
                        adjustments.Add(new Adjustment()
                        {
                            Id = _adj.Id,
                            AdjustmentDate = _adj.AdjustmentDate,
                            adminBillId = _adj.adminBillId,
                            AdjustmentAmount = _adj.AdjustmentAmount,
                            ReferenceNo = _adj.ReferenceNo,
                            isApproved = false,
                            ApprovedDate = _adj.ApprovedDate
                        });
                    }
                }
                
            }
            Window win = Window.GetWindow(this);
            win.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAdjustments();
            txtHeader.Text = "Adjustments";
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (billId != 0)
            {
                var row = grdAdjustments.SelectedItem as Adjustment;
                if (row != null && row.isApproved == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Adjustments without Approval") != null)
                    {
                        if (adjustments.Find(x => x.Id == row.Id) != null)
                        {
                            adjustments[adjustments.FindIndex(x => x.Id == row.Id)].isApproved = true;
                            adjustments[adjustments.FindIndex(x => x.Id == row.Id)].ApprovedDate = DateTime.Now;
                            DXMessageBox.Show("Adjustment Approved!");
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to Approve Adjustments!");
                    }
                }
            }
            else
            {
                DXMessageBox.Show("Admin Bill is not Saved yet!");
            }
        }

        private void MbtnPendingForApproval_Click(object sender, RoutedEventArgs e)
        {
            if(billId != 0)
            {
                if( SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "View(Pending for Approval) Adjustments List") != null)
                {
                    txtHeader.Text = "Pending for Approval Adjustments";
                    grdAdjustments.ItemsSource = adjustments.Where(x => x.isApproved != true).ToList();
                }
                else
                {
                    DXMessageBox.Show("Permission required to View Pending for Approval Adjustments!");
                }
            }
            else
            {
                DXMessageBox.Show("Admin Bill is not Saved yet!");
            }
        }

        private void View_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
           
            var adj = grdAdjustments.GetFocusedRow() as Adjustment;
            var rowHandle = grdAdjustments.GetSelectedRowHandles();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustments") == null && adj != null && adj.Id > 0)
            {
                    DXMessageBox.Show("Permission Required to Edit already Added Adjustment!");
                //e.Value = e.OldValue;
                //grdAdjustments.SetCellValue(rowHandle[0], e.Column, e.OldValue);
                if (e.Column.FieldName == "AdjustmentAmount")
                    adj.AdjustmentAmount =Convert.ToDouble(  e.OldValue);
                if (e.Column.FieldName == "AdjustmentDate")
                    adj.AdjustmentDate = Convert.ToDateTime(e.OldValue);
                if (e.Column.FieldName == "ReferenceNo")
                    adj.ReferenceNo = e.OldValue.ToString();
                if (e.Column.FieldName == "AdjustmentDate")
                    adj.AdjustmentDate = Convert.ToDateTime(e.OldValue);
            }
            
            
        }
    }
}
